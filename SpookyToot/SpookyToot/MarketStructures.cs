using System;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace SpookyToot
{
    public static class MarketStructure
    {

        public static List<OxyPlot.Annotations.LineAnnotation> DefineSupportResistanceZonesPivots(Stock T, Stock.Interval Period)
        {
            List<OxyPlot.Annotations.LineAnnotation> SAR = new List<OxyPlot.Annotations.LineAnnotation>();
            List<TradingPeriod> TList = new List<TradingPeriod>();
            int VolaTilityRange = 0;
            int min = 0;
            double AnnualisationFactor = 0;
            switch (Period)
            {
                case Stock.Interval.Hour:
                    TList = T.HourlyHist;
                    VolaTilityRange = 20;
                    AnnualisationFactor = 262*6;
                    min = 10;
                    break;
                case Stock.Interval.Day:
                    TList = T.DailyHist;
                    VolaTilityRange = 25;
                    AnnualisationFactor = 262;
                    min = 10;
                    break;
                case Stock.Interval.Week:
                    TList = T.WeeklyHist;
                    VolaTilityRange = 6;
                    AnnualisationFactor = 37.3;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 2;
                    AnnualisationFactor = 8.6;
                    min = 1;
                    break;
            }

            /// Get Support
            /// Adam Grimes formula


            List<TradingPeriod> LowPivots = new List<TradingPeriod>(TList.Where(x => x.IsPivotLow[0]));

            for (int i = LowPivots.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(TList.IndexOf(LowPivots[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);
                double yOne = Math.Abs(LowPivots[i].Close * (1 + Math.Pow(STDev, 2)));
                double yTwo = Math.Abs(LowPivots[i].Close * (1 - Math.Pow(STDev, 2)));

                List<TradingPeriod> Temps = new List<TradingPeriod>();

                for (int x = i; x >= 0; x--)
                {

                    if (LowPivots[x].Close > yTwo && LowPivots[x].Close < yOne) Temps.Add(LowPivots[x]);
                    else break;
                }

                for (int x = i; x < LowPivots.Count - 1; x++)
                {

                    if (LowPivots[x].Close > yTwo && LowPivots[x].Close < yOne) Temps.Add(LowPivots[x]);
                    else break;
                }

                if (Temps.Count > min)
                {
                    OxyPlot.Annotations.LineAnnotation temp = new OxyPlot.Annotations.LineAnnotation();
                    OxyPlot.Annotations.LineAnnotation temp2 = new OxyPlot.Annotations.LineAnnotation();

                    temp.Type = LineAnnotationType.Horizontal;
                    temp.MinimumX = Temps.Min(x => x.Index);
                    temp.MaximumX = Temps.Max(x => x.Index);
                    temp.Y = Temps.Min(x => x.Close);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;
                    temp.Color = OxyColors.Red;

                    temp2.Type = LineAnnotationType.Horizontal;
                    temp2.MinimumX = Temps.Min(x => x.Index);
                    temp2.MaximumX = Temps.Max(x => x.Index);
                    temp2.Y = Temps.Max(x => x.Close);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;
                    temp2.Color = OxyColors.Red;


                    //double High1 = Math.Abs(temp.Y * (1 + Math.Pow(STDev, 2)));
                    //double Low1 = Math.Abs(temp.Y * (1 - Math.Pow(STDev, 2)));
                    //double High2 = Math.Abs(temp2.Y * (1 + Math.Pow(STDev, 2)));
                    //double Low2 = Math.Abs(temp2.Y * (1 - Math.Pow(STDev, 2)));
                    //List<OxyPlot.Annotations.LineAnnotation> MoreTemps1 = new List<LineAnnotation>();
                    //List<OxyPlot.Annotations.LineAnnotation> MoreTemps2 = new List<LineAnnotation>();

                    //MoreTemps1 = SAR.Where(x => x.Y < High1 && x.Y > Low1).ToList();
                    //MoreTemps2 = SAR.Where(x => x.Y < High2 && x.Y > Low2).ToList();

                    //if (MoreTemps1.Count > 0)
                    //{
                    //    temp.MinimumX = MoreTemps1.Min(x => x.MinimumX);
                    //    temp.MaximumX = MoreTemps1.Max(x => x.MaximumX);
                    //    temp.Y = MoreTemps1.Average(x => x.Y);
                    //    //foreach (var v in MoreTemps1)
                    //    //{
                    //    //    SAR.Remove(v);
                    //    //}
                    //    SAR.Add(temp);

                    //}
                    //else SAR.Add(temp);

                    //if (MoreTemps2.Count > 0)
                    //{
                    //    temp2.MinimumX = MoreTemps2.Min(x => x.MinimumX);
                    //    temp2.MaximumX = MoreTemps2.Max(x => x.MaximumX);
                    //    temp2.Y = MoreTemps2.Average(x => x.Y);
                    //    //foreach (var c in MoreTemps2)
                    //    //{
                    //    //    SAR.Remove(c);
                    //    //}
                    //    SAR.Add(temp2);
                    //}
                    //else SAR.Add(temp2);

                    SAR.Add(temp);
                    SAR.Add(temp2);
                }


                foreach (var t in Temps)
                {
                    i--;

                    TList.Remove(TList.Last());

                }
            }

            /// Get Resistance
            /// 
            switch (Period)
            {
                case Stock.Interval.Hour:
                    TList = T.HourlyHist;
                    VolaTilityRange = 20;
                    AnnualisationFactor = 262 * 6;
                    min = 10;
                    break;
                case Stock.Interval.Day:
                    TList = T.DailyHist;
                    VolaTilityRange = 25;
                    AnnualisationFactor = 262;
                    min = 10;
                    break;
                case Stock.Interval.Week:
                    TList = T.WeeklyHist;
                    VolaTilityRange = 6;
                    AnnualisationFactor = 37.3;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 2;
                    AnnualisationFactor = 8.6;
                    min = 1;
                    break;
            }

            List<TradingPeriod> HighPivots = new List<TradingPeriod>(TList.Where(x => x.IsPivotHigh[0]));


            for (int i = HighPivots.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(TList.IndexOf( HighPivots[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);

                double yOne = Math.Abs(HighPivots[i].Close * (1 + Math.Pow(STDev, 2)));
                double yTwo = Math.Abs(HighPivots[i].Close * (1 - Math.Pow(STDev, 2)));

                List<TradingPeriod> Temps = new List<TradingPeriod>();

                for (int x = i; x >= 0; x--)
                {
                    if (HighPivots[x].High > yTwo && HighPivots[x].High < yOne) Temps.Add(HighPivots[x]);
                    else break;
                }


                for (int x = i; x < HighPivots.Count - 1; x++)
                {
                    if (HighPivots[x].High > yTwo && HighPivots[x].High < yOne) Temps.Add(HighPivots[x]);
                    else break;
                }

                if (Temps.Count > min)
                {


                    OxyPlot.Annotations.LineAnnotation temp = new OxyPlot.Annotations.LineAnnotation();
                    OxyPlot.Annotations.LineAnnotation temp2 = new OxyPlot.Annotations.LineAnnotation();

                    temp.Type = LineAnnotationType.Horizontal;
                    temp.MinimumX = Temps.Min(x => x.Index);
                    temp.MaximumX = Temps.Max(x => x.Index);
                    temp.Y = Temps.Max(x => x.Close);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;
                    temp.Color = OxyColors.LawnGreen;

                    temp2.Type = LineAnnotationType.Horizontal;
                    temp2.MinimumX = Temps.Min(x => x.Index);
                    temp2.MaximumX = Temps.Max(x => x.Index);
                    temp2.Y = Temps.Min(x => x.Close);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;
                    temp2.Color = OxyColors.LawnGreen;

                    SAR.Add(temp);
                    SAR.Add(temp2);

                }
                foreach (var t in Temps)
                {
                    i--;

                    TList.Remove(HighPivots.Last());
                }

            }

            return SAR;
        }
    }
}
