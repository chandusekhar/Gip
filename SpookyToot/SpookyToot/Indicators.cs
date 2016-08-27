using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Models.Regression.Linear;
using TicTacTec.TA.Library;

namespace SpookyToot
{

    [Serializable]
    public class Indicators
    {
        public List<SSTO> SSTOIndicators { get; set; }
        public List<MACD> MACDIndicators { get; set; }
        public List<RSI> RSIIndicators { get; set; }
        public List<Acos> AcosIndicators { get; set; }
        public List<AD> ADIndicators { get; set; }
        public List<ADD> ADDIndicators { get; set; }
        public List<Adx> IND3Indicators { get; set; }
        public List<AdOsc> AdOscIndicators { get; set; }
        public List<AdxR> AdxRIndicators { get; set; }
        public List<Apo> ApoIndicators { get; set; }
        public List<Aroon> AroonIndicators { get; set; }
        public List<AroonOsc> AroonOscIndicators { get; set; }
        public List<Asin> AsinIndicators { get; set; }
        public List<Atan> AtanIndicators { get; set; }
        public List<ATR> AtrIndicators { get; set; }
        public List<AveragePrice> AveragePriceIndicators { get; set; }
        public List<BollingerBands> BollingerBandsIndicators { get; set; }
        public List<Beta> BetaIndicators { get; set; }
        public List<Bop> BopIndicators { get; set; }
        public List<Cci> CciIndicators { get; set; }
        public List<Ceil> CeilIndicators { get; set; }
        public List<Cmo> CmoIndicators { get; set; }
        public List<Cos> CosIndicators { get; set; }
        public List<Cosh> CoshIndicators { get; set; }
        public List<Dema> DemaIndicators { get; set; }
        public List<Div> DivIndicators { get; set; }
        public List<Dx> DxIndicators { get; set; }
        public List<Ema> EmaIndicators { get; set; }
        public List<Exp> ExpIndicators { get; set; }
        public List<Floor> FloorIndicators { get; set; }
        public List<FRAMA> FRAMAIndicators { get; set; }
        public List<HtDcPeriod> HtDcPeriodIndicators { get; set; }
        public List<HtDcPhase> HtDcPhaseIndicators { get; set; }
        public List<HtPhasor> HtPhasorIndicators { get; set; }
        public List<HtSine> HtSineIndicators { get; set; }
        public List<HtTrendline> HtTrendlineIndicators { get; set; }
        public List<HtTrendMode> HtTrendModeIndicators { get; set; }
        public List<Kama> KamaIndicators { get; set; }
        public List<LinearReg> LinearRegIndicators { get; set; }
        public List<LinearRegAngle> LinearRegAngleIndicators { get; set; }
        public List<LinearRegIntercept> LinearRegInterceptIndicators { get; set; }
        public List<LinearRegSlope> LinearRegSlopeIndicators { get; set; }
        public List<Ln> LnIndicators { get; set; }
        public List<Log10> Log10Indicators { get; set; }
        public List<MacdExt> MacdExtIndicators { get; set; }
        public List<MacdFix> MacdFixIndicators { get; set; }
        public List<Mama> MamaIndicators { get; set; }
        public List<Max> MaxIndicators { get; set; }
        public List<MaxIndex> MaxIndexIndicators { get; set; }
        public List<MedPrice> MedPriceIndicators { get; set; }
        public List<Mfi> MfiIndicators { get; set; }
        public List<MidPoint> MidPointIndicators { get; set; }
        public List<MidPrice> MidPriceIndicators { get; set; }
        public List<Min> MinIndicators { get; set; }
        public List<MinIndex> MinIndexIndicators { get; set; }
        public List<MinMax> MinMaxIndicators { get; set; }
        public List<MinusDI> MinusDIIndicators { get; set; }
        public List<MinusDM> MinusDMIndicators { get; set; }
        public List<Mom> MomIndicators { get; set; }
        public List<MovingAverage> MovingAverageIndicators { get; set; }
        public List<MovingAverageVariablePeriod> MovingAverageVariablePeriodIndicators { get; set; }
        public List<Mult> MultIndicators { get; set; }
        public List<Natr> NatrIndicators { get; set; }
        public List<Obv> ObvIndicators { get; set; }
        public List<LinearRegressionIndicators> LinearRegressionTrendIndicators { get; set; }
        public List<PlusDI> PlusDIIndicators { get; set; }
        public List<PlusDM> PlusDMIndicators { get; set; }
        public List<Ppo> PpoIndicators { get; set; }
        public List<Roc> RocIndicators { get; set; }
        public List<RocP> RocPIndicators { get; set; }
        public List<RocR> RocRIndicators { get; set; }
        public List<RocR100> RocR100Indicators { get; set; }
        public List<Sar> SarIndicators { get; set; }
        public List<SarExt> SarExtIndicators { get; set; }
        public List<SimpleTrend> SimpleTrensIndicators { get; set; }
        public List<Sin> SinIndicators { get; set; }
        public List<Sinh> SinhIndicators { get; set; }
        public List<Sma> SmaIndicators { get; set; }
        public List<Sqrt> SqrtIndicators { get; set; }
        public List<StdDev> StdDevIndicators { get; set; }
        public List<StochF> StochFIndicators { get; set; }
        public List<StochRsi> StochRsiIndicators { get; set; }
        public List<Sub> SubIndicators { get; set; }
        public List<Sum> SumIndicators { get; set; }
        public List<T3> T3Indicators { get; set; }
        public List<Tan> TanIndicators { get; set; }
        public List<Tanh> TanhIndicators { get; set; }
        public List<Tema> TemaIndicators { get; set; }
        public List<Trima> TrimaIndicators { get; set; }
        public List<Trix> TrixIndicators { get; set; }
        public List<TrueRange> TrueRangeIndicators { get; set; }
        public List<Tsf> TsfIndicators { get; set; }
        public List<TypPrice> TypPriceIndicators { get; set; }
        public List<UltOsc> UltOscIndicators { get; set; }
        public List<WclPrice> WclPriceIndicators { get; set; }
        public List<Variance> VarianceIndicators { get; set; }
        public List<WillR> WillRIndicators { get; set; }
        public List<Wma> WmaIndicators { get; set; }


