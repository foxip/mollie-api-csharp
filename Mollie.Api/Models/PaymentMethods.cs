using System.Collections.Generic;

namespace Mollie.Api.Models
{
    public class PaymentMethods : BaseList
    {
        public List<PaymentMethod> data { get; set; }
    }
}