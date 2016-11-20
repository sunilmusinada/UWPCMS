using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Helpers
{
    internal class UserObservation
    {
        public long UserKey { get; set; }
        public string Answer { get; set; }

        public int questionID { get; set; }
        public int ObservationNumber { get; set; }
    }
}
