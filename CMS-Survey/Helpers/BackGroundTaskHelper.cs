using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveySyncComponent;
using Windows.ApplicationModel.Background;

namespace CMS_Survey.Template
{
   internal class BackGroundTaskHelper
    {
        private bool taskRegistered=false;
        //public bool TaskRegistered = false;
        public bool TaskRegistered
        {
            get
            {
                foreach (var task in Windows.ApplicationModel.Background.BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == Constants.TaskName)
                    {
                        tempTask =task.Value ;
                        taskRegistered = true;
                        break;
                    }
                }
                return taskRegistered;
            }
        }
        BackgroundTaskRegistration SyncTask;
        IBackgroundTaskRegistration tempTask;
        public void RegisterTask()
        {
            BackgroundTaskBuilder builder = CreateBuilder();
            SyncTask = builder.Register();
           
        }

        private static BackgroundTaskBuilder CreateBuilder()
        {
            var builder = new BackgroundTaskBuilder();

            builder.Name = Constants.TaskName;
            builder.TaskEntryPoint = "SurveySyncComponent.SyncSurveyBackgroundTask";
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.InternetAvailable, false));
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            return builder;
        }

        public void Unregister()
        {
            tempTask.Unregister(true);
        }

    }
}
