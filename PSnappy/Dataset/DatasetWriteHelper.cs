using PSnappy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetWriteHelper
    {
        Task TruncateAsync(IDbConnection connection, string tablename);
        Task ExecuteStoredProcedureAsync(string connectionString, string storedProcedureName, int spCommandTimeout = 30);
        Task WriteAsync<T>(
            IEnumerable<T> items,
            Func<IEnumerable<T>, IDataReader> datareaderconverter,
            string connectionString,
            string destinationtable,
            int sqlTimeout = 180
        );
    }

    public class DatasetWriteHelper : IDatasetWriteHelper
    {
        private readonly int _degreesofparallelism;

        private readonly ISqlHelper _sqlHelper;
        protected readonly IStatusLogger _logger;

        public DatasetWriteHelper(
            IStatusLogger logger,
            ISqlHelper sqlHelper
        )
        {
            _logger = logger;
            _sqlHelper = sqlHelper;

            _degreesofparallelism = Environment.ProcessorCount;
            if (_degreesofparallelism > 8)
            {
                _degreesofparallelism = 8;
            }
        }

        public async Task TruncateAsync(IDbConnection connection, string tablename)
        {
            try
            {
                await connection.TruncateTableAsync(tablename);
            }
            catch (Exception ex)
            {
                _logger.LogStatus(string.Format("An error occurred while truncating the table {0}.", tablename), StatusType.Error);
                _logger.LogStatus(ex.ToString(), StatusType.Error);
            }
        }

        public async Task ExecuteStoredProcedureAsync(string connectionString, string storedProcedureName, int spCommandTimeout = 30)
        {
            using var conn = await _sqlHelper.GetOpenConnectionAsync(connectionString);
            var cmd = _sqlHelper.GetSPCommand(storedProcedureName, null, spCommandTimeout);
            cmd.Connection = conn;
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task WriteAsync<T>(
            IEnumerable<T> items,
            Func<IEnumerable<T>, IDataReader> datareaderconverter,
            string connectionString,
            string destinationtable,
            int sqlTimeout = 180
        )
        {
            try
            {
                var partitions = Partition(items, _degreesofparallelism);

                var tasks = partitions.AsParallel()
                    .WithDegreeOfParallelism(_degreesofparallelism)
                    .Select(x => Task.Run(async () => await BulkInsertAsync(x, datareaderconverter, connectionString, destinationtable, sqlTimeout)))
                    .ToArray();
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogStatus(string.Format("An error occurred while writing to the table {0}.", destinationtable), StatusType.Error);
                _logger.LogStatus(ex.ToString(), StatusType.Error);
            }
        }

        private async Task BulkInsertAsync<T>(
            IEnumerable<T> items,
            Func<IEnumerable<T>, IDataReader> datareaderconverter,
            string connectionString,
            string destinationtable,
            int sqlTimeout = 180
        )
        {
            using var conn = await _sqlHelper.GetOpenConnectionAsync(connectionString, sqlTimeout);
            await conn.BulkCopyAsync(datareaderconverter(items), destinationtable);
        }

        private static IEnumerable<IEnumerable<T>> Partition<T>(IEnumerable<T> items, int partitions)
        {
            var i = 0;
            return items.GroupBy(e => i++ % partitions);
        }
    }
}
