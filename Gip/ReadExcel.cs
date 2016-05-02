using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Data.SqlClient;


namespace Gip
{
    public static class ReadExcel
    {
        public static object ExcelObject { get; set; }

        public static void update()
        {
            var Excel = new Microsoft.Office.Interop.Excel.Application();
            var Data = Excel.Workbooks.Open(@"C:\Users\Ben Roberts\Dropbox\Misc\Portfolio\Data.xlsm");

            List<Stock> Market = new List<Stock>(); 

            for(int i = 2; i < Data.Worksheets.Count+1; i++)
            {

                Worksheet currentSheet = Data.Sheets[i];
                
                Stock Temp = new Stock();
                Temp.History = new List<TradingDay>();

                Temp.Ticker = currentSheet.Name;

                Range excelRange = currentSheet.UsedRange;
               

                object[,] valueArray = (object[,])excelRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);
                
                for(int x = 3; x < valueArray.GetLength(0) + 1; x++)
                {
                    
                    DateTime Date = Convert.ToDateTime(valueArray[x, 1]);
                    double open = Convert.ToDouble(valueArray[x, 2]);
                    double high = Convert.ToDouble(valueArray[x, 3]);
                    double low = Convert.ToDouble(valueArray[x, 4]);
                    double close = Convert.ToDouble(valueArray[x, 5]);
                    double volume = Convert.ToDouble(valueArray[x, 6]);

                    TradingDay TempDay = new TradingDay(Date, open, close, high, low, volume);

                    Temp.History.Add(TempDay);
                }
                

            }

            
        }

    }
}
