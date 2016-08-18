using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows .Media .Media3D ;
using System.Windows.Media;
namespace KneeInnovation3D.EntityTools
{
   public  class MathRoutines
    {

       public static void LeastSquareBestFitSphere(Point3DCollection inputpoints, int N, double nstop, out Point3D centrePoint, out double radius)
        {
            double xsum = 0, ysum = 0, zsum = 0;
            double xsumsq = 0;
            double ysumsq = 0;
            double zsumsq = 0;
            double xsumcube = 0;
            double ysumcube = 0;
            double zsumcube = 0;


            foreach (var v in inputpoints)
            {
                xsum +=  v.X;
                ysum +=   v.Y;
                zsum +=  v.Z;
                xsumsq += v.X * v.X;
                ysumsq += v.Y * v.Y;
                zsumsq += v.Z * v.Z;
                xsumcube += v.X * v.X * v.X;
                ysumcube += v.Y * v.Y * v.Y;
                zsumcube += v.Z * v.Z * v.Z;
            }
         
        
            int npoints = inputpoints.Count;

            double xn = xsum / npoints;        //sum( X[n] )
            double xn2 = xsumsq / npoints;    //sum( X[n]^2 )
            double xn3 = xsumcube / npoints;    //sum( X[n]^3 )
            double yn = ysum / npoints;        //sum( Y[n] )
            double yn2 = ysumsq / npoints;    //sum( Y[n]^2 )
            double yn3 = ysumcube / npoints;    //sum( Y[n]^3 )
            double zn = zsum / npoints;        //sum( Z[n] )
            double zn2 = zsumsq / npoints;    //sum( Z[n]^2 )
            double zn3 = zsumcube / npoints;    //sum( Z[n]^3 )



            double xy = 0; // (Xsum * Ysum);        //sum( X[n] * Y[n] )
            double xz = 0; // (Xsum * Zsum) / npoints;        //sum( X[n] * Z[n] )
            double yz = 0; // (Ysum * Zsum) / npoints;        //sum( Y[n] * Z[n] )
            double x2Y = 0; // (Xsumsq * Ysum) / npoints;    //sum( X[n]^2 * Y[n] )
            double x2Z = 0; // (Xsumsq * Zsum) / npoints;    //sum( X[n]^2 * Z[n] )
            double y2X = 0; // (Ysumsq * Xsum) / npoints;    //sum( Y[n]^2 * X[n] )
            double y2Z = 0; // (Ysumsq * Zsum) / npoints;    //sum( Y[n]^2 * Z[n] )
            double z2X = 0; // (Zsumsq * Xsum) / npoints;    //sum( Z[n]^2 * X[n] )
            double z2Y = 0; // (Zsumsq * Ysum) / npoints;    //sum( Z[n]^2 * Y[n] )


            foreach (var v in inputpoints)
            {
                xy += (v.X * v.Y);
                xz += (v.X * v.Z);
                yz += (v.Y * v.Z);
                x2Y += (v.X * v.X * v.Y );
                x2Z += (v.X * v.X * v.Z);
                y2X += (v.Y * v.Y * v.X);
                y2Z += (v.Y * v.Y * v.Z);
                z2X += (v.Z * v.Z * v.X);
                z2Y += (v.Z * v.Z * v.Y);
            }

            xy = xy / npoints;
            xz = xz / npoints;
            yz = yz / npoints;
            x2Y = x2Y / npoints;
            x2Z = x2Z / npoints;
            y2X = y2X / npoints;
            y2Z = y2Z / npoints;
            z2X = z2X / npoints;
            z2Y = z2Y / npoints;

            //Reduction of multiplications
            double f0 = xn2 + yn2 + zn2;
            double f1 = 0.5 * f0;
            double f2 = -8.0 * (xn3 + y2X + z2X);
            double f3 = -8.0 * (x2Y + yn3 + z2Y);
            double f4 = -8.0 * (x2Z + y2Z + zn3);

            //Set initial conditions:

            double a = xn;
            double b = yn;
            double c = zn;

            //First iteration computation:
            double a2 = a * a;
            double b2 = b * b;
            double c2 = c * c;
            double qs = a2 + b2 + c2;
            double qb = -2 * (a * xn + b * yn + c * zn);

            //Set initial conditions:
            double rsq = f0 + qb + qs;

            //First iteration computation:
            double q0 = 0.5 * (qs - rsq);
            double q1 = f1 + q0;
            double q2 = 8 * (qs - rsq + qb + f0);
            double aA, aB, aC, nA, nB, nC, dA, dB, dC;

            //Iterate N times, ignore stop condition.
            int n = 0;
            while (n != N)
            {
                n++;

                //Compute denominator:
                aA = q2 + 16 * (a2 - 2 * a * xn + xn2);
                aB = q2 + 16 * (b2 - 2 * b * yn + yn2);
                aC = q2 + 16 * (c2 - 2 * c * zn + zn2);
                aA = (aA == 0) ? 1.0 : aA;
                aB = (aB == 0) ? 1.0 : aB;
                aC = (aC == 0) ? 1.0 : aC;

                //Compute next iteration
                nA = a - ((f2 + 16 * (b * xy + c * xz + xn * (-a2 - q0) + a * (xn2 + q1 - c * zn - b * yn))) / aA);
                nB = b - ((f3 + 16 * (a * xy + c * yz + yn * (-b2 - q0) + b * (yn2 + q1 - a * xn - c * zn))) / aB);
                nC = c - ((f4 + 16 * (a * xz + b * yz + zn * (-c2 - q0) + c * (zn2 + q1 - a * xn - b * yn))) / aC);

                //Check for stop condition
                dA = (nA - a);
                dB = (nB - b);
                dC = (nC - c);
             if ((dA * dA + dB * dB + dC * dC) <= nstop) { break; }

                //Compute next iteration's values
                a = nA;
                b = nB;
                c = nC;
                a2 = a * a;
                b2 = b * b;
                c2 = c * c;
                qs = a2 + b2 + c2;
                qb = -2 * (a * xn + b * yn + c * zn);
                rsq = f0 + qb + qs;
                q0 = 0.5 * (qs - rsq);
                q1 = f1 + q0;
                q2 = 8 * (qs - rsq + qb + f0);
            }
            centrePoint = new Point3D(a, b, c);
            radius = Math.Sqrt ( rsq);
         
        }

