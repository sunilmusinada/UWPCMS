using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Models
{
    internal class Hospital
    {
        public Int64 providerKey { get; set; }
        public string ccn { get; set; }
        public string facilityName { get; set; }

        public string stateCode { get; set; }
    }
}
