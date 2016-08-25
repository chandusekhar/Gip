using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SpookyToot
{
    
    public class GraphOverlays 
    {
        public string Name { get; set; }
        public bool Show { get;  set; }
        public object Overlay { get; set; }
        public Stock.Interval Period { get; set; }

        private void Draw(object sender, EventArgs e)
        {
            if (Show)
            {
               
            }
        }
        
    }
}
