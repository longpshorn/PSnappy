using System;
using System.Collections.Generic;
using System.Data;
using PSnappy;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    internal static class DatasetSchemaHelper
    {
        public static IDataReader ToDataReader(this IEnumerable<TotalM1Result> items)
        {
            var l = new List<Func<TotalM1Result, object>>();
            l.Add(e => e.EntityId);
            l.Add(e => e.Entity);
            l.Add(e => e.PortfolioName);
            l.Add(e => e.SecurityCode);
            l.Add(e => e.SecurityName);
            l.Add(e => e.ForeignDomestic);
            l.Add(e => e.TaxLotGuid);
            l.Add(e => e.TaxSubLotId);
            l.Add(e => e.BookAssetType);
            l.Add(e => e.AssetType);
            l.Add(e => e.AssetDescription);
            l.Add(e => e.SubAssetDescription);
            l.Add(e => e.SecurityClientPutCallDescription);
            l.Add(e => e.DeepInTheMoneyOverride);
            l.Add(e => e.ClientOpenCode);
            l.Add(e => e.ClientCloseCode);
            l.Add(e => e.CloseTradeDate);
            l.Add(e => e.CloseSettleDate);
            l.Add(e => e.BookGainLossCharacterizationDescription);
            l.Add(e => e.TaxGainLossCharacterizationDescription);
            l.Add(e => e.M1Name);
            l.Add(e => e.NewFieldName);
            l.Add(e => e.M1Value);
            l.Add(e => e.M1Amount);
            l.Add(e => e.BookIncome);
            l.Add(e => e.GroupName);
            l.Add(e => e.SubGroupName);
            l.Add(e => e.VizGroup);
            l.Add(e => e.TaxRate);
            l.Add(e => e.Rate);
            l.Add(e => e.Character);
            l.Add(e => e.RenamedTaxCharacter);
            l.Add(e => e.SummarizedTaxCharacter);
            l.Add(e => e.OpenTradeDate);
            l.Add(e => e.OpenSettlementDate);
            l.Add(e => e.LotAdjustedOpenTradeDate);
            l.Add(e => e.LPTaxCharacter);
            l.Add(e => e.GPTaxCharacter);
            l.Add(e => e.Unrealized);
            return items.ToUnsafeDataReader(l);
        }
    }
}