       public static double DegreesToRad(double degrees)
        {
            double rad = degrees * 0.0174532925;
            return rad;
        }

       private double RadianToDegree(double angle)
       {
           return angle * (180.0 / Math.PI);
       }

       public static void HoughTransform(Point3DCollection inputpoints, double searchrad)
        {
         //assumes a 0.4 mm tolerance
            //0 = rad - 0.1
            //2 = rad + 0.1

            //List<Accumulator> acc = new List<Accumulator>();

            double rad = searchrad ;

            Dictionary<int, Point3D> acc = new Dictionary<int, Point3D >();
            int count = 0;

            Point3DCollection whatthehel = new Point3DCollection();
            
            for(int i = 0; i< inputpoints .Count ; ++i)
            {           
                 for (int a = 0; a< 360; ++a)
                 {
                     for (int b = 0; b< 180; ++b) // 180 TO 270 TOP HEM
                     {

                         double cx = inputpoints[i].X - rad * (Math.Cos(DegreesToRad(a)) * Math.Sin(DegreesToRad(b)));
                         double cy = inputpoints[i].Y - rad * (Math.Sin(DegreesToRad(a)) * Math.Sin(DegreesToRad(b)));
                         double cz = inputpoints[i].Z - rad * (Math.Cos(DegreesToRad(b)));

                         whatthehel.Add(new Point3D(cx, cy, cz));

                         acc.Add(count, new Point3D(cx, cy,cz));
                         count++;
                          //acc.Add (new  Accumulator (new Point3D (Math.Round (cx, 0), cy, cz), rad ));

                          b +=9;
                     }

                      a +=9;
                 }

                 i += 20;
             
            }


            System.Collections.Generic.IEnumerable<System.Linq.IGrouping<Point3D, System.Collections.Generic.KeyValuePair<int, Point3D>>> linqlist = from t in acc group t by t.Value;

            int max = 0;
            Point3D p;

            Point3DCollection textl = new Point3DCollection();

            foreach (var coincident in linqlist)
            {
                if (coincident.Count() > 35)
                {
                    max = coincident.Count();
                    textl.Add(coincident.Key);
                }
            }


            EntityTools.PointCollections.WriteDelcamPicFile(new List<Point3DCollection>() { whatthehel }, @"C:\temp\cps");
            foreach(var v in linqlist )
            {
                max = 1;
               
            }


        //for for every point (x, y, z) do 
        //    for (r = rmin;r ≤ rmax;r + +) do 
        //        for θ = 0;θ ≤ 2π;θ + + do 
        //            for ϕ = 0;ϕ ≤ π;ϕ + + do 
        //                cx = x−rcosθsinϕ 
        //                    cy = y−rsinθsinϕ 
        //                        cz = z−rcosϕ 
        //                            Accumulator[r][cx][cy][cz] + + end 
        //    end 
        //        end 
        //    end 
        //        Search Accumulator for peak
            Point3DCollection pntc = new Point3DCollection();
            foreach (var v in acc)
            {
              // pntc.Add(v.Point);
            }
            EntityTools.PointCollections.WriteDelcamPicFile(new List<Point3DCollection>() { pntc }, @"C:\temp\ppout.pic");
        }

