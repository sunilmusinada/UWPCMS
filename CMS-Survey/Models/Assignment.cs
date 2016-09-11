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
   internal class Assignment:ViewModelBase
    {

        private Int64 assignment_ID;
        public Int64 Assignment_ID
        {
            get { return assignment_ID; }
            set { if (SetProperty(ref assignment_ID, value)) IsDirty = true; }
        }
        private Int64 survey_Key;
        public Int64 Survey_Key
        {
            get { return survey_Key; }
            set { if (SetProperty(ref survey_Key, value)) IsDirty = true; }
        }
        private Int64 user_Key;
        public Int64 User_Key
        {
            get { return user_Key; }
            set { if (SetProperty(ref user_Key, value)) IsDirty = true; }
        }

        private string emailId;
        public string EmailID
        {
            get { return emailId; }
            set { if (SetProperty(ref emailId, value)) IsDirty = true; }
        }

        private bool isDirty = false;
        public bool IsDirty
        {
            get { return isDirty; }
            set { SetProperty(ref isDirty, value); }
        }

        public void DeleteAssignments(long Assignment_ID)
        {
            Database.Assignment_table assignTable = new Database.Assignment_table();
            assignTable.deleteAssignment(Assignment_ID);
        }
        public List<Assignment> GetAssignments()
        {
            Database.Assignment_table assignTable = new Database.Assignment_table();
           return assignTable.GetAssignments();

        }

        public async void InsertAssignment(Assignment assignment)
        {
            Database.Assignment_table assignTable = new Database.Assignment_table();
            await assignTable.InsertAssignment(assignment);
        }

    }
}
