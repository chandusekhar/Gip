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
using System.Windows.Controls.DataVisualization.Charting;

namespace ChartTute
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();




            Dictionary<DateTime, int> T = new Dictionary<DateTime, int>();
            T.Add(DateTime.Now, 100);
            T.Add(DateTime.Now.AddMonths(1), 110);
            T.Add(DateTime.Now.AddMonths(2), 150);
            

            ((LineSeries)MyChart.Series[0]).ItemsSource = T;
                

            UserControl1 OtherChart = new UserControl1(T);
            Graph.Children.Add(OtherChart);
    }
    }
}