        public static List<double> CleanTicTac(List<double> input)
        {
            double[] Shift = new double[input.Count];
            int count = 0;
            int otherCount = input.Count - 1;
            for (int i = input.Count - 1; i >= 0; i--)
            {
                if (input[i] != 0)
                {
                    Shift[otherCount] = input[i];
                    otherCount--;
                }
                else count++;
            }

            List<double> V = new List<double>();
            for (int i = 0; i < Shift.Length; i++)
            {
                V.Add(Shift[i]);
            }
            V.Insert(0, 0);
            return V;
        }


        public List<KeyValuePair<int, double>> GetIntoChartFormat(List<double> input)

        {
            double[] Shift = new double[input.Count];
            int count = 0;
            int otherCount = input.Count - 1;
            for (int i = input.Count - 1; i >= 0; i--)
            {
                if (input[i] != 0)
                {
                    Shift[otherCount] = input[i];
                    otherCount--;
                }
                else count++;
            }

            List<KeyValuePair<int, double>> V = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < Shift.Length; i++)
            {
                V.Add(new KeyValuePair<int, double>(i, Shift[i]));
            }

            for (int i = V.Count - 1; i >= 0; i--)
            {
                if (V[i].Value == 0) V.RemoveAt(i);
            }
            return V;
        }

        public class LinearRegressionIndicators
        {
            public List<double> TrendLines { get; set; }

            public LinearRegressionIndicators(List<double> Prices, double maxError)
            {
                TrendLines = new List<double>();
                int TrackPrices = Prices.Count;
                int LoopCount = 0;
                bool detectError = false;


                while ((TrackPrices > 0) && !detectError)
                {
                    double Calculatederror = double.MaxValue;
                    List<double> WorkingSpace = Prices.GetRange(TrendLines.Count, TrackPrices);


                    while ((Calculatederror > maxError) && !detectError)
                    {
                        List<double> PotentialTrenLine = new List<double>();
                        LoopCount++;

                        Accord.Statistics.Models.Regression.Linear.SimpleLinearRegression Calculate = new SimpleLinearRegression();
                        Calculatederror = Calculate.Regress(GetPeriods(WorkingSpace), WorkingSpace.ToArray());
                        if (Calculatederror > maxError)
                        {
                            WorkingSpace.RemoveAt(WorkingSpace.Count - 1);
                        }
                        else
                        {
                            PotentialTrenLine.AddRange(Calculate.Compute(GetPeriods(WorkingSpace)));
                            //int Min = PotentialTrenLine.IndexOf(PotentialTrenLine.Min());
                            if (PotentialTrenLine.Count < 20)
                            {
                                foreach (var p in WorkingSpace)
                                {
                                    TrendLines.Add(TrendLines.Last());
                                }
                            }
                            else TrendLines.AddRange(PotentialTrenLine);
                            //TrendLines.AddRange(PotentialTrenLine.GetRange(0, Min));
                        }

                    }
                    TrackPrices = Prices.Count - TrendLines.Count;

                }
                for (int i = TrendLines.Count - 1; i >= 0; i--)
                {
                    TrendLines[i] = TrendLines[i] - 0.1;
                }

            }



            public double[] GetPeriods(List<double> Inputs)
            {
                List<double> Outputs = new List<double>();
                for (int i = 0; i < Inputs.Count; i++)
                {
                    Outputs.Add(i);
                }

                return Outputs.ToArray();
            }

        }


        public class SimpleTrend
        {
            public List<double> TrendLines { get; set; }

            public SimpleTrend(int res, List<double> Prices)
            {
                int resStart = 0;
                int resFinish = res;

                TrendLines = new List<double>();

                List<double> peaks = new List<double>();
                List<int> peakPer = new List<int>();
                List<double> troughs = new List<double>();
                List<int> troughPer = new List<int>();

                while ((resStart + res) < Prices.Count)
                {
                    peaks.Add(Prices.GetRange(resStart, res).Max());
                    int pr = Prices.GetRange(resStart, res).IndexOf(Prices.GetRange(resStart, res).Max());
                    peakPer.Add(pr + resStart);
                    troughs.Add(Prices.GetRange(resStart, res).Min());
                    int tr = Prices.GetRange(resStart, res).IndexOf(Prices.GetRange(resStart, res).Min());
                    troughPer.Add(tr + resStart);

                    resStart += res;
                    resFinish += res;

                }

                for (int i = 0; i < troughs.Count - 1; i++)
                {
                    int traversePeriod = 0;
                    double increment = 0;
                    if (i == 0)
                    {
                        traversePeriod = troughPer[i];
                        increment = (troughs[i])/traversePeriod;
                    }
                    else
                    {
                        traversePeriod = (troughPer[i] - troughPer[i - 1]);
                        increment = (troughs[i] - troughs[i - 1])/traversePeriod;
                    }



                    for (int u = 0; u < traversePeriod; u++)
                    {
                        if (i > 0) TrendLines.Add(((double) troughs[i - 1] + increment*u));
                        else TrendLines.Add(increment*(double) u/(double) traversePeriod);
                    }
                }
            }

        }

        public class MACD
        {
            public List<double> Fast { get; set; }
            public List<double> Signal { get; set; }
            public List<double> Slow { get; set; }
            public string Timeperiod;

            public MACD(int fast, int slow, int signal, int recent, int early, List<double> real)
            {
                Fast = new List<double>();
                Signal = new List<double>();
                Slow = new List<double>();
                Timeperiod = fast.ToString() + " " + slow.ToString() + " " + signal.ToString();

                double[] Real = real.ToArray();
                int optInFastPeriod = fast;
                int SlowPeriod = slow;
                int optInSignalPeriod = signal;
                int endindex = recent + early;

                int begidx;
                int NBElement;
                double[] MACD = new double[Math.Abs(early - recent) + 1];
                double[] MACSignal = new double[Math.Abs(early - recent) + 1];
                double[] MACDHist = new double[Math.Abs(early - recent) + 1];

                TicTacTec.TA.Library.Core.Macd(recent, early, Real, optInFastPeriod, SlowPeriod, optInSignalPeriod,
                    out begidx, out NBElement, MACD, MACSignal, MACDHist);
                Fast = CleanTicTac(MACD.ToList());
                Signal = CleanTicTac(MACSignal.ToList());
                Slow = CleanTicTac(MACDHist.ToList());

            }

        }

        public class FRAMA
        {
            public List<double> FRAMAHist { get; set; }
            public string TimePeriod;

