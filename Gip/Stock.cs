using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Gip
{

    public class Stock 
    {
        [Key]
        public string Ticker { get; set; }
        public List<TradingDay> History { get; set; }
        public double SSTO { get; set; }
        public double MACD { get; set; }

        public double GetSSTO(int kperiod, int dperiod, DateTime Fromdte)
        {
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - Fromdte.Ticks)).First();
            int index = History.IndexOf(mod);
            double k = SSTOk(kperiod, Fromdte);
            double d = SSTOd(dperiod, kperiod, Fromdte);

            k = k - d;

            return k;               
        }

        public double SSTOk(int kperiod, DateTime FromDate)
        {
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - FromDate.Ticks)).First();
            int index = History.IndexOf(mod);

            double High = 0;
            double Low = 99999999;
            for(int i = 0; i< kperiod; i++)
            {
                if (History[i + index].ClosingPrice < Low) Low = History[i + index].ClosingPrice;
                if (History[i + index].ClosingPrice > High) High = History[i + index].ClosingPrice;
            }
            

            double K = 100 * ((History[0 + index].ClosingPrice) - Low)/(High - Low);
            return K;
        }

        public double SSTOd(int dPeriod, int kPeriod, DateTime FromDate)
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

        public double GetSMA(int period, DateTime FromDate)
        {
            double SMA = 0.0;
            double Period = Convert.ToDouble(period);
            TradingDay mod = History.OrderBy(x => Math.Abs(x.TradeDate.Ticks - FromDate.Ticks)).First();
            int index = History.IndexOf(mod);

            for (int i = 0; i < period; i++)
            {
                SMA += History[i + index].ClosingPrice;
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
            //double Weight = 1-(1)

            double numerator = History[index].ClosingPrice;
            double denomn = 1.0;

            for (int i = period; i >= 1; i--)
            {
                numerator += Math.Pow((1.0 - Weight), i) * History[i + index].ClosingPrice;
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

    public class TradingDay
    {
        [Key]
        public DateTime TradeDate { get; set; }
        public double OpeningPrice { get; set; }
        public double ClosingPrice { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
        public double DailyChange { get; set; }
        public double PrevClose { get; set; }
        public double AdjustedClose { get; set; }

        public TradingDay(DateTime date, double open, double close)
        {
            TradeDate = date;
            OpeningPrice = open;
            ClosingPrice = close;
            if (PrevClose > 0) DailyChange = ((PrevClose - ClosingPrice) / PrevClose) * 100;
        }
        public TradingDay(DateTime date, double open, double close, double high, double low)
        {
            TradeDate = date;
            OpeningPrice = open;
            ClosingPrice = close;
            High = high;
            Low = low;
            if (PrevClose > 0) DailyChange = ((PrevClose - ClosingPrice) / PrevClose) * 100;
        }
        public TradingDay(DateTime date, double open, double close, double high, double low, double AdjCl, double PrCl, double volume)
        {
            TradeDate = date;
            OpeningPrice = open;
            ClosingPrice = close;
            High = high;
            Low = low;
            AdjustedClose = AdjCl;
            PrevClose = PrCl;
            Volume = volume;
            if(PrevClose>0) DailyChange = ((PrevClose - ClosingPrice) / PrevClose) * 100;
        }
    }
}
