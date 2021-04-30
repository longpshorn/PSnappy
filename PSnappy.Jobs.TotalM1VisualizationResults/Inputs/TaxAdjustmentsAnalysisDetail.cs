using System;
using System.Collections.Generic;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class TaxAdjustmentsAnalysisDetail
    {
        public int EntityId { get; set; }
        public string EntityDesc { get; set; }
        public string PortfolioName { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityName { get; set; }
        public string ForeignDomestic { get; set; }
        public Guid TaxLotGuid { get; set; }
        public string TaxSubLotId { get; set; }
        public string BookAssetType { get; set; }
        public string AssetType { get; set; }
        public string AssetDescription { get; set; }
        public string SubAssetDescription { get; set; }
        public string SecurityClientPutCallDescription { get; set; }
        public string DeepInTheMoneyOverride { get; set; }
        public string ClientOpenCode { get; set; }
        public string ClientCloseCode { get; set; }
        public string CloseTradeDate { get; set; }
        public string CloseSettleDate { get; set; }
        public string BookGainLossCharacterizationDescription { get; set; }
        public string TaxGainLossCharacterizationDescription { get; set; }
        public string OpenTradeDate { get; set; }
        public string OpenSettlementDate { get; set; }
        public string LotAdjustedOpenTradeDate { get; set; }

        public Dictionary<string, string> AggregationValues { get; set; }
    }
}
