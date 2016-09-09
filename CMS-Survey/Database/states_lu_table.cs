using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_Survey.Models;
namespace CMS_Survey.Database
{
    class states_lu_table
    {
        SQLiteConnection db = App.conn;

        

        public void deleteAll()
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = "DELETE FROM states_lu";
            using (var query = db.Prepare(sql))
            {
                query.Step();
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
        }
        public List<State> GetAllStates()
        {
            List<State> States = new List<State>();
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = "Select * FROM states_lu";
            using (var query = db.Prepare(sql))
            {
                while (query.Step().Equals(SQLiteResult.ROW))
                {
                    State st = new Models.State();
                    st.stateCode = (string)query[0];
                    st.stateName = (string)query[1];
                    States.Add(st);
                }
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();

            }
            return States;
        }

    }
}
