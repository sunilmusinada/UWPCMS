﻿using CMS_Survey.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CMS_Survey.Database
{
    class users_table
    {
        SQLiteConnection db = App.conn;

        public void deleteAll()
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = "DELETE FROM users";
            using (var users = db.Prepare(sql))
            {
                users.Step();
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
        }
        public void deleteUser(long userKey)
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = string.Format("DELETE FROM users where User_Key={}",userKey);
            using (var users = db.Prepare(sql))
            {
                users.Step();
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
        }
        public async Task UpdateUser(string userName,string Password,long UserKey)
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = string.Format(@" UPDATE[users] SET[UserId] = ""{0}"",[Password] = ""{1}"" WHERE[User_Key]={2}",userName,Password,UserKey);
            using (var users = db.Prepare(sql))
            {
                users.Step();
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
        }
        public async Task UpdateUserDetails(string firstName, string LastName,string email,string State, long UserKey)
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = string.Format(@" UPDATE[users] SET[First_Name] = ""{0}"",[Last_Name] = ""{1}"",[email] = ""{2}"",[state] = ""{3}"" WHERE[User_Key]={4}", firstName,LastName,email,State,UserKey);
            using (var users = db.Prepare(sql))
            {
                users.Step();
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
        }
        public async Task insertUser(User user)
        {

            try
            {
                using (var statement = db.Prepare(" BEGIN TRANSACTION"))
                {
                    statement.Step();
                }

                string insert_sql = string.Format(" INSERT INTO users (User_Key,UserId, Password, First_Name, Last_Name, email, role, state) VALUES({0}, '{1}', '{2}', '{3}', '{4}', '{5}', {6},'{7}')",user.userKey, user.userName, user.Password, user.FirstName, user.LastName, user.Email,user.Role, user.State);
                System.Diagnostics.Debug.WriteLine(insert_sql);
                using (var userinsert = db.Prepare(insert_sql))
                {
                    userinsert.Step();
                }
                //COMMIT to accept all changes
                using (var statement = db.Prepare("COMMIT TRANSACTION"))
                {
                    statement.Step();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public User getUser(string userId,string password)
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            User user = null;
            string get_sql =string.Format ("SELECT User_Key, UserId, Password, First_Name, Last_Name, email, role, state FROM users WHERE UserId = '{0}' and Password='{1}'",userId,password);
            
            using (var statement = db.Prepare(get_sql))
            {
               
                System.Diagnostics.Debug.WriteLine(get_sql);
                if (statement.Step().Equals(SQLiteResult.ROW))
                {
                    user = new User()
                    {
                        userKey = (Int64)statement[0],
                        userName = (string)statement[1],
                        Password = (string)statement[2],
                        FirstName = (string)statement[3],
                        LastName = (string)statement[4],
                        Email = (string)statement[5],
                        Role = (Int64)statement[6],
                        State = (string)statement[7]

                    };
                }
            }
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            return user;
        }

        public bool DoesUserExist(string userId,string password,out Int64 UserKey)

        {
            bool exists = false;
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            UserKey=-1;
            User user = null;
            //string get_sql = string.Format(@"SELECT * FROM[users] where userid = '{0}' & Password='{1}'",userId,password);
            string get_sql = string.Format(@"SELECT * FROM[users] where userid = '{0}' ", userId);
            using (var statement = db.Prepare(get_sql))
            {
                
                if (SQLiteResult.ROW == statement.Step())
                {
                    // var count = (int)statement[1];
                    if (statement.DataCount <= 0)
                    {
                        exists = false;
                        return exists;
                    }
                    else
                    {
                        exists = true;
                        UserKey = Convert.ToInt64(statement[0]);
                        if (string.IsNullOrEmpty(Convert.ToString(statement[2])))
                            exists = false;
                    }
                    
                   
                }
            }
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
            return exists;


         

        }
        public bool DoesUserExist(string userId, out Int64 UserKey)

        {
            bool exists = false;
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            UserKey = -1;
            User user = null;
            string get_sql = string.Format(@"SELECT * FROM[users] where userid = '{0}'", userId);
            using (var statement = db.Prepare(get_sql))
            {

                if (SQLiteResult.ROW == statement.Step())
                {
                    // var count = (int)statement[1];
                    if (statement.DataCount <= 0)
                    {
                        exists = false;
                        return exists;
                    }


                }
            }
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
            return exists;




        }

        public async Task BulkInsertUsers(List<User> users)
        {
            if (users == null)
                return;
            try
            {
                long userId;
                using (var statement = db.Prepare(" BEGIN TRANSACTION"))
                {
                    statement.Step();
                }
                string insert_sql;
                foreach (User user in users)
                {
                    if (DoesUserExist(user.userName, user.Password, out userId))
                    {
                        await UpdateUserDetails(user.FirstName, user.LastName, user.Email, user.State, userId);
                        continue;
                    }
                    if(DoesUserExist(user.userName,out userId))
                    deleteUser(user.userKey);
                    insert_sql = string.Format(" INSERT INTO users (User_Key,UserId, Password, First_Name, Last_Name, email, role, state) VALUES({0}, '{1}', '{2}', '{3}', '{4}', '{5}', {6},'{7}')", user.userKey, user.userName, user.Password, user.FirstName, user.LastName, user.Email, user.Role, user.State);
                    System.Diagnostics.Debug.WriteLine(insert_sql);
                    using (var userinsert = db.Prepare(insert_sql))
                {
                  
                    userinsert.Step();
                }
                }
                //COMMIT to accept all changes
                using (var statement = db.Prepare("COMMIT TRANSACTION"))
                {
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetUserMailsForState(string StateCode)
        {

            List<string> EmailIds = new List<string>();
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            
            string get_sql;
            if (StateCode=="ALL")
                get_sql = string.Format("SELECT email,First_Name FROM users");
            else
                get_sql = string.Format("SELECT email,First_Name FROM users WHERE state = '{0}'",StateCode);

            using (var statement = db.Prepare(get_sql))
            {

                System.Diagnostics.Debug.WriteLine(get_sql);
                while (statement.Step().Equals(SQLiteResult.ROW))
                {
                    var mail = string.Format("{0} ({1})", Convert.ToString(statement[1]), Convert.ToString(statement[0]));
                    EmailIds.Add(mail);

                }
            }
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            return EmailIds;
        }

        public async Task<List<string>> GetUsersForState(string StateCode)
        {
            

            List<string> userNames = new List<string>();
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            { 
                statement.Step();
            }

            string get_sql;
            if (StateCode == "ALL")
                get_sql = string.Format("SELECT First_Name,Last_Name FROM users");
            else
                get_sql = string.Format("SELECT First_Name,Last_Name FROM users WHERE state = '{0}'", StateCode);

            using (var statement = db.Prepare(get_sql))
            {

                System.Diagnostics.Debug.WriteLine(get_sql);
                while (statement.Step().Equals(SQLiteResult.ROW))
                {
                    var mail = string.Format("{0} {1}", Convert.ToString(statement[0]), Convert.ToString(statement[1]));
                    userNames.Add(mail);

                }
            }
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            return userNames;
        }

        public async Task<List<User>> GetFullUsersForState(string StateCode)
        {


            List<User> userNames = new List<User>();
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }

            string get_sql;
            if (StateCode == "ALL")
                get_sql = string.Format("SELECT * FROM users");
            else
                get_sql = string.Format("SELECT * FROM users WHERE state = '{0}'", StateCode);

            using (var statement = db.Prepare(get_sql))
            {

                //var usr = new User();
                System.Diagnostics.Debug.WriteLine(get_sql);
                while (statement.Step().Equals(SQLiteResult.ROW))
                {
                    var usr = new User();
                    usr.userKey = Convert.ToInt64(Convert.ToString(statement[0]));
                    usr.userName = Convert.ToString(statement[1]).Replace("."," ");
                    usr.FirstName= Convert.ToString(statement[3]);
                    usr.LastName= Convert.ToString(statement[4]); 
                    usr.Email= Convert.ToString(statement[5]);
                    usr.Role= Convert.ToInt64(statement[6]);
                    usr.State= Convert.ToString(statement[7]);

                    //var mail = string.Format("{0},{1}", Convert.ToString(statement[0]), Convert.ToString(statement[1]));
                    //userNames.Add(mail);
                    userNames.Add(usr);
                }
            }
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            return userNames;
        }
    }
}
