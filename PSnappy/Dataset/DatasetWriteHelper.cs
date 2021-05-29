using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetWriteHelper
    {
        void Truncate(IDbConnection connection, string tablename);
        Task ExecuteStoredProcedureAsync(string server, string database, string storedProcedureName, int spCommandTimeout = 30);
        Task WriteAsync<T>(
            IEnumerable<T> items,
            Func<IEnumerable<T>, IDataReader> datareaderconverter,
            string server,
            string database,
            string destinationtable,
            int sqlTimeout = 180
        );
    }

    public class DatasetWriteHelper : IDatasetWriteHelper
    {
        private static readonly int _degreesofparallelism;

        static DatasetWriteHelper()
        {
            _degreesofparallelism = Environment.ProcessorCount;
            if (_degreesofparallelism > 8)
            {
                _degreesofparallelism = 8;
            }
        }

        protected readonly IStatusLogger _logger;

        public DatasetWriteHelper(IStatusLogger logger)
        {
            _logger = logger;
        }

        public void Truncate(IDbConnection connection, string tablename)
        {
            try
            {
                connection.TruncateTable(tablename);
            }
            catch (Exception ex)
            {
                _logger.LogStatus(string.Format("An error occurred while truncating the table {0}.", tablename), StatusType.Error);
                _logger.LogStatus(ex.ToString(), StatusType.Error);
            }
        }

        public async Task ExecuteStoredProcedureAsync(string server, string database, string storedProcedureName, int spCommandTimeout = 30)
        {
            using (var conn = SqlServerHelper.GetConnection(server, database))
            {
                conn.Open();
                var cmd = SqlServerHelper.GetSPCommand(storedProcedureName, null, spCommandTimeout);
                cmd.Connection = conn;
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task WriteAsync<T>(
            IEnumerable<T> items,
            Func<IEnumerable<T>, IDataReader> datareaderconverter,
            string server,
            string database,
            string destinationtable,
            int sqlTimeout = 180
        )
        {
            try
            {
                var partitions = Partition(items, _degreesofparallelism);

                var tasks = partitions.AsParallel()
                    .WithDegreeOfParallelism(_degreesofparallelism)
                    .Select(x => Task.Run(async () => await BulkInsertAsync(x, datareaderconverter, server, database, destinationtable, sqlTimeout)))
                    .ToArray();
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogStatus(string.Format("An error occurred while writing to the table {0}.", destinationtable), StatusType.Error);
                _logger.LogStatus(ex.ToString(), StatusType.Error);
            }
        }

        private static async Task BulkInsertAsync<T>(
            IEnumerable<T> items,
            Func<IEnumerable<T>, IDataReader> datareaderconverter,
            string server,
            string database,
            string destinationtable,
            int sqlTimeout = 180
        )
        {
            using (var conn = SqlServerHelper.GetConnection(server, database, sqlTimeout))
            {
                conn.Open();
                await conn.BulkCopyAsync(datareaderconverter(items), destinationtable);
            }
        }

        private static IEnumerable<IEnumerable<T>> Partition<T>(IEnumerable<T> items, int partitions)
        {
            var i = 0;
            return items.GroupBy(e => i++ % partitions);
        }
    }
}
