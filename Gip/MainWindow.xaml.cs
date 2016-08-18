using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xaml.Schema;
using TicTacTec.TA.Library;
using System.Windows.Controls.DataVisualization.Charting;

namespace Gip
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
            
            Startup NewProgram = new Startup();

            AllStocks = NewProgram.InitialiseStocks(true);

            //List<TradingDay> T = new List<TradingDay>();
            //foreach(var v in AllStocks)
            //{
            //    T.AddRange(v.History.Where(x => x.ClosingPrice == 0));

            //}

            //System.IO.StreamWriter t = new System.IO.StreamWriter(@"C:\Temp\errors.xml");
            
            //System.Xml.Serialization.XmlSerializer ty = new System.Xml.Serialization.XmlSerializer(T.GetType());
            //ty.Serialize(t,T);
            //t.Close();


            StockList.ItemsSource = AllStocks.Select(x => x.Ticker).Distinct();
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
            Startup n =new Startup();
            List<Stock> AllStock = n.InitialiseStocks(false);
            AllStocks = n.UpdatePrices(AllStock);
        }

        private void CalcStoic_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Stock CalcMe = AllStocks.Where(x => x.Ticker == StockList.Text).First();
            
                int begid = -1;
                int noble = -1;
                Core.RetCode c;
                CalcMe.tawrap(Convert.ToInt32(SSk.Text), Convert.ToInt32(SSd.Text), Convert.ToInt32(slowSSd.Text), Date.SelectedDate.Value, Convert.ToInt32(MAPeriod.Text),out begid,out noble, out c);

                StoichKresult.Content = begid;
                StoichDresult.Content = noble;

                Indicators f = new Indicators();
                f.ClosingPrice = new Dictionary<DateTime, double>();
                for(int i = Convert.ToInt32(MAPeriod.Text) - 1 ; i >= 0; i--)
                {
                    if (!f.ClosingPrice.ContainsKey(CalcMe.History[i].TradeDate))
                    {
                        f.ClosingPrice.Add(CalcMe.History[i].TradeDate, CalcMe.History[i].ClosingPrice);
                    }              
                }

                Chart p = new Chart();

                graph.Children.Add(p);

            }
            catch
            {
                System.Windows.MessageBox.Show("oops");
            }
            
        }

        private void CalcMA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stock CalcMe = AllStocks.Where(x => x.Ticker == StockList.Text).First();
                SMAResult.Content = CalcMe.GetSMA(Convert.ToInt32(MAPeriod.Text), Date.SelectedDate.Value);
                EMAResult.Content = CalcMe.GetEMA(Convert.ToInt32(MAPeriod.Text), Date.SelectedDate.Value);
                List<double> Plah = new List<double>();
                Plah = CalcMe.GetMACD(26, 12, 9, Date.SelectedDate.Value);

            }
            catch
            {
                System.Windows.MessageBox.Show("oops");
            }
        }
    }
}
