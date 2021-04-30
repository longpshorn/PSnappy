using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PSnappy
{
    public interface IDatasetBuildHelper
    {
        Task BuildAsync(SqlConnection connection, string command, Action<IDataReader> BuildAction);
        int GetOrdinal(IDataReader reader, string fieldname);
    }

    public class DatasetBuildHelper : IDatasetBuildHelper
    {
        protected readonly IStatusLogger _logger;

        public DatasetBuildHelper(IStatusLogger logger)
        {
            _logger = logger;
        }

        public int GetOrdinal(IDataReader reader, string fieldname)
        {
            var ordinal = reader.GetOrdinal(fieldname);
            if (ordinal == -1)
            {
                _logger.LogStatus($"An error occurred while trying to read the column {fieldname}.", StatusType.Error);
            }

            return ordinal;
        }

        public async Task BuildAsync(SqlConnection connection, string command, Action<IDataReader> BuildAction)
        {
            var reader = await GetReaderAsync(connection, command);
            if (reader != null)
            {
                BuildAction(reader);
                reader.Close();
            }
        }

        private async Task<IDataReader> GetReaderAsync(SqlConnection connection, string command)
        {
            try
            {
                var cmd = SqlServerHelper.GetSPCommand(command);
                cmd.Connection = connection;
                return await SqlServerHelper.GetDataReaderAsync(cmd);
            }
            catch (Exception ex)
            {
                _logger.LogStatus($"An error occurred while trying to read data from {command}.", StatusType.Error);
                _logger.LogStatus(ex.ToString(), StatusType.Error);
                return null;
            }
        }
    }
}
