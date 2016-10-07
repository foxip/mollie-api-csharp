/*
Copyright (c) 2014, Fox Internet Programming, www.foxip.net
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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Mollie.Api
{

	#region Enums
	/// <summary>
	/// Payment method
	/// </summary>
	public enum Method
	{
		ideal,
		creditcard,
		mistercash,
		sofort,
		banktransfer,
		directdebit,
		belfius,
		kbc,
		bitcoin,
		paypal,
		paysafecard,
	}	
	/// <summary>
	/// Payment status
	/// </summary>
	public enum Status
	{
		/// <summary>
		/// The payment has been created, but no other status has been reached yet.
		/// </summary>
		open,

		/// <summary>
		/// Your customer has cancelled the payment.
		/// </summary>
		cancelled,

		/// <summary>
		/// The payment has been started but not yet complete.
		/// </summary>
		pending,

		/// <summary>
		/// The payment has been paid for.
		/// </summary>
		paid,

		/// <summary>
		/// The payment has been paid for and we have transferred the sum to your bank account.
		/// </summary>
		paidout,

		/// <summary>
		/// The payment has been refunded.
		/// </summary>
		refunded,

		/// <summary>
		/// The payment has expired, for example, your customer has closed the payment screen.
		/// </summary>
		expired,
		
		/// <summary>
	        /// The payment has failed and can not be finished with another payment method.
	        /// </summary>
	        failed,
	
	        /// <summary>
	        /// The payment has been charged back after a complaint in case of creditcard, directdebit and paypal.
	        /// </summary>
	        charged_back,
	}
	#endregion

	#region Plain objects
	public class Payment
	{
		/// <summary>
		/// (Required) The exact amount you want to charge your buyer in Euro's. If you want to charge € 99,95 provide 99.95 as the value.
		/// </summary>
		public decimal amount { get; set; }
		
		/// <summary>
		/// (Required) The description for this payment. This will be shown to your buyer in our payment screens and on their bank statement if the payment method supports that.
		/// </summary>
		public string description { get; set; }

		/// <summary>
		/// (Required) The URL that you want us to send your buyer to after he completes the payment. It's important to include some kind of unique identifier to this URL - like an order ID - so that you can directly show the right screen to your buyer.
		/// </summary>
		public string redirectUrl { get; set; }

		/// <summary>
		/// (Optional) Use this parameter to directly select a payment method to use. Your buyer will then skip the payment method selection screen and will be sent directly to the payment method.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public Method? method { get; set; }

		/// <summary>
		/// (Optional) Use this parameter to store your own custom parameters with the payment. When you retrieve the payment details, you'll get these custom parameters back as well. You can provide around 1 KB of data.
		/// </summary>
		public string metadata { get; set; }

		/// <summary>
		/// (Required) The URL that you want us to send your buyer to after he completes the payment. It's important to include some kind of unique identifier to this URL - like an order ID - so that you can directly show the right screen to your buyer.
		/// </summary>
		public string webhookUrl { get; set; }

		/// <summary>
		/// (Optional) If you pass a valid URL as this parameter, we will use this URL as the web hook instead of the web hook that is set in the web site profile.
        /// Valid values: nl, fr, de, en, es
		/// </summary>
		public string locale { get; set; }

		//(Optional) Creditcard and/or paypal parameters. Countries must be specified in ISO 3166-1 alpha-2 format.
		public string billingAddress { get; set; }
		public string billingCity { get; set; }
		public string billingRegion { get; set; }
		public string billingPostal { get; set; }
		public string billingCountry { get; set; }
		public string shippingAddress { get; set; }
		public string shippingCity { get; set; }
		public string shippingRegion { get; set; }
		public string shippingPostal { get; set; }
		public string shippingCountry { get; set; }

		/// <summary>
		/// (Optional) Bank transfer parameter: Email address of the customer where he/she will receive the bank transfer details.
		/// </summary>
		public string billingEmail { get; set; }

		/// <summary>
		/// (Optional) Date that the payment automatically expires. Format YYYY-MM-DD.
		/// </summary>
		public string dueDate { get; set; }

		/// <summary>
		/// (Optional) Paysafecard parameter. Used for identifying the customer. For example the remote IP address of the customer.
		/// </summary>
		public string customerReference { get; set; }

		/// <summary>
		/// (Optional) iDEAL issuer id. The id could for example be ideal_INGBNL2A. The returned paymentUrl will then directly point to the ING web site.
		/// </summary>
		public string issuer { get; set; }

	}

	public class PaymentStatus
	{
		/// <summary>
		/// The transaction ID for the payment
		/// </summary>
		public string id { get; set; }

		/// <summary>
		/// test or live, This depends on what API key you used in creating the payment
		/// </summary>
		public string mode { get; set; }

		/// <summary>
		/// The exact date and time the payment was created, in ISO-8601 format. All dates and times are in the GMT time zone.
		/// </summary>
		public DateTime? createdDatetime { get; set; }

		/// <summary>
		/// The current status of the payment.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public Status? status { get; set; }

		/// <summary>
		/// The exact date and time the payment was marked as paid, in ISO-8601 format.
		/// </summary>
		public DateTime? paidDatetime { get; set; }

		/// <summary>
		/// The exact date and time the payment was cancelled, in ISO-8601 format.
		/// </summary>
		public DateTime? cancelledDatetime { get; set; }

        /// <summary>
        /// The exact date and time the payment expired, in ISO-8601 format.
        /// </summary>
        public DateTime? expiredDatetime { get; set; }

        /// <summary>
        /// The time until a payment will expire in ISO 8601 duration format.
        /// </summary>
        public string expiryPeriod { get; set; }

		/// <summary>
		/// The amount you specified for this payment in Euro's.
		/// </summary>
		public decimal amount { get; set; }

		/// <summary>
		/// The description of the payment.
		/// </summary>
		public string description { get; set; }

		/// <summary>
		/// The payment method that was ultimately used to complete this payment or null when no payment method was selected yet.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public Method? method { get; set; }

		/// <summary>
		/// Any details the specific payment method provided after completing the payment. In case of iDEAL this will contain the bank account number (IBAN).
		/// </summary>
		public PaymentDetails details { get; set; }

		/// <summary>
		/// Contains various links relevant to the payment. The most important one is paymentUrl, after creating your payment you should send your buyer to this URL. Warning: you cannot use the paymentUrl in an iframe.
		/// </summary>
		public Links links { get; set; }

		/// <summary>
		/// Contains any metadata you've provided.
		/// </summary>
		public string metadata { get; set; }
	}

	public class PaymentDetails
	{
		public string consumerName { get; set; }
		public string consumerAccount { get; set; }
		public string consumerBic { get; set; }

		public string cardHolder { get; set; }
		public string cardNumber { get; set; }
		public string cardSecurity { get; set; }

		public string bankName { get; set; }
		public string bankAccount { get; set; }
		public string bankBic { get; set; }
		public string transferReference { get; set; }

		public string bitcoinAddress { get; set; }
		public string bitcoinAmount { get; set; }
		public string bitcoinRate { get; set; }
		public string bitcoinUri { get; set; }

		public string paypalReference { get; set; }

		public string customerReference { get; set; }
	}

	/// <summary>
	/// iDeal issuer
	/// </summary>
	public class Issuer
	{
		public string id { get; set; }
		public string name { get; set; }
		public string method { get; set; }
	}

	/// <summary>
	/// Ideal issuer set
	/// </summary>
	public class Issuers
	{
		public int totalCount { get; set; }
		public int offset { get; set; }
		public int count { get; set; }
		public List<Issuer> data { get; set; }
	}	

	public class Links
	{
		public string paymentUrl { get; set; }
		public string redirectUrl { get; set; }
	}

	public class RefundStatus
	{
		public string id { get; set; }
		public PaymentStatus payment { get; set; }
		public decimal amountRefunded { get; set; }
		public decimal amountRemaining { get; set; }
		public DateTime? refundedDatetime { get; set; }
	}

    public class PaymentMethods
    {
        public int totalCount { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public List<PaymentMethod> data { get; set; }
    }

    public class PaymentMethodAmount
    {
        public decimal minimum { get; set; }
        public decimal maximum { get; set; }
    }

    public class PaymentMethodImage
    {
        public string normal { get; set; }
        public string bigger { get; set; }
    }

    public class PaymentMethod
    {
        public string id { get; set; }
        public string description { get; set; }
        public PaymentMethodAmount amount { get; set; }
        public PaymentMethodImage image { get; set; }
    }
	#endregion

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
			string jsonData = LoadWebRequest("POST", "payments", JsonConvert.SerializeObject(payment));
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
