using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaasOne.Base;
using MaasOne.Finance;
using MaasOne.Finance.YahooFinance;
using MaasOne;
using MaasOne.Search.BOSS;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.Windows.Forms;


namespace GipPer
{
    public partial class StockContext : DbContext
    {
        public StockContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFileName=C:\Temp\WOERK.mdf;Integrated Security=true;")
        {
            Database.SetInitializer<StockContext>(new CreateDatabaseIfNotExists<StockContext>());
        }

        public DbSet<TradingDay> St1 { get; set; }
    }

    public class StockdbInitialiser : CreateDatabaseIfNotExists<StockContext>
    {
        static Type GetListType(Type type)
        {
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }
            return null;
        }

        public void Seed()
        {
            List<string> Ticks = new List<string>();
            StockContext Db = new StockContext();
            Ticks = GetData.TickerstoGet();
            List<TradingDay> Results = new List<TradingDay>();

            Results.AddRange(GetData.DownloadStocks(Ticks, new DateTime(2010, 1, 1)));

            Db.Database.Create();
            Db.St1.AddRange(Results);
            Db.SaveChanges();

            base.Seed(Db);
        }
    }

    public static class GetData
    {

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

        public static string[] testTicker()
        {
            string[] Tick = { "CBA.AX", "BHP.AX" };

            

            return Tick;
        }

        public static List<TradingDay> DownloadStocks(List<string> Ticks, DateTime From)
        {
            List<TradingDay> Market = new List<TradingDay>();
            int c = 1;

            
            


            HistQuotesDownload dl = new HistQuotesDownload();

            dl.Settings.IDs = Ticks.ToArray();
            dl.Settings.FromDate = From;
            dl.Settings.ToDate = DateTime.Today;
            dl.Settings.Interval = HistQuotesInterval.Daily;

            Response<HistQuotesResult> resp = dl.Download();

            foreach (HistQuotesDataChain hqc in resp.Result.Chains)
            {

                foreach (HistQuotesData hqd in hqc)
                {
                    TradingDay tempday = new TradingDay(hqc.ID, c, hqd.TradingDate.ToLocalTime(), hqd.Open, hqd.Close,
                        hqd.High,
                        hqd.Low, hqd.CloseAdjusted, hqd.PreviousClose, (int)hqd.Volume);

                    c++;
                    Market.Add(tempday);
                }
            }

            return Market;
        }




        public static List<TradingDay> DownloadStocksUsingString(string Ticks, DateTime From)
        {
            List<TradingDay> Market = new List<TradingDay>();
            int c = 1;

            HistQuotesDownload dl = new HistQuotesDownload();

            dl.Settings.IDs = new string[] { Ticks };
            dl.Settings.FromDate = From;
            dl.Settings.ToDate = DateTime.Today;
            dl.Settings.Interval = HistQuotesInterval.Daily;

            Response<HistQuotesResult> resp = dl.Download();

            foreach (HistQuotesDataChain hqc in resp.Result.Chains)
            {

                foreach (HistQuotesData hqd in hqc)
                {
                    TradingDay tempday = new TradingDay(hqc.ID, c, hqd.TradingDate.ToLocalTime(), hqd.Open, hqd.Close,
                        hqd.High,
                        hqd.Low, hqd.CloseAdjusted, hqd.PreviousClose, (int)hqd.Volume);

                    c++;
                    Market.Add(tempday);
                }
            }

            return Market;
        }

        public static void RemoveHolidays()
        {
            //string HolidayFilePath = Path.Combine(Environment.CurrentDirectory, @"Resources\Holidays.txt");
            string HolidayFilePath = @"C:\Users\Ben Roberts\Dropbox\Gip2\GipPer\GipPer\Resources\Holidays.txt";
            string command = "";


            System.IO.Stream q = new FileStream(HolidayFilePath,FileMode.Open);

            StreamReader t = new StreamReader(q,Encoding.ASCII);

            List<DateTime> Holidays = new List<DateTime>();

            while (!t.EndOfStream)
            {
                Holidays.Add(Convert.ToDateTime(t.ReadLine()));
            }

            foreach (var v in Holidays)
            {
                
                command += v.ToShortDateString() + " OR ";
            }

            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename=C:\Users\Ben Roberts\Dropbox\WOERK.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;";

            con.Open();
            DbCommand w = con.CreateCommand();
            w.Connection = con;

            for (int i = 0; i < Holidays.Count; i++)
            {
                w.CommandText = "DELETE FROM StockHist Where Date = '" + Holidays[i].ToShortDateString() + " 10:00:00 AM';"; 
                DbDataReader p = w.ExecuteReader();
                int ps = p.RecordsAffected;
                p.Close();
                w.CommandText = "DELETE FROM StockHist Where Date = '" + Holidays[i].ToShortDateString() + " 11:00:00 AM';";
                DbDataReader s = w.ExecuteReader();
                int yp = s.RecordsAffected;
                s.Close();

            }


        }
    }
}
