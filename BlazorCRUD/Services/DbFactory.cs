using Microsoft.Data.SqlClient;
using System;

namespace BlazorCRUD.Services
{

    public interface IDbFactory
    {
        SqlConnection GetConnection();
    }

    public class DbFactory : IDbFactory
    {
        private readonly string _connectionString;
        public DbFactory() { }

        public DbFactory(string connectionString) 
        {
            this._connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
