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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mollie.Api.Models;

namespace Mollie.Api
{
    public class MollieClient : IDisposable
    {
        // Endpoint of the remote API.
        const string API_ENDPOINT = "https://api.mollie.nl";

        // Version of the remote API.
        const string API_VERSION = "v1";

        private readonly string _apiKey;

        private JsonSerializerOptions _options;
        private HttpClient _httpClient;

        /// <param name="apiKey">The Mollie API key, starting with 'test_' or 'live_'</param>
        public MollieClient(string apiKey)
        {
            _apiKey = apiKey.Trim();

            if ((apiKey == null) || (!Regex.IsMatch(apiKey, "^(live|test)_\\w+$")))
            {
                throw new ArgumentException($"Invalid API key: {apiKey}. An API key must start with 'test_' or 'live_'.");
            }
            
            _options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(API_ENDPOINT + "/" + API_VERSION + "/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        #region Issuers API
        /// <summary>
        /// Retrieve an issuer object by specifying an issuer identifier.
        /// </summary>
        /// <returns></returns>
        public async Task<Issuer> GetIssuer(string id)
        {
            return await GetRequest<Issuer>($"issuers/{id}");
        }

        /// <summary>
        /// To integrate the iDeal issuer selection step on your own web site.
        /// </summary>
        /// <returns></returns>
        public async Task<Issuers> ListIssuers()
        {
            return await GetRequest<Issuers>("issuers");
        }
        #endregion

        #region Payments API

        /// <summary>
        /// Create a payment
        /// </summary>
        public async Task<PaymentStatus> CreatePayment(Payment payment)
        {
            return await SendRequest<PaymentStatus, Payment>(HttpMethod.Post, "payments", payment);
        }

        /// <summary>
        /// Get status of a payment
        /// </summary>
        /// <param name="id">The id of the payment</param>
        public async Task<PaymentStatus> GetPayment(string id)
        {
            return await GetRequest<PaymentStatus>($"payments/{id}");
        }

        /// <summary>
        /// Get list of a payments
        /// </summary>
        public async Task<Payments> GetPayments()
        {
            return await GetRequest<Payments>("payments");
        }

        /// <summary>
        /// To refund a payment, you must have sufficient balance with Mollie for deducting the refund and its fees. You can find your current balance on the on the Mollie controlpanel.
        /// At the moment you can only process refunds for iDEAL, Bancontact/Mister Cash, SOFORT Banking, creditcard and bank transfer payments.
        /// </summary>
        /// <param name="id">The id of the payment</param>
        public async Task<RefundStatus> CreateRefund(string id, Refund refund)
        {
            return await SendRequest<RefundStatus, Refund>(HttpMethod.Post, $"payments/{id}/refunds", refund);
        }

        /// <summary>
        /// Retrieve all refunds for the given payment.
        /// </summary>
        /// <param name="id">The id of the payment</param>
        /// <returns></returns>
        public async Task<Refunds> ListRefunds(string id)
        {
            return await GetRequest<Refunds>($"payments/{id}/refunds");
        }

        #endregion

        #region Methods API
        /// <summary>
        /// Fetch all payment methods for your profile
        /// </summary>
        public async Task<PaymentMethods> GetPaymentMethods()
        {
            return await GetRequest<PaymentMethods>("methods");
        }
        #endregion

        #region Customers API
        public async Task<GetCustomer> CreateCustomer(CreateCustomer customer)
        {
            return await SendRequest<GetCustomer, CreateCustomer>(HttpMethod.Post, "customers", customer);
        }

        public async Task<GetCustomer> GetCustomer(string id)
        {
            return await GetRequest<GetCustomer>($"customers/{id}");
        }
        public async Task<Customers> ListCustomers()
        {
            return await GetRequest<Customers>("customers");
        }
        #endregion

        #region Mandates API
        public async Task<Mandates> ListMandates(string customerId)
        {
            return await GetRequest<Mandates>($"customers/{customerId}/mandates");
        }
        #endregion
                
        private async Task<T> SendRequest<T,D>(HttpMethod httpMethod, string resource, D data)
        {
            HttpResponseMessage response;

            using (var request = new HttpRequestMessage(httpMethod, resource))
            {
                request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                response = await _httpClient.SendAsync(request);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonSerializer.Deserialize<T>(responseString, _options);
        }

        private async Task<T> GetRequest<T>(string resource)
        {
            HttpResponseMessage response;

            using (var request = new HttpRequestMessage(HttpMethod.Get, resource))
            {
                response = await _httpClient.SendAsync(request);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return JsonSerializer.Deserialize<T>(responseString, _options);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}