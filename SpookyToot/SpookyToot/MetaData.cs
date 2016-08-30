using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SpookyToot
{
    public  class MetaData
    {
        public ObservableCollection<string> Tickers { get; set; }
        public List<Stock> Cache { get; set; }

        //needs to be new thread

        public void NextTikker()
        {
            string oldTick = Cache[4].StockName;
            int i = Tickers.IndexOf(oldTick) + 1;
            if (i == Tickers.Count) i = 0;

            YahooApiInterface F = new YahooApiInterface();
            List<Stock> G = new List<Stock>();
            G .AddRange(F.getYahooData(new List<string>() {Tickers[i]}, new DateTime(2013, 01, 01)));

            Cache[0] = Cache[1];
            Cache[1] = Cache[2];
            Cache[2] = Cache[3];
            Cache[3] = Cache[4];
            Cache[4] = G[0];
        }

        public void LastTikker()
        {
            string oldTick = Cache[0].StockName;
            int i = Tickers.IndexOf(oldTick) - 1;
            if (i <0) i = Tickers.Count - 1;

            YahooApiInterface F = new YahooApiInterface();
            List<Stock> G = new List<Stock>();
            G.AddRange(F.getYahooData(new List<string>() { Tickers[i] }, new DateTime(2013, 01, 01)));

            Cache[4] = Cache[3];
            Cache[3] = Cache[2];
            Cache[2] = Cache[1];
            Cache[1] = Cache[0];
            Cache[0] = G[0];
        }

        public MetaData()
        {
            Tickers = new ObservableCollection<string>();
            Cache = new List<Stock>();
            if (File.Exists("C:\\Temp\\AllListedCompanies.csv"))
            {
                BackUpData("C:\\Temp\\AllListedCompanies.csv");

                using (Stream stream = File.Open("C:\\Temp\\AllListedCompanies.csv", FileMode.Open))
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
            Cache.AddRange(F.getYahooData(new List<string>() {Tickers[Tickers.Count-2],Tickers[Tickers.Count-1],Tickers[0],Tickers[1],Tickers[2]}, new DateTime(2013, 01, 01)));
        }

        public void LoadData(string path)
        {
            Tickers = new ObservableCollection<string>();
            Cache = new List<Stock>();
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
