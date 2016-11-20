/*
Copyright (c) 2016, Fox Internet Programming, www.foxip.net
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Mollie.Api.Models;
using Newtonsoft.Json;

namespace Mollie.Api
{
    public class MollieClient
    {

        // Version of this Mollie client.
        const string CLIENT_VERSION = "1.0.1";

        // Endpoint of the remote API.
        const string API_ENDPOINT = "https://api.mollie.nl";

        // Version of the remote API.
        const string API_VERSION = "v1";

        private string _api_key;

        private string _last_request;
        private string _last_response;
		
        /// <param name="api_key">The Mollie API key, starting with 'test_' or 'live_'</param>
        public void setApiKey(string api_key)
        {
            api_key = api_key.Trim();

            if (!Regex.IsMatch(api_key, "^(live|test)_\\w+$"))
            {
                throw new Exception(String.Format("Invalid API key: '{0}'. An API key must start with 'test_' or 'live_'.", api_key));
            }

            _api_key = api_key;
        }		

        /// <summary>
        /// To integrate the iDeal issuer selection step on your own web site.
        /// </summary>
        /// <returns></returns>
        public Issuers GetIssuers()
        {
            string jsonData = LoadWebRequest("GET", "issuers", "");
            Issuers issuers = JsonConvert.DeserializeObject<Issuers>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return issuers;
        }

        /// <summary>
        /// Start a payment
        /// </summary>
        /// <param name="payment">Payment object</param>
        /// <returns></returns>
        public PaymentStatus StartPayment(Payment payment)
        {
            string jsonData = LoadWebRequest("POST", "payments", JsonConvert.SerializeObject(payment, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            PaymentStatus status = JsonConvert.DeserializeObject<PaymentStatus>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore}); 
            return status;
        }

        /// <summary>
        /// Get status of a payment
        /// </summary>
        /// <param name="id">The id of the payment</param>
        /// <returns></returns>
        public PaymentStatus GetStatus(string id)
        {
            string jsonData = LoadWebRequest("GET", "payments" + "/" + id, "");
            PaymentStatus status = JsonConvert.DeserializeObject<PaymentStatus>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return status;
        }

        /// <summary>
        /// To refund a payment, you must have sufficient balance with Mollie for deducting the refund and its fees. You can find your current balance on the on the Mollie controlpanel.
        /// At the moment you can only process refunds for iDEAL, Bancontact/Mister Cash, SOFORT Banking, creditcard and bank transfer payments.
        /// </summary>
        /// <param name="id">The id of the payment</param>
        /// <returns></returns>
        public RefundStatus Refund(string id)
        {
            return Refund(id, 0);
        }
		
        /// <summary>
        /// To refund a payment, you must have sufficient balance with Mollie for deducting the refund and its fees. You can find your current balance on the on the Mollie controlpanel.
        /// At the moment you can only process refunds for iDEAL, Bancontact/Mister Cash, SOFORT Banking, creditcard and bank transfer payments.
        /// </summary>
        /// <param name="id">The id of the payment</param>
        /// <param name="amount">The amount to refund</param>
        /// <returns></returns>
        public RefundStatus Refund(string id, decimal amount)
        {
            string jsonData = LoadWebRequest("POST", "payments" + "/" + id + "/refunds", (amount == 0) ? "" : JsonConvert.SerializeObject(new { amount = amount }));
            RefundStatus refundStatus = JsonConvert.DeserializeObject<RefundStatus>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return refundStatus;
        }

        /// <summary>
        /// Fetch all payment methods for your profile
        /// </summary>
        /// <returns></returns>
        public PaymentMethods GetPaymentMethods()
        {
            string jsonData = LoadWebRequest("GET", "methods", "");
            PaymentMethods methods = JsonConvert.DeserializeObject<PaymentMethods>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return methods;
        }

        public GetCustomer CreateCustomer(CreateCustomer customer)
        {
            string jsonData = LoadWebRequest("POST", "customers", JsonConvert.SerializeObject(customer, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            GetCustomer getCustomer = JsonConvert.DeserializeObject<GetCustomer>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return getCustomer;
        }

        public GetCustomer GetCustomer(string id)
        {
            string jsonData = LoadWebRequest("GET", "customers/" + id, "");
            GetCustomer getCustomer = JsonConvert.DeserializeObject<GetCustomer>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return getCustomer;
        }

        /// <summary>
        /// Returns the last json request data
        /// </summary>
        public string LastRequest
        {
            get { return _last_request; }
        }

        /// <summary>
        /// Returns the last json response data
        /// </summary>
        public string LastResponse
        {
            get { return _last_response; }
        }

        private string LoadWebRequest(string httpMethod, string resource, string postData)
        {
            _last_request = postData;

            string url = API_ENDPOINT + "/" + API_VERSION + "/" + resource;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["Proxy"]))
            {
                WebProxy proxy = new WebProxy(new Uri(ConfigurationManager.AppSettings["Proxy"]));
                request.Proxy = proxy;
            }

            request.Method = httpMethod;
            request.Accept = "application/json";
            request.Headers.Add("Authorization", "Bearer " + _api_key);
            request.UserAgent = "Foxip Mollie Client v" + CLIENT_VERSION;

            if (postData != "")
            {
                //Send the request and get the response
                request.ContentType = "application/json";
                using (StreamWriter streamOut = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII))
                {
                    streamOut.Write(postData);
                    streamOut.Close();
                }
            }

            string strResponse = "";

            try
            {
                using (StreamReader streamIn = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    strResponse = streamIn.ReadToEnd();
                    streamIn.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException && ((WebException)ex).Status == WebExceptionStatus.ProtocolError)
                {
                    WebResponse errResp = ((WebException)ex).Response;
                    using (Stream respStream = errResp.GetResponseStream())
                    {
                        using (StreamReader r = new StreamReader(respStream))
                        {
                            strResponse = r.ReadToEnd();
                            r.Close();
                        }
                        respStream.Close();
                        throw new Exception(strResponse);
                    }
                }
            }

            _last_response = strResponse;

            return strResponse;
        }
    }
}