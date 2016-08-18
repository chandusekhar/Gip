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
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Shapes;

namespace ChartTute
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1(Dictionary<DateTime, int> G)
        {
            InitializeComponent();
            ((LineSeries)BenChart.Series[0]).ItemsSource = G;
        }
    }
}
