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
    public partial class UserSurveyAccess : ViewModelBase
    {
        #region Properties

        private int userSurveyKey;
        public int UserSurveyKey
        {
            get { return userSurveyKey; }
            set { if (SetProperty(ref userSurveyKey, value)) IsDirty = true; }
        }
        private Int64 userKey;
        public Int64 UserKey
        {
            get { return userKey; }
            set { if (SetProperty(ref userKey, value)) IsDirty = true; }
        }
        private int surveyKey;
        public int SurveyKey
        {
            get { return surveyKey; }
            set { if (SetProperty(ref surveyKey, value)) IsDirty = true; }
        }
        private string status;
        public string Status
        {
            get { return status; }
            set { if (SetProperty(ref status, value)) IsDirty = true; }
        }
        private global::System.DateTime createDate;
        public global::System.DateTime CreateDate
        {
            get { return createDate; }
            set { if (SetProperty(ref createDate, value)) IsDirty = true; }
        }
        private Int64 createUser;
        public Int64 CreateUser
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
        private global::System.Nullable<Int64> modifyUser;
        public global::System.Nullable<Int64> ModifyUser
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

        internal UserSurveyAccess (int userSurveyKey, Int64 userKey, int surveyKey, string status, global::System.DateTime createDate, Int64 createUser, global::System.DateTime modifyDate, Int64 modifyUser)
        {
            this.userSurveyKey = userSurveyKey;
            this.userKey = userKey;
            this.surveyKey = surveyKey;
            this.status = status;
            this.createDate = createDate;
            this.createUser = createUser;
            this.modifyDate = modifyDate;
            this.modifyUser = modifyUser;
        }
    }

}
