using System;
using System.CodeDom;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using OxyPlot.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FontWeights = OxyPlot.FontWeights;


namespace SpookyToot
{
    public class GraphControl
    {
        public Example ModelViewHourly { get; private set; }
        public Example ModelViewDaily { get; private set; }
        public Example ModelViewWeekly { get; private set; }
        public Example ModelViewMonthly { get; private set; }
        public Stock CurrentStock { get; private set; }
        public string Ticker { get; set; }

        public ObservableCollection<GraphOverlays> CurrentOverlays { get; private set; }

        public GraphControl()
        {
            Ticker = "sgh.AX";
            YahooApiInterface T = new YahooApiInterface();
            List<Stock> SGH = new List<Stock>(T.getYahooData(new List<string>() { Ticker }, new DateTime(2013, 01, 01)));
            update(SGH[0]);
        }

        public void update(Stock stk)
        {
            CurrentStock = stk;

            CurrentOverlays = new ObservableCollection<GraphOverlays>();
            if(CurrentStock.HourlyHist.Count >0) ModelViewHourly = GenerateGraph(CurrentStock, Stock.Interval.Hour);
            if (CurrentStock.DailyHist.Count > 0) ModelViewDaily = GenerateGraph(CurrentStock, Stock.Interval.Day);
            if (CurrentStock.WeeklyHist.Count > 0) ModelViewWeekly = GenerateGraph(CurrentStock, Stock.Interval.Week);
            if (CurrentStock.MonthlyHist.Count > 0) ModelViewMonthly = GenerateGraph(CurrentStock, Stock.Interval.Month);

            ModelViewHourly.Model.InvalidatePlot(true);
            ModelViewDaily.Model.InvalidatePlot(true);
            ModelViewWeekly.Model.InvalidatePlot(true);
            ModelViewMonthly.Model.InvalidatePlot(true);

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

        public Example GenerateGraph(Stock Stck, Stock.Interval Period)
        {
            int length = 0;
            double startPost = 0;
            double endPOs = 0;
            List<TradingPeriod> TradingList = new List<TradingPeriod>();
            switch (Period)
            {
                case Stock.Interval.Hour:
                    length = Stck.HourlyHist.Count;
                    TradingList = Stck.HourlyHist;
                    startPost = length + 6;
                    endPOs = length - 50;
                    break;
                case Stock.Interval.Day:
                    length = Stck.DailyHist.Count;
                    TradingList = Stck.DailyHist;
                    startPost = length + 6;
                    endPOs = length - 110;
                    break;
                case Stock.Interval.Week:
                    length = Stck.WeeklyHist.Count;
                    TradingList = Stck.WeeklyHist;
                    startPost = length + 6;
                    endPOs = length - 80;
                    break;
                case Stock.Interval.Month:
                    length = Stck.MonthlyHist.Count;
                    TradingList = Stck.MonthlyHist;
                    startPost = length + 3;
                    endPOs = length - 48;
                    break;
            }

            VolumeStyle style = VolumeStyle.Combined;
            bool naturalY = false;
            bool naturalV = false;

            var pm = new PlotModel {};

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
            foreach (var v in TradingList)
            {
                OhlcvItem Temp = new OhlcvItem();
                Temp.BuyVolume = (v.Volume);
                Temp.Close = v.Close;
                Temp.High = v.High;
                Temp.Low = v.Low;
                Temp.Open = v.Open;
                Temp.X = v.Index;

                series.Append(Temp);
            }

            // create visible window
            var Istart = length -10;
            var Iend = length -1;
            var Ymin = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.Low).Min();
            var Ymax = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.High).Max();

            // setup axes
            var timeAxis = new OxyPlot.Axes.DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = endPOs,
                Maximum = startPost,

                //StartPosition = Xmax - TimeSpan.FromDays(180).Ticks,
                //EndPosition = Xmax,
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
                Maximum = naturalV ? double.NaN : TradingList.Max(x => x.Volume)
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

