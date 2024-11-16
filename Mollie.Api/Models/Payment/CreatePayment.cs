using Mollie.Api.Enums;

namespace Mollie.Api.Models.Payment
{
    public class CreatePayment
    {

        /// <summary>
        /// (Required) The exact amount you want to charge your buyer in Euro's. If you want to charge � 99,95 provide 99.95 as the value.
        /// </summary>
        public Amount amount { get; set; }

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
        public Method? method { get; set; }

        /// <summary>
        /// (Optional) Use this parameter to store your own custom parameters with the payment. When you retrieve the payment details, you'll get these custom parameters back as well. You can provide around 1 KB of data.
        /// </summary>
        public string metadata { get; set; }

        /// <summary>
        /// (Optional) The webhook URL where we will send payment status updates to. The webhookUrl is optional, but without a webhook you will miss out on important status changes to your payment.
        /// </summary>
        public string webhookUrl { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to when the customer explicitly cancels the payment. If this URL is not provided, the customer will be redirected to the redirectUrl instead.
        /// </summary>
        public string cancelUrl { get; set; } = null;

        /// <summary>
        /// Allow you to preset the language to be used in the payment screens shown to the consumer. When this parameter is not provided, the browser language will be used instead (which is usually more accurate).
        /// Valid values: nl, fr, de, en, es
        /// </summary>
        public string locale { get; set; }

        //(Optional) The customer's billing address details. We advise to provide these details to improve fraud protection and conversion. This is particularly relevant for card payments.
        public Address billingAddress { get; set; }

        //(Optional) The customer's shipping address details. We advise to provide these details to improve fraud protection and conversion. This is particularly relevant for card payments.
        public Address shippingAddress { get; set; }


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
        public SequenceType? sequenceType { get; set; }

        /// <summary>
        /// (Optional) The ID of the customer for whom the payment is being created.
        /// </summary>
        public string customerId { get; set; }

        /// <summary>
        /// (Optional) When creating recurring payments, a specific mandate ID may be supplied to indicate which of the consumer's accounts should be credited.
        /// </summary>
        public string mandateId { get; set; }
    }
}