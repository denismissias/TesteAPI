using API.Model;
using Dapper;
using System;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace API.Data
{
    public class APIData
    {
        public static String DbFile
        {
            get
            {
                return ConfigurationManager.AppSettings["DbFile"].ToString();
            }
        }

        public static SQLiteConnection DbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }

        public String SaveAPI(APIModel model)
        {
            if (!File.Exists(DbFile))
            {
                CreateDataBase();
            }

            using (SQLiteConnection connection = DbConnection())
            {
                try
                {
                    connection.Open();

                    connection.Query<String>(@"INSERT INTO Api (Id, Url, CreateDate) Values (@Id, @Url, @CreateDate)", model);

                    return connection.ResultCode().ToString();
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        public APIModel GetById(String Id)
        {
            if (!File.Exists(DbFile))
            {
                return null;
            }

            using (SQLiteConnection connection = DbConnection())
            {
                try
                {
                    connection.Open();

                    APIModel model = connection.Query<APIModel>(@"SELECT Url, CreateDate FROM Api WHERE Id = @Id", new { Id }).FirstOrDefault();

                    if (connection.ResultCode().Equals(SQLiteErrorCode.Ok) && model != null)
                    {
                        return model;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        private void CreateDataBase()
        {
            using (SQLiteConnection connection = DbConnection())
            {
                connection.Open();

                connection.Execute(@"CREATE TABLE Api (Id TEXT identity primary key, Url TEXT not null, CreateDate DATETIME not null)");
            }
        }
    }
}