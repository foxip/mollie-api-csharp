using System;
using System.Text.Json.Serialization;
using Mollie.Api.Enums;

namespace Mollie.Api.Models.Payment
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
        public DateTime? createdAt { get; set; }

        /// <summary>
        /// The current status of the payment.
        /// </summary>
        public Status? status { get; set; }

        /// <summary>
        /// Whether or not the payment can be cancelled.
        /// </summary>
        public bool? isCancelable { get; set; }

        /// <summary>
        /// The exact date and time the payment was marked as paid, in ISO-8601 format.
        /// </summary>
        public DateTime? paidAt { get; set; }

        /// <summary>
        /// The exact date and time the payment was cancelled, in ISO-8601 format.
        /// </summary>
        public DateTime? canceledAt { get; set; }

        /// <summary>
        /// The exact date and time the payment expired, in ISO-8601 format.
        /// </summary>
        public DateTime? expiredAt { get; set; }

        /// <summary>
        /// The date and time the payment failed, in ISO 8601 format. This parameter is omitted if the payment did not fail (yet).
        /// </summary>
        public DateTime? failedAt { get; set; }

        /// <summary>
        /// The amount you specified for this payment in Euro's.
        /// </summary>
        public Amount amount { get; set; }

        /// <summary>
        /// Only available when refunds are available for this payment – The total amount in EURO that is already refunded. For some payment methods, this amount may be higher than the payment amount, for example to allow reimbursement of the costs for a return shipment to the customer.
        /// </summary>
        public Amount amountRefunded { get; set; }

        /// <summary>
        /// Only available when refunds are available for this payment – The remaining amount in EURO that can be refunded.
        /// </summary>
        public Amount amountRemaining { get; set; }

        /// <summary>
        /// The description of the payment.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// The payment method that was ultimately used to complete this payment or null when no payment method was selected yet.
        /// </summary>
        public Method? method { get; set; }

        /// <summary>
        /// Any details the specific payment method provided after completing the payment. In case of iDEAL this will contain the bank account number (IBAN).
        /// </summary>
        public PaymentDetails details { get; set; }

        /// <summary>
        /// Contains various links relevant to the payment. The most important one is paymentUrl, after creating your payment you should send your buyer to this URL. Warning: you cannot use the paymentUrl in an iframe.
        /// </summary>
        public Links _links { get; set; }

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

        public SequenceType sequenceType { get; set; }

        /// <summary>
        /// If the payment is a recurring payment, this field will hold the ID of the mandate used to authorize the recurring payment.
        /// </summary>
        public string mandateId { get; set; }

        /// <summary>
        /// Only available for recurring payments – When implementing the Subscriptions API, any recurring charges resulting from the subscription will hold the ID of the subscription that triggered the payment.
        /// </summary>
        public string subscriptionId { get; set; }

        /// <summary>
        /// This optional field will contain the approximate amount that will be settled to your account, converted to the currency your account is settled in.
        /// </summary>
        public Amount settlementAmount { get; set; }


    }
}