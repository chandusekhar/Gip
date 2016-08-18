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

namespace Gip
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public Chart()
        {
            InitializeComponent();

            ((LineSeries)mcChart.Series[0]).ItemsSource = new KeyValuePair<DateTime, int>[]{
        new KeyValuePair<DateTime,int>(DateTime.Now, 100),
        new KeyValuePair<DateTime,int>(DateTime.Now.AddMonths(1), 130),
        new KeyValuePair<DateTime,int>(DateTime.Now.AddMonths(2), 150),
        new KeyValuePair<DateTime,int>(DateTime.Now.AddMonths(3), 125)

        };
        }
    }
}