       public static void FindSpheresUsingClusters(Point3DCollection inputpoints, double searchrad, out List <Point3DCollection > clusters, double zlimit, bool filter)
       {
           try
           {
               List<Point3D> pt = inputpoints.ToList();
               pt.Sort((x, y) => y.Z.CompareTo(x.Z));
              
               List<Point3D> selectedPoints;

               if (filter == true)
               {
                   selectedPoints = pt.TakeWhile(p => p.Z >= zlimit).ToList();
               }
               else
               {

                   selectedPoints = inputpoints.ToList();
               }

               List<BlPoint> lListPoints = new List<BlPoint>();
               foreach (var v in selectedPoints) lListPoints.Add(new BlPoint((float)v.X, (float)v.Y, (float)v.Z));
               double lDist = searchrad;

               List<Cluster> lListClusters = new List<Cluster>();
               // take a point to create one cluster for starters
               BlPoint lP1 = lListPoints[0];

               lListPoints.Remove(lP1);

               // so far, we have a one-point cluster
               lListClusters.Add(new Cluster(lDist, lP1));

               List<Cluster> lListAttainableClusters;
               Cluster lC;
               foreach (BlPoint p in lListPoints)
               {
                   lListAttainableClusters = new List<Cluster>();
                   lListAttainableClusters = lListClusters.FindAll(x => x.IsPointReachable(p));
                   lListClusters.RemoveAll(x => x.IsPointReachable(p));
                   lC = new Cluster(lDist, p);
                   // merge point's "reachable" clusters
                   if (lListAttainableClusters.Count > 0)
                       lC.AnnexCluster(lListAttainableClusters.Aggregate((c, x) =>
                         c = Cluster.MergeClusters(x, c)));
                   lListClusters.Add(lC);
                   lListAttainableClusters = null;
                   lC = null;
               }  // of loop over candidate points


               clusters = new List<Point3DCollection>();

               for (int i = 0; i < lListClusters.Count; i++)
               {
                   Point3DCollection pl = new Point3DCollection();
                   foreach (var v in lListClusters[i].ListPoints)
                   {
                       pl.Add(new Point3D(v.X, v.Y, v.Z));
                   }
                   clusters.Add(pl);
                   // EntityTools .PointCollections .WriteDelcamPICFile (new List<Point3DCollection> () {pl}, @"C:\temp\a" + i );
               }
           }
           catch
           {
               clusters = null;
           }

         //KneeInnovation3D .EntityTools.PointCollections.WriteDelcamPicFile(new List<Point3DCollection>() { clusters[0] }, @"C:\temp\a.pic");
         //KneeInnovation3D.EntityTools.PointCollections.WriteDelcamPicFile(new List<Point3DCollection>() { clusters[1] }, @"C:\temp\b.pic");
         //KneeInnovation3D.EntityTools.PointCollections.WriteDelcamPicFile(new List<Point3DCollection>() { clusters[2] }, @"C:\temp\c.pic");

       }

