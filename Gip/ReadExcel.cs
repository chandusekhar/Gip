using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.Linq.Mapping;
using MaasOne;
using MaasOne.Base;
using MaasOne.Finance;
using MaasOne.Finance.YahooFinance;


namespace Gip
{
    public static class GetData
    {
        public static object ExcelObject { get; set; }


        //public static void updateThruExcel()
        //{
        //    var Excel = new Microsoft.Office.Interop.Excel.Application();
        //    var Data = Excel.Workbooks.Open(@"C:\Users\Ben Roberts\Dropbox\Misc\Portfolio\Data.xlsm");
        //    int count = 0;
        //    List<Stock> Market = new List<Stock>();

        //    for (int i = 2; i < Data.Worksheets.Count + 1; i++)
        //    {

        //        Worksheet currentSheet = Data.Sheets[i];

        //        Stock Temp = new Stock();
        //        Temp.History = new List<TradingDay>();

        //        Temp.Ticker = currentSheet.Name;

        //        Range excelRange = currentSheet.UsedRange;


        //        object[,] valueArray = (object[,])excelRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);

        //        for (int x = 3; x < valueArray.GetLength(0) + 1; x++)
        //        {

        //            DateTime Date = Convert.ToDateTime(valueArray[x, 1]);
        //            double open = Convert.ToDouble(valueArray[x, 2]);
        //            double high = Convert.ToDouble(valueArray[x, 3]);
        //            double low = Convert.ToDouble(valueArray[x, 4]);
        //            double close = Convert.ToDouble(valueArray[x, 5]);
        //            double volume = Convert.ToDouble(valueArray[x, 6]);

        //            if (open == 0 || high == 0 || low == 0 || close == 0 || volume == 0) count++;

        //            TradingDay TempDay = new TradingDay(Date, open, close, high, low, volume);

        //            Temp.History.Add(TempDay);
        //        }

        //        int sum = count;
        //    }
        //}

        //public static void GetTickers2()
        //{
        //    MaasOne.Finance.YahooFinance.I

        //    MaasOne.Finance.YahooFinance.AlphabeticIDIndexDownload t = new AlphabeticIDIndexDownload();
        //    DownloadClient<AlphabeticIDIndexResult> baset = t;

        //    AlphabeticIDIndexSettings settings = t.Settings;
        //    settings.TopIndex = null;

        //    Response<AlphabeticIDIndexResult> Resp = t.Download();

        //    List<string> Tickers = new List<string>();
        //    foreach (var hqc in Resp.Result.Items)
        //    {
        //        Tickers.Add(hqc.Index);

        //    }
        //    Tickers.Add("t");
        //}

