using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaasOne.Finance.YahooPortfolio;

namespace GipPer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Stock> AllStocks;

        public MainWindow()
        {
            InitializeComponent();
            EndDate.SelectedDate = DateTime.Now;
            StartDate.SelectedDate = DateTime.Now.AddYears(-5);

            Startup NewProgram = new Startup();
            
            AllStocks = NewProgram.InitialiseStocks(false);
            //AllStocks = NewProgram.UpdatePrices(AllStocks);

            TestHolding Benj = new TestHolding();
            Benj.CashHeld = 2000;
            DateTime Start = new DateTime(2014, 01, 01);

            Benj = Strategies.TestThesis(AllStocks, Start, Benj);
            bool Continue = true;
            while (Continue)
            {
                if (Benj.TickHistSell.Last().SellDate < DateTime.Now.AddDays(-10) && Benj.CashHeld > 50)
                {
                    Start = Benj.TickHistSell.Last().SellDate;
                    Benj = Strategies.TestThesis(AllStocks, Start, Benj);
                }
                else Continue = false;
            }
            
            Benj.PrintData();



            StockList.ItemsSource = AllStocks.Select(x => x.Ticker).Distinct();

            //DateTime Recent = new DateTime(2016, 5, 13);
            //DateTime Early = new DateTime(2016, 2, 1);
            //List<double> Results = new List<double>();
            //List<List<double>> ListResults = new List<List<double>>();

            //foreach (var p in AllStocks)
            //{
            //    int r1 = p.GetIndex(Recent);
            //    int e1 = p.GetIndex(Early);
            //    p.indicate.EmaIndicators = new Indicators.Ema(r1, e1,
            //    p.History.Select(x => x.ClosingPrice).ToArray(), 10);
            //}


            //List<KeyValuePair<int, double>> ff = AllStocks[0].indicate.GetIntoChartFormat(AllStocks[0].History.Where(x => x.TradeDate > DateTime.Now.AddYears(-1)).Select(x => (double)x.ClosingPrice).Reverse().ToList());
            //Chart Close = new Chart(ff);
            //closeingP.Children.Add(Close);

            //int optinTimePeriod = 15;
            //int OptinFastPeriod = 5;
            //int OptinSlowPeriod = 20;
            //foreach(var c in AllStocks)
            //{
            //    int recent = c.GetIndex(DateTime.Now);
            //    int early = c.GetIndex(DateTime.Now.AddMonths(-4));
            //    double[] OP = c.History.GetRange(recent, early - recent).Select(x => x.OpeningPrice).ToArray();
            //    double[] CP = c.History.GetRange(recent, early - recent).Select(x => x.ClosingPrice).ToArray();
            //    double[] HP = c.History.GetRange(recent, early - recent).Select(x => x.High).ToArray();
            //    double[] LP = c.History.GetRange(recent, early - recent).Select(x => x.Low).ToArray();
            //    double[] Vol = c.History.GetRange(recent, early - recent).Select(x => (double)x.Volume).ToArray();
            //    c.indicate.AcosIndicators = new Indicators.Acos(recent, early, OP);
            //    c.indicate.ADDIndicators = new Indicators.ADD(recent, early, OP, CP);
            //    c.indicate.ADIndicators = new Indicators.AD(recent, early, HP, LP, CP, Vol);
            //    c.indicate.AdOscIndicators = new Indicators.AdOsc(recent, early, HP, LP, CP, Vol,OptinFastPeriod, OptinSlowPeriod);
            //    c.indicate.AdxRIndicators = new Indicators.AdxR(recent, early, HP, LP, CP, optinTimePeriod);
            //    c.indicate.ApoIndicators = new Indicators.Apo(recent, early, CP, OptinFastPeriod, OptinSlowPeriod);

            //}

        }

        public void ReinitialiseDB()
        {
            System.IO.File.Delete(@"C:\Temp\WOERK.mdf");
            System.IO.File.Delete(@"C:\TEMP\WOERK_log.ldf");
            StockContext b = new StockContext();
            StockdbInitialiser t = new StockdbInitialiser();
            t.Seed();
        }

        public void UpdateDB()
        {
            Startup n = new Startup();
            List<Stock> AllStock = n.InitialiseStocks(false);
            AllStocks = n.UpdatePrices(AllStock);
        }

        private void CalcStoic_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime Recent = EndDate.SelectedDate.Value;
                DateTime Early = StartDate.SelectedDate.Value;
                Stock k = AllStocks.Where(x => x.Ticker == StockList.Text).First();
               
                int P = Convert.ToInt32(MAPeriod.Text);


                k.GetWeeklyAdjustment();

                int recent = k.GetIndexWeekly(Recent);
                int early = k.GetIndexWeekly(Early);

                k.indicate = new Indicators();
                k.indicate.EmaIndicators = new Indicators.Ema(early, recent, k.WeeklyHistory.Select(x => x.ClosingPrice).ToArray(), P);

                SSTOGraph Peter = new SSTOGraph(k.indicate.GetIntoChartFormat(k.WeeklyHistory.GetRange(early, recent - early).Select(x => x.ClosingPrice).ToList()), k.indicate.GetIntoChartFormat(k.indicate.EmaIndicators.EmaHist));
                closeingP.Children.Add(Peter);




                TestHolding Benj = new TestHolding();
                Benj.CashHeld = 2000;
                DateTime Start = new DateTime(2013, 01, 01);

                Benj = Strategies.TestThesis(AllStocks, Start, Benj);
                bool Continue = true;
                while (Continue)
                {
                    if (Benj.TickHistSell.Last().SellDate < DateTime.Now.AddDays(-10) && Benj.CashHeld > 50)
                    {
                        Start = Benj.TickHistSell.Last().SellDate;
                        Benj = Strategies.TestThesis(AllStocks, Start, Benj);
                    }
                    else Continue = false;
                }

                Benj.PrintData();

            }
            catch
            {
                System.Windows.MessageBox.Show("ss", "ss", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            


        }

    }
}