       public static void Matrix3ToEulerAnglesYxz(Matrix3D matrix, out double rfYAngle, out double rfPAngle, out double rfRAngle, out Boolean isUnique)
       {
           rfPAngle =  Math.Asin(-matrix.M23 );

           if (rfPAngle < Math.PI * 0.5)
           {
               if (rfPAngle > -Math.PI * 0.5)
               {
                   rfYAngle = Math.Atan2 (matrix.M13 , matrix.M33 );
                   rfRAngle = Math.Atan2 (matrix.M21, matrix.M22);
                   isUnique = true;
                   return;
               }
               else
               {
                   double fRmY = Math.Atan2(-matrix.M12, matrix.M11);
                   rfRAngle =  (0f); 
                   rfYAngle = rfRAngle - fRmY;
                   isUnique = false;
                   return;
               }
           }
           else
           {

               double  fRpY = Math.Atan2(-matrix.M12, matrix.M11);
               rfRAngle = (0f);  
               rfYAngle = fRpY - rfRAngle;
               isUnique = false;
               return;
           }

       }

       public static Transform3D CreatedTransformFromEuler(double x, double y , double z, double yaw, double pitch, double roll)
       {
           Transform3D trans = Transform3D.Identity;

            double c1 = Math.Cos(EntityTools.MathRoutines.DegreesToRad(pitch));
            double s1 = Math.Sin(EntityTools.MathRoutines.DegreesToRad(pitch));
            double c2 = Math.Cos(EntityTools.MathRoutines.DegreesToRad(yaw));
            double s2 = Math.Sin(EntityTools.MathRoutines.DegreesToRad(yaw));
            double c3 = Math.Cos(EntityTools.MathRoutines.DegreesToRad(roll));
            double s3 = Math.Sin(EntityTools.MathRoutines.DegreesToRad(roll));

            Matrix3D mat = new Matrix3D();

            mat.M11 = c2 * c3 - s1 * s2 * s3;
            mat.M21 = -c1 * s3;
            mat.M31 = s2 * c3 + s1 * c2 * s3;

            mat.M12 = c2 * s3 + s1 * s2 * c3;
            mat.M22= c1*c3;
            mat.M32 = s2 * s3 - s1 * c2 * c3;

            mat.M13 = -c1 * s2;
            mat.M23 = s1;
            mat.M33 = c1 * c2;

            mat.OffsetX = x;
            mat.OffsetY = y;
            mat.OffsetZ = z;

            MatrixTransform3D matrans = new MatrixTransform3D(mat);

            Transform3DGroup tranGroup = new Transform3DGroup();
            tranGroup.Children.Add(matrans );

            return (Transform3D)tranGroup; 

         }

