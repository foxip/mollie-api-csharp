using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mollie.Api.Models.Customer
{
    public class CreateCustomer
    {
        public string name { get; set; }
        public string email { get; set; }
        public string locale { get; set; }
        public string metadata { get; set; }


    }
}
