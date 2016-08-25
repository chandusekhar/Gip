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
            switch (Period)
            {
                case Stock.Interval.Day:
                    TList = T.DailyHist;
                    VolaTilityRange = 20;
                    min = 3;
                    break;
                case Stock.Interval.Week:
                    TList = T.WeeklyHist;
                    VolaTilityRange = 4;
                    min = 2;
                    break;
                case Stock.Interval.Month:
                    TList = T.MonthlyHist;
                    VolaTilityRange = 1;
                    min = 1;
                    break;
            }
            
            /// Get Support

            List<TradingPeriod> LowPivots = new List<TradingPeriod>(TList.Where(x=>x.IsPivotLow[0]));

            for (int i = LowPivots.Count-1; i >= 0; i--)
            {
                if (i < VolaTilityRange) VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(TList.GetRange(TList.IndexOf(LowPivots[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.Close).ToArray());

                double yOne = LowPivots[i].Low  * 1.02;
                double yTwo = LowPivots[i].Low *0.98;

                List<TradingPeriod> Temps = new List<TradingPeriod>();

                for(int x = i; x >= 0; x--)
                {
                    if (LowPivots[x].Low > yTwo && LowPivots[x].Low < yOne) Temps.Add(LowPivots[x]);
                    else break;
                }
                for(int x = i; x < LowPivots.Count; x++)
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
                    temp.Y = Temps.Min(x => x.Close);
                    //temp.MaximumY = Temps.Max(x => x.Close);
                    //temp.Color = OxyColors.Transparent;

                    temp.Color = OxyColors.Wheat;
                  
                    SAR.Add(temp);
                }
            }

            /// Get Resistance

            //for (int i = T.Count; i >= 0 )
                return SAR;
        }
    }
}
