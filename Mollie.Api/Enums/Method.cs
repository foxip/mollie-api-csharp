using System.Text.Json.Serialization;

namespace Mollie.Api.Enums
{
    /// <summary>
    /// Payment method
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Method
    {
        alma,
        applepay,
        bacs,
        bancomatpay,
        bancontact,
        banktransfer,
        belfius,
        blik,
        creditcard,
        directdebit,
        eps,
        giftcard,
        ideal,
        kbc,
        mybank,
        paypal,
        paysafecard,
        pointofsale,
        przelewy24,
        satispay,
        trustly,
        twint
    }
}



