using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CMS_Survey.Database
{
    class CreateDatabase
    {
        public static void LoadDatabse(SQLiteConnection db)
        {
            //Creating answers table
            string answers_sql = @"CREATE TABLE IF NOT EXISTS
                                    answers (
                                        User_Surver_Access_Answers_Key INT      NOT NULL,
                                        Html_Control_Id                INT      NOT NULL,
                                        Answer                         TEXT     COLLATE NOCASE,
                                        Create_Date                    DATETIME NOT NULL,
                                        Create_User                    INT      NOT NULL,
                                        Modify_Date                    DATETIME DEFAULT NULL,
                                        Modify_User                    INT      DEFAULT NULL,
                                        PRIMARY KEY (
                                            User_Surver_Access_Answers_Key,
                                            Html_Control_Id
                                        ),
                                        CONSTRAINT Fk_User_Survey_Access_Answers_Key FOREIGN KEY (
                                            User_Surver_Access_Answers_Key
                                        )
                                        REFERENCES user_survey_access (User_Survey_Key) ON DELETE NO ACTION
                                                                                        ON UPDATE NO ACTION
                                    );";
            using (var statement = db.Prepare(answers_sql))
            {
                statement.Step();
            }

            //Creating providers_lu table
            string providers_lu_sql = @"CREATE TABLE IF NOT EXISTS
                                    providers_lu (
                                        Provider_Key      INTEGER       PRIMARY KEY AUTOINCREMENT
                                                                        NOT NULL,
                                        Region            VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        State             VARCHAR (2)   DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        CCN               VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        PRVDR_INTRNL_NUM  VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        FAC_INTRNL_ID     VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Facility_Name     VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Type_of_Ownership VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Street_Address    VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        City              VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Zip_Code          VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        County            VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Phone_Number      VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        State_Region      VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Certified_Beds    VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Provider_Type     VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Provider_Subtype  VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Deemed_Status     VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        Deeming_Agency    VARCHAR (255) DEFAULT NULL
                                                                        COLLATE NOCASE,
                                        CONSTRAINT providers_lu_ibfk_1 FOREIGN KEY (
                                            State
                                        )
                                        REFERENCES states_lu (State_Code) ON DELETE RESTRICT
                                                                          ON UPDATE RESTRICT
                                    );";
            using (var statement = db.Prepare(providers_lu_sql))
            {
                statement.Step();
            }

            //Creating states_lu table
            string states_lu_sql = @"CREATE TABLE IF NOT EXISTS
                                    states_lu (
                                        State_Code  VARCHAR (2)  PRIMARY KEY
                                                                 NOT NULL
                                                                 COLLATE NOCASE,
                                        State_Name  VARCHAR (45) NOT NULL
                                                                 COLLATE NOCASE,
                                        Create_Date DATETIME     NOT NULL,
                                        Create_User INT          NOT NULL,
                                        Modify_Date DATETIME     DEFAULT NULL,
                                        Modify_User INT          DEFAULT NULL
                                    );";
            using (var statement = db.Prepare(states_lu_sql))
            {
                statement.Step();
            }

            //Creating survey table
            string survey_sql = @"CREATE TABLE IF NOT EXISTS
                                    survey (
                                        Survey_Key          INTEGER  PRIMARY KEY AUTOINCREMENT
                                                                     NOT NULL,
                                        Survery_Type_Key    INT      NOT NULL,
                                        Action_Date         DATETIME NOT NULL,
                                        Next_Date           DATETIME DEFAULT NULL,
                                        Survey_Started_User INT      DEFAULT NULL,
                                        isActive                     NOT NULL
                                                                     DEFAULT '0',
                                        Create_Date         DATETIME NOT NULL,
                                        Create_User         INT      NOT NULL,
                                        Modify_Date         DATETIME DEFAULT NULL,
                                        Modify_User         INT      DEFAULT NULL,
                                        CONSTRAINT Fk_Survey_Started_User_Key FOREIGN KEY (
                                            Survey_Started_User
                                        )
                                        REFERENCES users (User_Key) ON DELETE NO ACTION
                                                                    ON UPDATE NO ACTION,
                                        CONSTRAINT Fk_Survey_Type_Key FOREIGN KEY (
                                            Survery_Type_Key
                                        )
                                        REFERENCES survey_type_lu (Survey_Type_Key) ON DELETE NO ACTION
                                                                                    ON UPDATE NO ACTION
                                    );";
            using (var statement = db.Prepare(survey_sql))
            {
                statement.Step();
            }

            //Creating survey_type_lu table
            string survey_type_lu_sql = @"CREATE TABLE IF NOT EXISTS
                                    survey_type_lu (
                                        Survey_Type_Key INTEGER       PRIMARY KEY AUTOINCREMENT
                                                                      NOT NULL,
                                        Survey_Num      VARCHAR (45)  NOT NULL
                                                                      COLLATE NOCASE,
                                        Survey_Name     VARCHAR (500) NOT NULL
                                                                      COLLATE NOCASE,
                                        Surver_Version  FLOAT (10, 0) NOT NULL,
                                        Create_Date     DATETIME      NOT NULL,
                                        Create_User     INT           NOT NULL,
                                        Modify_Date     DATETIME      DEFAULT NULL,
                                        Modify_User     INT           DEFAULT NULL
                                    );";
            using (var statement = db.Prepare(survey_type_lu_sql))
            {
                statement.Step();
            }

            //Creating user_survey_access_lu table
            string user_survey_access_sql = @"CREATE TABLE IF NOT EXISTS
                                    user_survey_access (
                                        User_Survey_Key INTEGER       PRIMARY KEY AUTOINCREMENT
                                                                      NOT NULL,
                                        User_Key        INT           NOT NULL,
                                        Survey_key      INT           NOT NULL,
                                        Status          VARCHAR (100) DEFAULT NULL
                                                                      COLLATE NOCASE,
                                        Create_Date     DATETIME      DEFAULT NULL,
                                        Create_User     INT           DEFAULT NULL,
                                        Modify_Date     DATETIME      DEFAULT NULL,
                                        Modify_User     INT           DEFAULT NULL,
                                        CONSTRAINT FK_Survey_Provider_key__Survey_Provider_State_Provider_Key FOREIGN KEY (
                                            Survey_key
                                        )
                                        REFERENCES survey (Survey_Key) ON DELETE NO ACTION
                                                                       ON UPDATE NO ACTION,
                                        CONSTRAINT Fk_User_Key___User_User_Key FOREIGN KEY (
                                            User_Key
                                        )
                                        REFERENCES users (User_Key) ON DELETE NO ACTION
                                                                    ON UPDATE NO ACTION
                                    );";
            using (var statement = db.Prepare(user_survey_access_sql))
            {
                statement.Step();
            }

            //Creating users_lu table
            string users_sql = @"CREATE TABLE IF NOT EXISTS
                                    users (
                                        User_Key   INTEGER       PRIMARY KEY    NOT NULL,
                                        UserId     VARCHAR (45)  NOT NULL
                                                                 COLLATE NOCASE,
                                        Password   VARCHAR (45)  NOT NULL
                                                                 COLLATE NOCASE,
                                        First_Name VARCHAR (45)  NOT NULL
                                                                 COLLATE NOCASE,
                                        Last_Name  VARCHAR (45)  NOT NULL
                                                                 COLLATE NOCASE,
                                        email      VARCHAR (150) NOT NULL
                                                                 COLLATE NOCASE,
                                        role       INT           NOT NULL,
                                        state      VARCHAR (3)   NOT NULL
                                                                 COLLATE NOCASE
                                    );";
            using (var statement = db.Prepare(users_sql))
            {
                statement.Step();
            }

            // Turn on Foreign Key constraints
            string sql = @"PRAGMA foreign_keys = ON";
            using (var statement = db.Prepare(sql))
            {
                statement.Step();
            }

        }

        public static void CopyDataBase()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var path = Path.Combine(folder.Path, "Surveydb.sqlite");
            if (File.Exists(path))
                return;
            var currentworkingDirectory = System.IO.Directory.GetCurrentDirectory();
            var destinationpath = Path.Combine(currentworkingDirectory, "Surveydb.sqlite");
            File.Copy(destinationpath, path);
        }
    }
}