       /// <summary>
       /// Generic routint for generating an alignment transform from 3 sphers - JKH 4/5/2015
       /// </summary>
       /// <param name="points"></param>
       /// <returns></returns>
       public static Transform3D GetTransformfrom3Points(Point3DCollection points)
       {


           if (points.Count != 3) return Transform3D.Identity;

           double L1 = (points[0] - points[1]).Length;
           double L2 = (points[1] - points[2]).Length;
           double L3 = (points[0] - points[2]).Length;

           List<double> lengthlist = new List<double>() { L1,L2,L3};
           List<double> lengthlist2 = new List<double>();
           foreach (double d in lengthlist.OrderBy(d => d))
           {
               lengthlist2.Add(d);
           }

           lengthlist2.Reverse();

           Point3DCollection newPL = new Point3DCollection();


           int count = 0;

           for(int i= 0; i < lengthlist.Count ; i++)
           {
               if (lengthlist2[count ] == lengthlist[i])
               {
                   newPL.Add((points[i]));
                   i = -1;
                   count ++;

                   if (count == 3) break;
               }
 
           }



           if (newPL.Count != 3) return Transform3D.Identity;
           Vector3D V1 = newPL[0] - newPL[1];
           Point3D Mid = ViewPortTools.MathUtils.GetMidPoint(newPL[0], newPL[1]);
           V1.Normalize();
           Vector3D V2 = newPL[0] - newPL[2];
           Point3D Origin = ViewPortTools.MathUtils.GetMidPoint(Mid, newPL[2]);
           Vector3D V3 = Vector3D.CrossProduct(V1, V2);
           V3.Normalize();
           V2 = Vector3D.CrossProduct(V1, V3);

           return ViewPortTools.MathUtils.TransformFromCordSys(Origin, V3, V2, V1);
       }


       public static Transform3D GetTransformfrom3PointsNoReorder(Point3DCollection points)
       {
           if (points.Count != 3) return Transform3D.Identity;
           Vector3D V1 = points[0] - points[1];
           Point3D Mid = ViewPortTools.MathUtils.GetMidPoint(points[0], points[1]);
           V1.Normalize();
           Vector3D V2 = Mid - points[2];
           Point3D Origin = ViewPortTools.MathUtils.GetMidPoint(Mid, points[2]);
           Vector3D V3 = Vector3D.CrossProduct(V1, V2);
           V3.Normalize();
           V2 = Vector3D.CrossProduct(V1, V3);
    
           return ViewPortTools.MathUtils.TransformFromCordSys(Origin, V3, V2, V1);

       }


       public static Transform3D GetTransformfrom3PointsNoReorderV2(Point3DCollection points)
       {
           if (points.Count != 3) return Transform3D.Identity;
           Vector3D V1 = points[0] - points[1];
           Point3D Mid = ViewPortTools.MathUtils.GetMidPoint(points[0], points[1]);
           V1.Normalize();
           Vector3D V2 = Mid - points[2];
           Point3D Origin = ViewPortTools.MathUtils.GetMidPoint(Mid, points[2]);
           Vector3D V3 = Vector3D.CrossProduct(V1, V2);
           V3.Normalize();
           V2 = Vector3D.CrossProduct(V1, V3);
           V1 = Vector3D.CrossProduct(V2, V3);

           return ViewPortTools.MathUtils.TransformFromCordSys(Origin, V3, V2, V1);

       }


       public static Transform3D GetTransformTo3PointsNoReorder(Point3DCollection points)
       {
           if (points.Count != 3) return Transform3D.Identity;
           Vector3D V1 = points[0] - points[1];
           Point3D Mid = ViewPortTools.MathUtils.GetMidPoint(points[0], points[1]);
           V1.Normalize();
           Vector3D V2 = Mid - points[2];
           Point3D Origin = ViewPortTools.MathUtils.GetMidPoint(Mid, points[2]);
           Vector3D V3 = Vector3D.CrossProduct(V1, V2);
           V3.Normalize();
           V2 = Vector3D.CrossProduct(V1, V3);

           return ViewPortTools.MathUtils.TransformtoCordSys(Origin, V1, V2, V3);

       }

