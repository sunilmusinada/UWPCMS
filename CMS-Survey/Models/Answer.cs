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
    public partial class Answer : ViewModelBase
    {
        #region Properties

        private int userSurverAccessAnswersKey;
        public int UserSurverAccessAnswersKey
        {
            get { return userSurverAccessAnswersKey; }
            set { if (SetProperty(ref userSurverAccessAnswersKey, value)) IsDirty = true; }
        }
        private int htmlControlId;
        public int HtmlControlId
        {
            get { return htmlControlId; }
            set { if (SetProperty(ref htmlControlId, value)) IsDirty = true; }
        }
        private string answer1;
        public string Answer1
        {
            get { return answer1; }
            set { if (SetProperty(ref answer1, value)) IsDirty = true; }
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

        internal Answer (int userSurverAccessAnswersKey,string Answer,int htmlControlId, global::System.DateTime createDate, Int64 createUser, global::System.DateTime modifyDate, Int64 modifyUser)
        {
            this.userSurverAccessAnswersKey = userSurverAccessAnswersKey;
            this.answer1 = Answer;
            this.htmlControlId = htmlControlId;
            this.createDate = createDate;
            this.createUser = createUser;
            this.modifyDate = modifyDate;
            this.modifyUser = modifyUser;
        }
    }

}
