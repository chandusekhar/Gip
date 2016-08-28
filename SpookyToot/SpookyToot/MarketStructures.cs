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


        public static List<OxyPlot.Annotations.LineAnnotation> DefineSupportResistanceZones(Stock T, Stock.Interval Period)
        {
            List<OxyPlot.Annotations.LineAnnotation> SAR = new List<OxyPlot.Annotations.LineAnnotation>();
            List<TradingPeriod> TList = new List<TradingPeriod>();
            int VolaTilityRange = 0;
            int min = 0;
            double AnnualisationFactor = 0;
            switch (Period)
            {
                case Stock.Interval.Day:
                    TList = T.DailyHist;
                    VolaTilityRange = 20;
                    AnnualisationFactor = 262;
                    min = 10;
                    break;
                case Stock.Interval.Week:
                    TList = T.WeeklyHist;
                    VolaTilityRange = 10;
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

            for (int i = TList.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double[] gh = TList.GetRange(i - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray();
                double SDev = Accord.Statistics.Measures.StandardDeviation(gh);
                double Sqr = Math.Sqrt(AnnualisationFactor);

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(i - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);
                double yOne = Math.Abs(TList[i].Close*(1 + Math.Pow(STDev,2)));
                double yTwo = Math.Abs(TList[i].Close*(1 - Math.Pow(STDev,2)));

                List<TradingPeriod> Temps = new List<TradingPeriod>();

                for (int x = i; x >= 0; x--)
                {

                    if (TList[x].Close > yTwo && TList[x].Close < yOne) Temps.Add(TList[x]);
                    else break;
                }

                for (int x = i; x < TList.Count - 1; x++)
                {

                    if (TList[x].Close > yTwo && TList[x].Close < yOne) Temps.Add(TList[x]);
                    else break;
                }

                if (Temps.Count > min)
                {
                    OxyPlot.Annotations.LineAnnotation temp = new OxyPlot.Annotations.LineAnnotation();
                    OxyPlot.Annotations.LineAnnotation temp2 = new OxyPlot.Annotations.LineAnnotation();
                    
                    temp.Type = LineAnnotationType.Horizontal;
                    temp.MinimumX = Temps.Min(x => x.Index);
                    temp.MaximumX = Temps.Max(x => x.Index);
                    temp.Y = Temps.Max(x=>x.Close);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;
                    temp.Color = OxyColors.Wheat;

                    temp2.Type = LineAnnotationType.Horizontal;
                    temp2.MinimumX = Temps.Min(x => x.Index);
                    temp2.MaximumX = Temps.Max(x => x.Index);
                    temp2.Y = Temps.Min(x => x.Close);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;
                    temp2.Color = OxyColors.Wheat;


                    double High1 = Math.Abs(temp.Y * (1 + Math.Pow(STDev, 2)));
                    double Low1 = Math.Abs(temp.Y * (1 - Math.Pow(STDev, 2)));
                    double High2 = Math.Abs(temp2.Y * (1 + Math.Pow(STDev, 2)));
                    double Low2 = Math.Abs(temp2.Y * (1 - Math.Pow(STDev, 2)));
                    List<OxyPlot.Annotations.LineAnnotation> MoreTemps1 = new List<LineAnnotation>();
                    List<OxyPlot.Annotations.LineAnnotation> MoreTemps2 = new List<LineAnnotation>();

                    MoreTemps1 = SAR.Where(x => x.Y < High1 && x.Y > Low1).ToList();
                    MoreTemps2 = SAR.Where(x => x.Y < High2 && x.Y > Low2).ToList();

                    if (MoreTemps1.Count > 0)
                    {
                        temp.MinimumX = MoreTemps1.Min(x => x.MinimumX);
                        temp.MaximumX = MoreTemps1.Max(x => x.MaximumX);
                        temp.Y = MoreTemps1.Average(x => x.Y);
                        //foreach (var v in MoreTemps1)
                        //{
                        //    SAR.Remove(v);
                        //}
                        SAR.Add(temp);

                    }
                    else SAR.Add(temp);

                    if (MoreTemps2.Count > 0)
                    {
                        temp2.MinimumX = MoreTemps2.Min(x => x.MinimumX);
                        temp2.MaximumX = MoreTemps2.Max(x => x.MaximumX);
                        temp2.Y = MoreTemps2.Average(x => x.Y);
                        //foreach (var c in MoreTemps2)
                        //{
                        //    SAR.Remove(c);
                        //}
                        SAR.Add(temp2);
                    }
                    else SAR.Add(temp2);

                    //SAR.Add(temp);
                    //SAR.Add(temp2);
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
                case Stock.Interval.Day:
                    TList = T.DailyHist;
                    VolaTilityRange = 262;
                    AnnualisationFactor = 262;
                    min = 10;
                    break;
                case Stock.Interval.Week:
                    TList = T.WeeklyHist;
                    VolaTilityRange = 262 / 5;
                    AnnualisationFactor = 262 / 5;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 262 / 20;
                    AnnualisationFactor = 262 / 20;
                    min = 1;
                    break;
            }

            List<TradingPeriod> HighPivots = new List<TradingPeriod>(TList.Where(x => x.IsPivotHigh[0]));


            for (int i = TList.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(i - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);

                double yOne = Math.Abs(TList[i].Close * (1 + Math.Pow(STDev, 2)));
                double yTwo = Math.Abs(TList[i].Close * (1 - Math.Pow(STDev, 2)));

                List<TradingPeriod> Temps = new List<TradingPeriod>();

                for (int x = i; x >= 0; x--)
                {
                    if (TList[x].High > yTwo && TList[x].High < yOne) Temps.Add(TList[x]);
                    else break;
                }


                for (int x = i; x < TList.Count - 1; x++)
                {
                    if (TList[x].High > yTwo && TList[x].High < yOne) Temps.Add(TList[x]);
                    else break;
                }

                if (Temps.Count > min)
                {
                    OxyPlot.Annotations.LineAnnotation temp = new OxyPlot.Annotations.LineAnnotation();


                    temp.Type = LineAnnotationType.Horizontal;
                    temp.MinimumX = Temps.Min(x => x.Index);
                    temp.MaximumX = Temps.Max(x => x.Index);
                    temp.Y = Temps.Max(x=>x.High);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;

                    temp.Color = OxyColors.Green;

                    SAR.Add(temp);
                }
                foreach (var t in Temps)
                {
                    i--;

                    TList.Remove(TList.Last());
                }

            }

            switch (Period)
            {
                case Stock.Interval.Day:
                    TList = T.DailyHist;
                    VolaTilityRange = 262;
                    AnnualisationFactor = 262;
                    min = 10;
                    break;
                case Stock.Interval.Week:
                    TList = T.WeeklyHist;
                    VolaTilityRange = 262 / 5;
                    AnnualisationFactor = 262 / 5;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 262 / 20;
                    AnnualisationFactor = 262 / 20;
                    min = 1;
                    break;
            }


            for (int i = TList.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(i - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);

                double yOne = Math.Abs(TList[i].Close * (1 + Math.Pow(STDev, 2)));
                double yTwo = Math.Abs(TList[i].Close * (1 - Math.Pow(STDev, 2)));

                List<TradingPeriod> Temps = new List<TradingPeriod>();

                for (int x = i; x >= 0; x--)
                {
                    if (TList[x].Low > yTwo && TList[x].Low < yOne) Temps.Add(TList[x]);
                    else break;
                }


                for (int x = i; x < TList.Count - 1; x++)
                {
                    if (TList[x].Low > yTwo && TList[x].Low < yOne) Temps.Add(TList[x]);
                    else break;
                }

                if (Temps.Count > min)
                {
                    OxyPlot.Annotations.LineAnnotation temp = new OxyPlot.Annotations.LineAnnotation();


                    temp.Type = LineAnnotationType.Horizontal;
                    temp.MinimumX = Temps.Min(x => x.Index);
                    temp.MaximumX = Temps.Max(x => x.Index);
                    temp.Y = Temps.Min(x=>x.Low);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;

                    temp.Color = OxyColors.Red;

                    SAR.Add(temp);
                }
                foreach (var t in Temps)
                {
                    i--;

                    TList.Remove(TList.Last());
                }

            }

            return SAR;
        }
    }
}