       public static Transform3D GetTransformTo3PointsNoReorderV2(Point3DCollection points)
       {
           if (points.Count != 3) return Transform3D.Identity;
           Vector3D V1 = points[0] - points[1];
           Point3D Mid = ViewPortTools.MathUtils.GetMidPoint(points[0], points[1]);
           V1.Normalize();
           Vector3D V2 = Mid - points[2];
           Point3D Origin = ViewPortTools.MathUtils.GetMidPoint(Mid, points[2]);
           Vector3D V3 = Vector3D.CrossProduct(V1, V2);
           V3.Normalize();
           V2 = Vector3D.CrossProduct(V1, V3);
           V1 = Vector3D.CrossProduct(V2, V3);

           return ViewPortTools.MathUtils.TransformtoCordSys(Origin, V1, V2, V3);

       }
    }

   public partial class BlPoint : IComparer<BlPoint>
   {
       public float X { get; set; }
       public float Y { get; set; }
       public float Z { get; set; }


       public BlPoint()
       {

       }  // of parameterless constructor

       public BlPoint(float pX, float pY, float pZ)
           : this()
       {
           X = pX;
           Y = pY;
           Z = pZ;
       }  // of overloaded constructor

       public double Distance(BlPoint pP)
       {
           double lD = 0.0;

           lD = Math.Sqrt(Math.Pow((pP.X - X), 2) + Math.Pow((pP.Y - Y), 2));
           //double d = (Math.Pow((p_P.x - x), 2) + Math.Pow((p_P.y - y), 2));
 
           //l_d = InvSqrt(d);
           //l_d = 1 / l_d;
    
           return lD;

       }  // of Distance()

       public static double Distance(BlPoint pP1, BlPoint pP2)
       {
           double lD = 0.0;

           lD = Math.Sqrt(Math.Pow((pP1.X - pP2.X), 2) + Math.Pow((pP1.Y - pP2.Y), 2));


           return lD;

       }  // of Distance()

       public static  double InvSqrt(double x)
       {
           double xhalf = 0.5f * x;
           int i = BitConverter.ToInt32(BitConverter.GetBytes(x), 0);
           i = 0x5f3759df - (i >> 1);
           x = BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
           x = x * (1.5f - xhalf * x * x);
           return x;
       }

       public int Compare(BlPoint pP1, BlPoint pP2)
       {
           int lV;

           double lD = BlPoint.Distance(pP1, pP2);

           if (lD == 0)
               lV = 0;
           else if (lD > 0)
               lV = 1;
           else
               lV = -1;

           return lV;

       } 

   }  

   public partial class Cluster
    {
        public List<BlPoint > ListPoints { get; protected set; }
        public double Dist { get; protected set; }

        protected Cluster()
        {
            ListPoints = new List<BlPoint>();
        
        }  // of parameterless constructor

        protected Cluster( double pDist)
            :this()
        {
            Dist = pDist;
        
        }  // of overloaded constructor

        public Cluster(double pDist, BlPoint pPoint)
            : this(pDist)
        {
            ListPoints.Add(pPoint);
        }  // of overloaded constructor

        public bool IsPointReachable(BlPoint pPoint)
        {
            if (ListPoints.FindAll(x => x.Distance(pPoint) <= Dist).Count > 0)
                return true;
            else
                return false;

        }  // of IsPointReachable()

        public bool AnnexCluster(Cluster pCluster)
        {
            bool lBSuccess = true;

            ListPoints.AddRange(pCluster.ListPoints);

            return lBSuccess;

        }  // of AnnexCluster()

        public static Cluster MergeClusters(Cluster pC1, Cluster pC2)
        {
            if (pC2.ListPoints.Count > 0)
                pC1.AnnexCluster(pC2);

            return pC1;

        }  // of MergeClusters()

    }

   public partial class Accumulator
   {
       private double  _rad;

       public double  Rad
       {
           get { return _rad; }
           set { _rad = value; }
       }

       private Point3D  _point;

       public Point3D Point
       {
           get { return _point; }
           set { _point = value; }
       }

       public Accumulator(Point3D p, double r)
       {
           _rad = r;
           _point = p;
       }

   }
}