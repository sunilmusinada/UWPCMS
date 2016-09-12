using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SurveySyncComponent
{
    internal class Assignment_table
    {
        SQLiteConnection db ;
        internal Assignment_table(SQLiteConnection conn)
        {
            db = conn;

        }
      
        public void deleteAssignment(long Id)
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = string.Format("DELETE FROM Assignment where Assignment_ID={0}", Id);
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
        public void InsertAssignment(Assignment Assignment)
        {

            try
            {
                using (var statement = db.Prepare(" BEGIN TRANSACTION"))
                {
                    statement.Step();
                }
                string mail = Assignment.EmailID.Substring(Assignment.EmailID.IndexOf('(') + 1).Replace(")", "");
                string insert_sql = string.Format("INSERT INTO [Assignment] ([Survey_Key],[User_Key],[EmailID])VALUES ({0},{1},'{2}')",Assignment.Survey_Key, Assignment.User_Key, mail);
                //System.Diagnostics.Debug.WriteLine(insert_sql);
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
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public List<Assignment> GetAssignments()
        {
            List<Assignment> Assignments;
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            Assignment Assign = null;
            string get_sql = string.Format("SELECT * FROM Assignment");

            using (var statement = db.Prepare(get_sql))
            {
                Assignments = new List<Assignment>();
                System.Diagnostics.Debug.WriteLine(get_sql);
                while (statement.Step().Equals(SQLiteResult.ROW))
                {
                    Assign = new Assignment()
                    {
                        Assignment_ID=Convert.ToInt64(statement[0]),
                        Survey_Key=Convert.ToInt64(statement[1]),
                        User_Key=Convert.ToInt64(statement[2]),
                        EmailID=Convert.ToString(statement[3])

                    };
                    Assignments.Add(Assign);
                }
            }
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            return Assignments;
        }

    }
}
