using System;

namespace Mollie.Api.Models
{
    public class Refund
    {
        public decimal amount { get; set; }
        public string description { get; set; }
    }
}