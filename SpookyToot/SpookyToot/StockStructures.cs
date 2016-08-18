using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpookyToot
{
    public class TradingPeriod
    {
        public DateTime Day { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double AdjClose { get; set; }
        public long Volume { get; set; }

        public TradingPeriod(DateTime day, double high, double low, double open, double close, double adjclose, long volume)
        {
            Day = day;
            High = high;
            Low = low;
            Open = open;
            Close = close;
            AdjClose = adjclose;
            Volume = volume;
        }

        public TradingPeriod()
        {

        }

    }

    public class Stock
    {
        public enum Interval
        {
            Day,
            Week,
            Month,
        };

        public List<TradingPeriod> DailyHist { get; set; }
        public List<TradingPeriod> WeeklyHist { get; set; }
        public List<TradingPeriod> MonthlyHist { get; set; }
        public string StockName { get; set; }

        public void BuildStockHist(StreamReader YahooData, string Ticker, Stock.Interval Period)
        {
            switch (Period)
            {
                case Interval.Day:

                    StockName = Ticker;
                    DailyHist = new List<TradingPeriod>();

                    YahooData.ReadLine();

                    while (!YahooData.EndOfStream)
                    {
                        string Temp = YahooData.ReadLine();
                        string[] TmpArry = Temp.Split(',');
                        double open;
                        double high;
                        double low;
                        double close;
                        double adjclose;
                        long volume;

                        double.TryParse(TmpArry[1], out open);
                        double.TryParse(TmpArry[2], out high);
                        double.TryParse(TmpArry[3], out low);
                        double.TryParse(TmpArry[4], out close);
                        long.TryParse(TmpArry[5], out volume);
                        double.TryParse(TmpArry[6], out adjclose);

                        TradingPeriod TmpDay = new TradingPeriod()
                        {
                            Day = Convert.ToDateTime(TmpArry[0]).ToLocalTime(),
                            Open = open,
                            High = high,
                            Low = low,
                            Close = close,
                            Volume = volume,
                            AdjClose = adjclose
                        };

                        DailyHist.Add(TmpDay);
                    }
                    break;

                case Interval.Week:

                    StockName = Ticker;
                    WeeklyHist = new List<TradingPeriod>();

                    YahooData.ReadLine();

                    while (!YahooData.EndOfStream)
                    {
                        string Temp = YahooData.ReadLine();
                        string[] TmpArry = Temp.Split(',');
                        double open;
                        double high;
                        double low;
                        double close;
                        double adjclose;
                        long volume;

                        double.TryParse(TmpArry[1], out open);
                        double.TryParse(TmpArry[2], out high);
                        double.TryParse(TmpArry[3], out low);
                        double.TryParse(TmpArry[4], out close);
                        long.TryParse(TmpArry[5], out volume);
                        double.TryParse(TmpArry[6], out adjclose);

                        TradingPeriod TmpWeek = new TradingPeriod()
                        {
                            Day = Convert.ToDateTime(TmpArry[0]).ToLocalTime(),
                            Open = open,
                            High = high,
                            Low = low,
                            Close = close,
                            Volume = volume,
                            AdjClose = adjclose
                        };

                        WeeklyHist.Add(TmpWeek);
                    }
                    break;

                case Interval.Month:

                    StockName = Ticker;
                    MonthlyHist = new List<TradingPeriod>();

                    YahooData.ReadLine();

                    while (!YahooData.EndOfStream)
                    {
                        string Temp = YahooData.ReadLine();
                        string[] TmpArry = Temp.Split(',');
                        double open;
                        double high;
                        double low;
                        double close;
                        double adjclose;
                        long volume;

                        double.TryParse(TmpArry[1], out open);
                        double.TryParse(TmpArry[2], out high);
                        double.TryParse(TmpArry[3], out low);
                        double.TryParse(TmpArry[4], out close);
                        long.TryParse(TmpArry[5], out volume);
                        double.TryParse(TmpArry[6], out adjclose);

                        TradingPeriod TmpWeek = new TradingPeriod()
                        {
                            Day = Convert.ToDateTime(TmpArry[0]).ToLocalTime(),
                            Open = open,
                            High = high,
                            Low = low,
                            Close = close,
                            Volume = volume,
                            AdjClose = adjclose
                        };

                        MonthlyHist.Add(TmpWeek);
                    }
                    break;
            }

        }

    }
}
