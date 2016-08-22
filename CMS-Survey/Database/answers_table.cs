using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Database
{
    class answers_table
    {
        SQLiteConnection db = App.conn;

        public void deleteAll()
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = "DELETE FROM answers";
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
    }
}
