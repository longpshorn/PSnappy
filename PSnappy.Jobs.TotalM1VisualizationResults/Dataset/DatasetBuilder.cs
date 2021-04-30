using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using PSnappy;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class DatasetBuilder : IDatasetBuilder
    {
        private readonly IDatasetBuildHelper _buildHelper;

        private IDatasetContext _context;
        public DatasetContext Context
        {
            get { return (DatasetContext)_context; }
        }

        public DatasetBuilder(IDatasetBuildHelper buildHelper)
        {
            _buildHelper = buildHelper;
        }

        public async Task BuildAsync(IDatasetContext dataset, string server, string database)
        {
            _context = dataset;

            using (var conn = SqlServerHelper.GetConnection(server, database))
            {
                conn.Open();
                await _buildHelper.BuildAsync(conn, "Adjustments.uspGetTaxAdjustmentsAnalysisDetail", BuildTaxAdjustmentsAnalysisDetail);
                await _buildHelper.BuildAsync(conn, "Adjustments.uspGetTotalM1Grouping", BuildTotalM1Grouping);
                await _buildHelper.BuildAsync(conn, "Adjustments.uspGetTotalM1TaxRates", BuildTotalM1TaxRates);
            }
        }

        private void BuildTaxAdjustmentsAnalysisDetail(IDataReader reader)
        {
            while (reader.Read())
            {
                var obj = new TaxAdjustmentsAnalysisDetail
                {
                    EntityId = reader.GetInt32(_buildHelper.GetOrdinal(reader, "EntityId")),
                    EntityDesc = reader.GetString(_buildHelper.GetOrdinal(reader, "EntityDesc")),
                    PortfolioName = reader.GetString(_buildHelper.GetOrdinal(reader, "PortfolioName")),
                    SecurityCode = reader.GetString(_buildHelper.GetOrdinal(reader, "SecurityCode")),
                    SecurityName = reader.GetString(_buildHelper.GetOrdinal(reader, "SecurityName")),
                    ForeignDomestic = reader.GetString(_buildHelper.GetOrdinal(reader, "ForeignDomestic")),
                    TaxLotGuid = reader.GetGuid(_buildHelper.GetOrdinal(reader, "TaxLotGuid")),
                    TaxSubLotId = reader.GetString(_buildHelper.GetOrdinal(reader, "TaxSubLotId")),
                    BookAssetType = reader.GetString(_buildHelper.GetOrdinal(reader, "BookAssetType")),
                    AssetType = reader.GetString(_buildHelper.GetOrdinal(reader, "AssetType")),
                    AssetDescription = reader.GetString(_buildHelper.GetOrdinal(reader, "AssetDescription")),
                    SubAssetDescription = reader.GetString(_buildHelper.GetOrdinal(reader, "SubAssetDescription")),
                    SecurityClientPutCallDescription = reader.GetString(_buildHelper.GetOrdinal(reader, "SecurityClientPutCallDescription")),
                    DeepInTheMoneyOverride = reader.GetString(_buildHelper.GetOrdinal(reader, "DeepInTheMoneyOverride")),
                    ClientOpenCode = reader.GetString(_buildHelper.GetOrdinal(reader, "ClientOpenCode")),
                    ClientCloseCode = reader.GetString(_buildHelper.GetOrdinal(reader, "ClientCloseCode")),
                    CloseTradeDate = reader.GetString(_buildHelper.GetOrdinal(reader, "CloseTradeDate")),
                    CloseSettleDate = reader.GetString(_buildHelper.GetOrdinal(reader, "CloseSettleDate")),
                    BookGainLossCharacterizationDescription = reader.GetString(_buildHelper.GetOrdinal(reader, "BookGainLossCharacterizationDescription")),
                    TaxGainLossCharacterizationDescription = reader.GetString(_buildHelper.GetOrdinal(reader, "TaxGainLossCharacterizationDescription")),
                    OpenTradeDate = reader.GetString(_buildHelper.GetOrdinal(reader, "OpenTradeDate")),
                    OpenSettlementDate = reader.GetString(_buildHelper.GetOrdinal(reader, "OpenSettlementDate")),
                    LotAdjustedOpenTradeDate = reader.GetString(_buildHelper.GetOrdinal(reader, "LotAdjustedOpenTradeDate")),

                    AggregationValues = new Dictionary<string, string>
                    {
                        { "BBALong3YearToSection988Amount", reader.GetString(_buildHelper.GetOrdinal(reader, "BBALong3YearToSection988Amount")) },
                        { "BBALongToSection988Amount", reader.GetString(_buildHelper.GetOrdinal(reader, "BBALongToSection988Amount")) },
                        { "BBAShortToSection988Amount", reader.GetString(_buildHelper.GetOrdinal(reader, "BBAShortToSection988Amount")) },
                        { "CRAdjustmentTriggeringReclassification", reader.GetString(_buildHelper.GetOrdinal(reader, "CRAdjustmentTriggeringReclassification")) },
                        { "CRCurrentYearCharacterReclassificationFromLongTermToLong3Year", reader.GetString(_buildHelper.GetOrdinal(reader, "CRCurrentYearCharacterReclassificationFromLongTermToLong3Year")) },
                        { "CRCurrentYearCharacterReclassificationFromShortTermToLong3Year", reader.GetString(_buildHelper.GetOrdinal(reader, "CRCurrentYearCharacterReclassificationFromShortTermToLong3Year")) },
                        { "CRCurrentYearCharacterReclassificationLong", reader.GetString(_buildHelper.GetOrdinal(reader, "CRCurrentYearCharacterReclassificationLong")) },
                        { "CRCurrentYearCharacterReclassificationSection1256", reader.GetString(_buildHelper.GetOrdinal(reader, "CRCurrentYearCharacterReclassificationSection1256")) },
                        { "CRCurrentYearCharacterReclassificationSection1296", reader.GetString(_buildHelper.GetOrdinal(reader, "CRCurrentYearCharacterReclassificationSection1296")) },
                        { "CRCurrentYearCharacterReclassificationSection988", reader.GetString(_buildHelper.GetOrdinal(reader, "CRCurrentYearCharacterReclassificationSection988")) },
                        { "CRCurrentYearCharacterReclassificationShort", reader.GetString(_buildHelper.GetOrdinal(reader, "CRCurrentYearCharacterReclassificationShort")) },
                        { "CSAdjustmentQuantity", reader.GetString(_buildHelper.GetOrdinal(reader, "CSAdjustmentQuantity")) },
                        { "CSCurrentPeriodGainRecognition", reader.GetString(_buildHelper.GetOrdinal(reader, "CSCurrentPeriodGainRecognition")) },
                        { "CSCurrentPeriodGainRecognitionFx", reader.GetString(_buildHelper.GetOrdinal(reader, "CSCurrentPeriodGainRecognitionFx")) },
                        { "CSCurrentPeriodGainRecognitionReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "CSCurrentPeriodGainRecognitionReversal")) },
                        { "CSCurrentPeriodGainRecognitionReversalFx", reader.GetString(_buildHelper.GetOrdinal(reader, "CSCurrentPeriodGainRecognitionReversalFx")) },
                        { "CSCurrentPeriodPotential", reader.GetString(_buildHelper.GetOrdinal(reader, "CSCurrentPeriodPotential")) },
                        { "CSCurrentPeriodPotentialFx", reader.GetString(_buildHelper.GetOrdinal(reader, "CSCurrentPeriodPotentialFx")) },
                        { "CSFirstLegTaxLot", reader.GetString(_buildHelper.GetOrdinal(reader, "CSFirstLegTaxLot")) },
                        { "CSPriorPeriodGainRecognitionReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "CSPriorPeriodGainRecognitionReversal")) },
                        { "CSPriorPeriodGainRecognitionReversalFx", reader.GetString(_buildHelper.GetOrdinal(reader, "CSPriorPeriodGainRecognitionReversalFx")) },
                        { "CSSecondLegTaxLot", reader.GetString(_buildHelper.GetOrdinal(reader, "CSSecondLegTaxLot")) },
                        { "DACapitalizedShortDividendAmount", reader.GetString(_buildHelper.GetOrdinal(reader, "DACapitalizedShortDividendAmount")) },
                        { "DAPotentialCapitalizedShortDividendAmount", reader.GetString(_buildHelper.GetOrdinal(reader, "DAPotentialCapitalizedShortDividendAmount")) },
                        { "DAPriorYearCapitalizedShortDividendAmount", reader.GetString(_buildHelper.GetOrdinal(reader, "DAPriorYearCapitalizedShortDividendAmount")) },
                        { "FairMarketValueBaseCurrency", reader.GetString(_buildHelper.GetOrdinal(reader, "FairMarketValueBaseCurrency")) },
                        { "LongShortPositionDescription", reader.GetString(_buildHelper.GetOrdinal(reader, "LongShortPositionDescription")) },
                        { "M2M988CurrentPeriodSec1256M2M988Amount", reader.GetString(_buildHelper.GetOrdinal(reader, "M2M988CurrentPeriodSec1256M2M988Amount")) },
                        { "M2M988PriorPeriodSection1256M2M988ReversalAmount", reader.GetString(_buildHelper.GetOrdinal(reader, "M2M988PriorPeriodSection1256M2M988ReversalAmount")) },
                        { "M2MCurrentPeriodSec1256M2MAmount", reader.GetString(_buildHelper.GetOrdinal(reader, "M2MCurrentPeriodSec1256M2MAmount")) },
                        { "M2MPriorPeriodSection1256M2MReversalAmount", reader.GetString(_buildHelper.GetOrdinal(reader, "M2MPriorPeriodSection1256M2MReversalAmount")) },
                        { "ModWashSaleCurrentPeriodCapShortDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodCapShortDeferral")) },
                        { "ModWashSaleCurrentPeriodCSReversalDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodCSReversalDeferral")) },
                        { "ModWashSaleCurrentPeriodCSReversalDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodCSReversalDeferralFx")) },
                        { "ModWashSaleCurrentPeriodDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodDeferral")) },
                        { "ModWashSaleCurrentPeriodDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodDeferralFx")) },
                        { "ModWashSaleCurrentPeriodReDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodReDeferral")) },
                        { "ModWashSaleCurrentPeriodReDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodReDeferralFx")) },
                        { "ModWashSaleCurrentPeriodREITROCDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodREITROCDeferral")) },
                        { "ModWashSaleCurrentPeriodReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodReversal")) },
                        { "ModWashSaleCurrentPeriodReversalFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodReversalFx")) },
                        { "ModWashSaleCurrentPeriodTopSideDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodTopSideDeferral")) },
                        { "ModWashSaleCurrentPeriodWashSaleReDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodWashSaleReDeferral")) },
                        { "ModWashSaleCurrentPeriodWashSaleReDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleCurrentPeriodWashSaleReDeferralFx")) },
                        { "ModWashSaleHoldingPeriodAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleHoldingPeriodAdjustment")) },
                        { "ModWashSaleHoldingPeriodGPAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleHoldingPeriodGPAdjustment")) },
                        { "ModWashSalePriorPeriodReDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalePriorPeriodReDeferral")) },
                        { "ModWashSalePriorPeriodReDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalePriorPeriodReDeferralFx")) },
                        { "ModWashSalePriorPeriodReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalePriorPeriodReversal")) },
                        { "ModWashSalePriorPeriodReversalFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalePriorPeriodReversalFx")) },
                        { "ModWashSalePriorPeriodWashSaleReDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalePriorPeriodWashSaleReDeferral")) },
                        { "ModWashSalePriorPeriodWashSaleReDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalePriorPeriodWashSaleReDeferralFx")) },
                        { "ModWashSalePriorPeriodWashSaleUnsettledShortRefersalDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalePriorPeriodWashSaleUnsettledShortRefersalDeferral")) },
                        { "ModWashSalePriorPeriodWashSaleUnsettledShortRefersalDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalePriorPeriodWashSaleUnsettledShortRefersalDeferralFx")) },
                        { "ModWashSaleQuantity", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleQuantity")) },
                        { "ModWashSalesDebtMkDiscAccrualDeferrals", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalesDebtMkDiscAccrualDeferrals")) },
                        { "ModWashSalesDebtMkPremAccrualDeferrals", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalesDebtMkPremAccrualDeferrals")) },
                        { "ModWashSalesDebtOIDAccrualDeferrals", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSalesDebtOIDAccrualDeferrals")) },
                        { "ModWashSaleSection1296M2MCarryoverAdjDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "ModWashSaleSection1296M2MCarryoverAdjDeferral")) },
                        { "NormalizedQuantity", reader.GetString(_buildHelper.GetOrdinal(reader, "NormalizedQuantity")) },
                        { "OpenTradeAdjustedCostBasis", reader.GetString(_buildHelper.GetOrdinal(reader, "OpenTradeAdjustedCostBasis")) },
                        { "REITCurrentPeriodGrossLongTermGainREITReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITCurrentPeriodGrossLongTermGainREITReturnOfCapitalAdjustment")) },
                        { "REITCurrentPeriodGrossLongTermLossREITReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITCurrentPeriodGrossLongTermLossREITReturnOfCapitalAdjustment")) },
                        { "REITCurrentPeriodGrossShortTermGainREITReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITCurrentPeriodGrossShortTermGainREITReturnOfCapitalAdjustment")) },
                        { "REITCurrentPeriodGrossShortTermLossREITReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITCurrentPeriodGrossShortTermLossREITReturnOfCapitalAdjustment")) },
                        { "REITCurrentPeriodLongTermCapitalGainDividends", reader.GetString(_buildHelper.GetOrdinal(reader, "REITCurrentPeriodLongTermCapitalGainDividends")) },
                        { "REITCurrentPeriodRecognizedLongTerm3YrGainReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITCurrentPeriodRecognizedLongTerm3YrGainReturnOfCapitalAdjustment")) },
                        { "REITCurrentPeriodRecognizedLongTermGainReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITCurrentPeriodRecognizedLongTermGainReturnOfCapitalAdjustment")) },
                        { "REITCurrentPeriodRecognizedShortTermGainReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITCurrentPeriodRecognizedShortTermGainReturnOfCapitalAdjustment")) },
                        { "REITGrossLongTermGainLongTermCapitalDistributionReclass", reader.GetString(_buildHelper.GetOrdinal(reader, "REITGrossLongTermGainLongTermCapitalDistributionReclass")) },
                        { "REITGrossShortTermLossLongTermCapitalDistributionReclass", reader.GetString(_buildHelper.GetOrdinal(reader, "REITGrossShortTermLossLongTermCapitalDistributionReclass")) },
                        { "REITPriorPeriodGrossLongTermGainREITReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITPriorPeriodGrossLongTermGainREITReturnOfCapitalAdjustment")) },
                        { "REITPriorPeriodGrossLongTermLossREITReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITPriorPeriodGrossLongTermLossREITReturnOfCapitalAdjustment")) },
                        { "REITPriorPeriodGrossShortTermGainREITReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITPriorPeriodGrossShortTermGainREITReturnOfCapitalAdjustment")) },
                        { "REITPriorPeriodGrossShortTermLossREITReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "REITPriorPeriodGrossShortTermLossREITReturnOfCapitalAdjustment")) },
                        { "REITSection1250Dividends", reader.GetString(_buildHelper.GetOrdinal(reader, "REITSection1250Dividends")) },
                        { "RICCurrentPeriodGrossLongTermGainRICReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICCurrentPeriodGrossLongTermGainRICReturnOfCapitalAdjustment")) },
                        { "RICCurrentPeriodGrossLongTermLossRICReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICCurrentPeriodGrossLongTermLossRICReturnOfCapitalAdjustment")) },
                        { "RICCurrentPeriodGrossShortTermGainRICReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICCurrentPeriodGrossShortTermGainRICReturnOfCapitalAdjustment")) },
                        { "RICCurrentPeriodGrossShortTermLossRICReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICCurrentPeriodGrossShortTermLossRICReturnOfCapitalAdjustment")) },
                        { "RICCurrentPeriodLongTermCapitalGainDividends", reader.GetString(_buildHelper.GetOrdinal(reader, "RICCurrentPeriodLongTermCapitalGainDividends")) },
                        { "RICCurrentPeriodRecognizedLongTerm3YrGainReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICCurrentPeriodRecognizedLongTerm3YrGainReturnOfCapitalAdjustment")) },
                        { "RICCurrentPeriodRecognizedLongTermGainReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICCurrentPeriodRecognizedLongTermGainReturnOfCapitalAdjustment")) },
                        { "RICCurrentPeriodRecognizedShortTermGainReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICCurrentPeriodRecognizedShortTermGainReturnOfCapitalAdjustment")) },
                        { "RICGrossLongTermGainLongTermCapitalDistributionReclass", reader.GetString(_buildHelper.GetOrdinal(reader, "RICGrossLongTermGainLongTermCapitalDistributionReclass")) },
                        { "RICGrossShortTermLossLongTermCapitalDistributionReclass", reader.GetString(_buildHelper.GetOrdinal(reader, "RICGrossShortTermLossLongTermCapitalDistributionReclass")) },
                        { "RICPriorPeriodGrossLongTermGainRICReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICPriorPeriodGrossLongTermGainRICReturnOfCapitalAdjustment")) },
                        { "RICPriorPeriodGrossLongTermLossRICReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICPriorPeriodGrossLongTermLossRICReturnOfCapitalAdjustment")) },
                        { "RICPriorPeriodGrossShortTermGainRICReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICPriorPeriodGrossShortTermGainRICReturnOfCapitalAdjustment")) },
                        { "RICPriorPeriodGrossShortTermLossRICReturnOfCapitalAdjustment", reader.GetString(_buildHelper.GetOrdinal(reader, "RICPriorPeriodGrossShortTermLossRICReturnOfCapitalAdjustment")) },
                        { "RICSection1250Dividends", reader.GetString(_buildHelper.GetOrdinal(reader, "RICSection1250Dividends")) },
                        { "RIKRedemptionInKindBookGainLossReversalAmount", reader.GetString(_buildHelper.GetOrdinal(reader, "RIKRedemptionInKindBookGainLossReversalAmount")) },
                        { "TopSideACAdjustmentDate", reader.GetString(_buildHelper.GetOrdinal(reader, "TopSideACAdjustmentDate")) },
                        { "TopSideACCalculationAmount", reader.GetString(_buildHelper.GetOrdinal(reader, "TopSideACCalculationAmount")) },
                        { "TopSidePTCAdjustmentPriorToCalculationAmountReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "TopSidePTCAdjustmentPriorToCalculationAmountReversal")) },
                        { "TotalGainLossBaseCurrency", reader.GetString(_buildHelper.GetOrdinal(reader, "TotalGainLossBaseCurrency")) },
                        { "TSAAssignedFromTaxLot", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAAssignedFromTaxLot")) },
                        { "TSAAssignedToTaxLot", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAAssignedToTaxLot")) },
                        { "TSACurrentDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentDeferralFx")) },
                        { "TSACurrentDeferralLongTerm", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentDeferralLongTerm")) },
                        { "TSACurrentDeferralLongTerm3Year", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentDeferralLongTerm3Year")) },
                        { "TSACurrentDeferralShortTerm", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentDeferralShortTerm")) },
                        { "TSACurrentPeriodIdentLossRecognition", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodIdentLossRecognition")) },
                        { "TSACurrentPeriodIdentLossRecognitionReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodIdentLossRecognitionReversal")) },
                        { "TSACurrentPeriodPotentialUnidentOffsetToSucr1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentOffsetToSucr1256LossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentOffsetToSucrLongTerm1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentOffsetToSucrLongTerm1256LossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentOffsetToSucrLongTerm3YearLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentOffsetToSucrLongTerm3YearLossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentOffsetToSucrLongTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentOffsetToSucrLongTermLossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentOffsetToSucrLossDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentOffsetToSucrLossDeferralFx")) },
                        { "TSACurrentPeriodPotentialUnidentOffsetToSucrShortTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentOffsetToSucrShortTermLossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentSucr1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentSucr1256LossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentSucrLongTerm1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentSucrLongTerm1256LossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentSucrLongTerm3YearLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentSucrLongTerm3YearLossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentSucrLongTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentSucrLongTermLossDeferral")) },
                        { "TSACurrentPeriodPotentialUnidentSucrLossDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentSucrLossDeferralFx")) },
                        { "TSACurrentPeriodPotentialUnidentSucrShortTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodPotentialUnidentSucrShortTermLossDeferral")) },
                        { "TSACurrentPeriodUnidentOffset1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffset1256LossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetLongTerm1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetLongTerm1256LossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetLongTerm3YearLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetLongTerm3YearLossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetLongTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetLongTermLossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetLossDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetLossDeferralFx")) },
                        { "TSACurrentPeriodUnidentOffsetShortTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetShortTermLossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetToSucr1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetToSucr1256LossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetToSucrLongTerm1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetToSucrLongTerm1256LossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetToSucrLongTerm3YearLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetToSucrLongTerm3YearLossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetToSucrLongTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetToSucrLongTermLossDeferral")) },
                        { "TSACurrentPeriodUnidentOffsetToSucrLossDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetToSucrLossDeferralFx")) },
                        { "TSACurrentPeriodUnidentOffsetToSucrShortTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentOffsetToSucrShortTermLossDeferral")) },
                        { "TSACurrentPeriodUnidentSucr1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentSucr1256LossDeferral")) },
                        { "TSACurrentPeriodUnidentSucrLongTerm1256LossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentSucrLongTerm1256LossDeferral")) },
                        { "TSACurrentPeriodUnidentSucrLongTerm3YearLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentSucrLongTerm3YearLossDeferral")) },
                        { "TSACurrentPeriodUnidentSucrLongTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentSucrLongTermLossDeferral")) },
                        { "TSACurrentPeriodUnidentSucrLossDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentSucrLossDeferralFx")) },
                        { "TSACurrentPeriodUnidentSucrShortTermLossDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "TSACurrentPeriodUnidentSucrShortTermLossDeferral")) },
                        { "TSADeferralTaxStraddleTypeDescription", reader.GetString(_buildHelper.GetOrdinal(reader, "TSADeferralTaxStraddleTypeDescription")) },
                        { "TSAPriorPeriodIdentLossRecognitionReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAPriorPeriodIdentLossRecognitionReversal")) },
                        { "TSAPriorPeriodUnident1256Reversal", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAPriorPeriodUnident1256Reversal")) },
                        { "TSAPriorPeriodUnidentLongTerm1256Reversal", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAPriorPeriodUnidentLongTerm1256Reversal")) },
                        { "TSAPriorPeriodUnidentLongTerm3YearReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAPriorPeriodUnidentLongTerm3YearReversal")) },
                        { "TSAPriorPeriodUnidentLongTermReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAPriorPeriodUnidentLongTermReversal")) },
                        { "TSAPriorPeriodUnidentReversalFx", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAPriorPeriodUnidentReversalFx")) },
                        { "TSAPriorPeriodUnidentShortTermReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "TSAPriorPeriodUnidentShortTermReversal")) },
                        { "URBookGainLossReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "URBookGainLossReversal")) },
                        { "WashSaleCurrentPeriodCapShortDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodCapShortDeferral")) },
                        { "WashSaleCurrentPeriodCSReversalDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodCSReversalDeferral")) },
                        { "WashSaleCurrentPeriodCSReversalDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodCSReversalDeferralFx")) },
                        { "WashSaleCurrentPeriodDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodDeferral")) },
                        { "WashSaleCurrentPeriodDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodDeferralFx")) },
                        { "WashSaleCurrentPeriodReDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodReDeferral")) },
                        { "WashSaleCurrentPeriodReDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodReDeferralFx")) },
                        { "WashSaleCurrentPeriodREITROCDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodREITROCDeferral")) },
                        { "WashSaleCurrentPeriodReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodReversal")) },
                        { "WashSaleCurrentPeriodReversalFx", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodReversalFx")) },
                        { "WashSaleCurrentPeriodTopSideDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleCurrentPeriodTopSideDeferral")) },
                        { "WashSalePriorPeriodReDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalePriorPeriodReDeferral")) },
                        { "WashSalePriorPeriodReDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalePriorPeriodReDeferralFx")) },
                        { "WashSalePriorPeriodReversal", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalePriorPeriodReversal")) },
                        { "WashSalePriorPeriodReversalFx", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalePriorPeriodReversalFx")) },
                        { "WashSalePriorPeriodWashSaleUnsettledShortRefersalDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalePriorPeriodWashSaleUnsettledShortRefersalDeferral")) },
                        { "WashSalePriorPeriodWashSaleUnsettledShortRefersalDeferralFx", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalePriorPeriodWashSaleUnsettledShortRefersalDeferralFx")) },
                        { "WashSaleQuantity", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleQuantity")) },
                        { "WashSalesDebtMkDiscAccrualDeferrals", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalesDebtMkDiscAccrualDeferrals")) },
                        { "WashSalesDebtMkPremAccrualDeferrals", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalesDebtMkPremAccrualDeferrals")) },
                        { "WashSalesDebtOIDAccrualDeferrals", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSalesDebtOIDAccrualDeferrals")) },
                        { "WashSaleSection1296M2MCarryoverAdjDeferral", reader.GetString(_buildHelper.GetOrdinal(reader, "WashSaleSection1296M2MCarryoverAdjDeferral")) }
                    }
                };

                Context.TaxAdjustmentsAnalysisDetails.Add(obj);
            }
        }

        private void BuildTotalM1Grouping(IDataReader reader)
        {
            while (reader.Read())
            {
                var key = reader.GetString(_buildHelper.GetOrdinal(reader, "OldFieldName"));
                var obj = new TotalM1Grouping
                {
                    NewFieldName = reader.GetString(_buildHelper.GetOrdinal(reader, "NewFieldName")),
                    GroupName = reader.GetString(_buildHelper.GetOrdinal(reader, "GroupName")),
                    SubGroupName = reader.GetString(_buildHelper.GetOrdinal(reader, "SubGroupName")),
                    VizGroup = reader.GetString(_buildHelper.GetOrdinal(reader, "VizGroup"))
                };

                Context.TotalM1Groupings.Add(key, obj);
            }
        }

        private void BuildTotalM1TaxRates(IDataReader reader)
        {
            while (reader.Read())
            {
                var key = reader.GetString(_buildHelper.GetOrdinal(reader, "Character"));
                var obj = new TotalM1TaxRate
                {
                    TaxRate = reader.GetDecimal(_buildHelper.GetOrdinal(reader, "TaxRate")),
                    Rate = reader.GetInt32(_buildHelper.GetOrdinal(reader, "Rate")),
                    RenamedTaxCharacter = reader.GetString(_buildHelper.GetOrdinal(reader, "RenamedTaxCharacter")),
                    SummarizedTaxCharacter = reader.GetString(_buildHelper.GetOrdinal(reader, "SummarizedTaxCharacter")),
                    LPTaxCharacter = reader.GetString(_buildHelper.GetOrdinal(reader, "LPTaxCharacter")),
                    GPTaxCharacter = reader.GetString(_buildHelper.GetOrdinal(reader, "GPTaxCharacter")),
                    Unrealized = reader.GetBoolean(_buildHelper.GetOrdinal(reader, "Unrealized"))
                };

                Context.TotalM1TaxRates.Add(key, obj);
            }
        }
    }
}
