namespace Mollie.Api.Models
{
    public class PaymentMethod
    {
        public string id { get; set; }
        public string description { get; set; }
        public PaymentMethodAmount amount { get; set; }
        public PaymentMethodImage image { get; set; }
    }
}