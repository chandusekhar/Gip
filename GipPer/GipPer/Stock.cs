using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Text;
using MaasOne.Finance;
using MaasOne.Base;
using MaasOne.Finance.YahooFinance;
using System.ComponentModel.DataAnnotations.Schema;
using TicTacTec.TA.Library;
using System.Threading.Tasks;

namespace GipPer
{
    public class Startup
    {
        public List<Stock> UpdatePrices(List<Stock> Current)
        {
            Dictionary<string, DateTime> UpdateMe = new Dictionary<string, DateTime>();
            int count = -1;

            foreach (var v in Current)
            {
                DateTime d = v.DailyHistory.Max(x => x.TradeDate);
                if ((DateTime.Now.Ticks - d.Ticks) > TimeSpan.FromHours(24).Ticks)
                {
                    UpdateMe.Add(v.Ticker, d.AddDays(1));
                }
            }

            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename=C:\Users\Ben Roberts\Dropbox\WOERK.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;";

            con.Open();
            DbCommand t = con.CreateCommand();
            t.Connection = con;
            t.CommandText = "SELECT MAX(Count) From StockHist;";
            using (DbDataReader dr = t.ExecuteReader())
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        object c = dr.GetValue(0);
                        count = (int) c;
                    }
                }
                else return null;

            List<TradingDay> UpdatedStocks = new List<TradingDay>();
            foreach (var m in UpdateMe)
            {

                UpdatedStocks.AddRange(GetData.DownloadStocksUsingString(m.Key, m.Value));
            }

            for (int i = 0; i < UpdatedStocks.Count; i++)
            {
                UpdatedStocks[i].Count = count + 1;
                count++;
            }

            var context = new StockContext();
            context.St1.AddRange(UpdatedStocks);
            context.SaveChanges();

            List<Stock> NewStocks = new List<Stock>();

            NewStocks = this.InitialiseStocks(false);
            return NewStocks;

        }

        public List<Stock> InitialiseStocks(bool test)
        {

            List<Stock> Result = new List<Stock>();

            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename=C:\Temp\WOERK.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;";

            con.Open();
            DbCommand t = con.CreateCommand();

            if (test)
            {
                Result.Add(new Stock());
                Result.Add(new Stock());
                Result.Add(new Stock());
                Result.Add(new Stock());
                Result[0].Ticker = "SGH.AX";
                Result[1].Ticker = "BHP.AX";
                Result[2].Ticker = "EGH.AX";
                Result[3].Ticker = "RFG.AX";


            }
            else
            {

                t.Connection = con;
                t.CommandText = "SELECT DISTINCT Ticker FROM StockHist;";

                using (DbDataReader dr = t.ExecuteReader())
                {
                    int i = 0;
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Stock temp = new Stock();
                            object c = dr.GetValue(0);
                            temp.Ticker = (string) c;
                            Result.Add(temp);

                            i++;
                        }
                    }
                }
            }

            foreach (var c in Result)
            {

                c.DailyHistory = new List<TradingDay>();
                string temp = "SELECT * FROM StockHist WHERE Ticker = '" + c.Ticker + "';";
                t.CommandText = temp;
                using (DbDataReader dr = t.ExecuteReader())
                {
                    int i = 0;
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            object x = dr.GetValue(2);
                            DateTime date = (DateTime) dr.GetValue(2);
                            double Opening = (double) dr.GetValue(3);
                            double Closing = (double) dr.GetValue(4);
                            double High = (double) dr.GetValue(5);
                            double Low = (double) dr.GetValue(6);
                            int Volume = (int) dr.GetValue(7);
                            double PrevClose = (double) dr.GetValue(9);
                            double AdjClose = (double) dr.GetValue(10);

                            TradingDay tempDay = new TradingDay(c.Ticker, i, date, Opening, Closing, High, Low, AdjClose,
                                PrevClose, Volume);
                            c.DailyHistory.Add(tempDay);

                            i++;
                        }
                    }
                }
                //c.SortDates();
            }
            //CompanyFinancialHighlights p = new CompanyFinancialHighlights();
            //p.ss
            return Result;
        }
    }

    public class Stock
    {
        public string Ticker { get; set; }
        public List<TradingDay> DailyHistory { get; set; }
        public List<TradingDay> WeeklyHistory { get; set; }
        public Indicators indicate;

        public List<double> TestEmaFormat = new List<double>();
        public List<double> TestRSIFormat = new List<double>();
        public int CWP;
        public int CDP;

        public int GetIndexDaily(DateTime date)
        {
            TradingDay mod = DailyHistory.OrderBy(x => Math.Abs(x.TradeDate.Ticks - date.Ticks)).First();
            int index = DailyHistory.IndexOf(mod);
            return index;
        }

        public int GetIndexWeekly(DateTime date)
        {
            TradingDay mod = WeeklyHistory.OrderBy(x => Math.Abs(x.TradeDate.Ticks - date.Ticks)).First();
            int index = WeeklyHistory.IndexOf(mod);
            return index;
        }


        public void SortDates()
        {
            DailyHistory.Reverse();
        }

        public void GetWeeklyAdjustment()
        {
            List<TradingDay> ReturnedValue = new List<TradingDay>();
            this.WeeklyHistory = new List<TradingDay>();

            ReturnedValue.AddRange(this.DailyHistory.Where((x, index) => (index + 1) % 5 == 0));

            WeeklyHistory.AddRange(ReturnedValue);
        }

        public List<double> CleanTicTac(List<double> input)
        {
            double[] Shift = new double[input.Count];
            int count = 0;
            int otherCount = input.Count - 1;
            for (int i = input.Count - 1; i >= 0; i--)
            {
                if (input[i] != 0)
                {
                    Shift[otherCount] = input[i];
                    otherCount--;
                }
                else count++;
            }

            List<double> V = new List<double>();
            for (int i = 0; i < Shift.Length; i++)
            {
                V.Add(Shift[i]);
            }

            //for (int i = V.Count - 1; i >= 0; i--)
            //{
            //    if (V[i] == 0) V.RemoveAt(i);
            //}
            return V;
        }
    }



    [Table("StockHist")]
    public class TradingDay
    {
        [Column("Ticker")]
        public string Ticker { get; set; }

        [Key]
        [Column("Count")]
        public int Count { get; set; }

        [Column("Date")]
        public DateTime TradeDate { get; set; }

        [Column("Opening Price")]
        public double OpeningPrice { get; set; }

        [Column("Closing Price")]
        public double ClosingPrice { get; set; }

        [Column("High")]
        public double High { get; set; }

        [Column("Low")]
        public double Low { get; set; }

        [Column("Volume")]
        public int Volume { get; set; }

        [Column("Daily Change")]
        public double DailyChange { get; set; }

        [Column("Previous Close")]
        public double PrevClose { get; set; }

        [Column("Adjusted Close")]
        public double AdjustedClose { get; set; }

        public double MidPrice { get; set; }

        public TradingDay()
        {

        }

        public TradingDay(string tik, int coun, DateTime date, double open, double close, double high, double low,
            double AdjCl, double PrCl, int volume)
        {
            Count = coun;
            Ticker = tik;
            TradeDate = date;
            OpeningPrice = open;
            ClosingPrice = close;
            High = high;
            Low = low;
            AdjustedClose = AdjCl;
            PrevClose = PrCl;
            Volume = volume;
            MidPrice = Math.Abs((open - close)/2.0 + open);
            if (PrevClose > 0) DailyChange = ((PrevClose - ClosingPrice)/PrevClose)*100;
        }



    }
}



