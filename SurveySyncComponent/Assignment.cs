using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveySyncComponent
{
   internal class Assignment
    {

        public string EmailID { get; set; }
        public long Survey_Key { get; set; }
        public long User_Key { get; set; }
        public long Assignment_ID { get; set; }
    }
}
