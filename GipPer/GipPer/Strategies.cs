using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace GipPer
{
    static class Strategies
    {
        public static List<Stock> GetPeriodofreturn(List<Stock> Stocks, out Dictionary<string, DateTime> Reslts)
        {
            TimeSpan Timeperiod = new TimeSpan(60,0,0,0);
            double Returns = 1.2;
            List<Stock> Examples = new List<Stock>();
            Reslts = new Dictionary<string, DateTime>();

            foreach (var p in Stocks)
            {
                double CurrentReturn = -1.0;
                DateTime lastdate = new DateTime(2012, 1, 1);
                DateTime CurrentDate = p.DailyHistory.Select(x => x.TradeDate).Max();
                while ((CurrentDate - Timeperiod) > lastdate && CurrentReturn < Returns)
                {             
                    int recent = p.GetIndexDaily(CurrentDate);
                    int early = p.GetIndexDaily(CurrentDate - Timeperiod);

                    CurrentReturn = (p.DailyHistory[recent].ClosingPrice / p.DailyHistory[early].ClosingPrice);
                    if (CurrentReturn > Returns)
                    {
                        Examples.Add(p);
                        Reslts.Add(p.Ticker, CurrentDate - Timeperiod);
                    }
                    else CurrentDate = CurrentDate.AddDays(-1);
                }
            }

            return Examples;
        }

        public static TestHolding TestThesis(List<Stock>  StockMarket, DateTime startTime, TestHolding Holder)
        {


            foreach (var p in StockMarket)
            {
                p.GetWeeklyAdjustment();

                p.CWP = p.GetIndexWeekly(startTime);
                p.CDP = p.GetIndexDaily(startTime);

                int recent = p.GetIndexWeekly(DateTime.Now);
                int early = p.GetIndexWeekly(startTime - TimeSpan.FromDays(365*10));

                int RecDay = p.GetIndexDaily(DateTime.Now);
                int earlDay = p.GetIndexDaily(startTime - TimeSpan.FromDays(365*10));
                
                p.indicate = new Indicators();
                p.indicate.EmaIndicators = new Indicators.Ema(early,recent, p.WeeklyHistory.Select(x=>x.ClosingPrice).ToArray(), 75);
                p.indicate.RSIIndicators = new Indicators.RSI(earlDay,RecDay,p.DailyHistory.Select(x=>(double)x.Volume).ToArray(),14);

                p.TestEmaFormat.AddRange(p.CleanTicTac(p.indicate.EmaIndicators.EmaHist));
                p.TestRSIFormat.AddRange(p.CleanTicTac(p.indicate.RSIIndicators.RSIHist));
            }
            
            
            List<Stock> ValidStocks = new List<Stock>();
            List<Stock> InvestMECandidates = new List<Stock>();

            foreach (var r in StockMarket)
            {
                if (r.TestEmaFormat.Count > r.CWP) ValidStocks.Add(r);
            }

            Stock InvestMe = new Stock();


            while (InvestMECandidates.Count == 0)
            {
                InvestMECandidates = new List<Stock>();
                InvestMECandidates.AddRange(
                    ValidStocks.Where(
                        x =>
                            x.TestEmaFormat[x.CWP] / (x.WeeklyHistory[x.CWP].ClosingPrice) < 1 &&
                            x.TestEmaFormat[x.CWP] / (x.WeeklyHistory[x.CWP].ClosingPrice) > 0.8 
                        ));

                for (int l = InvestMECandidates.Count - 1; l >= 0; l--)
                {
                    List<double> priorrange = new List<double>();
                    bool noZero = true;
                    bool NotEnoughData = false;
                    int zerocount = 0;
                    while (noZero)
                    {
                        if (InvestMECandidates[l].TestEmaFormat[zerocount] != 0 )
                        {
                            noZero = false;
                        }
                        else
                        {
                            if (InvestMECandidates[l].TestEmaFormat.Count - 1 <= zerocount)
                            {
                                NotEnoughData = true;
                                noZero = false;
                            }
                            zerocount++;
                        }
                    }
                    if (!NotEnoughData)
                    {
                        priorrange.AddRange(InvestMECandidates[l].TestEmaFormat.GetRange(zerocount, InvestMECandidates[l].CWP));

                        double gradient = (priorrange.Last() - priorrange.First())/priorrange.Count;
                        if (gradient < -0.05) InvestMECandidates.Remove(InvestMECandidates[l]);
                    }
                    else InvestMECandidates.Remove(InvestMECandidates[l]);
                }


                if (InvestMECandidates.Count == 0)
                {
                    for (int i = ValidStocks.Count - 1; i >= 0; i--)
                    {
                        ValidStocks[i].CWP = ValidStocks[i].CWP + 1;
                        if ( ValidStocks[i].WeeklyHistory.Count - 1 < ValidStocks[i].CWP)
                        {
                            ValidStocks.Remove(ValidStocks[i]);
                        }
                    }
                }
                if (ValidStocks.Count == 0)
                {
                    Holder.Sell("No Opportunities", null, DateTime.Now);
                    return Holder;
                }
            }

            Random randd = new Random();
            int qw = randd.Next(0, InvestMECandidates.Count);
            InvestMe = InvestMECandidates[qw];
            startTime = InvestMe.WeeklyHistory[InvestMe.CWP].TradeDate;
            Holder.Buy(InvestMe,startTime);
            int periodCount = 0;
            double originalEMA = InvestMe.TestEmaFormat[InvestMe.CWP];
            
            double BuyPrice = InvestMe.DailyHistory[InvestMe.CDP].High;

            bool PriceAboveEMa = true;

            bool TakeReturns = true;

            bool StopLoss = true;

            bool c4 = true;

            bool SlowEarner = true;
            bool BigDiff = true;

            startTime = startTime.Add(TimeSpan.FromDays(7));

            while (PriceAboveEMa && TakeReturns && StopLoss && c4 && SlowEarner && BigDiff)
            {
                startTime = startTime.Add(TimeSpan.FromDays(1));
                InvestMe.CDP = InvestMe.GetIndexDaily(startTime);
                InvestMe.CWP = InvestMe.GetIndexWeekly(startTime);


                PriceAboveEMa = InvestMe.WeeklyHistory[InvestMe.CWP].ClosingPrice >
                      (InvestMe.TestEmaFormat[InvestMe.CWP] * 0.9);

                TakeReturns = InvestMe.DailyHistory[InvestMe.CDP].ClosingPrice / BuyPrice <2;

                //c3 = InvestMe.DailyHistory[BuyPeriod].ClosingPrice/
                //     InvestMe.DailyHistory[BuyPeriod + 1].ClosingPrice > 1.005;

                c4 = startTime < DateTime.Now.AddDays(-5);

                //SlowEarner = (InvestMe.DailyHistory[InvestMe.CDP].ClosingPrice - BuyPrice) / periodCount < /*10*/ *(InvestMe.TestEmaFormat[InvestMe.CWP] -originalEMA)/periodCount ;

                BigDiff = InvestMe.DailyHistory[InvestMe.CDP].ClosingPrice / InvestMe.TestEmaFormat[InvestMe.CWP] < 1.3;

                StopLoss = InvestMe.DailyHistory[InvestMe.CDP].ClosingPrice/BuyPrice > 0.9;

                periodCount ++;
            }

            string reason = "";
            if (!PriceAboveEMa) reason += "Closing price was lower than 1 times EMA.  ";
            if (!TakeReturns) reason += "Closing price return exceeded 300%.  ";
            if (!StopLoss) reason += "Closing price was less than 95% of buy price.  ";
            if (!c4) reason += "End of investment period.  ";
            if(!SlowEarner) reason += " Return was too low over the investment period. ";
            if (!BigDiff) reason += " Ratio of close to ema was big";

            Holder.Sell(reason, InvestMe,InvestMe.WeeklyHistory[InvestMe.CWP].TradeDate);

            return Holder;
        }


    }

    public class TestHolding
    {
        public string TickerHeld { get; set; }
        public int SharesHeld { get; set; }
        public double CashHeld { get; set; }
        public int Brokerage { get; set; }

        public List<BuyHist> TickHistBuy = new List<BuyHist>();
        public List<SellHist>TickHistSell = new List<SellHist>();

        public void Buy(Stock Candidate, DateTime CurrentPeriod)
        {
            int BuyPeriod = Candidate.GetIndexDaily(CurrentPeriod);

            Brokerage += 20;
            TickerHeld = Candidate.Ticker;
            SharesHeld = (int)(CashHeld/Candidate.DailyHistory[BuyPeriod].High);
            CashHeld -= (SharesHeld * Candidate.DailyHistory[BuyPeriod].High);
            
            BuyHist t = new BuyHist(CurrentPeriod,Candidate);
            TickHistBuy.Add(t);
        }

        public void Sell(string reason, Stock Candidate, DateTime CurrentPeriod)
        {
            if (Candidate == null)
            {
                SellHist o = new SellHist(reason, CurrentPeriod, null);
                TickHistSell.Add(o);
            }
            else
            {
                int SellPeriod = Candidate.GetIndexDaily(CurrentPeriod);

                Brokerage += 20;
                TickerHeld = "";
                CashHeld += (double)SharesHeld * (Candidate.DailyHistory[SellPeriod].Low);
                SharesHeld = 0;

                SellHist t = new SellHist(reason, CurrentPeriod, Candidate);
                TickHistSell.Add(t);
            }
            
        }

        public class BuyHist
        {
            public DateTime BuyDate { get; set; }
            public double BuyPrice { get; set; }
            public Stock BuyStock { get; set; }

            public string Tick;

            public BuyHist(DateTime Bougth, Stock stock)
            {
                int BuyCount = stock.GetIndexDaily(Bougth);
                BuyDate = Bougth;
                BuyStock = stock;
                Tick = stock.Ticker;
                BuyPrice = stock.DailyHistory[BuyCount].High;
            }

        }

        public class SellHist
        {
            public string Reason { get; set; }
            public DateTime SellDate { get; set; }
            public double SellPrice { get; set; }
            public Stock SellStock { get; set; }

            public string Tick;

            public SellHist(string reason, DateTime Sold, Stock stock)
            {
                if (stock == null)
                {
                    Reason = reason;
                    SellDate = Sold;
                    SellStock = null;
                    SellPrice = 0;
                }
                else
                {
                    int BuyCount = stock.GetIndexDaily(Sold);
                    SellDate = Sold;
                    SellStock = stock;
                    Tick = stock.Ticker;
                    SellPrice = stock.DailyHistory[BuyCount].Low;
                    Reason = reason;
                }
                
            }

        }

        public void PrintData()
        {
            StringBuilder g = new StringBuilder();

            foreach (var t in TickHistBuy)
            {
                g.Append(t.Tick +" " + t.BuyDate + " " + t.BuyPrice );
                g.AppendLine();
            }

            foreach (var t in TickHistSell)
            {
                g.Append(t.Reason + " " + t.Tick + " " + t.SellDate + " " +  t.SellPrice);
                g.AppendLine();
            }

            g.AppendLine("Cash:" + CashHeld + " Brokerage: " + Brokerage);

            Stream p = new FileStream(@"C:\Temp\Results.txt",FileMode.Create,FileAccess.ReadWrite);
            StreamWriter d =new StreamWriter(p,Encoding.ASCII);
            d.Write(g.ToString());
            d.Close();
            p.Close();

        }
    }
}
