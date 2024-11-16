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
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mollie.Api.Enums;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Refund;

namespace Mollie.Api
{
    public class MollieClient : IDisposable
    {
        // Endpoint of the remote API.
        const string API_ENDPOINT = "https://api.mollie.nl";

        // Version of the remote API.
        const string API_VERSION = "v2";

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
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull

            };

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(API_ENDPOINT + "/" + API_VERSION + "/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        #region Payments API
        public async Task<PaymentStatus> CreatePayment(CreatePayment payment)
        {
            return await SendRequest<PaymentStatus, CreatePayment>(HttpMethod.Post, "payments", payment)  ;
        }
        public async Task<PaymentStatus> GetPayment(string id)
        {
            return await GetRequest<PaymentStatus>($"payments/{id}");
        }
        public async Task<PaymentStatusesEmbedded> ListPayments()
        {
            return await GetRequest<PaymentStatusesEmbedded>("payments");
        }
        #endregion

        #region Refunds API
        public async Task<RefundStatus> CreateRefund(string id, Refund refund)
        {
            return await SendRequest<RefundStatus, Refund>(HttpMethod.Post, $"payments/{id}/refunds", refund);
        }
        public async Task<RefundsEmbedded> ListRefunds(string id)
        {
            return await GetRequest<RefundsEmbedded>($"payments/{id}/refunds");
        }
        #endregion

        #region Methods API
        public async Task<PaymentMethod> GetPaymentMethod(Method method)
        {
            return await GetRequest<PaymentMethod>($"methods/{method}?include=issuers");
        }
        public async Task<PaymentMethodsEmbedded> ListPaymentMethods()
        {
            return await GetRequest<PaymentMethodsEmbedded>("methods");
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
        public async Task<CustomersEmbedded> ListCustomers()
        {
            return await GetRequest<CustomersEmbedded>("customers");
        }
        #endregion

        #region Mandates API
        public async Task<MandatesEmbedded> ListMandates(string customerId)
        {
            return await GetRequest<MandatesEmbedded>($"customers/{customerId}/mandates");
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