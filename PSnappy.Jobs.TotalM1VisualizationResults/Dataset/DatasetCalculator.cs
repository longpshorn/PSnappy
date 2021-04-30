using System;
using System.Linq;
using PSnappy;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class DatasetCalculator : IDatasetCalculator
    {
        public void Calculate(IDatasetContext context)
        {
            var ctx = (DatasetContext)context;
            var reporter = (DatasetReporter)context.Reporter;

            var groupings = ctx.TotalM1Groupings;
            var rates = ctx.TotalM1TaxRates;

            var results = ctx.TaxAdjustmentsAnalysisDetails
                .SelectMany(x =>
                    x.AggregationValues
                        .Where(y =>
                            groupings.ContainsKey(y.Key)
                            && rates.ContainsKey(x.TaxGainLossCharacterizationDescription)
                            && !y.Value.Equals("0")
                            && !y.Value.Equals("-0")
                            && !y.Value.Equals("")
                        )
                        .Select(y =>
                        {
                            var group = groupings[y.Key];
                            var rate = rates[x.TaxGainLossCharacterizationDescription];
                            var isBookIncome = group.NewFieldName == "[Book Income]";
                            var isNumericValue = decimal.TryParse(y.Value, out decimal value);
                            var finalValue = (long)Math.Round(value, 0);

                            var parsedAssetType = int.TryParse(x.AssetType, out int assetType);
                            var parsedCloseTradeDate = DateTime.TryParse(x.CloseTradeDate, out DateTime closeTradeDate);
                            var parsedCloseSettleDate = DateTime.TryParse(x.CloseSettleDate, out DateTime closeSettleDate);
                            var parsedOpenTradeDate = DateTime.TryParse(x.OpenTradeDate, out DateTime openTradeDate);
                            var parsedOpenSettlementDate = DateTime.TryParse(x.OpenSettlementDate, out DateTime openSettlementDate);
                            var parsedLotAdjustedOpenTradeDate = DateTime.TryParse(x.LotAdjustedOpenTradeDate, out DateTime lotAdjustedOpenTradeDate);

                            return new TotalM1Result
                            {
                                EntityId = x.EntityId,
                                Entity = x.EntityDesc,
                                PortfolioName = x.PortfolioName,
                                SecurityCode = x.SecurityCode,
                                SecurityName = x.SecurityName,
                                ForeignDomestic = x.ForeignDomestic,
                                TaxLotGuid = x.TaxLotGuid,
                                TaxSubLotId = x.TaxSubLotId,
                                BookAssetType = x.BookAssetType,
                                AssetType = parsedAssetType ? (int?)assetType : null,
                                AssetDescription = x.AssetDescription,
                                SubAssetDescription = x.SubAssetDescription,
                                SecurityClientPutCallDescription = string.IsNullOrEmpty(x.SecurityClientPutCallDescription) ? null : x.SecurityClientPutCallDescription,
                                DeepInTheMoneyOverride = x.DeepInTheMoneyOverride,
                                ClientOpenCode = x.ClientOpenCode,
                                ClientCloseCode = x.ClientCloseCode,
                                CloseTradeDate = parsedCloseTradeDate ? (DateTime?)closeTradeDate : null,
                                CloseSettleDate = parsedCloseSettleDate ? (DateTime?)closeSettleDate : null,
                                BookGainLossCharacterizationDescription = x.BookGainLossCharacterizationDescription,
                                TaxGainLossCharacterizationDescription = x.TaxGainLossCharacterizationDescription,
                                M1Name = y.Key,
                                NewFieldName = group.NewFieldName,
                                M1Value = isBookIncome ? "0" : y.Value,
                                M1Amount = isBookIncome ? 0 : isNumericValue ? finalValue : 0,
                                BookIncome = isBookIncome ? finalValue : 0,
                                GroupName = group.GroupName,
                                SubGroupName = group.SubGroupName,
                                VizGroup = group.VizGroup,
                                TaxRate = rate.TaxRate,
                                Rate = rate.Rate,
                                Character = x.TaxGainLossCharacterizationDescription,
                                RenamedTaxCharacter = rate.RenamedTaxCharacter,
                                SummarizedTaxCharacter = rate.SummarizedTaxCharacter,
                                OpenTradeDate = parsedOpenTradeDate ? (DateTime?)openTradeDate : null,
                                OpenSettlementDate = parsedOpenSettlementDate ? (DateTime?)openSettlementDate : null,
                                LotAdjustedOpenTradeDate = parsedLotAdjustedOpenTradeDate ? (DateTime?)lotAdjustedOpenTradeDate : null,
                                LPTaxCharacter = rate.LPTaxCharacter,
                                GPTaxCharacter = rate.GPTaxCharacter,
                                Unrealized = rate.Unrealized
                            };
                        })
                        .AsParallel()
                );

            reporter.Results = results.ToList();
        }
    }
}
