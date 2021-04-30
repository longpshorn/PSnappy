using System.Threading.Tasks;
using PSnappy;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class DatasetWriter : IDatasetWriter
    {
        private readonly IDatasetWriteHelper _writeHelper;

        public DatasetWriter(IDatasetWriteHelper writeHelper)
        {
            _writeHelper = writeHelper;
        }

        public void Reset(string server, string database)
        {
            using (var conn = SqlServerHelper.GetConnection(server, database))
            {
                conn.Open();
                _writeHelper.Truncate(conn, "Visualization.TotalM1Results");
            }
        }

        public async Task SaveAsync(IDatasetContext context, string server, string database)
        {
            Reset(server, database);

            var r = (DatasetReporter)context.Reporter;
            var s = server;
            var d = database;

            // Outputs
            await _writeHelper.WriteAsync(r.GetAllTotalM1Results(), DatasetSchemaHelper.ToDataReader, s, d, "Visualization.TotalM1Results");
        }
    }
}
