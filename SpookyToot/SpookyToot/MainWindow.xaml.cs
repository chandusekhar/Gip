﻿using System;
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            var a = ((CheckBox)sender).DataContext as GraphOverlays;
            if (a != null)
            {
                var b = this.DataContext as GraphControl;

                if (a.Period == Stock.Interval.Day)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.PointAnnotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewDaily.Model.Annotations.Add(t);
                    }
                    DailyView.Model.InvalidatePlot(true);
                }
                if (a.Period == Stock.Interval.Week)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.PointAnnotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewWeekly.Model.Annotations.Add(t);
                    }
                    WeeklyMonthlyView.Model.InvalidatePlot(true);

                }
                if (a.Period == Stock.Interval.Month)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.PointAnnotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewMonthly.Model.Annotations.Add(t);
                    }
                    WeeklyMonthlyView.Model.InvalidatePlot(true);

                }

            }

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var a = ((CheckBox)sender).DataContext as GraphOverlays;
            if (a != null)
            {
                var b = this.DataContext as GraphControl;

                if (a.Period == Stock.Interval.Day)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.PointAnnotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewDaily.Model.Annotations.Remove(t);
                    }
                    DailyView.Model.InvalidatePlot(true);

                }
                if (a.Period == Stock.Interval.Week)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.PointAnnotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewWeekly.Model.Annotations.Remove(t);
                    }
                    WeeklyMonthlyView.Model.InvalidatePlot(true);

                }
                if (a.Period == Stock.Interval.Month)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.PointAnnotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewMonthly.Model.Annotations.Remove(t);
                    }
                    WeeklyMonthlyView.Model.InvalidatePlot(true);

                }


            }


        }
    }
}

