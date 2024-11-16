using System.Text.Json.Serialization;

namespace Mollie.Api.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MandateStatus
    {
        valid,
        pending,
        invalid

    }
}