            public FRAMA(int period, int fasterEMA, int SlowerEMA, List<double> Highs, List<double> Lows,
                List<double> Close)
            {
                FRAMAHist = new List<double>();

                TimePeriod = period.ToString() + " " + fasterEMA.ToString() + " " + SlowerEMA.ToString();

                if (period < Close.Count && SlowerEMA < Close.Count)
                {
                    for (int i = 0; i < period; i++)
                    {
                        FRAMAHist.Add(0);
                    }

                    double W = Math.Log(2.0/((double) SlowerEMA + 1.0));
                    double H = ((SlowerEMA - fasterEMA)/2.0) + fasterEMA;
                    double FRAMA1 = Close.GetRange(0, (int) H).Average();

                    for (int i = period; i < Close.Count; i++)
                    {
                        double HL1 = (Highs.GetRange(i - period, period/2).Max() -
                                      Lows.GetRange(i - period, period/2).Min())/(0.5*(double) period);
                        double HL2 = (Highs.GetRange(i - (period/2), period/2).Max() -
                                      Lows.GetRange(i - (period/2), period/2).Min())/(0.5*(double) period);
                        double HL = (Highs.GetRange(i - period, period).Max() - Lows.GetRange(i - period, period).Min())/
                                    (double) period;
                        double FractalDimension = (Math.Log10(HL1 + HL2) - Math.Log10(HL))/Math.Log10(2.0);
                        double Alpha = Math.Exp(W*(FractalDimension - 1.0));
                        double origN = (2.0 - Alpha)/Alpha;
                        double newN = (((double) SlowerEMA - (double) fasterEMA)*
                                       ((origN - 1.0)/((double) SlowerEMA - 1.0))) + (double) fasterEMA;
                        double newAlph = 2.0/(newN + 1.0);
                        double FRAMATemp;
                        if (i == period) FRAMATemp = FRAMA1 + newAlph*(Close[i] - FRAMA1);
                        else FRAMATemp = FRAMAHist.Last() + newAlph*(Close[i] - FRAMAHist.Last());

                        FRAMAHist.Add(FRAMATemp);
                    }
                }
                else
                {
                    for (int i = 0; i < Close.Count; i++)
                    {
                        FRAMAHist.Add(0);
                    }

                }

            }
        }

        public class SSTO
        {
            public List<double> SSTOK { get; set; }
            public List<double> SSTOD { get; set; }
            public string TimePeriod;


            //The Full Stochastic Oscillator is more advanced and more flexible than the Fast and Slow Stochastic
            //and can even be used to generate them.For example, a (14, 1, 3) Full Stochastic is equivalent to a(14, 3) Fast
            //Stochastic while a(12, 3, 2) Full Stochastic is identical to a(12, 2) Slow Stochastic.

            public SSTO(int fastk, int k, int d, int recent, int early, List<double> highs, List<double> lows,
                List<double> close)
            {
                SSTOK = new List<double>();
                SSTOD = new List<double>();
                TimePeriod = fastk.ToString() + " " + k.ToString() + " " + d.ToString();

                double[] Highs = highs.ToArray();
                double[] Lows = lows.ToArray();
                double[] Close = close.ToArray();

                int begidx;
                int NBElement;
                double[] slowk = new double[Math.Abs(early - recent) + 1];
                double[] slowd = new double[Math.Abs(early - recent) + 1];
                Core.RetCode a = TicTacTec.TA.Library.Core.Stoch(recent, early, Highs, Lows, Close, fastk, k,
                    Core.MAType.Ema, d,
                    Core.MAType.Ema, out begidx, out NBElement, slowk, slowd);

                SSTOK = CleanTicTac(slowk.ToList());
                SSTOD = CleanTicTac(slowd.ToList());
            }
        }

        public class RSI
        {
            public List<double> RSIHist { get; set; }

            public RSI(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                RSIHist = new List<double>();

                int begidx;
                int NBElement;
                double[] Real = new double[Math.Abs(early - recent)];
                TicTacTec.TA.Library.Core.Rsi(recent, early, inReal, optInTimePeriod, out begidx, out NBElement, Real);



                RSIHist = Real.ToList();
            }
        }

        public class Acos
        {
            public List<double> AcosHist { get; set; }

            public Acos(int recent, int early, double[] inReal)
            {
                AcosHist = new List<double>();

                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Acos(recent, early, inReal, out begidx, out NBElement, Returned);
                AcosHist = Returned.ToList();

            }
        }

        public class AD
        {
            public List<double> ADHist { get; set; }

            public AD(int recent, int early, double[] highs, double[] lows, double[] closes, double[] volumes)
            {
                ADHist = new List<double>();

                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Ad(recent, early, highs, lows, closes, volumes, out begidx, out NBElement,
                    Returned);
                ADHist = Returned.ToList();
            }
        }

        public class ADD
        {
            public List<double> ADDHist { get; set; }

            public ADD(int recent, int early, double[] inReal0, double[] inReal1)
            {
                ADDHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Add(recent, early, inReal0, inReal1, out begidx, out NBElement, Returned);
                ADDHist = Returned.ToList();
            }
        }

        public class AdOsc
        {
            public List<double> AdOscHist { get; set; }

            public AdOsc(int recent, int early, double[] Highs, double[] Lows, double[] Closes, double[] Volumes,
                int optInFastPeriod, int SlowPeroid)
            {
                AdOscHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.AdOsc(recent, early, Highs, Lows, Closes, Volumes, optInFastPeriod, SlowPeroid,
                    out begidx, out NBElement, Returned);
                AdOscHist = Returned.ToList();
            }
        }

        public class Adx
        {
            public List<double> AdxHist { get; set; }

            public Adx(int recent, int early, double[] HIghs, double[] Lows, double[] Closes, int optInTimePeriod)
            {
                AdxHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Adx(recent, early, HIghs, Lows, Closes, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                AdxHist = Returned.ToList();
            }
        }

        public class AdxR
        {
            public List<double> AdxRHist { get; set; }

            public AdxR(int recent, int early, double[] HIghs, double[] Lows, double[] Closes, int optInTimePeriod)
            {
                AdxRHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Adxr(recent, early, HIghs, Lows, Closes, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                AdxRHist = Returned.ToList();
            }
        }

        public class Apo
        {
            public List<double> ApoHist { get; set; }

