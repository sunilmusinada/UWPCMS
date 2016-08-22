using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Storage;
using System.IO;
using System.Net;
using CMS_Survey.Models;
namespace CMS_Survey.Services
{
    internal class HospitalName
    {

        #region Members and Contructors
        private static HospitalName _hospitalName;

        internal static HospitalName _HospitalName
        {
            get
            {
                if (_hospitalName == null)
                    _hospitalName = new HospitalName();
                return _hospitalName;
            }
            set
            {
                _hospitalName = value;
            }
        }

       

        internal List<Hospital> Hospitals;
       

        internal HospitalName()
        {

        }  
        #endregion

        internal async Task<List<Hospital>> GetHospitalsForState(string StateCode)
        {
            //await CallHospitalService(StateCode);
            var client = new HttpClient();
            Hospitals = new List<Hospital>();
            HttpResponseMessage response = await client.GetAsync(new Uri(string.Format("http://cms-specialtysurveys-internal.org/SurveyRest/rest/myresource/providers?state={0}", StateCode)));
          
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                Hospitals = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hospital>>(jsonString);
            //    await FileIO.WriteTextAsync(textFile, jsonString);
            }
            return Hospitals;
             //return Hospitals;
        }

       
    }
}