            AdjustYExtent(series,timeAxis,barAxis);


            if (naturalY == false)
            {
                timeAxis.AxisChanged += (sender, e) => AdjustYExtent(series, timeAxis, barAxis);
                //timeAxis.AxisChanged += (sender, e) => AdjustYExtent(series, timeAxis, volAxis);
            }

            ///Adding Pivot Annotation
            /// 
            Stck.GetPivots(Period);

            var ResistanceLines = new List<OxyPlot.Annotations.LineAnnotation>();
            var FirstOrderPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var SecondOrderPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var ThirdOrderPivots = new List<OxyPlot.Annotations.PointAnnotation>();

            foreach (var f in TradingList)
            {
                if (f.IsPivotHigh[0]) FirstOrderPivots.Add(new OxyPlot.Annotations.PointAnnotation() {Fill = OxyColors.LawnGreen, X = f.Index, Y = f.High});
                if (f.IsPivotHigh[1]) SecondOrderPivots.Add(new OxyPlot.Annotations.PointAnnotation() {Fill = OxyColors.Green, X = f.Index, Y = f.High});
                if (f.IsPivotHigh[2]) ThirdOrderPivots.Add(new OxyPlot.Annotations.PointAnnotation() {Fill = OxyColors.DarkGreen, X = f.Index, Y = f.High});
                if (f.IsPivotLow[0]) FirstOrderPivots.Add(new OxyPlot.Annotations.PointAnnotation() {Fill = OxyColors.Pink, X = f.Index, Y = f.Low});
                if (f.IsPivotLow[1]) SecondOrderPivots.Add(new OxyPlot.Annotations.PointAnnotation() {Fill = OxyColors.Red, X = f.Index, Y = f.Low});
                if (f.IsPivotLow[2]) ThirdOrderPivots.Add(new OxyPlot.Annotations.PointAnnotation() {Fill = OxyColors.DarkRed, X = f.Index, Y = f.Low});
            }
            ResistanceLines = MarketStructure.DefineSupportResistanceZonesPivots(Stck, Period);

            GraphOverlays FOP = new GraphOverlays();
            GraphOverlays SOP = new GraphOverlays();
            GraphOverlays TOP = new GraphOverlays();
            GraphOverlays Lines = new GraphOverlays();

            FOP.Name = "First Order Pivot";
            SOP.Name = "Second Order Pivot";
            TOP.Name = "Third Order Pivot";
            Lines.Name = "Resistance Lines";

            FOP.Overlay = FirstOrderPivots;
            SOP.Overlay = SecondOrderPivots;
            TOP.Overlay = ThirdOrderPivots;
            Lines.Overlay = ResistanceLines;

            FOP.Period = Period;
            SOP.Period = Period;
            TOP.Period = Period;
            Lines.Period = Period;

            CurrentOverlays.Add(FOP);
            CurrentOverlays.Add(SOP);
            CurrentOverlays.Add(TOP);
            CurrentOverlays.Add(Lines);
            ///Adding line annotation...


            var la = new OxyPlot.Annotations.LineAnnotation {Type = LineAnnotationType.Horizontal, Y = TradingList.Last().Close};
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
                            tmp = (OxyPlot.Annotations.ArrowAnnotation) e.HitTestResult.Element;
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
                    tmp.Text = string.Format("{0:0.##}%", ((tmp.StartPoint.Y - tmp.EndPoint.Y)*-100)/tmp.StartPoint.Y);

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
            pm.Title = Stck.StockName;

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
            var iend = series.FindByX(xmax);

            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            for (int i = istart; i < iend + 1; i++)
            {
                var bar = series.Items[i];
                ymin = Math.Min(ymin, bar.Low);
                ymax = Math.Max(ymax, bar.High);
            }

            yaxis.Zoom(ymin*(0.999), ymax*1.001);
        }
    }
}

