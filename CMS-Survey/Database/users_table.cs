using CMS_Survey.Models;
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

        public bool DoesUserExist(string userId)

        {
            bool exists = false;
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            User user = null;
            string get_sql = string.Format(@"SELECT * FROM[users] where userid = '{0}'",userId);
            using (var statement = db.Prepare(get_sql))
            {
                
                if (SQLiteResult.ROW == statement.Step())
                {
                   // var count = (int)statement[1];
                    if (statement.DataCount > 0)
                        exists = true;
                   
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
                using (var statement = db.Prepare(" BEGIN TRANSACTION"))
                {
                    statement.Step();
                }
                string insert_sql;
                foreach (User user in users)
                {


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
    }
}
