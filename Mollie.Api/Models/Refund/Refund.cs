using System;

namespace Mollie.Api.Models.Refund
{
    public class Refund
    {
        public Amount amount { get; set; }
        public string description { get; set; }
        public string metadata { get; set; }
    }
}