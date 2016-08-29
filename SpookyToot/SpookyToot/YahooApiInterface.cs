using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot.Series;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpookyToot
{
    class YahooApiInterface
    {

        public List<Stock> getYahooData(List<string> Ticker, DateTime FromDate)
        {
            List<Stock> ReturnedStocks = new List<Stock>();

            string url = "", dayFile = "", weekFile = "", directory = "", dayDirectory = "", weekDirectory = "";
            var dayOrWeek = new string[] { "h","d", "w","m" };
            var webClient = new WebClient();


            var urlPrototype = @"http://ichart.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g={7}&ignore=.csv";
            var dayFilePrototype = "{0}_{1}.{2}.{3}-{4}.{5}.{6}.csv";
            var weekFilePrototype = "{0}_{1}.{2}.{3}-{4}.{5}.{6}.csv";

            // Parameters for download
            foreach (var stock in Ticker)
            {
                Stock Temp = new Stock();

                foreach (var dayWeek in dayOrWeek)
                {
                    url = string.Format(urlPrototype, stock, FromDate.Month -1, FromDate.Day, FromDate.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, dayWeek);

                    try
                    {
                        switch (dayWeek)
                        {
                            case "d":
                                var dayFileName = string.Format(dayFilePrototype, stock.ToUpper(), FromDate.Month, FromDate.Day, FromDate.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
                                var readDatadaily = new StreamReader(webClient.OpenRead(url));
                                Temp.BuildStockHist(readDatadaily, stock, Stock.Interval.Day);
                                break;

                            case "w":
                                var weekFileName = string.Format(weekFilePrototype, stock.ToUpper(), FromDate.Month, FromDate.Day, FromDate.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
                                var readDataweekly = new StreamReader(webClient.OpenRead(url));
                                Temp.BuildStockHist(readDataweekly, stock, Stock.Interval.Week);
                                break;

                            case "m":
                                var monthFileName = string.Format(weekFilePrototype, stock.ToUpper(), FromDate.Month, FromDate.Day, FromDate.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
                                var readDatamonthly = new StreamReader(webClient.OpenRead(url));
                                Temp.BuildStockHist(readDatamonthly, stock, Stock.Interval.Month);
                                break;
                            case "h":
                                url = "http://chartapi.finance.yahoo.com/instrument/1.0/" + stock.ToUpper() + "/chartdata;type=quote;range=45d/csv";
                                var readDatahourly = new StreamReader(webClient.OpenRead(url));
                                Temp.BuildStockHist(readDatahourly, stock, Stock.Interval.Hour);
                                break;
                        }
                    }
                    catch
                    {
                       
                    }
                }
                ReturnedStocks.Add(Temp);

            }
            return ReturnedStocks;
        }
    }
}

///// 
///// Every url starts as:

//http://ichart.yahoo.com/table.csv?s=

//Then, it needs a stock name(e.g.: Microsoft):

//http://ichart.yahoo.com/table.csv?s=MSFT

//A From Date(e.g.: 01/01/2000):

//http://ichart.yahoo.com/table.csv?s=MSFT&a=0&b=1&c=2000

//A To Date(e.g.: 24/12/2014):

//http://ichart.yahoo.com/table.csv?s=MSFT&a=0&b=1&c=2000&d=11&e=24&f=2014

//Resolution of the data(daily, weekly, etc):

//http://ichart.yahoo.com/table.csv?s=MSFT&a=0&b=1&c=2000&d=11&e=24&f=2014&g=w

//File format:

//http://ichart.yahoo.com/table.csv?s=MSFT&a=0&b=1&c=2000&d=11&e=24&f=2014&g=w&ignore=.csv
///// 
///// 








