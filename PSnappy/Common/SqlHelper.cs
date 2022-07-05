using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PSnappy.Common
{
    public interface ISqlHelper
    {
        SqlConnection GetConnection(string server, string database, int timeout = 10);
        SqlConnection GetConnection(string connectionString, int timeout = 10);
        SqlConnection GetOpenConnection(string server, string database, int timeout = 10);
        SqlConnection GetOpenConnection(string connectionString, int timeout = 10);
        Task<SqlConnection> GetOpenConnectionAsync(string server, string database, int timeout = 10);
        Task<SqlConnection> GetOpenConnectionAsync(string connectionString, int timeout = 10);
        SqlCommand GetSPCommand(string sp, Dictionary<string, object> parameters = null, int timeout = 30);
        IDbCommand GetTextCommand(string text, int timeout = 30);
        IDbDataParameter CreateParameter(string name, DbType dbtype);
        IDbDataParameter CreateParameter(string name, DbType dbtype, object value);
        (string Server, string Database) GetServerAndDatabaseFromConnectionString(string connectionString);
    }

    public class SqlHelper : ISqlHelper
    {
        public SqlConnection GetConnection(string server, string database, int timeout = 10)
        {
            var builder = new SqlConnectionStringBuilder
            {
                MaxPoolSize = 10000,
                ConnectTimeout = timeout,
                IntegratedSecurity = true,
                DataSource = server,
                InitialCatalog = database
            };
#if DEBUG
            builder.ConnectTimeout = 180;
#endif
            var connection = new SqlConnection(builder.ToString());
            return connection;
        }

        public SqlConnection GetConnection(string connectionString, int timeout = 10)
        {
            var builder = new SqlConnectionStringBuilder
            {
                MaxPoolSize = 10000,
                ConnectTimeout = timeout,
                IntegratedSecurity = true,
                ConnectionString = connectionString
            };
#if DEBUG
            builder.ConnectTimeout = 180;
#endif
            var connection = new SqlConnection(builder.ToString());
            return connection;
        }

        public SqlConnection GetOpenConnection(string server, string database, int timeout = 10)
        {
            var c = GetConnection(server, database, timeout);
            c.Open();
            return c;
        }

        public SqlConnection GetOpenConnection(string connectionString, int timeout = 10)
        {
            var c = GetConnection(connectionString, timeout);
            c.Open();
            return c;
        }

        public async Task<SqlConnection> GetOpenConnectionAsync(string server, string database, int timeout = 10)
        {
            var c = GetConnection(server, database, timeout);
            await c.OpenAsync();
            return c;
        }

        public async Task<SqlConnection> GetOpenConnectionAsync(string connectionString, int timeout = 10)
        {
            var c = GetConnection(connectionString, timeout);
            await c.OpenAsync();
            return c;
        }

        public SqlCommand GetSPCommand(string sp, Dictionary<string, object> parameters = null, int timeout = 30)
        {
            var cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = sp,
                CommandTimeout = timeout
            };
            parameters?.ForEach(p => cmd.Parameters.Add(new SqlParameter(p.Key, p.Value)));
            return cmd;
        }

        public IDbCommand GetTextCommand(string text, int timeout = 30)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = text,
                CommandTimeout = timeout
            };
            return cmd;
        }

        public IDbDataParameter CreateParameter(string name, DbType dbtype)
        {
            return new SqlParameter(name, dbtype);
        }

        public IDbDataParameter CreateParameter(string name, DbType dbtype, object value)
        {
            SqlParameter p = new SqlParameter(name, dbtype)
            {
                Value = value
            };
            return p;
        }

        public (string Server, string Database) GetServerAndDatabaseFromConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            return (builder.DataSource, builder.InitialCatalog);
        }
    }
}
