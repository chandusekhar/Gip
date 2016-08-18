using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Common;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.ComponentModel.DataAnnotations.Schema;
using TicTacTec.TA.Library;
using System.Reflection;

namespace Gip
{
    public class Startup
    {
        public List<Stock> UpdatePrices(List<Stock> Current)
        {
            Dictionary<string,DateTime> UpdateMe = new Dictionary<string, DateTime>();
            int count = -1;

            foreach (var v in Current)
            {
                DateTime d = v.History.Max(x => x.TradeDate);
                if ((DateTime.Now.Ticks - d.Ticks) > TimeSpan.FromHours(24).Ticks)
                {
                    UpdateMe.Add(v.Ticker,d);
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
                        count = (int)c;
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
                          AttachDbFilename=C:\Users\Ben Roberts\Dropbox\WOERK.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;";

            con.Open();
            DbCommand t = con.CreateCommand();

            if (test)
            {
                Result.Add(new Stock());
                Result.Add(new Stock());
                Result[0].Ticker = "CBA.AX";
                Result[1].Ticker = "BHP.AX";
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
                            temp.Ticker = (string)c;
                            Result.Add(temp);

                            i++;
                        }
                    }
                }
            }

            foreach (var c in Result)
            {

                c.History = new List<TradingDay>();
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
                            DateTime date = (DateTime)dr.GetValue(2);
                            double Opening = (double)dr.GetValue(3);
                            double Closing = (double)dr.GetValue(4);
                            double High = (double)dr.GetValue(5);
                            double Low = (double)dr.GetValue(6);
                            int Volume = (int)dr.GetValue(7);
                            double PrevClose = (double)dr.GetValue(9);
                            double AdjClose = (double)dr.GetValue(10);

                            TradingDay tempDay = new TradingDay(c.Ticker, i, date, Opening, Closing, High, Low, AdjClose,
                                PrevClose, Volume);
                            c.History.Add(tempDay);

                            i++;
                        }
                    }
                }
                c.SortDates();
            }
                return Result;
            }
    }

    public class Stock
    {
        public string Ticker { get; set; }
        public List<TradingDay> History { get; set; }
        public List<double> Stoick { get; set; }
        public List<double> Stoicd { get; set; }

        public void tawrap(int fastk,int k, int d, DateTime from, int period, out int begid, out int noblemen, out Core.RetCode b)
        {
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - from.Ticks)).First();
            int index = History.IndexOf(mod);

            double[] Highs = History.Select(x => x.High).ToArray();
            double[] Lows = History.Select(x => x.Low).ToArray();
            double[] Close = History.Select(x => x.ClosingPrice).ToArray();
            int endindex = index + period;

            int begidx;
            int NBElement;
            double[] slowk = new double[endindex];
            double[] slowd = new double[endindex];
            Core.RetCode a = TicTacTec.TA.Library.Core.Stoch(index, endindex, Highs, Lows, Close, fastk, k, Core.MAType.Ema, d, Core.MAType.Ema, out begidx, out NBElement, slowk, slowd);

            begid = begidx;
            noblemen = NBElement;
            b = a;

            Stoick = slowk.ToList();
            Stoicd = slowd.ToList();

            System.IO.StreamWriter t = new System.IO.StreamWriter(@"C:\Temp\Res.csv");


            for (int i = 0; i < endindex; i++)
            {
                string wrietline = Environment.NewLine + slowk[i] + " ," + slowd[i];
                t.Write(wrietline);
            }
        }


        public List<double> GetMACD(int LongPeriod, int ShortPeriod, int Signal, DateTime From)
        {
            List<double> result = new List<double>();
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - From.Ticks)).First();
            int index = History.IndexOf(mod);
            double l = GetEMA(LongPeriod, From);
            double s = GetEMA(ShortPeriod, From);
            double sig = GetEMA(Signal, From);

            

            result.Add(l);
            result.Add(s);
            result.Add(sig);
            return result;

        }


        public List<double> GetSSTO(int kperiod, int dperiod, DateTime Fromdte, bool useEMA)
        {
            List<double> result = new List<double>();
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - Fromdte.Ticks)).First();
            int index = History.IndexOf(mod);
            double k = SSTOk(kperiod, History[index].TradeDate);
            double d;
            if (useEMA) d = SSTOdEMA(dperiod, kperiod, History[index].TradeDate);
            else d = SSTOd(dperiod, kperiod, History[index].TradeDate);
            result.Add(k);
            result.Add(d);

            

            if (useEMA)
            {
                double Period = Convert.ToDouble(dperiod);
                double Weight = (2.0 / (Period + 1.0));
                double numerator = d;
                double denomn = 1.0;

                for (int i = dperiod; i >= 1; i--)
                {
                    numerator += Math.Pow((1.0 - Weight), i) * SSTOdEMA(dperiod,kperiod, History[i + index].TradeDate);
                    denomn += Math.Pow((1.0 - Weight), i);
                }
                double EMA = numerator / denomn;

                d = EMA; 

            }
            else
            {
                for (int i = 1; i < dperiod; i++)
                {
                    d += SSTOd(dperiod, kperiod, History[index + i].TradeDate);
                }

                d = d / dperiod;
            }

            result.Add(d);          
            
            return result;          
        }

        private double SSTOk(int kperiod, DateTime FromDate)
        {
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - FromDate.Ticks)).First();
            int index = History.IndexOf(mod);

            double High = 0;
            double Low = 99999999;
            for(int i = 0; i< kperiod; i++)
            {
                if (History[i + index].Low < Low) Low = History[i + index].Low;
                if (History[i + index].High > High) High = History[i + index].High;
            }

            

            double K = 100* ((History[0 + index].AdjustedClose) - Low)/(High - Low);
            return K;
        }

        private double SSTOd(int dPeriod, int kPeriod, DateTime FromDate)
        {
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - FromDate.Ticks)).First();
            int index = History.IndexOf(mod);
            double Ks = new double();

            for(int i = 0; i < dPeriod; i++)
            {
                Ks += SSTOk(kPeriod, History[i + index].TradeDate);
            }
            Ks = Ks / dPeriod;
            return Ks;
        }

        private double SSTOdEMA(int dPeriod, int kPeriod, DateTime FromDate)
        {
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - FromDate.Ticks)).First();
            int index = History.IndexOf(mod);
            double Period = Convert.ToDouble(dPeriod);
            double Weight = (2.0 / (Period + 1.0));

            double numerator = SSTOk(kPeriod, History[index].TradeDate);
            double denomn = 1.0;

            for (int i = dPeriod; i >= 1; i--)
            {
                numerator += Math.Pow((1.0 - Weight), i) * SSTOk(kPeriod,History[i + index].TradeDate);
                denomn += Math.Pow((1.0 - Weight), i);
            }
            double EMA = numerator / denomn;

            return EMA;
        }

        public double GetSMA(int period, DateTime FromDate)
        {
            double SMA = 0.0;
            double Period = Convert.ToDouble(period);
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - FromDate.Ticks)).First();
            int index = History.IndexOf(mod);

            for (int i = 0; i < period; i++)
            {
                SMA += History[i + index].AdjustedClose;
            }

            SMA = SMA / Period;

            return SMA;
        }

        public double GetEMA(int period, DateTime FromDate)
        {
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - FromDate.Ticks)).First();
            int index = History.IndexOf(mod);
            double Period = Convert.ToDouble(period);

            double Weight = (2.0 / (Period + 1.0));

            double numerator = History[index].AdjustedClose;
            double denomn = 1.0;

            for (int i = period; i >= 1; i--)
            {
                numerator += Math.Pow((1.0 - Weight), i) * History[i + index].AdjustedClose;
                denomn += Math.Pow((1.0 - Weight), i);
            }
            double EMA = numerator / denomn;

            return EMA;             
        }

        public void SortDates()
        {
            History.Reverse();
        }
    }

    [Table("StockHist")]
    public class TradingDay
    {
        [Column("Ticker")]
        public  string Ticker { get; set; }
        [Key]
        [Column("Count")]
        public int Count { get; set; }    
        [Column("Date")] 
        public  DateTime TradeDate { get; set; }
        [Column("Opening Price")]
        public  double OpeningPrice { get; set; }
        [Column("Closing Price")]
        public  double ClosingPrice { get; set; }
        [Column("High")]
        public  double High { get; set; }
        [Column("Low")]
        public  double Low { get; set; }
        [Column("Volume")]
        public  int Volume { get; set; }
        [Column("Daily Change")]
        public  double DailyChange { get; set; }
        [Column("Previous Close")]
        public  double PrevClose { get; set; }
        [Column("Adjusted Close")]
        public  double AdjustedClose { get; set; }

        public TradingDay()
        {

        }

        public TradingDay(string tik,int coun,DateTime date, double open, double close, double high, double low, double AdjCl, double PrCl, int volume)
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
            if (PrevClose > 0) DailyChange = ((PrevClose - ClosingPrice) / PrevClose) * 100;
        }
    }

    public class Indicators
    {
        public Dictionary<DateTime, double> ClosingPrice { get; set; }

        public static readonly DependencyProperty ClosingPriceProperty =
        DependencyProperty.RegisterAttached("ClosingPrice",
        typeof(Dictionary<DateTime, double>),
        typeof(Indicators),
        new PropertyMetadata(CallBackWhenPropertyIsChanged));

        // Called when Property is retrieved
        public static Dictionary<DateTime, double> GetClosingPrice(DependencyObject obj)
        {
            return obj.GetValue(ClosingPriceProperty) as Dictionary<DateTime, double>;
        }

        // Called when Property is set
        public static void SetClosingPrice(
           DependencyObject obj,
            Dictionary<DateTime, double> value)
        {
            obj.SetValue(ClosingPriceProperty, value);
        }

        // Called when property is changed
        private static void CallBackWhenPropertyIsChanged(
         object sender,
         DependencyPropertyChangedEventArgs args)
        {
            var attachedObject = sender as Indicators;
            if (attachedObject != null)
            {
                // do whatever is necessary, for example
                // attachedObject.CallSomeMethod( 
                // args.NewValue as TargetPropertyType);
            }
        }
    }
}
