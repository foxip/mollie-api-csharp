using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models
{
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
        /// (Optional) If you pass a valid URL as this parameter, we will use this URL as the web hook instead of the web hook that is set in the web site profile.
        /// </summary>
        public string webhookUrl { get; set; }

        /// <summary>
        /// Allow you to preset the language to be used in the payment screens shown to the consumer. When this parameter is not provided, the browser language will be used instead (which is usually more accurate).
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
        /// (Optional) Date that the payment automatically expires. Format YYYY-MM-DD. For bank transfers.
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

        /// <summary>
        /// (Optional) Enables recurring payments. If set to first, a first payment for the customer is created, allowing the customer to agree to automatic recurring charges taking place on their account in the future. If set to recurring, the customer's card is charged automatically. 
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public RecurringType? recurringType { get; set; }

        /// <summary>
        /// (Optional) The ID of the customer for whom the payment is being created. 
        /// </summary>
        public string customerId { get; set; }

        /// <summary>
        /// (Optional) When creating recurring payments, a specific mandate ID may be supplied to indicate which of the consumer's accounts should be credited. 
        /// </summary>
        public string mandateId { get; set; }

        /// <summary>
        /// (Optional) Beneficiary name of the account holder. For SEPA direct debit. 
        /// </summary>
        public string consumerAccount { get; set; }

        /// <summary>
        /// (Optional) Beneficiary name of the account holder. For SEPA direct debit. 
        /// </summary>
        public string consumerName { get; set; }

    }
}