            public Apo(int recent, int early, double[] inReal, int optInFastPeriod, int optInSlowPeriod,
                Core.MAType optInMATYPE)
            {
                ApoHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Apo(recent, early, inReal, optInFastPeriod, optInSlowPeriod, optInMATYPE,
                    out begidx, out NBElement, Returned);
                ApoHist = Returned.ToList();
            }
        }

        public class Aroon
        {
            public List<double> AroonUp { get; set; }
            public List<double> AroonDown { get; set; }

            public Aroon(int recent, int early, double[] highs, double[] lows, int optInTimePeriod)
            {
                AroonDown = new List<double>();
                AroonUp = new List<double>();
                int begidx;
                int NBElement;
                double[] aroonUp = new double[early - recent];
                double[] aroonDown = new double[early - recent];
                TicTacTec.TA.Library.Core.Aroon(recent, early, highs, lows, optInTimePeriod, out begidx, out NBElement,
                    aroonDown, aroonUp);
                AroonUp = aroonUp.ToList();
                AroonDown = aroonDown.ToList();
            }
        }

        public class AroonOsc
        {
            public List<double> AroonOscHist { get; set; }

            public AroonOsc(int recent, int early, double[] inHigh, double[] inLow, int Timeperiod)
            {
                AroonOscHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.AroonOsc(recent, early, inHigh, inLow, Timeperiod, out begidx, out NBElement,
                    Returned);
                AroonOscHist = Returned.ToList();
            }
        }

        public class Asin
        {
            public List<double> AsinHist { get; set; }

            public Asin(int recent, int early, double[] inReal)
            {
                AsinHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Asin(recent, early, inReal, out begidx, out NBElement, Returned);
                AsinHist = Returned.ToList();
            }
        }

        public class Atan
        {
            public List<double> AtanHIst { get; set; }

            public Atan(int recent, int early, double[] inReal)
            {
                AtanHIst = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Atan(recent, early, inReal, out begidx, out NBElement, Returned);
                AtanHIst = Returned.ToList();
            }
        }

        public class ATR
        {
            public List<double> ATRHist { get; set; }

            public ATR(int recent, int early, double[] Highs, double[] Lows, double[] inClose, int timeperiod)
            {
                ATRHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Atr(recent, early, Highs, Lows, inClose, timeperiod, out begidx, out NBElement,
                    Returned);
                ATRHist = Returned.ToList();
            }
        }

        public class AveragePrice
        {
            public List<double> AveragePriceHist { get; set; }

            public AveragePrice(int recent, int early, double[] Opens, double[] Highs, double[] Lows, double[] inClose)
            {
                AveragePriceHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.AvgPrice(recent, early, Opens, Highs, Lows, inClose, out begidx, out NBElement,
                    Returned);
                AveragePriceHist = Returned.ToList();
            }
        }

        public class BollingerBands
        {
            public List<double> RealUpperHist { get; set; }
            public List<double> RealMidHist { get; set; }
            public List<double> RealLowerHist { get; set; }

            public BollingerBands(int recent, int early, double[] inReal, int OptinTimePeriod, int optInNbDevUp,
                int optInNbDevDn, Core.MAType OptInMATYPE)
            {
                RealUpperHist = new List<double>();
                RealMidHist = new List<double>();
                RealLowerHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Upper = new double[early - recent];
                double[] Mid = new double[early - recent];
                double[] Lower = new double[early - recent];
                TicTacTec.TA.Library.Core.Bbands(recent, early, inReal, OptinTimePeriod, optInNbDevUp, optInNbDevDn,
                    OptInMATYPE, out begidx, out NBElement, Upper, Mid, Lower);
                RealUpperHist = Upper.ToList();
                RealMidHist = Mid.ToList();
                RealLowerHist = Lower.ToList();

            }
        }

        public class Beta
        {
            public List<double> BetaHist { get; set; }

            public Beta(int recent, int early, double[] inReal0, double[] inReal1, int optInTimePeriod)
            {
                BetaHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Beta(recent, early, inReal0, inReal1, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                BetaHist = Returned.ToList();
            }
        }

        public class Bop
        {
            public List<double> BopHist { get; set; }

            public Bop(int recent, int early, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose)
            {
                BopHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Bop(recent, early, inOpen, inHigh, inLow, inClose, out begidx, out NBElement,
                    Returned);
                BopHist = Returned.ToList();
            }
        }

        public class Cci
        {
            public List<double> CciHist { get; set; }

            public Cci(int recent, int early, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod)
            {
                CciHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Cci(recent, early, inHigh, inLow, inClose, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                CciHist = Returned.ToList();
            }
        }



        public class Ceil
        {
            public List<double> CeilHist { get; set; }

            public Ceil(int recent, int early, double[] inReal)
            {
                CeilHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Ceil(recent, early, inReal, out begidx, out NBElement, Returned);
                CeilHist = Returned.ToList();
            }
        }

        public class Cmo
        {
            public List<double> CmoHist { get; set; }

            public Cmo(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                CmoHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Cmo(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                CmoHist = Returned.ToList();
            }
        }

        public class Correl
        {
            public List<double> CorrelHist { get; set; }

            public Correl(int recent, int early, double[] inReal0, double[] inReal1, int optInTimePeriod)
            {
                CorrelHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Correl(recent, early, inReal0, inReal1, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                CorrelHist = Returned.ToList();
            }
        }

        public class Cos
        {
            public List<double> CosHist { get; set; }

            public Cos(int recent, int early, double[] inReal)
            {
                CosHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Cos(recent, early, inReal, out begidx, out NBElement, Returned);
                CosHist = Returned.ToList();
            }
        }

        public class Cosh
        {
            public List<double> CoshHist { get; set; }

            public Cosh(int recent, int early, double[] inReal)
            {
                CoshHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Cosh(recent, early, inReal, out begidx, out NBElement, Returned);
                CoshHist = Returned.ToList();
            }
        }

        public class Dema
        {
            public List<double> DemaHist { get; set; }

            public Dema(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                DemaHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Dema(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                DemaHist = Returned.ToList();
            }
        }

        public class Div
        {
            public List<double> DivHist { get; set; }

            public Div(int recent, int early, double[] inReal0, double[] inReal1)
            {
                DivHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Div(recent, early, inReal0, inReal1, out begidx, out NBElement, Returned);
                DivHist = Returned.ToList();
            }
        }

        public class Dx
        {
            public List<double> DxHist { get; set; }

