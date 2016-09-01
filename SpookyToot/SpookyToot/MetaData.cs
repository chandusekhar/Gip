﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SpookyToot
{
    public class MetaData
    {
        public ObservableCollection<string> Tickers { get; set; }
        public Stock Current { get; set; }
        public Stock Forward { get; set; }
        public Stock FarForward { get; set; }
        public Stock Back { get; set; }
        public Stock Farback { get; set; }

        public void NextTikker()
        {
            string oldTick = FarForward.StockName;

            Forward = FarForward;
            Farback = Back;
            Back = Current;

            FarForward = null;
            int i = Tickers.IndexOf(oldTick) + 1;
            if (i == Tickers.Count) i = 0;

            YahooApiInterface F = new YahooApiInterface();
            List<Stock> G = new List<Stock>();
            G = F.getYahooData(new List<string>() { Tickers[i] }, new DateTime(2013, 01, 01));

            while (G[0].WeeklyHist == null || G[0].HourlyHist == null || G[0].DailyHist == null || G[0].MonthlyHist == null)
            {
                i = Tickers.IndexOf(G[0].StockName);
                Tickers.Remove(G[0].StockName);
                if (i == Tickers.Count) i = 0;
                G = new List<Stock>();
                G.AddRange(F.getYahooData(new List<string>() { Tickers[i] }, new DateTime(2013, 01, 01)));
            }


            FarForward= G[0];
        }

        public void LastTikker()
        {
            string oldTick = Farback.StockName;
            
            FarForward = Forward;
            Forward = Current;
            Back = Farback;
            Farback = null;

            int i = Tickers.IndexOf(oldTick) - 1;
            if (i <0) i = Tickers.Count - 1;

            YahooApiInterface F = new YahooApiInterface();
            List<Stock> G = new List<Stock>();
            G.AddRange(F.getYahooData(new List<string>() { Tickers[i] }, new DateTime(2013, 01, 01)));
            while (G[0].WeeklyHist == null || G[0].HourlyHist == null || G[0].DailyHist == null || G[0].MonthlyHist == null)
            {
                i = Tickers.IndexOf(G[0].StockName);
                Tickers.Remove(G[0].StockName);
                if (i == Tickers.Count) i = 0;
                G = new List<Stock>();
                G.AddRange(F.getYahooData(new List<string>() { Tickers[i] }, new DateTime(2013, 01, 01)));
            }

            Farback = G[0];
        }

        public MetaData()
        {
            Tickers = new ObservableCollection<string>();
            List<Stock> Cache = new List<Stock>();
            string Stocklist = "";

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".csv";
            Nullable<bool> result = dlg.ShowDialog();
            Stocklist = dlg.FileName;

            if (File.Exists(Stocklist))
            {
                BackUpData(Stocklist);

                using (Stream stream = File.Open(Stocklist, FileMode.Open))
                {
                    var p = new StreamReader(stream);
                    while (!p.EndOfStream)
                    {
                        var c = p.ReadLine();
                        c = c + ".AX";
                        Tickers.Add(c);
                    }
                }
            }
            YahooApiInterface F = new YahooApiInterface();
            Cache.AddRange(F.getYahooData(new List<string>() { Tickers[Tickers.Count - 2], Tickers[Tickers.Count - 1], Tickers[0], Tickers[1], Tickers[2] }, new DateTime(2013, 01, 01)));

            Farback = Cache[0];
            Back = Cache[1];
            Current = Cache[2];
            Forward = Cache[3];
            FarForward = Cache[4];
        }

        public void LoadData(string path)
        {
            Tickers = new ObservableCollection<string>();
            List<Stock> Cache = new List<Stock>();
            if (File.Exists(path))
            {
                BackUpData(path);

                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    var p = new StreamReader(stream);
                    while (!p.EndOfStream)
                    {
                        var c = p.ReadLine();
                        c = c + ".AX";
                        Tickers.Add(c);
                    }
                }
            }
            YahooApiInterface F = new YahooApiInterface();
            Cache.AddRange(F.getYahooData(new List<string>() { Tickers[Tickers.Count - 2], Tickers[Tickers.Count - 1], Tickers[0], Tickers[1], Tickers[2] }, new DateTime(2013, 01, 01)));

            Farback = Cache[0];
            Back = Cache[1];
            Current = Cache[2];
            Forward = Cache[3];
            FarForward = Cache[4];
        }

        public void BackUpData(string path)
        {
            if (File.Exists(path))
            {
                File.Copy(path, Path.ChangeExtension(path,".bak"), true);
            }
        }
    }
}
