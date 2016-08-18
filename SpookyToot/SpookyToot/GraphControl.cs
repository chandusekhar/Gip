using System;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.Annotations;


namespace SpookyToot
{
    public class GraphControl
    {
        public Example ModelViewDaily { get; private set; }
        public Example ModelViewWeekly { get; private set; }
        public Example ModelViewMonthly{ get; private set; }

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
                Temp.BuyVolume = (v.Volume / 10000);
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
            var barAxis = new OxyPlot.Axes.LinearAxis
            {
                Position = AxisPosition.Left,
                Key = series.BarAxisKey,
                StartPosition = 0.25,
                EndPosition = 1.0,
                Minimum = naturalY ? double.NaN : Ymin,
                Maximum = naturalY ? double.NaN : Ymax,
               
            };
            var volAxis = new OxyPlot.Axes.LinearAxis
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

        private static void AdjustYExtent(CandleStickAndVolumeSeries series, OxyPlot.Axes.DateTimeAxis xaxis, OxyPlot.Axes.LinearAxis yaxis)
        {
            var xmin = xaxis.ActualMinimum;
            var xmax = xaxis.ActualMaximum;

            var istart = series.FindByX(xmin);
            var iend = series.FindByX(xmax, istart);

            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            for (int i = istart; i <= iend; i++)
            {
                var bar = series.Items[i];
                ymin = Math.Min(ymin, bar.Low);
                ymax = Math.Max(ymax, bar.High);
            }

            var extent = ymax - ymin;
            var margin = extent * 0.10;

            yaxis.Zoom(ymin - margin, ymax + margin);
        }
    }
    }

