using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaasOne.Finance.YahooFinance;

namespace Gip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Stock> Results = new List<Stock>();
            List<string> Ticks = new List<string>();

            //Ticks = GetData.TickerstoGet();

            List<string> SmallSample = new List<string>();
            SmallSample.AddRange(GetData.testTicker());


            Results = GetData.Try2(SmallSample);
            //74.25 -> GOOG result
            //period of 4 and using ClosingPrice gets 74.252
            //SMA is perfect
            
            double r = Results[0].GetEMA(4, DateTime.Now);
            double s = Results[0].GetSMA(12, DateTime.Now);
        }
    }
}
