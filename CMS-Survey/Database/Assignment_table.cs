﻿using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_Survey.Models;

namespace CMS_Survey.Database
{
    internal class Assignment_table
    {
        SQLiteConnection db = App.conn;
        public void deleteAssignment(long Id)
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = string.Format("DELETE FROM Assignment where Assignment_ID={}", Id);
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
        public async Task InsertAssignment(Assignment Assignment)
        {

            try
            {
                using (var statement = db.Prepare(" BEGIN TRANSACTION"))
                {
                    statement.Step();
                }

                string insert_sql = string.Format("INSERT INTO [Assignment] ([Assignment_ID],[Survey_Key],[User_Key],[EmailID])VALUES ({0},{1},{2},'{3}')",Assignment.Assignment_ID,Assignment.Survey_Key,Assignment.User_Key,Assignment.EmailID);
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
