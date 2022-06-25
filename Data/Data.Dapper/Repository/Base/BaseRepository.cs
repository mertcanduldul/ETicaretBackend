using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Dapper.Repository.Base
{
    public class BaseRepository
    {
        internal readonly string _connectionString;

        internal IDbConnection _connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }


        public BaseRepository()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigReader.GetConnectionStringsValue("DbConnectionString"));
            var DbPassword = ConfigReader.GetConnectionStringsValue("DbPassword");
            builder.Password = DbPassword;
            _connectionString = builder.ConnectionString;
        }
    }
}

