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

        private readonly string _apiKey;

        private string _lastRequest;
        private string _lastResponse;

        /// <param name="apiKey">The Mollie API key, starting with 'test_' or 'live_'</param>
        public MollieClient(string apiKey)
        {
            if ((apiKey == null) || (!Regex.IsMatch(apiKey.Trim(), "^(live|test)_\\w+$")))
            {
                throw new Exception(String.Format("Invalid API key: '{0}'. An API key must start with 'test_' or 'live_'.", apiKey));
            }
            _apiKey = apiKey.Trim();
        }

        #region Issuers API
        /// <summary>
        /// Retrieve an issuer object by specifying an issuer identifier.
        /// </summary>
        /// <returns></returns>
        public Issuer GetIssuer(string id)
        {
            string jsonData = LoadWebRequest("GET", "issuers/" + id, "");
            Issuer issuer = JsonConvert.DeserializeObject<Issuer>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return issuer;
        }

        /// <summary>
        /// To integrate the iDeal issuer selection step on your own web site.
        /// </summary>
        /// <returns></returns>
        public Issuers ListIssuers()
        {
            string jsonData = LoadWebRequest("GET", "issuers", "");
            Issuers issuers = JsonConvert.DeserializeObject<Issuers>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return issuers;
        }
        #endregion

        #region Payments API

        /// <summary>
        /// Create a payment
        /// </summary>
        /// <param name="payment">Payment object</param>
        /// <returns></returns>
        public PaymentStatus CreatePayment(Payment payment)
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
        public PaymentStatus GetPayment(string id)
        {
            string jsonData = LoadWebRequest("GET", "payments" + "/" + id, "");
            PaymentStatus status = JsonConvert.DeserializeObject<PaymentStatus>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return status;
        }

        /// <summary>
        /// Get list of a payments
        /// </summary>
        /// <returns></returns>
        public Payments GetPayments()
        {
            string jsonData = LoadWebRequest("GET", "payments", "");
            Payments payments = JsonConvert.DeserializeObject<Payments>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return payments;
        }

        /// <summary>
        /// To refund a payment, you must have sufficient balance with Mollie for deducting the refund and its fees. You can find your current balance on the on the Mollie controlpanel.
        /// At the moment you can only process refunds for iDEAL, Bancontact/Mister Cash, SOFORT Banking, creditcard and bank transfer payments.
        /// </summary>
        /// <param name="id">The id of the payment</param>
        /// <param name="amount">Optional - The amount to refund</param>
        /// <param name="description">Optional – The description of the refund you are creating. This will be shown to the consumer on their card or bank statement when possible. Max 140 characters.</param>
        /// <returns></returns>
        public RefundStatus CreateRefund(string id, decimal amount = 0, string description = "")
        {
            string jsonData = LoadWebRequest("POST", "payments" + "/" + id + "/refunds", (amount == 0) ? "" : JsonConvert.SerializeObject(new { amount, description }));
            RefundStatus refundStatus = JsonConvert.DeserializeObject<RefundStatus>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return refundStatus;
        }

        /// <summary>
        /// Retrieve all refunds for the given payment.
        /// </summary>
        /// <param name="id">The id of the payment</param>
        /// <returns></returns>
        public Refunds ListRefunds(string id)
        {
            string jsonData = LoadWebRequest("POST", "payments" + "/" + id + "/refunds", "");
            Refunds refunds = JsonConvert.DeserializeObject<Refunds>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return refunds;
        }

        #endregion

        #region Methods API
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
        #endregion

        #region Customers API
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
        public Customers ListCustomers()
        {
            string jsonData = LoadWebRequest("GET", "customers", "");
            Customers customers = JsonConvert.DeserializeObject<Customers>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return customers;
        }
        #endregion

        #region Mandates API
        public Mandates ListMandates(string customerId)
        {
            string jsonData = LoadWebRequest("GET", "customers/" + customerId + "/mandates", "");
            Mandates mandates = JsonConvert.DeserializeObject<Mandates>(jsonData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return mandates;
        }
        #endregion

        /// <summary>
        /// Returns the last json request data
        /// </summary>
        public string LastRequest
        {
            get { return _lastRequest; }
        }

        /// <summary>
        /// Returns the last json response data
        /// </summary>
        public string LastResponse
        {
            get { return _lastResponse; }
        }

        private string LoadWebRequest(string httpMethod, string resource, string postData)
        {
            _lastRequest = postData;
            _lastResponse = "{}";

            string url = API_ENDPOINT + "/" + API_VERSION + "/" + resource;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["Proxy"]))
            {
                WebProxy proxy = new WebProxy(new Uri(ConfigurationManager.AppSettings["Proxy"]));
                request.Proxy = proxy;
            }

            request.Method = httpMethod;
            request.Accept = "application/json";
            request.Headers.Add("Authorization", "Bearer " + _apiKey);
            request.UserAgent = "Foxip Mollie Client v" + CLIENT_VERSION;
            request.Timeout = 10*1000; // 10 x 1000ms = 10s

            if (postData != "")
            {
                //Send the request and get the response
                request.ContentType = "application/json";
                using (Stream stream = request.GetRequestStream())
                {
                    using (StreamWriter streamOut = new StreamWriter(stream, System.Text.Encoding.ASCII))
                    {
                        streamOut.Write(postData);
                        streamOut.Close();
                    }
                    stream.Close();
                }
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (StreamReader streamIn = new StreamReader(stream))
                            {
                                _lastResponse = streamIn.ReadToEnd();
                                streamIn.Close();
                            }
                            stream.Close();
                        }
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException && ((WebException)ex).Status == WebExceptionStatus.ProtocolError)
                {
                    using (WebResponse errResp = ((WebException) ex).Response)
                    {
                        using (Stream respStream = errResp.GetResponseStream())
                        {
                            if (respStream != null)
                            {
                                using (StreamReader r = new StreamReader(respStream))
                                {
                                    _lastResponse = r.ReadToEnd();
                                    r.Close();
                                }
                                respStream.Close();
                            }
                        }
                        errResp.Close();
                    }
                    throw new Exception(_lastResponse);
                }
                else
                {
                    throw;
                }
            }

            return _lastResponse;
        }
    }
}