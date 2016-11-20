using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mollie.Api.Models
{
    public class GetCustomer
    {
        public string resource { get; set; }
        public string id { get; set; }
        /// <summary>
        /// live or test
        /// </summary>
        public string mode { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string locale { get; set; }
        public string metadata { get; set; }
        public string[] recentlyUsedMethods { get; set; }
        public DateTime createdDatetime { get; set; }

    }
}
