using System.Collections.Generic;
using PSnappy;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class DatasetContext : IDatasetContext
    {
        private IDatasetReporter _reporter;
        public IDatasetReporter Reporter
        {
            get
            {
                if (_reporter == null)
                {
                    _reporter = new DatasetReporter(this);
                }
                return _reporter;
            }
        }

        public List<TaxAdjustmentsAnalysisDetail> TaxAdjustmentsAnalysisDetails { get; set; }
        public Dictionary<string, TotalM1Grouping> TotalM1Groupings { get; set; }
        public Dictionary<string, TotalM1TaxRate> TotalM1TaxRates { get; set; }

        public DatasetContext()
        {
            TaxAdjustmentsAnalysisDetails = new List<TaxAdjustmentsAnalysisDetail>();
            TotalM1Groupings = new Dictionary<string, TotalM1Grouping>();
            TotalM1TaxRates = new Dictionary<string, TotalM1TaxRate>();
        }
    }
}
