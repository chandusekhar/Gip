﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool dayShwoing = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DailyView_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.S)
            {
                if (dayShwoing)
                {
                    dayShwoing = false;
                    DailyView.SetBinding(PlotView.ModelProperty, new Binding("ModelViewHourly.Model"));
                }
                else
                {
                    DailyView.SetBinding(PlotView.ModelProperty, new Binding("ModelViewDaily.Model"));
                    dayShwoing = true;
                }
                DailyView.Model.InvalidatePlot(true);

            }
            else if (e.Key == Key.Enter || e.Key == Key.D)
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
            else if (e.Key == Key.A)
            {
                var c = StockWindow.DataContext as GraphControl;
                var d = Market.DataContext as MetaData;

                c.update(d.Cache[1]);

                ThreadPool.QueueUserWorkItem(delegate (object state) {

                    this.Dispatcher.BeginInvoke(new Action(() => {
                        d.LastTikker();

                    }));
                });

                DailyView.SetBinding(PlotView.ModelProperty, new Binding("ModelViewDaily.Model"));
                dayShwoing = true;
                WeeklyMonthlyView.SetBinding(PlotView.ModelProperty, new Binding("ModelViewWeekly.Model"));
                WeekShowing = true;

                WeeklyMonthlyView.Model.InvalidatePlot(true);
                DailyView.Model.InvalidatePlot(true);

            }
            else if (e.Key == Key.F)
            {
                var c = StockWindow.DataContext as GraphControl;
                var d = Market.DataContext as MetaData;

                c.update(d.Cache[3]);

                ThreadPool.QueueUserWorkItem(delegate (object state) {

                    this.Dispatcher.BeginInvoke(new Action(() => {
                        d.NextTikker();

                    }));
                });
                

                DailyView.SetBinding(PlotView.ModelProperty, new Binding("ModelViewDaily.Model"));
                dayShwoing = true;
                WeeklyMonthlyView.SetBinding(PlotView.ModelProperty, new Binding("ModelViewWeekly.Model"));
                WeekShowing = true;

                WeeklyMonthlyView.Model.InvalidatePlot(true);
                DailyView.Model.InvalidatePlot(true);

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
                    var c = a.Overlay as List<OxyPlot.Annotations.Annotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewDaily.Model.Annotations.Add(t);
                    }
                    DailyView.Model.InvalidatePlot(true);
                }
                if (a.Period == Stock.Interval.Week)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.Annotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewWeekly.Model.Annotations.Add(t);
                    }
                    WeeklyMonthlyView.Model.InvalidatePlot(true);

                }
                if (a.Period == Stock.Interval.Month)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.Annotation>;
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
                if (a.Period == Stock.Interval.Hour)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.Annotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewDaily.Model.Annotations.Remove(t);
                    }
                    DailyView.Model.InvalidatePlot(true);

                }
                if (a.Period == Stock.Interval.Day)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.Annotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewDaily.Model.Annotations.Remove(t);
                    }
                    DailyView.Model.InvalidatePlot(true);

                }
                if (a.Period == Stock.Interval.Week)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.Annotation>;
                    foreach (var t in c)
                    {
                        b.ModelViewWeekly.Model.Annotations.Remove(t);
                    }
                    WeeklyMonthlyView.Model.InvalidatePlot(true);

                }
                if (a.Period == Stock.Interval.Month)
                {
                    var c = a.Overlay as List<OxyPlot.Annotations.Annotation>;
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

