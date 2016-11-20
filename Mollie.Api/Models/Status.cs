namespace Mollie.Api.Models
{
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
}