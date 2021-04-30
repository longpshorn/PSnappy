using System.Collections.Generic;
using PSnappy;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class DatasetReporter : IDatasetReporter
    {
        private readonly IDatasetContext _context;
        public DatasetContext Context
        {
            get { return (DatasetContext)_context; }
        }

        public DatasetReporter(IDatasetContext dataset)
        {
            _context = dataset;
            Clear();
            Results = new List<TotalM1Result>();
        }

        public List<TotalM1Result> Results { get; set; }

        public void Clear()
        {
            Results?.Clear();
        }

        public IEnumerable<TotalM1Result> GetAllTotalM1Results()
        {
            return Results;
        }
    }
}
