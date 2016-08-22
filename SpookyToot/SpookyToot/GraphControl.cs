using System;
using System.CodeDom;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using OxyPlot.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using OxyPlot.Annotations;
using FontWeights = OxyPlot.FontWeights;


namespace SpookyToot
{
    public class GraphControl
    {
        public Example ModelViewDaily { get; private set; }
        public Example ModelViewWeekly { get; private set; }
        public Example ModelViewMonthly{ get; private set; }

        //public bool SOPChecked
        //{
        //    get { return (bool)GetValue(SOPCheckedProperty); }
        //    set { SetValue(SOPCheckedProperty, value); }
        //}

        //public static readonly DependencyProperty SOPCheckedProperty = DependencyProperty.Register("SOPChecked", typeof(bool), typeof(CheckBox), new UIPropertyMetadata(false));

        public GraphControl()
        {

            YahooApiInterface T = new YahooApiInterface();
            List<Stock> SGH = new List<Stock>( T.getYahooData(new List<string>() { "SGH.AX" }, new DateTime(2014, 01, 01)));

            ModelViewDaily = GenerateGraph(SGH[0].StockName, SGH[0].DailyHist);
            ModelViewWeekly = GenerateGraph(SGH[0].StockName, SGH[0].WeeklyHist);
            ModelViewMonthly = GenerateGraph(SGH[0].StockName, SGH[0].MonthlyHist);

        }


        public class Example
        {
            public Example(PlotModel model, IPlotController controller = null)
            {
                this.Model = model;
                this.Controller = controller;
            }


            public IPlotController Controller { get; private set; }
            public PlotModel Model { get; private set; }
        }

        public Example GenerateGraph(string Stckname, List<TradingPeriod> Stck)
        {
            int length = Stck.Count;
            VolumeStyle style = VolumeStyle.Combined;
            bool naturalY = false;
            bool naturalV = false;

            var pm = new PlotModel {  };

            var series = new CandleStickAndVolumeSeries
            {
                PositiveColor = OxyColors.DarkGreen,
                NegativeColor = OxyColors.Red,
                PositiveHollow = false,
                NegativeHollow = false,
                SeparatorColor = OxyColors.Gray,
                SeparatorLineStyle = LineStyle.Dash,
                VolumeStyle = VolumeStyle.Combined
            };

    
            // create bars
            foreach (var v in Stck.OrderBy(x => x.Day.Ticks))
            {
                OhlcvItem Temp = new OhlcvItem();
                Temp.BuyVolume = (v.Volume);
                Temp.Close = v.Close;
                Temp.High = v.High;
                Temp.Low = v.Low;
                Temp.Open = v.Open;
                Temp.X = v.Day.Ticks;

                series.Append(Temp);
            }

            // create visible window
            var Istart = length - (int)Math.Round( 0.1*length);
            var Iend = length - (int)Math.Round( 0.08*length);
            var Ymin = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.Low).Min();
            var Ymax = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.High).Max();
            var Xmin = series.Items[Istart].X;
            var Xmax = series.Items[Iend].X;

