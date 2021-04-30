using System;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class TotalM1Result
    {
        public int EntityId { get; set; }
        public string Entity { get; set; }
        public string PortfolioName { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityName { get; set; }
        public string ForeignDomestic { get; set; }
        public Guid TaxLotGuid { get; set; }
        public string TaxSubLotId { get; set; }
        public string BookAssetType { get; set; }
        public int? AssetType { get; set; }
        public string AssetDescription { get; set; }
        public string SubAssetDescription { get; set; }
        public string SecurityClientPutCallDescription { get; set; }
        public string DeepInTheMoneyOverride { get; set; }
        public string ClientOpenCode { get; set; }
        public string ClientCloseCode { get; set; }
        public DateTime? CloseTradeDate { get; set; }
        public DateTime? CloseSettleDate { get; set; }
        public string BookGainLossCharacterizationDescription { get; set; }
        public string TaxGainLossCharacterizationDescription { get; set; }
        public string M1Name { get; set; }
        public string NewFieldName { get; set; }
        public string M1Value { get; set; }
        public long? M1Amount { get; set; }
        public long? BookIncome { get; set; }
        public string GroupName { get; set; }
        public string SubGroupName { get; set; }
        public string VizGroup { get; set; }
        public decimal? TaxRate { get; set; }
        public int? Rate { get; set; }
        public string Character { get; set; }
        public string RenamedTaxCharacter { get; set; }
        public string SummarizedTaxCharacter { get; set; }
        public DateTime? OpenTradeDate { get; set; }
        public DateTime? OpenSettlementDate { get; set; }
        public DateTime? LotAdjustedOpenTradeDate { get; set; }
        public string LPTaxCharacter { get; set; }
        public string GPTaxCharacter { get; set; }
        public bool? Unrealized { get; set; }
    }
}
