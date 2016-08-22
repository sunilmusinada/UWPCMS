using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CMS_Survey.Database;
namespace CMS_Survey.Models
{

    public partial class User : ViewModelBase
    {

        #region Properties
        public User CurrentUser = null;
        private Int64 userKey;
        public Int64 UserKey
        {
            get { return userKey; }
            set { if (SetProperty(ref userKey, value)) IsDirty = true; }
        }
        private string userId;
        public string UserId
        {
            get { return userId; }
            set { if (SetProperty(ref userId, value)) IsDirty = true; }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { if (SetProperty(ref password, value)) IsDirty = true; }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { if (SetProperty(ref firstName, value)) IsDirty = true; }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { if (SetProperty(ref lastName, value)) IsDirty = true; }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { if (SetProperty(ref email, value)) IsDirty = true; }
        }
        private Int64 role;
        public Int64 Role
        {
            get { return role; }
            set { if (SetProperty(ref role, value)) IsDirty = true; }
        }
        private string state;
        public string State
        {
            get { return state; }
            set { if (SetProperty(ref state, value)) IsDirty = true; }
        }
        private bool isDirty = false;
        public bool IsDirty
        {
            get { return isDirty; }
            set { SetProperty(ref isDirty, value); }
        }

        #endregion

        internal User(int userKey, string userId, string password, string firstName, string lastName, string email, int role, string state)
        {
            this.userKey = userKey;
            this.userId = userId;
            this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.role = role;
            this.state = state;
        }

        public User()
        {
        }
        public async Task InsertUser()
        {
            users_table usrTb = new Database.users_table();
            if(!usrTb.DoesUserExist(this.UserId))
            await usrTb.insertUser(this);
        }

        public void GetUser(string userId,string password)
        {
            users_table usrTb = new Database.users_table();
            CurrentUser = usrTb.getUser(userId, password);
        }
    }

}
