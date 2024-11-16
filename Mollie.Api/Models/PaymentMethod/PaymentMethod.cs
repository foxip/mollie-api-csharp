using System.Collections.Generic;

namespace Mollie.Api.Models.PaymentMethod
{
    public class PaymentMethod
    {
        public string id { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public Amount minimumAmount { get; set; }
        public Amount maximumAmount { get; set; }
        public PaymentMethodImage image { get; set; }
        public List<Issuer> issuers { get; set; } = new List<Issuer>();
    }
}