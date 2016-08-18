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

namespace Gip
{
    public partial class StockContext : DbContext
    {
        public StockContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFileName=C:\Users\Ben Roberts\Dropbox\WOERK.mdf;Integrated Security=true;")
        {
            Database.SetInitializer<StockContext>(new CreateDatabaseIfNotExists<StockContext>());
        }

        public DbSet<TradingDay> St1 { get; set; }
    //    public DbSet<TradingDay> St2 { get; set; }
    //    public DbSet<TradingDay> St3 { get; set; }
    //    public DbSet<TradingDay> St4 { get; set; }
    //    public DbSet<TradingDay> St5 { get; set; }
    //    public DbSet<TradingDay> St6 { get; set; }
    //    public DbSet<TradingDay> St7 { get; set; }
    //    public DbSet<TradingDay> St8 { get; set; }
    //    public DbSet<TradingDay> St9 { get; set; }
    //    public DbSet<TradingDay> St10 { get; set; }
    //    public DbSet<TradingDay> St11 { get; set; }
    //    public DbSet<TradingDay> St12 { get; set; }
    //    public DbSet<TradingDay> St13 { get; set; }
    //    public DbSet<TradingDay> St14 { get; set; }
    //    public DbSet<TradingDay> St15{ get; set; }
    //    public DbSet<TradingDay> St16{ get; set; }
    //    public DbSet<TradingDay> St17{ get; set; }
    //    public DbSet<TradingDay> St18{ get; set; }
    //    public DbSet<TradingDay> St19{ get; set; }
    //    public DbSet<TradingDay> St20 { get; set; }
    //    public DbSet<TradingDay> St21 { get; set; }
    //    public DbSet<TradingDay> St22 { get; set; }
    //    public DbSet<TradingDay> St23 { get; set; }
    //    public DbSet<TradingDay> St24 { get; set; }
    //    public DbSet<TradingDay> St25 { get; set; }
    //    public DbSet<TradingDay> St26 { get; set; }
    //    public DbSet<TradingDay> St27 { get; set; }
    //    public DbSet<TradingDay> St28 { get; set; }
    //    public DbSet<TradingDay> St29 { get; set; }
    //    public DbSet<TradingDay> St30 { get; set; }
    //    public DbSet<TradingDay> St31 { get; set; }
    //    public DbSet<TradingDay> St32 { get; set; }
    //   public DbSet<TradingDay> St33 { get; set; }
    //    public DbSet<TradingDay> St34 { get; set; }
    //    public DbSet<TradingDay> St35 { get; set; }
    //    public DbSet<TradingDay> St36 { get; set; }
    //    public DbSet<TradingDay> St37 { get; set; }
    //    public DbSet<TradingDay> St38 { get; set; }
    //    public DbSet<TradingDay> St39 { get; set; }
    //    public DbSet<TradingDay> St40{ get; set; }
    //    public DbSet<TradingDay> St41 { get; set; }
    //    public DbSet<TradingDay> St42 { get; set; }
    //    public DbSet<TradingDay> St43 { get; set; }
    //    public DbSet<TradingDay> St44 { get; set; }
    //    public DbSet<TradingDay> St45 { get; set; }
    //    public DbSet<TradingDay> St46{ get; set; }
    //    public DbSet<TradingDay> St47 { get; set; }
    //    public DbSet<TradingDay> St48 { get; set; }
    //public DbSet<TradingDay> St49 { get; set; }
    //public DbSet<TradingDay> St50 { get; set; }
    //public DbSet<TradingDay> St51 { get; set; }
    //public DbSet<TradingDay> St52 { get; set; }
    //public DbSet<TradingDay> St53 { get; set; }
    //public DbSet<TradingDay> St54 { get; set; }
    //public DbSet<TradingDay> St55{ get; set; }
    //public DbSet<TradingDay> St56 { get; set; }
    //public DbSet<TradingDay> St57 { get; set; }
    //public DbSet<TradingDay> St58 { get; set; }
    //public DbSet<TradingDay> St59 { get; set; }
    //public DbSet<TradingDay> St60 { get; set; }
    //public DbSet<TradingDay> St61 { get; set; }
    //public DbSet<TradingDay> St62 { get; set; }
    //public DbSet<TradingDay> St63 { get; set; }
    //public DbSet<TradingDay> St64 { get; set; }
    //public DbSet<TradingDay> St65 { get; set; }
    //    public DbSet<TradingDay> St66 { get; set; }
    //    public DbSet<TradingDay> St67 { get; set; }
    //    public DbSet<TradingDay> St68 { get; set; }
    //    public DbSet<TradingDay> St69 { get; set; }
    //    public DbSet<TradingDay> St70 { get; set; }
    //    public DbSet<TradingDay> St71 { get; set; }
    //    public DbSet<TradingDay> St72 { get; set; }
    //    public DbSet<TradingDay> St73 { get; set; }
    //    public DbSet<TradingDay> St74 { get; set; }
    //    public DbSet<TradingDay> St75 { get; set; }
    //    public DbSet<TradingDay> St76 { get; set; }
    //    public DbSet<TradingDay> St77 { get; set; }
    //    public DbSet<TradingDay> St78 { get; set; }
    //    public DbSet<TradingDay> St79 { get; set; }
    //    public DbSet<TradingDay> St80 { get; set; }
    //    public DbSet<TradingDay> St81 { get; set; }
    //    public DbSet<TradingDay> St82 { get; set; }
    //    public DbSet<TradingDay> St83 { get; set; }
    //    public DbSet<TradingDay> St84 { get; set; }
    //    public DbSet<TradingDay> St85 { get; set; }
    //    public DbSet<TradingDay> St86 { get; set; }
    //    public DbSet<TradingDay> St87 { get; set; }
    //    public DbSet<TradingDay> St88 { get; set; }
    //    public DbSet<TradingDay> St89 { get; set; }
    //    public DbSet<TradingDay> St90 { get; set; }
    //    public DbSet<TradingDay> St91 { get; set; }
    //    public DbSet<TradingDay> St92 { get; set; }
    //    public DbSet<TradingDay> St93 { get; set; }
    //    public DbSet<TradingDay> St94 { get; set; }
    //    public DbSet<TradingDay> St95 { get; set; }
    //    public DbSet<TradingDay> St96 { get; set; }
    //    public DbSet<TradingDay> St97 { get; set; }
    //    public DbSet<TradingDay> St98 { get; set; }
    //    public DbSet<TradingDay> St99 { get; set; }
    //    public DbSet<TradingDay> St100 { get; set; }
    //    public DbSet<TradingDay> St101 { get; set; }
    //    public DbSet<TradingDay> St102 { get; set; }
    //    public DbSet<TradingDay> St103 { get; set; }
    //    public DbSet<TradingDay> St104 { get; set; }
    //    public DbSet<TradingDay> St105 { get; set; }
    //    public DbSet<TradingDay> St106 { get; set; }
    //    public DbSet<TradingDay> St107 { get; set; }
    //    public DbSet<TradingDay> St108 { get; set; }
    //    public DbSet<TradingDay> St109 { get; set; }
    //    public DbSet<TradingDay> St110 { get; set; }
    //    public DbSet<TradingDay> St111 { get; set; }
    //    public DbSet<TradingDay> St112 { get; set; }
    //    public DbSet<TradingDay> St113 { get; set; }
    //    public DbSet<TradingDay> St114 { get; set; }
    //    public DbSet<TradingDay> St115 { get; set; }
    //    public DbSet<TradingDay> St116 { get; set; }
    //    public DbSet<TradingDay> St117 { get; set; }
    //    public DbSet<TradingDay> St118 { get; set; }
    //    public DbSet<TradingDay> St119 { get; set; }
    //    public DbSet<TradingDay> St120 { get; set; }
    //    public DbSet<TradingDay> St121 { get; set; }
    //    public DbSet<TradingDay> St122 { get; set; }
    //    public DbSet<TradingDay> St123 { get; set; }
    //    public DbSet<TradingDay> St124 { get; set; }
    //    public DbSet<TradingDay> St125 { get; set; }
    //    public DbSet<TradingDay> St126 { get; set; }
    //    public DbSet<TradingDay> St127 { get; set; }
    //    public DbSet<TradingDay> St128 { get; set; }
    //    public DbSet<TradingDay> St129 { get; set; }
    //    public DbSet<TradingDay> St130 { get; set; }
    //    public DbSet<TradingDay> St131 { get; set; }
    //    public DbSet<TradingDay> St132 { get; set; }
    //    public DbSet<TradingDay> St133 { get; set; }
    //    public DbSet<TradingDay> St134 { get; set; }
    //    public DbSet<TradingDay> St135 { get; set; }
    //    public DbSet<TradingDay> St136 { get; set; }
    //    public DbSet<TradingDay> St137 { get; set; }
    //    public DbSet<TradingDay> St138 { get; set; }
    //    public DbSet<TradingDay> St139 { get; set; }
    //    public DbSet<TradingDay> St140 { get; set; }
    //    public DbSet<TradingDay> St141 { get; set; }
    //    public DbSet<TradingDay> St142 { get; set; }
    //    public DbSet<TradingDay> St143 { get; set; }
    //    public DbSet<TradingDay> St144 { get; set; }
    //    public DbSet<TradingDay> St145 { get; set; }
    //    public DbSet<TradingDay> St146 { get; set; }
    //    public DbSet<TradingDay> St147 { get; set; }
    //    public DbSet<TradingDay> St148 { get; set; }
    //    public DbSet<TradingDay> St149 { get; set; }
    //    public DbSet<TradingDay> St150 { get; set; }
    //    public DbSet<TradingDay> St151 { get; set; }
    //    public DbSet<TradingDay> St152 { get; set; }
    //    public DbSet<TradingDay> St153 { get; set; }
    //    public DbSet<TradingDay> St154 { get; set; }
    //    public DbSet<TradingDay> St155 { get; set; }
    //    public DbSet<TradingDay> St156 { get; set; }
    //    public DbSet<TradingDay> St167 { get; set; }
    //    public DbSet<TradingDay> St168 { get; set; }
    //    public DbSet<TradingDay> St169 { get; set; }
    //    public DbSet<TradingDay> St170 { get; set; }
    //    public DbSet<TradingDay> St171 { get; set; }
    //    public DbSet<TradingDay> St172 { get; set; }
    //    public DbSet<TradingDay> St173 { get; set; }
    //    public DbSet<TradingDay> St174 { get; set; }
    //    public DbSet<TradingDay> St175 { get; set; }
    //    public DbSet<TradingDay> St176 { get; set; }
    //    public DbSet<TradingDay> St177 { get; set; }
    //    public DbSet<TradingDay> St178 { get; set; }
    //    public DbSet<TradingDay> St179 { get; set; }
    //    public DbSet<TradingDay> St180 { get; set; }
    //    public DbSet<TradingDay> St181 { get; set; }
    //    public DbSet<TradingDay> St182 { get; set; }
    //    public DbSet<TradingDay> St183 { get; set; }
    //    public DbSet<TradingDay> St184 { get; set; }
    //    public DbSet<TradingDay> St185 { get; set; }
    //    public DbSet<TradingDay> St186 { get; set; }
    //    public DbSet<TradingDay> St187 { get; set; }
    //    public DbSet<TradingDay> St188 { get; set; }
    //    public DbSet<TradingDay> St189 { get; set; }
    //    public DbSet<TradingDay> St190 { get; set; }
    //    public DbSet<TradingDay> St191 { get; set; }
    //    public DbSet<TradingDay> St192 { get; set; }
    //    public DbSet<TradingDay> St193 { get; set; }
    //    public DbSet<TradingDay> St194 { get; set; }
    //    public DbSet<TradingDay> St195 { get; set; }
    //    public DbSet<TradingDay> St196 { get; set; }
    //    public DbSet<TradingDay> St197 { get; set; }
    //    public DbSet<TradingDay> St198 { get; set; }
    //    public DbSet<TradingDay> St199 { get; set; }
    //    public DbSet<TradingDay> St200 { get; set; }
    //    public DbSet<TradingDay> St201 { get; set; }
    //    public DbSet<TradingDay> St202 { get; set; }
    //    public DbSet<TradingDay> St203 { get; set; }
    //    public DbSet<TradingDay> St204 { get; set; }
    //    public DbSet<TradingDay> St205 { get; set; }
    //    public DbSet<TradingDay> St206 { get; set; }
    //    public DbSet<TradingDay> St207 { get; set; }
    //    public DbSet<TradingDay> St208 { get; set; }
    //    public DbSet<TradingDay> St209 { get; set; }
    //    public DbSet<TradingDay> St210 { get; set; }
    //    public DbSet<TradingDay> St211 { get; set; }
    //    public DbSet<TradingDay> St212 { get; set; }
    //    public DbSet<TradingDay> St213 { get; set; }
    //    public DbSet<TradingDay> St214 { get; set; }
    //    public DbSet<TradingDay> St215 { get; set; }
    //    public DbSet<TradingDay> St216 { get; set; }
    //    public DbSet<TradingDay> St217 { get; set; }
    //    public DbSet<TradingDay> St218 { get; set; }
    //    public DbSet<TradingDay> St219 { get; set; }
    //    public DbSet<TradingDay> St220 { get; set; }
    //    public DbSet<TradingDay> St221 { get; set; }
    //    public DbSet<TradingDay> St222 { get; set; }
    //    public DbSet<TradingDay> St223 { get; set; }
    //    public DbSet<TradingDay> St224 { get; set; }
    //    public DbSet<TradingDay> St225 { get; set; }
    //    public DbSet<TradingDay> St226 { get; set; }
    //    public DbSet<TradingDay> St227 { get; set; }
    //    public DbSet<TradingDay> St228 { get; set; }
    //    public DbSet<TradingDay> St229 { get; set; }
    //    public DbSet<TradingDay> St230 { get; set; }
    //    public DbSet<TradingDay> St231 { get; set; }
    //    public DbSet<TradingDay> St232 { get; set; }
    //    public DbSet<TradingDay> St233 { get; set; }
    //    public DbSet<TradingDay> St234 { get; set; }
    //    public DbSet<TradingDay> St235 { get; set; }
    //    public DbSet<TradingDay> St236 { get; set; }
    //    public DbSet<TradingDay> St237 { get; set; }
    //    public DbSet<TradingDay> St238 { get; set; }
    //    public DbSet<TradingDay> St239 { get; set; }
    //    public DbSet<TradingDay> St240 { get; set; }
    //    public DbSet<TradingDay> St241 { get; set; }
    //    public DbSet<TradingDay> St242 { get; set; }
    //    public DbSet<TradingDay> St243 { get; set; }
    //    public DbSet<TradingDay> St244 { get; set; }
    //    public DbSet<TradingDay> St245 { get; set; }
    //    public DbSet<TradingDay> St246 { get; set; }
    //    public DbSet<TradingDay> St247 { get; set; }
    //    public DbSet<TradingDay> St248 { get; set; }
    //    public DbSet<TradingDay> St249 { get; set; }
    //    public DbSet<TradingDay> St250 { get; set; }
    //    public DbSet<TradingDay> St251 { get; set; }
    //    public DbSet<TradingDay> St252 { get; set; }
    //    public DbSet<TradingDay> St253 { get; set; }
    //    public DbSet<TradingDay> St254 { get; set; }
    //    public DbSet<TradingDay> St255 { get; set; }
    //    public DbSet<TradingDay> St256 { get; set; }
    //    public DbSet<TradingDay> St257 { get; set; }
    //    public DbSet<TradingDay> St258 { get; set; }
    //    public DbSet<TradingDay> St259 { get; set; }
    //    public DbSet<TradingDay> St260 { get; set; }
    //    public DbSet<TradingDay> St261 { get; set; }
    //    public DbSet<TradingDay> St262 { get; set; }
    //    public DbSet<TradingDay> St263 { get; set; }
    //    public DbSet<TradingDay> St264 { get; set; }
    //    public DbSet<TradingDay> St265 { get; set; }
    //    public DbSet<TradingDay> St266 { get; set; }
    //    public DbSet<TradingDay> St267 { get; set; }
    //    public DbSet<TradingDay> St268 { get; set; }
    //    public DbSet<TradingDay> St269 { get; set; }
    //    public DbSet<TradingDay> St270 { get; set; }
    //    public DbSet<TradingDay> St271 { get; set; }
    //    public DbSet<TradingDay> St272 { get; set; }
    //    public DbSet<TradingDay> St273 { get; set; }
    //    public DbSet<TradingDay> St274 { get; set; }
    //    public DbSet<TradingDay> St275 { get; set; }
    //    public DbSet<TradingDay> St276 { get; set; }
    //    public DbSet<TradingDay> St277 { get; set; }
    //    public DbSet<TradingDay> St278 { get; set; }
    //    public DbSet<TradingDay> St279 { get; set; }
    //    public DbSet<TradingDay> St280 { get; set; }
    //    public DbSet<TradingDay> St281 { get; set; }
    //    public DbSet<TradingDay> St282 { get; set; }
    //    public DbSet<TradingDay> St283 { get; set; }
    //    public DbSet<TradingDay> St284 { get; set; }
    //    public DbSet<TradingDay> St285 { get; set; }
    //    public DbSet<TradingDay> St286 { get; set; }
    //    public DbSet<TradingDay> St287 { get; set; }
    //    public DbSet<TradingDay> St288 { get; set; }
    //    public DbSet<TradingDay> St289 { get; set; }
    //    public DbSet<TradingDay> St290 { get; set; }
    //    public DbSet<TradingDay> St291 { get; set; }
    //    public DbSet<TradingDay> St292 { get; set; }
    //    public DbSet<TradingDay> St293 { get; set; }
    //    public DbSet<TradingDay> St294 { get; set; }
    //    public DbSet<TradingDay> St295 { get; set; }
    //    public DbSet<TradingDay> St296 { get; set; }
    //    public DbSet<TradingDay> St297 { get; set; }
    //    public DbSet<TradingDay> St298 { get; set; }
    //    public DbSet<TradingDay> St299 { get; set; }
    //    public DbSet<TradingDay> St300 { get; set; }
    //    public DbSet<TradingDay> St301 { get; set; }
    //    public DbSet<TradingDay> St302 { get; set; }
    //    public DbSet<TradingDay> St303 { get; set; }
    //    public DbSet<TradingDay> St304 { get; set; }
    //    public DbSet<TradingDay> St305 { get; set; }
    //    public DbSet<TradingDay> St306 { get; set; }
    //    public DbSet<TradingDay> St307 { get; set; }
    //    public DbSet<TradingDay> St308 { get; set; }
    //    public DbSet<TradingDay> St309 { get; set; }
    //    public DbSet<TradingDay> St310 { get; set; }
    //    public DbSet<TradingDay> St311 { get; set; }
    //    public DbSet<TradingDay> St312 { get; set; }
    //    public DbSet<TradingDay> St313 { get; set; }
    //    public DbSet<TradingDay> St314 { get; set; }
    //    public DbSet<TradingDay> St315 { get; set; }
    //    public DbSet<TradingDay> St317 { get; set; }
    //    public DbSet<TradingDay> St318 { get; set; }
    //    public DbSet<TradingDay> St319 { get; set; }
    //    public DbSet<TradingDay> St320 { get; set; }
    //    public DbSet<TradingDay> St321 { get; set; }
    //    public DbSet<TradingDay> St322 { get; set; }
    //    public DbSet<TradingDay> St323 { get; set; }
    //    public DbSet<TradingDay> St324 { get; set; }
    //    public DbSet<TradingDay> St325 { get; set; }
    //    public DbSet<TradingDay> St326 { get; set; }
    //    public DbSet<TradingDay> St327 { get; set; }
    //    public DbSet<TradingDay> St328 { get; set; }
    //    public DbSet<TradingDay> St329 { get; set; }
    //    public DbSet<TradingDay> St330 { get; set; }
    //    public DbSet<TradingDay> St331 { get; set; }
    //    public DbSet<TradingDay> St332 { get; set; }
    //    public DbSet<TradingDay> St333 { get; set; }
    //    public DbSet<TradingDay> St334 { get; set; }
    //    public DbSet<TradingDay> St335 { get; set; }
    //    public DbSet<TradingDay> St336 { get; set; }
    //    public DbSet<TradingDay> St337 { get; set; }
    //    public DbSet<TradingDay> St338 { get; set; }
    //    public DbSet<TradingDay> St339 { get; set; }
    //    public DbSet<TradingDay> St340 { get; set; }
    //    public DbSet<TradingDay> St341 { get; set; }
    //    public DbSet<TradingDay> St342 { get; set; }
    //    public DbSet<TradingDay> St343 { get; set; }
    //    public DbSet<TradingDay> St344 { get; set; }
    //    public DbSet<TradingDay> St345 { get; set; }
    //    public DbSet<TradingDay> St346 { get; set; }
    //    public DbSet<TradingDay> St347 { get; set; }
    //    public DbSet<TradingDay> St348 { get; set; }
    //    public DbSet<TradingDay> St349 { get; set; }
    //    public DbSet<TradingDay> St350 { get; set; }
    //    public DbSet<TradingDay> St351 { get; set; }
    //    public DbSet<TradingDay> St352 { get; set; }
    //    public DbSet<TradingDay> St353 { get; set; }
    //    public DbSet<TradingDay> St354 { get; set; }
    //    public DbSet<TradingDay> St355 { get; set; }
    //    public DbSet<TradingDay> St356 { get; set; }
    //    public DbSet<TradingDay> St357 { get; set; }
    //    public DbSet<TradingDay> St358 { get; set; }
    //    public DbSet<TradingDay> St359 { get; set; }
    //    public DbSet<TradingDay> St360 { get; set; }
    //    public DbSet<TradingDay> St361 { get; set; }
    //    public DbSet<TradingDay> St362 { get; set; }
    //    public DbSet<TradingDay> St363 { get; set; }
    //    public DbSet<TradingDay> St364 { get; set; }
    //    public DbSet<TradingDay> St365 { get; set; }
    //    public DbSet<TradingDay> St366 { get; set; }
    //    public DbSet<TradingDay> St367 { get; set; }
    //    public DbSet<TradingDay> St368 { get; set; }
    //    public DbSet<TradingDay> St369 { get; set; }
    //    public DbSet<TradingDay> St370 { get; set; }
    //    public DbSet<TradingDay> St371 { get; set; }
    //    public DbSet<TradingDay> St372 { get; set; }
    //    public DbSet<TradingDay> St373 { get; set; }
    //    public DbSet<TradingDay> St374 { get; set; }
    //    public DbSet<TradingDay> St375 { get; set; }
    //    public DbSet<TradingDay> St376 { get; set; }
    //    public DbSet<TradingDay> St377 { get; set; }
    //    public DbSet<TradingDay> St378 { get; set; }
    //    public DbSet<TradingDay> St379 { get; set; }
    //    public DbSet<TradingDay> St380 { get; set; }
    //    public DbSet<TradingDay> St381 { get; set; }
    //    public DbSet<TradingDay> St382 { get; set; }
    //    public DbSet<TradingDay> St383 { get; set; }
    //    public DbSet<TradingDay> St384 { get; set; }
    //    public DbSet<TradingDay> St385 { get; set; }
    //    public DbSet<TradingDay> St386 { get; set; }
    //    public DbSet<TradingDay> St387 { get; set; }
    //    public DbSet<TradingDay> St388 { get; set; }
    //    public DbSet<TradingDay> St389 { get; set; }
    //    public DbSet<TradingDay> St390 { get; set; }
    //    public DbSet<TradingDay> St391 { get; set; }
    //    public DbSet<TradingDay> St392 { get; set; }
    //    public DbSet<TradingDay> St393 { get; set; }
    //    public DbSet<TradingDay> St394 { get; set; }
    //    public DbSet<TradingDay> St395 { get; set; }
    //    public DbSet<TradingDay> St396 { get; set; }
    //    public DbSet<TradingDay> St397 { get; set; }
    //    public DbSet<TradingDay> St398 { get; set; }
    //    public DbSet<TradingDay> St399 { get; set; }
    //    public DbSet<TradingDay> St400 { get; set; }
    //    public DbSet<TradingDay> St401 { get; set; }
    //    public DbSet<TradingDay> St402 { get; set; }
    //    public DbSet<TradingDay> St403 { get; set; }
    //    public DbSet<TradingDay> St404 { get; set; }
    //    public DbSet<TradingDay> St405 { get; set; }
    //    public DbSet<TradingDay> St406 { get; set; }
    //    public DbSet<TradingDay> St407 { get; set; }
    //    public DbSet<TradingDay> St408 { get; set; }
    //    public DbSet<TradingDay> St409 { get; set; }
    //    public DbSet<TradingDay> St410 { get; set; }
    //    public DbSet<TradingDay> St411 { get; set; }
    //    public DbSet<TradingDay> St412 { get; set; }
    //    public DbSet<TradingDay> St413 { get; set; }
    //    public DbSet<TradingDay> St414 { get; set; }
    //    public DbSet<TradingDay> St415 { get; set; }
    //    public DbSet<TradingDay> St416 { get; set; }
    //    public DbSet<TradingDay> St417 { get; set; }
    //    public DbSet<TradingDay> St418 { get; set; }
    //    public DbSet<TradingDay> St419 { get; set; }
    //    public DbSet<TradingDay> St420 { get; set; }
    //    public DbSet<TradingDay> St421 { get; set; }
    //    public DbSet<TradingDay> St422 { get; set; }
    //    public DbSet<TradingDay> St423 { get; set; }
    //    public DbSet<TradingDay> St424 { get; set; }
    //    public DbSet<TradingDay> St425 { get; set; }
    //    public DbSet<TradingDay> St426 { get; set; }
    //    public DbSet<TradingDay> St427 { get; set; }
    //    public DbSet<TradingDay> St428 { get; set; }
    //    public DbSet<TradingDay> St429 { get; set; }
    //    public DbSet<TradingDay> St430 { get; set; }
    //    public DbSet<TradingDay> St431 { get; set; }
    //    public DbSet<TradingDay> St432 { get; set; }
    //    public DbSet<TradingDay> St433 { get; set; }
    //    public DbSet<TradingDay> St434 { get; set; }
    //    public DbSet<TradingDay> St435 { get; set; }
    //    public DbSet<TradingDay> St436 { get; set; }
    //    public DbSet<TradingDay> St437 { get; set; }
    //    public DbSet<TradingDay> St438 { get; set; }
    //    public DbSet<TradingDay> St439 { get; set; }
    //    public DbSet<TradingDay> St440 { get; set; }
    //    public DbSet<TradingDay> St441 { get; set; }
    //    public DbSet<TradingDay> St442 { get; set; }
    //    public DbSet<TradingDay> St443 { get; set; }
    //    public DbSet<TradingDay> St444 { get; set; }
    //    public DbSet<TradingDay> St445 { get; set; }
    //    public DbSet<TradingDay> St446 { get; set; }
    //    public DbSet<TradingDay> St447 { get; set; }
    //    public DbSet<TradingDay> St448 { get; set; }
    //    public DbSet<TradingDay> St449 { get; set; }
    //    public DbSet<TradingDay> St450 { get; set; }
    //    public DbSet<TradingDay> St451 { get; set; }
    //    public DbSet<TradingDay> St452 { get; set; }
    //    public DbSet<TradingDay> St453 { get; set; }
    //    public DbSet<TradingDay> St454 { get; set; }
    //    public DbSet<TradingDay> St455 { get; set; }
    //    public DbSet<TradingDay> St456 { get; set; }
    //    public DbSet<TradingDay> St457 { get; set; }
    //    public DbSet<TradingDay> St458 { get; set; }
    //    public DbSet<TradingDay> St459 { get; set; }
    //    public DbSet<TradingDay> St460 { get; set; }
    //    public DbSet<TradingDay> St461 { get; set; }
    //    public DbSet<TradingDay> St462 { get; set; }
    //    public DbSet<TradingDay> St463 { get; set; }
    //    public DbSet<TradingDay> St464 { get; set; }
    //    public DbSet<TradingDay> St465 { get; set; }
    //    public DbSet<TradingDay> St466 { get; set; }
    //    public DbSet<TradingDay> St467 { get; set; }
    //    public DbSet<TradingDay> St468 { get; set; }
    //    public DbSet<TradingDay> St469 { get; set; }
    //    public DbSet<TradingDay> St470 { get; set; }
    //    public DbSet<TradingDay> St471 { get; set; }
    //    public DbSet<TradingDay> St472 { get; set; }
    //    public DbSet<TradingDay> St473 { get; set; }
    //    public DbSet<TradingDay> St474 { get; set; }
    //    public DbSet<TradingDay> St475 { get; set; }
    //    public DbSet<TradingDay> St476 { get; set; }
    //    public DbSet<TradingDay> St477 { get; set; }
    //    public DbSet<TradingDay> St478 { get; set; }
    //    public DbSet<TradingDay> St479 { get; set; }
    //    public DbSet<TradingDay> St480 { get; set; }
    //    public DbSet<TradingDay> St481 { get; set; }
    //    public DbSet<TradingDay> St482 { get; set; }
    //    public DbSet<TradingDay> St483 { get; set; }
    //    public DbSet<TradingDay> St484 { get; set; }
    //    public DbSet<TradingDay> St485 { get; set; }
    //    public DbSet<TradingDay> St486 { get; set; }
    //    public DbSet<TradingDay> St487 { get; set; }
    //    public DbSet<TradingDay> St488 { get; set; }
    //    public DbSet<TradingDay> St489 { get; set; }
    //    public DbSet<TradingDay> St490 { get; set; }
    //    public DbSet<TradingDay> St491 { get; set; }
    //    public DbSet<TradingDay> St492 { get; set; }
    //    public DbSet<TradingDay> St493 { get; set; }
    //    public DbSet<TradingDay> St494 { get; set; }
    //    public DbSet<TradingDay> St495 { get; set; }
    //    public DbSet<TradingDay> St496 { get; set; }
    //    public DbSet<TradingDay> St497 { get; set; }
    //    public DbSet<TradingDay> St498 { get; set; }
    //    public DbSet<TradingDay> St499 { get; set; }
    //    public DbSet<TradingDay> St500 { get; set; }
        //public DbSet<Stock> Technicals { get; set; }

        
        
    }

    //public class DestConfig : EntityTypeConfiguration<StockContext>
    //{
    //    public DestConfig()
    //    {
    //        Property(d => d.Nam)
    //    }
    //}
    
}
