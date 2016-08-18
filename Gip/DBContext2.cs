using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaasOne.Finance.YahooFinance;
using System.Reflection;

namespace Gip
{
    public class StockdbInitialiser : CreateDatabaseIfNotExists<StockContext>
    {
        static Type GetListType(Type type)
        {
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }
            return null;
        }

        public void Seed()
        {            
            List<string> Ticks = new List<string>();
            StockContext Db = new StockContext();
            Ticks = GetData.TickerstoGet();
            List<TradingDay> Results = new List<TradingDay>();

            Results.AddRange(GetData.DownloadStocks(Ticks, new DateTime(2010,1,1)));
            
            Db.Database.Create();
            Db.St1.AddRange(Results);
            Db.SaveChanges();

            base.Seed(Db);
        }     
    }
}
