using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Gip
{
    public class StockContext : DbContext
    {
        public StockContext() : base("barry")
        {
            Database.SetInitializer<StockContext>(new CreateDatabaseIfNotExists<StockContext>());

        }


        public DbSet<TradingDay> MarketHistory { get; set; }
        public DbSet<Stock> Technicals { get; set; }

        public class StockdbInitialiser : CreateDatabaseIfNotExists<StockContext> 
        {
            internal void Seed()
            {
                
                List<Stock> Results = new List<Stock>();
                List<string> Ticks = new List<string>();
                Ticks = GetData.testTicker();
                Results = GetData.Try2(Ticks);

                StockContext context = GetData.CreateDB(Results);

                
                
                base.Seed(context);
            }
        }
        
    }
    
}
