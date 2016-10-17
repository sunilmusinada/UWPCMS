using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveySyncComponent
{
    internal class Surveyors
    {
        public long? surveyKey { get; set; }
        public List<long> userKeys { get; set; }
    }
}