            public Dx(int recent, int early, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod)
            {
                DxHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Dx(recent, early, inHigh, inLow, inClose, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                DxHist = Returned.ToList();
            }
        }

        public class Ema
        {
            public List<double> EmaHist { get; set; }
            public string Timeperiod;

            public Ema(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                Timeperiod = optInTimePeriod.ToString();
                EmaHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[Math.Abs(early - recent) + 1];
                TicTacTec.TA.Library.Core.Ema(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                EmaHist = CleanTicTac(Returned.ToList());

            }
        }

        public class Exp
        {
            public List<double> ExpHiist { get; set; }

            public Exp(int recent, int early, double[] inReal)
            {
                ExpHiist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Exp(recent, early, inReal, out begidx, out NBElement, Returned);
                ExpHiist = Returned.ToList();
            }
        }

        public class Floor
        {
            public List<double> FloorHist { get; set; }

            public Floor(int recent, int early, double[] inReal)
            {
                FloorHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Floor(recent, early, inReal, out begidx, out NBElement, Returned);
                FloorHist = Returned.ToList();
            }
        }

        public class HtDcPeriod
        {
            public List<double> HtDcPeriodHist { get; set; }

            public HtDcPeriod(int recent, int early, double[] inReal)
            {
                HtDcPeriodHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.HtDcPeriod(recent, early, inReal, out begidx, out NBElement, Returned);
                HtDcPeriodHist = Returned.ToList();
            }
        }

        public class HtDcPhase
        {
            public List<double> HtDcPhaseHist { get; set; }

            public HtDcPhase(int recent, int early, double[] inReal)
            {
                HtDcPhaseHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.HtDcPhase(recent, early, inReal, out begidx, out NBElement, Returned);
                HtDcPhaseHist = Returned.ToList();
            }
        }

        public class HtPhasor
        {
            public List<double> InPhaseHist { get; set; }
            public List<double> QuadratureHist { get; set; }

            public HtPhasor(int recent, int early, double[] inReal)
            {
                InPhaseHist = new List<double>();
                QuadratureHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                double[] Returned2 = new double[early - recent];
                TicTacTec.TA.Library.Core.HtPhasor(recent, early, inReal, out begidx, out NBElement, Returned, Returned2);
                InPhaseHist = Returned.ToList();
                QuadratureHist = Returned2.ToList();
            }
        }

        public class HtSine
        {
            public List<double> Sine { get; set; }
            public List<double> LeadSine { get; set; }

            public HtSine(int recent, int early, double[] inReal)
            {
                Sine = new List<double>();
                LeadSine = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                double[] Returned1 = new double[early - recent];
                TicTacTec.TA.Library.Core.HtSine(recent, early, inReal, out begidx, out NBElement, Returned, Returned1);
                Sine = Returned.ToList();
                LeadSine = Returned1.ToList();
            }
        }

        public class HtTrendline
        {
            public List<double> HtTrendlineHist { get; set; }

            public HtTrendline(int recent, int early, double[] inReal)
            {
                HtTrendlineHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.HtTrendline(recent, early, inReal, out begidx, out NBElement, Returned);
                HtTrendlineHist = CleanTicTac(Returned.ToList());
            }
        }

        public class HtTrendMode
        {
            public List<int> HtTrendModeHist { get; set; }

            public HtTrendMode(int recent, int early, double[] inReal)
            {
                HtTrendModeHist = new List<int>();
                int begidx;
                int NBElement;
                int[] Returned = new int[early - recent];
                TicTacTec.TA.Library.Core.HtTrendMode(recent, early, inReal, out begidx, out NBElement, Returned);
                HtTrendModeHist = Returned.ToList();
            }
        }

        public class Kama
        {
            public List<double> KamaHist { get; set; }

            public Kama(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                KamaHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Kama(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                KamaHist = CleanTicTac(Returned.ToList());
            }
        }

        public class LinearReg
        {
            public List<double> LinearRegHist { get; set; }

            public LinearReg(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                LinearRegHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[Math.Abs(early - recent) + 1];
                TicTacTec.TA.Library.Core.LinearReg(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                LinearRegHist = CleanTicTac(Returned.ToList());
            }
        }

        public class LinearRegAngle
        {
            public List<double> LinearRegAngleHist { get; set; }

            public LinearRegAngle(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                LinearRegAngleHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[Math.Abs(early - recent) + 1];
                TicTacTec.TA.Library.Core.LinearRegAngle(recent, early, inReal, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                LinearRegAngleHist = CleanTicTac(Returned.ToList());
            }
        }

        public class LinearRegIntercept
        {
            public List<double> LinearRegInterceptHist { get; set; }

            public LinearRegIntercept(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                LinearRegInterceptHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[Math.Abs(early - recent) + 1];
                TicTacTec.TA.Library.Core.LinearRegIntercept(recent, early, inReal, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                LinearRegInterceptHist = CleanTicTac(Returned.ToList());
            }
        }

        public class LinearRegSlope
        {
            public List<double> LinearRegSlopeHist { get; set; }

            public LinearRegSlope(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                LinearRegSlopeHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[Math.Abs(early - recent) + 1];
                TicTacTec.TA.Library.Core.LinearRegSlope(recent, early, inReal, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                LinearRegSlopeHist = CleanTicTac(Returned.ToList());
            }
        }

        public class Ln
        {
            public List<double> LnHist { get; set; }

            public Ln(int recent, int early, double[] inReal)
            {
                LnHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Ln(recent, early, inReal, out begidx, out NBElement, Returned);
                LnHist = Returned.ToList();
            }
        }

        public class Log10
        {
            public List<double> Log10Hist { get; set; }

            public Log10(int recent, int early, double[] inReal)
            {
                Log10Hist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Log10(recent, early, inReal, out begidx, out NBElement, Returned);
                Log10Hist = Returned.ToList();
            }
        }

        public class MacdExt
        {
            public List<double> MACD { get; set; }
            public List<double> MACDSignal { get; set; }
            public List<double> MACDHist { get; set; }

