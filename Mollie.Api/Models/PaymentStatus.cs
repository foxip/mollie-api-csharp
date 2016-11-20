using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models
{
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

        public decimal amountRefunded { get; set; }

        public decimal amountRemaining { get; set; }

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

        /// <summary>
        /// The consumer's locale, either forced on creation by specifying the locale parameter, or detected by us during checkout. 
        /// </summary>
        public string locale { get; set; }

        /// <summary>
        /// The identifier referring to the profile this payment was created on.
        /// </summary>
        public string profileId { get; set; }

        /// <summary>
        /// The identifier referring to the settlement this payment belongs to.
        /// </summary>
        public string settlementId { get; set; }

        /// <summary>
        /// If a customer ID was specified upon payment creation, the ID will be available here as well. 
        /// </summary>
        public string customerId { get; set; }

        /// <summary>
        /// First or recurring payment
        /// </summary>
        public RecurringType recurringType { get; set; }

        /// <summary>
        /// If the payment is a recurring payment, this field will hold the ID of the mandate used to authorize the recurring payment. 
        /// </summary>
        public string mandateId { get; set; }
    }
}