            // setup axes
            var timeAxis = new OxyPlot.Axes.DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = Xmin,
                Maximum = Xmax
            };
            var barAxis = new OxyPlot.Axes.LogarithmicAxis()
            {
                Position = AxisPosition.Left,
                Key = series.BarAxisKey,
                StartPosition = 0.25,
                EndPosition = 1.0,
                Minimum = naturalY ? double.NaN : Ymin,
                Maximum = naturalY ? double.NaN : Ymax,
               
            };
            var volAxis = new OxyPlot.Axes.LinearAxis()
            {
                Position = AxisPosition.Left,
                Key = series.VolumeAxisKey,
                StartPosition = 0.0,
                EndPosition = 0.22,
                Minimum = naturalV ? double.NaN : 0,
                Maximum = naturalV ? double.NaN : 5000
            };

            switch (style)
            {
                case VolumeStyle.None:
                    barAxis.Key = null;
                    barAxis.StartPosition = 0.0;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    break;

                case VolumeStyle.Combined:
                case VolumeStyle.Stacked:
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;

                case VolumeStyle.PositiveNegative:
                    volAxis.Minimum = naturalV ? double.NaN : -5000;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;
            }

            pm.Series.Add(series);

            if (naturalY == false)
            {
                timeAxis.AxisChanged += (sender, e) => AdjustYExtent(series, timeAxis, barAxis);
                //timeAxis.AxisChanged += (sender, e) => AdjustYExtent(series, timeAxis, volAxis);
            }

            ///Adding Pivot Annotation
            /// 

            var FirstOrderHighPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var SecondOrderHighPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var ThirdOrderHighPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var FirstOrderLowPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var SeconfOrderLowPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var ThirdOrderLowPivots = new List<OxyPlot.Annotations.PointAnnotation>();

            foreach (var f in Stck)
            {
                //if(f.IsPivotHigh[0]) FirstOrderHighPivots.Add(new OxyPlot.Annotations.PointAnnotation() {Fill = OxyColors.LawnGreen , X = f.Day.Ticks, Y = f.High});
                if (f.IsPivotHigh[1]) SecondOrderHighPivots.Add(new OxyPlot.Annotations.PointAnnotation() { Fill = OxyColors.Green, X = f.Day.Ticks, Y = f.High });
                if (f.IsPivotHigh[2]) ThirdOrderHighPivots.Add(new OxyPlot.Annotations.PointAnnotation() { Fill = OxyColors.DarkGreen, X = f.Day.Ticks, Y = f.High });
                //if (f.IsPivotLow[0]) FirstOrderLowPivots.Add(new OxyPlot.Annotations.PointAnnotation() { Fill = OxyColors.Pink, X = f.Day.Ticks, Y = f.Low });
                if (f.IsPivotLow[1]) SeconfOrderLowPivots.Add(new OxyPlot.Annotations.PointAnnotation() { Fill = OxyColors.Red, X = f.Day.Ticks, Y = f.Low });
                if (f.IsPivotLow[2]) ThirdOrderLowPivots.Add(new OxyPlot.Annotations.PointAnnotation() { Fill = OxyColors.DarkRed, X = f.Day.Ticks, Y = f.Low });
            }

            foreach (var p in FirstOrderHighPivots)
            {
            pm.Annotations.Add(p);
                }

            foreach (var p in SecondOrderHighPivots)
            {
                pm.Annotations.Add(p);
            }
            foreach (var p in ThirdOrderHighPivots)
            {
                pm.Annotations.Add(p);

            }
            foreach (var p in FirstOrderLowPivots)
            {
                pm.Annotations.Add(p);

            }
            foreach (var p in SeconfOrderLowPivots)
            {
                pm.Annotations.Add(p);

            }
            foreach (var p in ThirdOrderLowPivots)
            {
                pm.Annotations.Add(p);

            }



            ///Adding line annotation...



            var la = new OxyPlot.Annotations.LineAnnotation { Type = LineAnnotationType.Horizontal, Y = Stck.Last().Close };
            la.MouseDown += (s, e) =>
            {
                if (e.ChangedButton != OxyMouseButton.Left)
                {
                    return;
                }

                la.StrokeThickness *= 5;
                pm.InvalidatePlot(false);
                e.Handled = true;
            };

            // Handle mouse movements (note: this is only called when the mousedown event was handled)
            la.MouseMove += (s, e) =>
            {
                la.Y = la.InverseTransform(e.Position).Y;
                pm.InvalidatePlot(false);
                e.Handled = true;
            };
            la.MouseUp += (s, e) =>
            {
                la.StrokeThickness /= 5;
                pm.InvalidatePlot(false);
                e.Handled = true;
            };
            pm.Annotations.Add(la);

            OxyPlot.Annotations.ArrowAnnotation tmp = null;

            pm.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    // Create a new arrow annotation
                    tmp = new OxyPlot.Annotations.ArrowAnnotation();
                    tmp.HeadLength = 0;
                    tmp.StartPoint = tmp.EndPoint = timeAxis.InverseTransform(e.Position.X, e.Position.Y, barAxis);
                    pm.Annotations.Add(tmp);
                    e.Handled = true;
                }
                if (e.ChangedButton == OxyMouseButton.Middle)
                {
                    //delete old arrow annotation
                    if (e.HitTestResult != null)
                    {
                        if (e.HitTestResult.Element.GetType() == typeof(OxyPlot.Annotations.ArrowAnnotation))
                        {
                            tmp = (OxyPlot.Annotations.ArrowAnnotation)e.HitTestResult.Element;
                            pm.Annotations.Remove(tmp);
                            e.Handled = true;
                        }
                    }
                }
            };

            // Handle mouse movements (note: this is only called when the mousedown event was handled)
            pm.MouseMove += (s, e) =>
            {
                if (tmp != null)
                {
                    // Modify the end point
                    tmp.EndPoint = timeAxis.InverseTransform(e.Position.X, e.Position.Y, barAxis);
                    tmp.FontWeight = FontWeights.Bold;
                    tmp.Text = string.Format("{0:0.##}%", ((tmp.EndPoint.Y - tmp.StartPoint.Y) * 100) / tmp.EndPoint.Y);

                    // Redraw the plot
                    pm.InvalidatePlot(false);
                    e.Handled = true;
                }
            };

            pm.MouseUp += (s, e) =>
            {
                if (tmp != null)
                {
                    tmp = null;
                    e.Handled = true;
                }
            };

            var controller = new PlotController();

            //controller.UnbindAll();
            //controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands);
            //controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            //controller.BindMouseDown(OxyMouseButton.Middle,  );

            return new Example(pm, controller);
        }

        private static void AdjustYExtent(CandleStickAndVolumeSeries series, OxyPlot.Axes.DateTimeAxis xaxis, OxyPlot.Axes.LogarithmicAxis yaxis)
        {
            var xmin = xaxis.ActualMinimum;
            var xmax = xaxis.ActualMaximum;

            var istart = series.FindByX(xmin);
            var iend = series.FindByX(xmax, istart);

            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            for (int i = istart; i < iend - 1; i++)
            {
                var bar = series.Items[i];
                ymin = Math.Min(ymin, bar.Low);
                ymax = Math.Max(ymax, bar.High);
            }

            //ymin = 0.09545484566*Math.Exp(0.465168705655*xmin);
            //ymax = 0.09545484566 * Math.Exp(0.465168705655 * xmax);


            var extent = ymax - ymin;
            var margin = extent * 0.10;

            yaxis.Zoom(ymin - margin, ymax + margin);
        }
    }
    }