            public MacdExt(int recent, int early, double[] inReal, int optInFastPeriod, Core.MAType FastMAType,
                int optInSlowPeriod, Core.MAType SlowMAType, int optInSignalPeriod, Core.MAType SignalMAType)
            {
                MACD = new List<double>();
                MACDHist = new List<double>();
                MACDSignal = new List<double>();
                int begidx;
                int NBElement;
                double[] Macd = new double[Math.Abs(early - recent)];
                double[] MacdSignal = new double[Math.Abs(early - recent)];
                double[] MacdGist = new double[Math.Abs(early - recent)];
                TicTacTec.TA.Library.Core.MacdExt(recent, early, inReal, optInFastPeriod, FastMAType, optInSlowPeriod,
                    SlowMAType, optInSignalPeriod, SignalMAType, out begidx, out NBElement, Macd, MacdSignal, MacdGist);
                MACD = Macd.ToList();
                MACDSignal = MacdSignal.ToList();
                MACDHist = MacdGist.ToList();
            }
        }

        public class MacdFix
        {
            public List<double> MACD { get; set; }
            public List<double> MACDSignal { get; set; }
            public List<double> MACDHist { get; set; }

            public MacdFix(int recent, int early, double[] inReal, int optInSignalPeriod)
            {
                MACD = new List<double>();
                MACDSignal = new List<double>();
                MACDHist = new List<double>();
                int begidx;
                int NBElement;
                double[] macd = new double[Math.Abs(early - recent)];
                double[] macdsignal = new double[Math.Abs(early - recent)];
                double[] macdhist = new double[Math.Abs(early - recent)];
                TicTacTec.TA.Library.Core.MacdFix(recent, early, inReal, optInSignalPeriod, out begidx, out NBElement,
                    macd, macdsignal, macdhist);
                MACD = macd.ToList();
                MACDSignal = macdsignal.ToList();
                MACDHist = macdhist.ToList();
            }
        }

        public class Mama
        {
            public List<double> MAMA { get; set; }
            public List<double> FAMA { get; set; }

            public Mama(int recent, int early, double[] inReal, int optInFastLimit, int optInSlowLimit)
            {
                MAMA = new List<double>();
                FAMA = new List<double>();

                int begidx;
                int NBElement;
                double[] mama = new double[early - recent];
                double[] fama = new double[early - recent];
                TicTacTec.TA.Library.Core.Mama(recent, early, inReal, optInFastLimit, optInSlowLimit, out begidx,
                    out NBElement, mama, fama);
                MAMA = mama.ToList();
                FAMA = fama.ToList();
            }
        }

        public class Max
        {
            public List<double> MaxHist { get; set; }

            public Max(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                MaxHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Max(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                MaxHist = Returned.ToList();
            }
        }

        public class MaxIndex
        {
            public List<int> MaxIndexHist { get; set; }

            public MaxIndex(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                MaxIndexHist = new List<int>();
                int begidx;
                int NBElement;
                int[] Returned = new int[early - recent];
                TicTacTec.TA.Library.Core.MaxIndex(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                MaxIndexHist = Returned.ToList();
            }
        }

        public class MedPrice
        {
            public List<double> MedPriceHist { get; set; }

            public MedPrice(int recent, int early, double[] inHigh, double[] inLow)
            {
                MedPriceHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.MedPrice(recent, early, inHigh, inLow, out begidx, out NBElement, Returned);
                MedPriceHist = Returned.ToList();
            }
        }

        public class Mfi
        {
            public List<double> MfiHist { get; set; }

            public Mfi(int recent, int early, double[] inHigh, double[] inLow, double[] inClose, double[] inVolume,
                int optInTimePeriod)
            {
                MfiHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Mfi(recent, early, inHigh, inLow, inClose, inVolume, optInTimePeriod,
                    out begidx, out NBElement, Returned);
                MfiHist = Returned.ToList();
            }
        }

        public class MidPoint
        {
            public List<double> MidPointHist { get; set; }

            public MidPoint(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                MidPointHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.MidPoint(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                MidPointHist = Returned.ToList();
            }
        }

        public class MidPrice
        {
            public List<double> MidPriceHidt { get; set; }

            public MidPrice(int recent, int early, double[] inHigh, double[] inLow, int optInTimePeriod)
            {
                MidPriceHidt = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.MidPrice(recent, early, inHigh, inLow, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                MidPriceHidt = Returned.ToList();
            }
        }

        public class Min
        {
            public List<double> MinHist { get; set; }

            public Min(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                MinHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Min(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                MinHist = Returned.ToList();
            }
        }

        public class MinIndex
        {
            public List<int> MinIndexHist { get; set; }

            public MinIndex(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                MinIndexHist = new List<int>();
                int begidx;
                int NBElement;
                int[] Returned = new int[early - recent];
                TicTacTec.TA.Library.Core.MinIndex(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                MinIndexHist = Returned.ToList();
            }
        }

        public class MinMax
        {
            public List<double> MinHist { get; set; }
            public List<double> MaxHist { get; set; }

            public MinMax(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                MinHist = new List<double>();
                MaxHist = new List<double>();

                int begidx;
                int NBElement;
                double[] outMin = new double[early - recent];
                double[] outmax = new double[early - recent];

                TicTacTec.TA.Library.Core.MinMax(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    outMin, outmax);
                MinHist = outMin.ToList();
                MaxHist = outmax.ToList();
            }
        }

        public class MinusDI
        {
            public List<double> MinusDIHist { get; set; }

            public MinusDI(int recent, int early, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
                int optInTimePeriod)
            {
                MinusDIHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.MinusDI(recent, early, inHigh, inLow, inClose, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                MinusDIHist = Returned.ToList();
            }
        }

        public class MinusDM
        {
            public List<double> MinusDMHist { get; set; }

            public MinusDM(int recent, int early, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
                int optInTimePeriod)
            {
                MinusDMHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.MinusDM(recent, early, inHigh, inLow, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                MinusDMHist = Returned.ToList();
            }
        }

        public class Mom
        {
            public List<double> MomHist { get; set; }

            public Mom(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                MomHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Mom(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                MomHist = Returned.ToList();
            }
        }

        public class MovingAverage
        {
            public List<double> MovingAverageHist { get; set; }

            public MovingAverage(int recent, int early, double[] inReal, int optInTimePeriod, Core.MAType MAType)
            {
                MovingAverageHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.MovingAverage(recent, early, inReal, optInTimePeriod, MAType, out begidx,
                    out NBElement, Returned);
                MovingAverageHist = Returned.ToList();
            }
        }

        public class MovingAverageVariablePeriod
        {
            public List<double> MovingAverageVariablePeriodHist { get; set; }

