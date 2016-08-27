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
                    min = 3;
                    break;
                case Stock.Interval.Week:
                    TList = T.WeeklyHist;
                    VolaTilityRange = 4;
                    AnnualisationFactor = 262/5;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 1;
                    AnnualisationFactor = 262/12;
                    min = 1;
                    break;
            }

            /// Get Support
            /// http://investexcel.net/calculate-historical-volatility-excel/


            List<TradingPeriod> LowPivots = new List<TradingPeriod>(TList.Where(x => x.IsPivotLow[0]));

            for (int i = LowPivots.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                //double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(TList.IndexOf(LowPivots[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor) ;

                double yOne = LowPivots[i].Low * 1.02;
                double yTwo = LowPivots[i].Low * 0.98;

                List<TradingPeriod> Temps = new List<TradingPeriod>();

                for (int x = i; x >= 0; x--)
                {
                    if (LowPivots[x].Low > yTwo && LowPivots[x].Low < yOne) Temps.Add(LowPivots[x]);
                    else break;
                }

                for (int x = i; x < LowPivots.Count-1; x++)
                {                
                    if (LowPivots[x].Low > yTwo && LowPivots[x].Low < yOne) Temps.Add(LowPivots[x]);

                    else break;
                }

                if (Temps.Count > min)
                {
                    OxyPlot.Annotations.LineAnnotation temp = new OxyPlot.Annotations.LineAnnotation();

                    temp.Type = LineAnnotationType.Horizontal;
                    temp.MinimumX = Temps.Min(x => x.Day.Ticks);
                    temp.MaximumX = Temps.Max(x => x.Day.Ticks);
                    temp.Y = LowPivots[i].Low;
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;

                    temp.Color = OxyColors.Wheat;

                    SAR.Add(temp);
                }
                foreach (var t in Temps)
                {
                    i--;

                    LowPivots.Remove(t);

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
                    min = 4;
                    break;
                case Stock.Interval.Week:
                    TList = T.WeeklyHist;
                    VolaTilityRange = 4;
                    AnnualisationFactor = 262 / 5;
                    min = 4;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 1;
                    AnnualisationFactor = 262 / 12;
                    min = 4;
                    break;
            }

            List<TradingPeriod> HighPivots = new List<TradingPeriod>(TList.Where(x => x.IsPivotHigh[0]));


            for (int i = HighPivots.Count - 1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                //double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(TList.IndexOf(HighPivots[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.ReturnSeries).ToArray()) * Math.Sqrt(AnnualisationFactor);

                double yOne = HighPivots[i].High *1.02;
                double yTwo = HighPivots[i].High *0.98;

                List<TradingPeriod> Temps = new List<TradingPeriod>();

                for (int x = i; x >= 0; x--)
                {
                    if (HighPivots[x].High > yTwo && HighPivots[x].High < yOne) Temps.Add(HighPivots[x]);
                    else break;
                }


                for (int x = i; x < HighPivots.Count-1; x++)
                {
                    if (HighPivots[x].High > yTwo && HighPivots[x].High < yOne) Temps.Add(HighPivots[x]);
                    else break;
                }

                if (Temps.Count > min)
                {
                    OxyPlot.Annotations.LineAnnotation temp = new OxyPlot.Annotations.LineAnnotation();


                    temp.Type = LineAnnotationType.Horizontal;
                    temp.MinimumX = Temps.Min(x => x.Day.Ticks);
                    temp.MaximumX = Temps.Max(x => x.Day.Ticks);
                    temp.Y = HighPivots[i].High;
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;

                    temp.Color = OxyColors.Red;

                    SAR.Add(temp);
                }
                foreach (var t in Temps)
                {
                    i--;

                    HighPivots.Remove(t);
                }

            }
            return SAR;
        }
    }
}
