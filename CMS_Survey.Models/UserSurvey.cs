﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Models
{
    public class UserSurvey
    {
        public string surveyKey { get; set; }
        public string surveyNumber { get; set; }
        public string surveyType { get; set; }
        public string surveyProvider { get; set; }
        public string status { get; set; }
        public object endDate { get; set; }
        public object actionDate { get; set; }
        public object supervisorComments { get; set; }
        public object startDate { get; set; }
        public bool active { get; set; }
        public string endDateString { get; set; }
        public string actionDateString { get; set; }
        public string startDateString { get; set; }

        public List<long> otherSurveyerKeys { get; set; }
        private SCommand ecmd;
        public SCommand EditCommand
        {
            get
            {
                if (ecmd == null)
                    ecmd = new SCommand(this,"Edit");
                return ecmd;
            }
        }
        private SCommand vcmd;
        public SCommand ViewCommand
        {
            get
            {
                if (vcmd == null)
                    vcmd = new SCommand(this, "View");
                return vcmd;
            }
        }
        private SCommand Rvcmd;
        public SCommand ReViewCommand
        {
            get
            {
                if (Rvcmd == null)

                    Rvcmd = new SCommand(this, "Review");
                return Rvcmd;
            }
        }
        private SCommand VcCmd;
        public SCommand ViewCommentsCommand
        {
            get
            {
                if (VcCmd == null)

                    VcCmd = new SCommand(this, "ViewComments");
                return VcCmd;
            }
        }
        private SCommand Dcmd;
        public SCommand DeleteCommand
        {
            get
            {
                if (Dcmd == null)
                    Dcmd = new SCommand(this, "Delete");
                return Dcmd;
            }
        }
        private SCommand AdsCmd;
        public SCommand AddSurveyorCommand
        {
            get
            {
                if (AdsCmd == null)
                    AdsCmd = new SCommand(this, "AddSurveyor");
                return AdsCmd;
            }
        }
    }
}
