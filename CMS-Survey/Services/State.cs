using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Storage;
using System.IO;
using System.Net;

namespace CMS_Survey.Services
{
    internal class State
    {

        //public Dictionary<string,string> StateCodeDictionary { get { }; set; }
        #region MembersandConstructor
        private static State _state;
        public static State StateObject
        {
            get
            {
                if (_state == null)
                    _state = new State();

                return _state;
            }
        }

        internal State()
        {
            GetStates();
        }
        private List<Models.State> _StateCode;
        private IStorageFile textFile { get; set; }

        public List<Models.State> StateCode
        {
            get { return _StateCode; }
            set { _StateCode = value; }
        }
        #endregion




        private List<Models.State> GetStates()
        {
            var folder = ApplicationData.Current.LocalFolder;
            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "States.json");
            if(!File.Exists(FilePath))
            {
                CallStateService();
            }
            if(StateCode == null)
            {
                StateCode = new List<Models.State>();
                CreateStateDictionary();
                return StateCode;
            }
            else
            {
                return StateCode;
            }

        }

        private async void CallStateService()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri("http://cms-specialtysurveys-internal.org/SurveyRest/rest/myresource/states"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var folder = ApplicationData.Current.LocalFolder;
                string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "States.json");
                textFile = (IStorageFile)null;
                if (!File.Exists(FilePath))
                {
                    textFile =  await folder.CreateFileAsync("States.json");
                }
                else
                {
                    textFile = await folder.GetFileAsync("States.json");
                }
                await FileIO.WriteTextAsync(textFile, jsonString);
            }
        }

        private void CreateStateDictionary()
        {
            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "States.json");
            try
            {
                using (StreamReader file = File.OpenText(FilePath))
                {
                    var json = file.ReadToEnd();
                    StateCode = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.State>>(json);
                    // System.Diagnostics.Debug.WriteLine(result);
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        public string GetCodeforState(string StateName)
        {
            var Code=StateCode.Where(e => e.stateName.ToUpper().Equals(StateName.ToUpper())).Select(e => e.stateCode).FirstOrDefault();
            return Code;
        }
    }
}
