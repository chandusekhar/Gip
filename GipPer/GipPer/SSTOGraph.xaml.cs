using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GipPer
{
    /// <summary>
    /// Interaction logic for SSTOGraph.xaml
    /// </summary>
    public partial class SSTOGraph : UserControl
    {
        public SSTOGraph(List<KeyValuePair<int, double>> K, List<KeyValuePair<int, double>> D)
        {
            InitializeComponent();

            ((LineSeries)mcChart.Series[0]).ItemsSource = K.ToArray();
            ((LineSeries)mcChart.Series[1]).ItemsSource = D.ToArray();
        }
    }
}
