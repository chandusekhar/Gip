using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<bool> IsPivotHigh { get; set; }
        public List<bool> IsPivotLow { get; set; }


        public TradingPeriod(DateTime day, double high, double low, double open, double close, double adjclose, long volume)
        {
            IsPivotHigh = new List<bool>(){false, false,false,false};
            IsPivotLow = new List<bool>() {false, false, false, false};

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
            IsPivotHigh = new List<bool>() { false, false, false, false };
            IsPivotLow = new List<bool>() { false, false, false, false };
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

        public void GetPivots(Stock.Interval Period)
        {
            List<TradingPeriod> TempList = new List<TradingPeriod>();
            List<TradingPeriod> Hist = new List<TradingPeriod>();

            switch (Period)
            {
                    case Interval.Day:
                    Hist = DailyHist;
                    break;
                    case Interval.Week:
                    Hist = WeeklyHist;
                    break;
                    case Interval.Month:
                    Hist = MonthlyHist;
                    break;
            }


            for (int i = 1; i < Hist.Count -1; i++)
            {
                if (Hist[i].High > Hist[i - 1].High && Hist[i].High > Hist[i + 1].High) Hist[i].IsPivotHigh[0] = true;
                if (Hist[i].Low < Hist[i - 1].Low && Hist[i].Low < Hist[i + 1].Low) Hist[i].IsPivotLow[0] = true;
            }

            TempList = Hist.Where(x => x.IsPivotHigh[0]).ToList();

            for (int i = 1; i < TempList.Count - 1; i++)
            {
                if (TempList[i].High > TempList[i - 1].High && TempList[i].High > TempList[i + 1].High)
                {
                    Hist[Hist.IndexOf(TempList[i])].IsPivotHigh[1] = true;
                    Hist[Hist.IndexOf(TempList[i])].IsPivotHigh[0] = false;
                 }
            }

            TempList = Hist.Where(x => x.IsPivotHigh[1]).ToList();

            for (int i = 1; i < TempList.Count - 1; i++)
            {
                if (TempList[i].High > TempList[i - 1].High && TempList[i].High > TempList[i + 1].High)
                {
                    Hist[Hist.IndexOf(TempList[i])].IsPivotHigh[2] = true;
                    Hist[Hist.IndexOf(TempList[i])].IsPivotHigh[1] = false;
                 }
            }

            TempList = Hist.Where(x => x.IsPivotLow[0]).ToList();

            for (int i = 1; i < TempList.Count - 1; i++)
            {
                if (TempList[i].Low < TempList[i - 1].Low && TempList[i].Low < TempList[i + 1].Low)
                {
                    Hist[Hist.IndexOf(TempList[i])].IsPivotLow[1] = true;
                    Hist[Hist.IndexOf(TempList[i])].IsPivotLow[0] = false;
                }
            }

            TempList = Hist.Where(x => x.IsPivotLow[1]).ToList();

            for (int i = 1; i < TempList.Count - 1; i++)
            {
                if (TempList[i].Low < TempList[i - 1].Low && TempList[i].Low < TempList[i + 1].Low)
                {
                    Hist[Hist.IndexOf(TempList[i])].IsPivotLow[2] = true;
                    Hist[Hist.IndexOf(TempList[i])].IsPivotLow[1] = false;
                }
            }

            switch (Period)
            {
                case Interval.Day:
                    DailyHist = Hist;
                    break;
                case Interval.Week:
                    WeeklyHist = Hist;
                    break;
                case Interval.Month:
                    MonthlyHist = Hist;
                    break;
            }


        }

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

                    GetPivots(Interval.Day);

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
                    GetPivots(Interval.Week);
                    
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
                    GetPivots(Interval.Month);

                    break;
            }

        }

    }
}
