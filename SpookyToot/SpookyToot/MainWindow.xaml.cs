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
using OxyPlot;
using OxyPlot.Wpf;

namespace SpookyToot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool WeekShowing = true;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void WeeklyView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (WeekShowing)
                {
                    WeekShowing = false;
                    WeeklyMonthlyView.SetBinding(PlotView.ModelProperty, new Binding("ModelViewMonthly.Model"));
                }
                else
                {
                    WeeklyMonthlyView.SetBinding(PlotView.ModelProperty, new Binding("ModelViewWeekly.Model"));
                    WeekShowing = true;
                }
                WeeklyMonthlyView.Model.InvalidatePlot(true);

            }
        }


    }
}
