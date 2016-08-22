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
    public partial class StatesLu : ViewModelBase
    {
        #region Properties
        private string stateCode;
        public string StateCode
        {
            get { return stateCode; }
            set { if (SetProperty(ref stateCode, value)) IsDirty = true; }
        }
        private string stateName;
        public string StateName
        {
            get { return stateName; }
            set { if (SetProperty(ref stateName, value)) IsDirty = true; }
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
        private global::System.DateTime modifyDate;
        public global::System.DateTime ModifyDate
        {
            get { return modifyDate; }
            set { if (SetProperty(ref modifyDate, value)) IsDirty = true; }
        }
        private int modifyUser;
        public int ModifyUser
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

        #endregion "Properties"

        internal StatesLu(string stateCode)
        {
            this.StateCode = stateCode;
        }

        internal StatesLu(string stateCode, string stateName, global::System.DateTime createDate, int createUser, global::System.DateTime modifyDate, int modifyUser)
        {
            this.stateCode = stateCode;
            this.stateName = stateName;
            this.createDate = createDate;
            this.createUser = createUser;
            this.modifyDate = modifyDate;
            this.modifyUser = modifyUser;
        }
    }

}
