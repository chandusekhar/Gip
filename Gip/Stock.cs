﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gip
{
    public class Stock
    {
        public string Ticker { get; set; }
        public List<TradingDay> History { get; set; }
        
    }

    public class TradingDay
    {
        public DateTime date { get; set; }
        public double OpeningPrice { get; set; }
        public double ClosingPrice { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
        public double DailyChange { get; set; }
        public double AdjustedClose { get; set; }

        public TradingDay(DateTime date, double open, double close)
        {
            OpeningPrice = open;
            ClosingPrice = close;
            DailyChange = ((close - open) / open) * 100;
        }
        public TradingDay(DateTime date, double open, double close, double high, double low)
        {
            OpeningPrice = open;
            ClosingPrice = close;
            High = high;
            Low = low;
            DailyChange = ((close - open) / open) * 100;
        }
        public TradingDay(DateTime date, double open, double close, double high, double low, double volume)
        {
            OpeningPrice = open;
            ClosingPrice = close;
            High = high;
            Low = low;
            Volume = volume;
            DailyChange = ((close - open) / open) * 100;
        }
    }
}