        public static List<string> TickerstoGet()
        {
            string[] Ticks = new string[]
            {
                "1PG", "3PL", "8IH", "A2M", "AAC", "AAD", "AAI", "ABA", "ABC", "ABP", "ACR", "ACX", "ADA", "ADH", "AFA",
                "AFG", "AGG", "AGI", "AGL", "AHG", "AHX", "AHY", "AHZ", "AIA", "AIO", "AJA", "AJD", "AJX", "AKP", "ALL",
                "ALQ", "ALU", "AMA", "AMC", "AMP", "ANN", "ANZ", "AOG", "APA", "APD", "APE", "API", "APN", "APO", "APZ",
                "AQG", "ARB", "ARF", "ASB", "AST", "ASX", "ASZ", "AUB", "AVB", "AVJ", "AVN", "AWC", "AWE", "AYS", "AZJ",
                "BAL", "BAP", "BBG", "BBN", "BDR", "BEN", "BFC", "BFG", "BGA", "BGL", "BHP", "BKL", "BKN", "BKW", "BLA",
                "BLD", "BLX", "BNO", "BOC", "BOQ", "BPA", "BPT", "BRG", "BRS", "BSL", "BTT", "BWP", "BWX", "BXB", "CAB",
                "CAJ", "CAQ", "CAR", "CBA", "CCL", "CCP", "CCV", "CDA", "CDD", "CDP", "CDU", "CEN", "CGC", "CGF", "CGL",
                "CHC", "CII", "CIM", "CKF", "CL1", "CLH", "CMA", "CMW", "CNU", "COH", "CPU", "CQR", "CSL", "CSR", "CSV",
                "CTD", "CTX", "CUP", "CUV", "CVC", "CVN", "CVO", "CVT", "CVW", "CWN", "CWP", "CWY", "CYB", "CZA", "CZZ",
                "DCG", "DCN", "DDR", "DFM", "DLX", "DME", "DMP", "DNA", "DOW", "DRM", "DTL", "DUE", "DVN", "DWS", "DXS",
                "DYE", "ECX", "EGH", "EHE", "ELD", "EML", "ENN", "EPW", "EPX", "EQT", "ERA", "EVN", "EVT", "EWC", "EZL",
                "FAN", "FAR", "FBU", "FET", "FFT", "FLK", "FLN", "FLT", "FMG", "FNP", "FPH", "FRI", "FSA", "FSF", "FXJ",
                "FXL", "GBT", "GDI", "GEG", "GEM", "GHC", "GJT", "GMA", "GMF", "GMG", "GNC", "GNG", "GOR", "GOW", "GOZ",
                "GPT", "GRR", "GTY", "GUD", "GWA", "GXL", "GXY", "GZL", "HFA", "HFR", "HGG", "HLO", "HPI", "HSN", "HSO",
                "HTA", "HUB", "HUO", "HVN", "HZN", "IAG", "ICQ", "IDR", "IDX", "IEL", "IFL", "IFM", "IFN", "IGL", "IGO",
                "ILU", "IMF", "INA", "INM", "IOF", "IPD", "IPH", "IPL", "IRE", "IRI", "ISD", "ISU", "IVC", "JBH", "JHC",
                "JHX", "KAM", "KAR", "KCN", "KMD", "KSC", "LAU", "LEP", "LHC", "LIC", "LLC", "LNG", "LNK", "LOV", "LYC",
                "MAH", "MAQ", "MBE", "MEA", "MEZ", "MFG", "MGC", "MGR", "MGX", "MIG", "MIL", "MIN", "MLB", "MLD", "MLX",
                "MMS", "MND", "MNF", "MNS", "MNY", "MOC", "MP1", "MPL", "MQA", "MQG", "MRM", "MRN", "MSB", "MTR", "MTS",
                "MUA", "MVF", "MVP", "MYO", "MYR", "MYS", "MYX", "NAB", "NAN", "NCK", "NCM", "NEA", "NEC", "NEU", "NHC",
                "NHF", "NSR", "NST", "NTC", "NUF", "NVT", "NWF", "NWS", "NXT", "OBJ", "OCL", "OFX", "OGC", "OML", "ONT",
                "ORA", "ORE", "ORG", "ORI", "ORL", "OSH", "OVH", "OZL", "PAC", "PAY", "PBG", "PDN", "PEA", "PEN", "PEP",
                "PFL", "PGC", "PGH", "PHG", "PHI", "PLS", "PME", "PMP", "PMV", "PNV", "PPC", "PPG", "PPS", "PPT", "PRG",
                "PRO", "PRR", "PRT", "PRU", "PRY", "PSI", "PSQ", "PTM", "PWH", "QAN", "QBE", "QMS", "QUB", "RCG", "RCR",
                "RCT", "REA", "REG", "REH", "RFF", "RFG", "RHC", "RHL", "RHP", "RIC", "RIO", "RKN", "RMD", "RMS", "RRL",
                "RSG", "RVA", "RWH", "S32", "SAI", "SAR", "SBM", "SCG", "SCP", "SDA", "SDF", "SDG", "SEA", "SEH", "SEK",
                "SEN", "SFH", "SFR", "SGF", "SGH", "SGM", "SGN", "SGP", "SGR", "SHJ", "SHL", "SHV", "SIO", "SIP", "SIQ",
                "SIT", "SIV", "SKB", "SKC", "SKI", "SKT", "SLC", "SLK", "SLM", "SLR", "SMA", "SMX", "SOL", "SOM", "SPK",
                "SPL", "SPO", "SRF", "SRV", "SRX", "SSM", "SST", "STO", "SUL", "SUN", "SVW", "SWM", "SXL", "SXY", "SYD",
                "SYR", "TAH", "TBR", "TCH", "TCL", "TEN", "TFC", "TGA", "TGP", "TGR", "TGS", "TIX", "TLS", "TME", "TNE",
                "TNG", "TOE", "TOF", "TOX", "TPE", "TPM", "TRS", "TTC", "TTS", "TWE", "TZN", "UBN", "UGL", "UOS", "URF",
                "VAH", "VCX", "VIT", "VLA", "VLW", "VOC", "VRL", "VRT", "VTG", "WBA", "WBC", "WCB", "WEB", "WES", "WFD",
                "WHC", "WIG", "WLD", "WLF", "WLL", "WOR", "WOW", "WPL", "WSA", "WTP", "XIP", "XRO", "YAL", "YOW", "ZEL",
                "ZIM"
            };

            List<string> ReturnTicks = new List<string>();
            foreach (string v in Ticks)
            {
                ReturnTicks.Add(v + ".AX");
            }

            return ReturnTicks;
        }

        public static List<string> testTicker()
        {
            string Tick = "CBA.AX";
            List<string> testTick = new List<string>();
            testTick.Add(Tick);
            return testTick;
        }

        public static List<Stock> Try2(List<string> Ticks)
        {
            List<Stock> Market = new List<Stock>();




            foreach (var v in Ticks)
            {
                Stock TempStock = new Stock();
                TempStock.History = new List<TradingDay>();

                HistQuotesDownload dl = new HistQuotesDownload();

                dl.Settings.IDs = new string[] {v};
                dl.Settings.FromDate = new DateTime(2010, 1, 1);
                dl.Settings.ToDate = DateTime.Today;
                dl.Settings.Interval = HistQuotesInterval.Daily;

                Response<HistQuotesResult> resp = dl.Download();

                foreach (HistQuotesDataChain hqc in resp.Result.Chains)
                {
          
                    foreach (HistQuotesData hqd in hqc)
                    {
                        TradingDay tempday = new TradingDay(hqd.TradingDate.ToLocalTime(), hqd.Open, hqd.Close, hqd.High,
                            hqd.Low, hqd.CloseAdjusted, hqd.PreviousClose, hqd.Volume);
                        TempStock.History.Add(tempday);
   
                    }

                }

                TempStock.Ticker = v;
                TempStock.SortDates();
                Market.Add(TempStock);
            }

            return Market;
        }

        public static StockContext CreateDB(List<Stock> Stocks)
        {

                var tab = new StockContext();
            foreach (var c in Stocks)
            {
                tab.Technicals.Attach(c);

                foreach (var x in c.History)
                {

                    tab.MarketHistory.Attach(x);


                }

            }

                tab.SaveChanges();


            return tab;
        }
    }
}