            public MovingAverageVariablePeriod(int recent, int early, double[] inReal, double[] inPeriods,
                int optInMinPeriod, int optInMaxPeriod, Core.MAType MAType)
            {
                MovingAverageVariablePeriodHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.MovingAverageVariablePeriod(recent, early, inReal, inPeriods, optInMinPeriod,
                    optInMaxPeriod, MAType, out begidx, out NBElement, Returned);
                MovingAverageVariablePeriodHist = Returned.ToList();
            }
        }

        public class Mult
        {
            public List<double> MultHist { get; set; }

            public Mult(int recent, int early, double[] inReal0, double[] inReal1)
            {
                MultHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Mult(recent, early, inReal0, inReal1, out begidx, out NBElement, Returned);
                MultHist = Returned.ToList();
            }
        }

        public class Natr
        {
            public List<double> NatrHist { get; set; }

            public Natr(int recent, int early, int optInTimePeriod, double[] inHigh, double[] inLow, double[] inClose)
            {
                NatrHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Natr(recent, early, inHigh, inLow, inClose, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                NatrHist = Returned.ToList();
            }
        }

        public class Obv
        {
            public List<double> ObvHist { get; set; }

            public Obv(int recent, int early, double[] inReal, double[] inVolume)
            {
                ObvHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Obv(recent, early, inReal, inVolume, out begidx, out NBElement, Returned);
                ObvHist = Returned.ToList();
            }
        }

        public class PlusDI
        {
            public List<double> PlusDIHist { get; set; }

            public PlusDI(int recent, int early, int optInTimePeriod, double[] inHigh, double[] inLow, double[] inClose)
            {
                PlusDIHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.PlusDI(recent, early, inHigh, inLow, inClose, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                PlusDIHist = Returned.ToList();
            }
        }

        public class PlusDM
        {
            public List<double> PlusDMHist { get; set; }

            public PlusDM(int recent, int early, int optInTimePeriod, double[] inHigh, double[] inLow)
            {
                PlusDMHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.PlusDM(recent, early, inHigh, inLow, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                PlusDMHist = Returned.ToList();
            }
        }

        public class Ppo
        {
            public List<double> PpoHist { get; set; }

            public Ppo(int recent, int early, double[] inReal, int optInFastPeriod, int optInSlowPeriod,
                Core.MAType optInMAType)
            {
                PpoHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Ppo(recent, early, inReal, optInFastPeriod, optInSlowPeriod, optInMAType,
                    out begidx, out NBElement, Returned);
                PpoHist = Returned.ToList();
            }
        }

        public class Roc
        {
            public List<double> RocHist { get; set; }

            public Roc(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                RocHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Roc(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                RocHist = Returned.ToList();
            }
        }

        public class RocP
        {
            public List<double> RocPHist { get; set; }

            public RocP(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                RocPHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.RocP(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                RocPHist = Returned.ToList();
            }
        }

        public class RocR
        {
            public List<double> RocRHist { get; set; }

            public RocR(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                RocRHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.RocR(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                RocRHist = Returned.ToList();
            }
        }

        public class RocR100
        {
            public List<double> RocR100Hist { get; set; }

            public RocR100(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                RocR100Hist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.RocR100(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                RocR100Hist = Returned.ToList();
            }
        }

        public class Sar
        {
            public List<double> SarHist { get; set; }

            public Sar(int recent, int early, double optInAcceleration, double[] inHigh, double[] inLow,
                double optInMaximum)
            {
                SarHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Sar(recent, early, inHigh, inLow, optInAcceleration, optInMaximum, out begidx,
                    out NBElement, Returned);
                SarHist = Returned.ToList();
            }
        }

        public class SarExt
        {
            public List<double> SarExtHist { get; set; }

            public SarExt(int recent, int early, double[] inHigh, double[] inLow, double optInStartValue,
                double optInOffsetOnReverse, double optInAccelerationInitLong, double optInAccelerationLong,
                double optInAccelerationMaxLong, double optInAccelerationInitShort, double optInAccelerationShort,
                double optInAccelerationMaxShort)
            {
                SarExtHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.SarExt(recent, early, inHigh, inLow, optInStartValue, optInOffsetOnReverse,
                    optInAccelerationInitLong, optInAccelerationLong, optInAccelerationMaxLong,
                    optInAccelerationInitShort, optInAccelerationShort, optInAccelerationMaxShort, out begidx,
                    out NBElement, Returned);
                SarExtHist = Returned.ToList();
            }
        }

        public class Sin
        {
            public List<double> SinHist { get; set; }

            public Sin(int recent, int early, double[] inReal)
            {
                SinHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Sin(recent, early, inReal, out begidx, out NBElement, Returned);
                SinHist = Returned.ToList();
            }
        }

        public class Sinh
        {
            public List<double> SinhHist { get; set; }

            public Sinh(int recent, int early, double[] inReal)
            {
                SinhHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Sinh(recent, early, inReal, out begidx, out NBElement, Returned);
                SinhHist = Returned.ToList();
            }
        }

        public class Sma
        {
            public List<double> SmaHist { get; set; }

            public Sma(int recent, int early, double[] inReal, int optInTimePeriod, double[] inLow, double[] inClose)
            {
                SmaHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Sma(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                SmaHist = Returned.ToList();
            }
        }

        public class Sqrt
        {
            public List<double> SqrtHist { get; set; }

            public Sqrt(int recent, int early, double[] inReal)
            {
                SqrtHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Sqrt(recent, early, inReal, out begidx, out NBElement, Returned);
                SqrtHist = Returned.ToList();
            }
        }

        public class StdDev
        {
            public List<double> StdDevHist { get; set; }

            public StdDev(int recent, int early, double[] inReal, int optInTimePeriod, int optInNbDev, double[] inClose)
            {
                StdDevHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.StdDev(recent, early, inReal, optInTimePeriod, optInNbDev, out begidx,
                    out NBElement, Returned);
                StdDevHist = Returned.ToList();
            }
        }

        public class StochF
        {
            public List<double> FastKHist { get; set; }
            public List<double> FastDHist { get; set; }


