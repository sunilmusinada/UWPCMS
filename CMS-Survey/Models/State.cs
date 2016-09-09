using CMS_Survey.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Models
{
    public class State
    {
        private List<State> m_States;
        public List<State> States { get {

                states_lu_table stable = new states_lu_table();
                return stable.GetAllStates();
            }  }
        public string stateCode { get; set; }
        public string stateName { get; set; }

        
       
       
    }
}
