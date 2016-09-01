using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace SpookyToot
{
    public class TradingPeriod
    {
        public int Index { get; set; }
        public DateTime Day { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double AdjClose { get; set; }
        public long Volume { get; set; }
        public double ReturnSeries { get; set; }
       
        public List<bool> IsPivotHigh { get; set; }
        public List<bool> IsPivotLow { get; set; }


        public TradingPeriod(DateTime day, double high, double low, double open, double close, double adjclose, long volume)
        {
            IsPivotHigh = new List<bool>() {false, false, false, false};
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
            IsPivotHigh = new List<bool>() {false, false, false, false};
            IsPivotLow = new List<bool>() {false, false, false, false};
        }
    }

    public class Stock
    {
        public enum Interval
        {
            Hour,
            Day,
            Week,
            Month,
        };

        public List<TradingPeriod> HourlyHist { get; set; }
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
                case Interval.Hour:
                    Hist = HourlyHist;
                    break;
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

            for (int i = 1; i < Hist.Count - 1; i++)
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
                    //Hist[Hist.IndexOf(TempList[i])].IsPivotHigh[0] = false;
                }
            }

            TempList = Hist.Where(x => x.IsPivotHigh[1]).ToList();

            for (int i = 1; i < TempList.Count - 1; i++)
            {
                if (TempList[i].High > TempList[i - 1].High && TempList[i].High > TempList[i + 1].High)
                {
                    Hist[Hist.IndexOf(TempList[i])].IsPivotHigh[2] = true;
                    //Hist[Hist.IndexOf(TempList[i])].IsPivotHigh[1] = false;
                }
            }

            TempList = Hist.Where(x => x.IsPivotLow[0]).ToList();

            for (int i = 1; i < TempList.Count - 1; i++)
            {
                if (TempList[i].Low < TempList[i - 1].Low && TempList[i].Low < TempList[i + 1].Low)
                {
                    Hist[Hist.IndexOf(TempList[i])].IsPivotLow[1] = true;
                    //Hist[Hist.IndexOf(TempList[i])].IsPivotLow[0] = false;
                }
            }

            TempList = Hist.Where(x => x.IsPivotLow[1]).ToList();

            for (int i = 1; i < TempList.Count - 1; i++)
            {
                if (TempList[i].Low < TempList[i - 1].Low && TempList[i].Low < TempList[i + 1].Low)
                {
                    Hist[Hist.IndexOf(TempList[i])].IsPivotLow[2] = true;
                    //Hist[Hist.IndexOf(TempList[i])].IsPivotLow[1] = false;
                }
            }
        }

        public async void BuildStockHist(StreamReader YahooData, string Ticker, Stock.Interval Period)
        {
            switch (Period)
            {
                case Interval.Hour:

                    StockName = Ticker;
                    HourlyHist = new List<TradingPeriod>();
                    string a = "";


                    while (!a.Contains("volume:"))
                    {
                        a = YahooData.ReadLine();
                        //YahooData.ReadLine();
                    }
                    YahooData.ReadLine();
                    if (!YahooData.EndOfStream)
                    {
                        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        List<double> Opens = new List<double>();
                        List<double> Highs = new List<double>();
                        List<double> Lows = new List<double>();
                        List<double> Closes = new List<double>();
                        List<long> volumes = new List<long>();
                        int Currenthour = -1;

                        while (!YahooData.EndOfStream)
                        {

                            string Temp = YahooData.ReadLine();
                            string[] TmpArry = Temp.Split(',');
                            double open;
                            double high;
                            double low;
                            double close;
                            long volume;
                            long time;

                            long.TryParse(TmpArry[0], out time);
                            double.TryParse(TmpArry[1], out close);
                            double.TryParse(TmpArry[2], out high);
                            double.TryParse(TmpArry[3], out low);
                            double.TryParse(TmpArry[4], out open);
                            long.TryParse(TmpArry[5], out volume);

                            DateTime Temporig = origin;
                            Temporig = Temporig.AddSeconds(time);
                            if (Currenthour == -1) Currenthour = Temporig.Hour;
                            if (Temporig.Hour == Currenthour)
                            {
                                Opens.Add(open);
                                Highs.Add(high);
                                Lows.Add(low);
                                Closes.Add(close);
                                volumes.Add(volume);
                            }
                            else
                            {
                                Currenthour = Temporig.Hour;
                                TradingPeriod TmpDay = new TradingPeriod()
                                {
                                    Day = Convert.ToDateTime(Temporig).ToLocalTime(),
                                    Open = Opens.First(),
                                    High = Highs.Max(),
                                    Low = Lows.Min(),
                                    Close = Closes.Last(),
                                    Volume = volumes.Sum(),
                                };

                                Opens = new List<double>();
                                Highs = new List<double>();
                                Lows = new List<double>();
                                Closes = new List<double>();
                                volumes = new List<long>();
                                Opens.Add(open);
                                Highs.Add(high);
                                Lows.Add(low);
                                Closes.Add(close);
                                volumes.Add(volume);

                                HourlyHist.Add(TmpDay);
                            }
                        }
                        HourlyHist = HourlyHist.OrderBy(x => x.Day.Ticks).ToList();

                        for (int i = 1; i < HourlyHist.Count; i++)
                        {
                            HourlyHist[i].ReturnSeries = HourlyHist[i].Close/HourlyHist[i - 1].Close - 1;
                        }
                        for (int i = 0; i < HourlyHist.Count; i++)
                        {
                            HourlyHist[i].Index = i + 1;
                        }
                    }

                    break;

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

                    DailyHist = DailyHist.OrderBy(x => x.Day.Ticks).ToList();

                    for (int i = 1; i < DailyHist.Count; i++)
                    {
                        DailyHist[i].ReturnSeries = DailyHist[i].AdjClose/DailyHist[i - 1].AdjClose - 1;
                    }
                    for (int i = 0; i < DailyHist.Count; i++)
                    {
                        DailyHist[i].Index = i + 1;
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

                    WeeklyHist = WeeklyHist.OrderBy(x => x.Day.Ticks).ToList();

                    for (int i = 1; i < WeeklyHist.Count; i++)
                    {
                        WeeklyHist[i].ReturnSeries = WeeklyHist[i].AdjClose/WeeklyHist[i - 1].AdjClose - 1;
                    }
                    for (int i = 0; i < WeeklyHist.Count; i++)
                    {
                        WeeklyHist[i].Index = i + 1;
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
                    MonthlyHist = MonthlyHist.OrderBy(x => x.Day.Ticks).ToList();
                    for (int i = 1; i < MonthlyHist.Count; i++)
                    {
                        MonthlyHist[i].ReturnSeries = MonthlyHist[i].AdjClose/MonthlyHist[i - 1].AdjClose - 1;
                    }
                    for (int i = 0; i < MonthlyHist.Count; i++)
                    {
                        MonthlyHist[i].Index = i + 1;
                    }
                    break;
            }

        }

    }
}