            public StochF(int recent, int early, double[] inHigh, double[] inLow, double[] inClose,
                int optInFastK_Period, int optInFastD_Period, Core.MAType optInFastD_MAType)
            {
                FastKHist = new List<double>();
                FastDHist = new List<double>();

                int begidx;
                int NBElement;
                double[] FastK = new double[early - recent];
                double[] FastD = new double[early - recent];

                TicTacTec.TA.Library.Core.StochF(recent, early, inHigh, inLow, inClose, optInFastK_Period,
                    optInFastD_Period, optInFastD_MAType, out begidx, out NBElement, FastK, FastD);
                FastKHist = FastK.ToList();
                FastDHist = FastD.ToList();

            }
        }

        public class StochRsi
        {
            public List<double> FastKHist { get; set; }
            public List<double> FastDHist { get; set; }


            public StochRsi(int recent, int early, double[] inReal, int optInTimePeriod, int optInFastK_Period,
                int optInFastD_Period, Core.MAType optInFastD_MAType)
            {
                FastKHist = new List<double>();
                FastDHist = new List<double>();

                int begidx;
                int NBElement;
                double[] outFastK = new double[early - recent];
                double[] outFastD = new double[early - recent];

                TicTacTec.TA.Library.Core.StochRsi(recent, early, inReal, optInTimePeriod, optInFastK_Period,
                    optInFastD_Period, optInFastD_MAType, out begidx, out NBElement, outFastK, outFastD);
                FastKHist = outFastK.ToList();
                FastDHist = outFastD.ToList();

            }
        }

        public class Sub
        {
            public List<double> SubHist { get; set; }

            public Sub(int recent, int early, double[] inReal0, double[] inReal1)
            {
                SubHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Sub(recent, early, inReal0, inReal1, out begidx, out NBElement, Returned);
                SubHist = Returned.ToList();
            }
        }

        public class Sum
        {
            public List<double> SumHist { get; set; }

            public Sum(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                SumHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Sum(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                SumHist = Returned.ToList();
            }
        }

        public class T3
        {
            public List<double> T3Hist { get; set; }

            public T3(int recent, int early, double[] inReal, int optInTimePeriod, int optInVFactor, double[] inClose)
            {
                T3Hist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.T3(recent, early, inReal, optInTimePeriod, optInVFactor, out begidx,
                    out NBElement, Returned);
                T3Hist = Returned.ToList();
            }
        }

        public class Tan
        {
            public List<double> TanHist { get; set; }

            public Tan(int recent, int early, double[] inReal)
            {
                TanHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Tan(recent, early, inReal, out begidx, out NBElement, Returned);
                TanHist = Returned.ToList();
            }
        }

        public class Tanh
        {
            public List<double> TanhHist { get; set; }

            public Tanh(int recent, int early, double[] inReal)
            {
                TanhHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Tanh(recent, early, inReal, out begidx, out NBElement, Returned);
                TanhHist = Returned.ToList();
            }
        }

        public class Tema
        {
            public List<double> TemaHist { get; set; }

            public Tema(int recent, int early, double[] inReal, int optInTimePeriod, double[] inLow, double[] inClose)
            {
                TemaHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Tema(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                TemaHist = Returned.ToList();
            }
        }

        public class Trima
        {
            public List<double> TrimaHist { get; set; }

            public Trima(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                TrimaHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Trima(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                TrimaHist = Returned.ToList();
            }
        }

        public class Trix
        {
            public List<double> TrixHist { get; set; }

            public Trix(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                TrixHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Trix(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                TrixHist = Returned.ToList();
            }
        }

        public class TrueRange
        {
            public List<double> TrueRangeHist { get; set; }

            public TrueRange(int recent, int early, double[] inHigh, double[] inLow, double[] inClose)
            {
                TrueRangeHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.TrueRange(recent, early, inHigh, inLow, inClose, out begidx, out NBElement,
                    Returned);
                TrueRangeHist = Returned.ToList();
            }
        }

        public class Tsf
        {
            public List<double> TsfHist { get; set; }

            public Tsf(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                TsfHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Tsf(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                TsfHist = Returned.ToList();
            }
        }

        public class TypPrice
        {
            public List<double> TypPriceHist { get; set; }

            public TypPrice(int recent, int early, double[] inHigh, double[] inLow, double[] inClose)
            {
                TypPriceHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.TypPrice(recent, early, inHigh, inLow, inClose, out begidx, out NBElement,
                    Returned);
                TypPriceHist = Returned.ToList();
            }
        }

        public class UltOsc
        {
            public List<double> UltOscHist { get; set; }

            public UltOsc(int recent, int early, double[] inHigh, double[] inLow, double[] inClose, int optInTimePeriod1,
                int optInTimePeriod2, int optInTimePeriod3)
            {
                UltOscHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.UltOsc(recent, early, inHigh, inLow, inClose, optInTimePeriod1,
                    optInTimePeriod2, optInTimePeriod3, out begidx, out NBElement, Returned);
                UltOscHist = Returned.ToList();
            }
        }

        public class Variance
        {
            public List<double> VarianceHist { get; set; }

            public Variance(int recent, int early, double[] inReal, int optInTimePeriod, double optInNbDev)
            {
                VarianceHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Variance(recent, early, inReal, optInTimePeriod, optInNbDev, out begidx,
                    out NBElement, Returned);
                VarianceHist = Returned.ToList();
            }
        }

        public class WclPrice
        {
            public List<double> WclPriceHist { get; set; }

            public WclPrice(int recent, int early, double[] inHigh, double[] inLow, double[] inClose)
            {
                WclPriceHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.WclPrice(recent, early, inHigh, inLow, inClose, out begidx, out NBElement,
                    Returned);
                WclPriceHist = Returned.ToList();
            }
        }

        public class WillR
        {
            public List<double> WillRHist { get; set; }

            public WillR(int recent, int early, int optInTimePeriod, double[] inHigh, double[] inLow, double[] inClose)
            {
                WillRHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.WillR(recent, early, inHigh, inLow, inClose, optInTimePeriod, out begidx,
                    out NBElement, Returned);
                WillRHist = Returned.ToList();
            }
        }

        public class Wma
        {
            public List<double> WmaHist { get; set; }

            public Wma(int recent, int early, double[] inReal, int optInTimePeriod)
            {
                WmaHist = new List<double>();
                int begidx;
                int NBElement;
                double[] Returned = new double[early - recent];
                TicTacTec.TA.Library.Core.Wma(recent, early, inReal, optInTimePeriod, out begidx, out NBElement,
                    Returned);
                WmaHist = Returned.ToList();
            }
        }
    }
}

