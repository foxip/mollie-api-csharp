using System;

namespace Mollie.Api.Models.Payment
{
    public class Address
    {
        public string title { get; set; }
        public string givenName { get; set; }
        public string familyName { get; set; }
        public string organizationName { get; set; }
        public string streetAndNumber { get; set; }
        public string streetAdditional { get; set; }
        public string postalCode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }
    }
}
