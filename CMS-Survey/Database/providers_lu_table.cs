using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_Survey.Models;
namespace CMS_Survey.Database
{
    class providers_lu_table
    {
        SQLiteConnection db = App.conn;

        public void deleteAll()
        {
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            string sql = "DELETE FROM providers_lu";
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

        public List<ProvidersLu> GetProviderSForState(string StateCode)
        {
            string sql = string.Format(@"SELECT *
  FROM [providers_lu] where State='{}'", StateCode);
            List<ProvidersLu> providers = new List<ProvidersLu>();
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            
            using (var query = db.Prepare(sql))
            {
                while (query.Step().Equals(SQLiteResult.ROW))
                {
                    providers.Add(new ProvidersLu((Int32)query[0],
                        (string)query[1],
                        (string)query[2],
                        (string)query[3],
                         (string)query[4],
                        (string)query[5],
                        (string)query[6],
                         (string)query[7],
                        (string)query[8],
                        (string)query[9],
                         (string)query[10],
                        (string)query[11],
                        (string)query[12],
                         (string)query[13],
                        (string)query[14],
                        (string)query[15],
                         (string)query[16],
                        (string)query[17],
                        (string)query[18]
                        ));
                }
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
            return providers;
        }

        public List<Hospital> GetHospitalsForState(string StateCode)
        {
            string sql = string.Format(@"SELECT *
  FROM [providers_lu] where State='{0}'", StateCode);
            List<Hospital> providers = new List<Hospital>();
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }

            using (var query = db.Prepare(sql))
            {
                while (query.Step().Equals(SQLiteResult.ROW))
                {
                    var hospital = new Hospital();
                    hospital.providerKey = (Int64)query[0];
                    hospital.ccn = (string)query[3];
                    hospital.facilityName = (string)query[6];
                    providers.Add(hospital);
                }
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
            return providers;
        }

        public Hospital GetHospitalForProvCode(int Code)
        {
            string sql = string.Format(@"SELECT *
  FROM [providers_lu] where Provider_Key={0}", Code);
            Hospital hospital = new Hospital();
            using (var statement = db.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }

            using (var query = db.Prepare(sql))
            {
                if (query.Step().Equals(SQLiteResult.ROW))
                {
                
                    hospital.providerKey = (Int64)query[0];
                    hospital.ccn = (string)query[3];
                    hospital.facilityName = (string)query[6];
                   
                }
            }
            //COMMIT to accept all changes
            using (var statement = db.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
            return hospital;
        }

        public async Task BulkInsertProviders(List<Hospital> _hospitals)
        {
            if (_hospitals == null)
                return;
            try
            {
                long userId;
                using (var statement = db.Prepare(" BEGIN TRANSACTION"))
                {
                    statement.Step();
                }
                string insert_sql;
                foreach (Hospital hsp in _hospitals)
                {
                    
                    insert_sql = string.Format(@"INSERT INTO [providers_lu] ([Provider_Key]
           ,[CCN]
           ,[Facility_Name]
          )
     VALUES
           ({0}
           ,'{1}'
           ,""{2}""
            ); ", hsp.providerKey, hsp.ccn,hsp.facilityName);
                    
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
