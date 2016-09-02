using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.Annotations;

namespace SpookyToot
{
    
    public class Pivots 
    {
        public string Name { get; set; }
        public bool Show { get; set; }
        public List<PointAnnotation> Overlay { get; set; }
        public Stock.Interval Period { get; set; }

        public void Calcultae(Stock Calculate)
        {
            Overlay = Calculate.GetPivots(Period);
            var FirstOrderPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var SecondOrderPivots = new List<OxyPlot.Annotations.PointAnnotation>();
            var ThirdOrderPivots = new List<OxyPlot.Annotations.PointAnnotation>();
        }

    }

    public class Resistances
    {
        public Stock.Interval Period { get; set; }
        public string Name { get; set; }
        public bool Show { get; set; }
        public List<LineAnnotation> ResistanceLine { get; set; }

        public void Calcultae(Stock Calculate)
        {
            ResistanceLine = MarketStructure.DefineSupportResistanceZonesPivots(Calculate, Period);           
        }

        public Resistances()
        {
            
        }

    }

}
