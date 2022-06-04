using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace PSnappy
{
    public static class SqlExtensions
    {
        public static IDbCommand GetTextCommand(string text, int timeout = 30)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = text,
                CommandTimeout = timeout
            };
            return cmd;
        }

        public static void TruncateTable(this IDbConnection connection, string tablename)
        {
            var cmd = GetTextCommand(string.Format("TRUNCATE TABLE {0}", tablename));
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
