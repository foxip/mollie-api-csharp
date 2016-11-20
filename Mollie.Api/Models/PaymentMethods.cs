using System.Collections.Generic;

namespace Mollie.Api.Models
{
    public class PaymentMethods
    {
        public int totalCount { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public List<PaymentMethod> data { get; set; }
    }
}