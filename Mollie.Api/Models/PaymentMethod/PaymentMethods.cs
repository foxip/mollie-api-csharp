using System.Collections.Generic;

namespace Mollie.Api.Models.PaymentMethod
{
    public class PaymentMethods
    {
        public List<PaymentMethod> methods { get; set; } = new List<PaymentMethod>();
    }
}