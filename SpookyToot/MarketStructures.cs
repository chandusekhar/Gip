using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using OxyPlot.Wpf;

namespace SpookyToot
{
	public static class MarketStructure
	{
        public static List<OxyPlot.Annotations.RectangleAnnotation> DefineSupportResistanceZones(List<TradingPeriod> T)
        {
            List<OxyPlot.Annotations.RectangleAnnotation> SAR = new List<OxyPlot.Annotations.RectangleAnnotation>();

            /// Get Support

            List<TradingPeriod> LowPivots = new List<TradingPeriod>(T.Where(x => x.IsPivotLow[0]));

            for (int i = LowPivots.Count; i >= 0; i--)
            {
                int VolaTilityRange;
                if (i > 20) VolaTilityRange = 20;
                else VolaTilityRange = i;

                double STDev = Accord.Statistics.Measures.StandardDeviation(T.GetRange(T.IndexOf(LowPivots[i]) - VolaTilityRange, VolaTilityRange).Select(x => x.Close).ToArray());

                double yOne = T[i].Close + T[i].Close * 0.1 * STDev;
                double yTwo = T[i].Close - T[i].Close * 0.1 * STDev;

                List<TradingPeriod> Temps = new List<TradingPeriod>(T.Where(x => x.Close > yTwo && x.Close < yOne).ToList());

                if (Temps.Count > 2)
                {
                    OxyPlot.Annotations.LineAnnotation temp = new OxyPlot.Annotations.LineAnnotation();
                    temp.MinimumX = Temps.Min(x => x.Day.Ticks);
                    temp.MinimumY = Temps.Min(x => x.Close);
                    temp.MaximumX = Temps.Max(x => x.Day.Ticks);
                    temp.MaximumY = Temps.Max(x => x.Close);
                    temp.Color = OxyPlot.OxyColors.CornflowerBlue;

                }

            }

            /// Get Resistance

            //for (int i = T.Count; i >= 0 )



            return SAR;
        }
    }
}
