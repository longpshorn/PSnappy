using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PSnappy
{
    public static class SqlServerHelper
    {
        public static SqlConnection GetConnection(string server, string database, int timeout = 10)
        {
            var csb = new SqlConnectionStringBuilder();
            csb.MaxPoolSize = 10000;
            csb.ConnectTimeout = timeout;
#if DEBUG
            csb.ConnectTimeout = 180;
#endif
            csb.IntegratedSecurity = true;
            csb.DataSource = server;
            csb.InitialCatalog = database;
            return new SqlConnection(csb.ConnectionString);
        }

        public static SqlCommand GetSPCommand(string sp, Dictionary<string, object> parameters = null, int timeout = 30)
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sp;
            cmd.CommandTimeout = timeout;
            parameters?.ForEach(p => cmd.Parameters.Add(new SqlParameter(p.Key, p.Value)));
            return cmd;
        }

        public static IDbCommand GetTextCommand(string text, int timeout = 30)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = text;
            cmd.CommandTimeout = timeout;
            return cmd;
        }

        public static IDbDataParameter CreateParameter(string name, DbType dbtype)
        {
            return new SqlParameter(name, dbtype);
        }

        public static IDbDataParameter CreateParameter(string name, DbType dbtype, object value)
        {
            SqlParameter p = new SqlParameter(name, dbtype);
            p.Value = value;
            return p;
        }

        public static async Task<IDataReader> GetDataReaderAsync(this SqlCommand command)
        {
            IDataReader reader = await command.ExecuteReaderAsync();
            return reader.ToSafeDataReader();
        }

        public static Dictionary<string, int> GetOrdinalDictionary(this IDataReader reader)
        {
            var dic = new Dictionary<string, int>();
            var count = reader.FieldCount;
            for (int i = 0; i < count; i++)
            {
                dic.Add(reader.GetName(i), i);
            }
            return dic;
        }

        public static void TruncateTable(this IDbConnection connection, string tablename)
        {
            var cmd = SqlServerHelper.GetTextCommand(string.Format("TRUNCATE TABLE {0}", tablename));
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
        }

        public static async Task BulkCopyAsync(this SqlConnection connection, IDataReader reader, string destinationtable, int batchSize = 10000, int timeout = 30, bool useTabLock = true, SqlTransaction transaction = null)
        {
            var options = useTabLock ? SqlBulkCopyOptions.TableLock : SqlBulkCopyOptions.Default;

            SqlBulkCopy bc = new SqlBulkCopy(connection as SqlConnection, options, transaction);
            try
            {
                var d = reader.GetSchemaTable();
                if (d != null)
                {
                    var columns = d.Columns;
                    foreach (DataColumn column in columns)
                    {
                        bc.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                }
                bc.BulkCopyTimeout = timeout;
                bc.DestinationTableName = destinationtable;
                bc.BatchSize = batchSize;
                await bc.WriteToServerAsync(reader);
            }
            finally
            {
                if (bc != null) bc.Close();
            }
        }
    }
}
