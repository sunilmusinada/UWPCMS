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

    public partial class Survey : ViewModelBase
    {
        #region Properties
        private int surveyKey;
        public int SurveyKey
        {
            get { return surveyKey; }
            set { if (SetProperty(ref surveyKey, value)) IsDirty = true; }
        }
        private int surveryTypeKey;
        public int SurveryTypeKey
        {
            get { return surveryTypeKey; }
            set { if (SetProperty(ref surveryTypeKey, value)) IsDirty = true; }
        }
        private global::System.DateTime actionDate;
        public global::System.DateTime ActionDate
        {
            get { return actionDate; }
            set { if (SetProperty(ref actionDate, value)) IsDirty = true; }
        }
        private global::System.Nullable<System.DateTime> nextDate;
        public global::System.Nullable<System.DateTime> NextDate
        {
            get { return nextDate; }
            set { if (SetProperty(ref nextDate, value)) IsDirty = true; }
        }
        private global::System.Nullable<int> surveyStartedUser;
        public global::System.Nullable<int> SurveyStartedUser
        {
            get { return surveyStartedUser; }
            set { if (SetProperty(ref surveyStartedUser, value)) IsDirty = true; }
        }
        private bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { if (SetProperty(ref isActive, value)) IsDirty = true; }
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

        internal Survey(int surveyKey, int surveryTypeKey, global::System.DateTime actionDate, bool isActive, global::System.DateTime createDate, Int64 createUser, global::System.DateTime modifyDate, Int64 modifyUser)
        {
            this.surveyKey = surveyKey;
            this.surveryTypeKey = surveryTypeKey;
            this.actionDate = actionDate;
            this.isActive = isActive;
            this.createDate = createDate;
            this.createUser = createUser;
            this.modifyDate = modifyDate;
            this.modifyUser = modifyUser;
        }
    }

}
