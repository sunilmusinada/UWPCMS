using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CMS_Survey.Models
{
    public partial class SurveyTypeLu : ViewModelBase
    {
        #region Properties

        private int surveyTypeKey;
        public int SurveyTypeKey
        {
            get { return surveyTypeKey; }
            set { if (SetProperty(ref surveyTypeKey, value)) IsDirty = true; }
        }
        private string surveyNum;
        public string SurveyNum
        {
            get { return surveyNum; }
            set { if (SetProperty(ref surveyNum, value)) IsDirty = true; }
        }
        private string surveyName;
        public string SurveyName
        {
            get { return surveyName; }
            set { if (SetProperty(ref surveyName, value)) IsDirty = true; }
        }
        private decimal surverVersion;
        public decimal SurverVersion
        {
            get { return surverVersion; }
            set { if (SetProperty(ref surverVersion, value)) IsDirty = true; }
        }
        private global::System.DateTime createDate;
        public global::System.DateTime CreateDate
        {
            get { return createDate; }
            set { if (SetProperty(ref createDate, value)) IsDirty = true; }
        }
        private int createUser;
        public int CreateUser
        {
            get { return createUser; }
            set { if (SetProperty(ref createUser, value)) IsDirty = true; }
        }
        private global::System.Nullable<System.DateTime> modifyDate;
        public global::System.Nullable<System.DateTime> ModifyDate
        {
            get { return modifyDate; }
            set { if (SetProperty(ref modifyDate, value)) IsDirty = true; }
        }
        private global::System.Nullable<int> modifyUser;
        public global::System.Nullable<int> ModifyUser
        {
            get { return modifyUser; }
            set { if (SetProperty(ref modifyUser, value)) IsDirty = true; }
        }
        private bool isDirty = false;
        public bool IsDirty
        {
            get { return isDirty; }
            set { SetProperty(ref isDirty, value); }
        }

        #endregion

        internal SurveyTypeLu (int surveyTypeKey, string surveyNum, string surveyName, decimal surverVersion, global::System.DateTime createDate, int createUser, global::System.DateTime modifyDate, int modifyUser)
        {
            this.surveyTypeKey = surveyTypeKey;
            this.surveyNum = surveyNum;
            this.surveyName = surveyName;
            this.surverVersion = surverVersion;
            this.createDate = createDate;
            this.createUser = createUser;
            this.modifyDate = modifyDate;
            this.modifyUser = modifyUser;
        }
        internal SurveyTypeLu()
        { }
    }

}
