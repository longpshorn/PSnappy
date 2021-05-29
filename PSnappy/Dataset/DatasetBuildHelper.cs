using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace PSnappy
{
    public interface IDatasetBuildHelper
    {
        void Build(SqlConnection connection, string command, Action<IDataReader> BuildAction, Dictionary<string, object> parameters = null, int commandTimeout = 30);
        Task BuildAsync(SqlConnection connection, string command, Action<IDataReader> BuildAction, Dictionary<string, object> parameters = null, int commandTimeout = 30);
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

        public void Build(SqlConnection connection, string command, Action<IDataReader> BuildAction, Dictionary<string, object> parameters = null, int commandTimeout = 30)
        {
            var reader = GetReader(connection, command, parameters, commandTimeout);
            if (reader != null)
            {
                BuildAction(reader);

                reader.Close();
            }
        }

        public async Task BuildAsync(SqlConnection connection, string command, Action<IDataReader> BuildAction, Dictionary<string, object> parameters = null, int commandTimeout = 30)
        {
            var reader = await GetReaderAsync(connection, command, parameters, commandTimeout);
            if (reader != null)
            {
                BuildAction(reader);
                reader.Close();
            }
        }

        private IDataReader GetReader(SqlConnection connection, string command, Dictionary<string, object> parameters = null, int commandTimeout = 30)
        {
            try
            {
                return ((DbDataReader)connection.ExecuteReader(command, parameters, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure))
                    .ToSafeDataReader();
            }
            catch (Exception ex)
            {
                _logger.LogStatus($"An error occurred while trying to read data from {command}.", StatusType.Error);
                _logger.LogStatus(ex.ToString(), StatusType.Error);
                return null;
            }
        }

        private async Task<IDataReader> GetReaderAsync(SqlConnection connection, string command, Dictionary<string, object> parameters = null, int commandTimeout = 30)
        {
            try
            {
                return (await connection.ExecuteReaderAsync(command, parameters, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure))
                    .ToSafeDataReader();
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
