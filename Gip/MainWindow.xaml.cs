using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            
            

            //

            List<string> SmallSample = new List<string>();
            SmallSample.AddRange(GetData.testTicker());


            //74.25 -> GOOG result
            //period of 4 and using ClosingPrice gets 74.252

            StockContext b = new StockContext();
            StockContext.StockdbInitialiser t = new StockContext.StockdbInitialiser();
            t.Seed();

        }

    }
}
