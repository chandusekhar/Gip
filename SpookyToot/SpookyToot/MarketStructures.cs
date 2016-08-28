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
            int AnnualisationFactor = 0;
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
                    VolaTilityRange = 4;
                    AnnualisationFactor = 262 / 5;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 2;
                    AnnualisationFactor = 262 / 12;
                    min = 1;
                    break;
            }

            /// Get Support
            /// Adam Grimes formula


            List<TradingPeriod> LowPivots = new List<TradingPeriod>(TList.Where(x => x.IsPivotLow[0]));

            for (int i = TList.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(TList.IndexOf(TList[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);
                double yOne = TList[i].Close + STDev;
                double yTwo = TList[i].Close - STDev;

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
                    SAR.Add(temp);

                    SAR.Add(temp2);
                }
                foreach (var t in Temps)
                {
                    i--;

                    TList.Remove(t);

                }
            }

            /// Get Resistance
            /// 
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
                    VolaTilityRange = 4;
                    AnnualisationFactor = 262 / 5;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 1;
                    AnnualisationFactor = 262 / 12;
                    min = 2;
                    break;
            }

            List<TradingPeriod> HighPivots = new List<TradingPeriod>(TList.Where(x => x.IsPivotHigh[0]));


            for (int i = TList.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(TList.IndexOf(TList[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);

                double yOne = TList[i].High + STDev ;
                double yTwo = TList[i].High - STDev;

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

                    TList.Remove(t);
                }

            }

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
                    VolaTilityRange = 4;
                    AnnualisationFactor = 262 / 5;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 1;
                    AnnualisationFactor = 262 / 12;
                    min = 2;
                    break;
            }


            for (int i = TList.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(TList.IndexOf(TList[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);

                double yOne = TList[i].Low + STDev;
                double yTwo = TList[i].Low - STDev;

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

                    TList.Remove(t);
                }

            }

            return SAR;
        }
    }
}
