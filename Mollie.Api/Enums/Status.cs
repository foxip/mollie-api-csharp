using System.Text.Json.Serialization;

namespace Mollie.Api.Enums
{
    /// <summary>
    /// Payment status
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        /// <summary>
        /// The payment has been created, but no other status has been reached yet.
        /// </summary>
        open,

        /// <summary>
        /// Your customer has canceled the payment.
        /// </summary>
        canceled,

        /// <summary>
        /// The payment has been started but not yet complete.
        /// </summary>
        pending,

        /// <summary>
        /// The payment has been paid for.
        /// </summary>
        paid,

        /// <summary>
        /// The payment has expired, for example, your customer has closed the payment screen.
        /// </summary>
        expired,

        /// <summary>
        /// The payment has failed and can not be finished with another payment method.
        /// </summary>
        failed,

        /// <summary>
        /// If the payment method supports captures, the payment method will have this status for as long as new captures can be created.
        /// Currently this status is only possible for the payment methods Cards, Klarna Pay Now , Klarna Pay Later, Klarna Slice it, Billie, and Riverty.
        /// </summary>
        authorized
    }
}