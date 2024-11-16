using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mollie.Api.Models.Customer
{
    public class GetCustomer
    {
        public string id { get; set; }
        /// <summary>
        /// live or test
        /// </summary>
        public string mode { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string locale { get; set; }
        public string metadata { get; set; }
        public DateTime createdAt { get; set; }
        public Links _links { get; set; } = new Links();


    }
}
