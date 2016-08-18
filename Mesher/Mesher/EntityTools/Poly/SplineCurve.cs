using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Media.Media3D ;

namespace KneeInnovation3D.EntityTools
{
    public class SplineCurve
    {
        private string _mCurveName; //            'Name of composite curve stored
        private int _mCompId = -1;
        private bool _mUsingWorld;
        public int MNumPoints; //                'Number of points in curve
        public Point3D[] MPoints; //                 'Array of key points on curve
        public Vector3D[] MDirBefore; //              'Array of DIRB on curve
        public double[] MMagBefore; //                  'Array of MAGB on curve
        public bool[] MDirBeforeFree; //              'Array indicating wether DIRB is tangent free
        public bool[] MMagBeforeFree; //              'Array indicating wether MAGB is tangent free
        public Vector3D[] MDirAfter; //                'Array of DIRA on curve
        public double[] MMagAfter; //                   'Array of MAGA on curve
        public bool[] MDirAfterFree; //              'Array indicating wether DIRA is tangent free
        public bool[] MMagAfterFree; //               'Array indicating wether MAGA is tangent free

        public bool IsClosed;
        private double _eps = 0.00001;

        public SplineCurve(Point3DCollection bestFitPoints)
        {
            _mCurveName = "";
            _mUsingWorld = true;
            MNumPoints = bestFitPoints.Count -1;

            MPoints = new Point3D[MNumPoints];
            MDirBefore = new Vector3D[MNumPoints];
            MMagBefore = new double[MNumPoints];
            MDirBeforeFree = new bool[MNumPoints];
            MMagBeforeFree = new bool[MNumPoints];
            MDirAfter = new Vector3D[MNumPoints];
            MMagAfter = new double[MNumPoints ];
            MDirAfterFree = new bool[MNumPoints ];
            MMagAfterFree = new bool[MNumPoints ];

            for (int i = 0; i < bestFitPoints.Count -1; i++)
            {
                MPoints[i] = bestFitPoints[i];
            }

            IsClosed = false;
            FreeCurveDirections();
            FreeCurveMagnitudes();
        }

        public void FreeCurveMagnitudes()
        {
            double a = 0;
            double b = 0;
            double f = 0;
            double m = 0;
            double chord = 0;
            int lastP;
            Vector3D ca;
            Vector3D cb;
         

            if (IsClosed) lastP = MNumPoints - 2;
            else lastP = MNumPoints - 1;

            for (int p = 0; p < lastP; p++)
            {
                if (IsClosed == true || p < lastP)
                {
                    int incrementP = PointIncrement(p, lastP, 1);

                    chord = (MPoints[incrementP] - MPoints[p]).Length;
                    ca = (MPoints[incrementP] - MPoints[p]);
                    ca.Normalize();
                    double aFac = Vector3D.DotProduct(MDirAfter[p], ca);

                    if (aFac < -1) aFac = -1;
                    if (aFac > 1) aFac = 1;
                    a = Math.Acos(aFac);

                    double bFac = Vector3D.DotProduct(ca, MDirBefore[incrementP]);

                    if (bFac < -1) bFac = -1;
                    if (bFac > 1) bFac = 1;
                    b = Math.Acos(bFac);
                    f = 2 - Math.Cos(a - b);

                    m = 2 * chord / (1 + f * Math.Cos(b));
                    m = Math.Abs(m);
                    if (m > chord * 2) m = chord * 2;
                    MMagAfter[p] = m;
                }

                if (IsClosed == true || p > 0)
                {
                    int incrementP = PointIncrement(p, lastP, -1);


                    chord = (MPoints[p] - MPoints[incrementP]).Length;
                    cb = (MPoints[p] - MPoints[incrementP]);
                    cb.Normalize();

                    double aFac = Vector3D.DotProduct(MDirBefore[p], cb);
                    if (aFac < -1) aFac = -1;
                    if (aFac > 1) aFac = 1;
                    a = Math.Acos(aFac);

                    double bFac = Vector3D.DotProduct(cb, MDirAfter[incrementP]);
                    if (bFac < -1) bFac = -1;
                    if (bFac > 1) bFac = 1;
                    b = Math.Acos(bFac);

                    f = 2 - Math.Cos(a - b);
                    m = 2 * chord / (1 + f * Math.Cos(b));
                    m = Math.Abs(m);
                    if (m > chord * 2) m = chord * 2;
                    MMagBefore[p] = m;
                }

                MMagAfterFree[p] = true;
                MMagBeforeFree[p] = true;
            }
        }

        private int PointIncrement(int p, int lastP, int increment)
        {
            int tmp;
            tmp = p + increment;
            if (tmp < 0)
                tmp += 1 + lastP;

            if (tmp > lastP)
                tmp = tmp - lastP - 1;

            return tmp;
        }

        public void FreeCurveDirections()
        {
            Vector3D d;
            Vector3D db;
            Vector3D da;
            Vector3D cb;
            Vector3D c2B;
            Vector3D ca;
            Vector3D c2A;
            double wb = 0;
            double wa = 0;
            double wc = 0;
            int lastP = 0;

            if (MNumPoints == 2)
            {
                Vector3D vec = (MPoints[1] - MPoints[0]);
                vec.Normalize();
                MDirAfter[0] = vec;
                MDirBefore[0] = vec;
                MDirAfterFree[0] = false;
                MDirBeforeFree[0] = false;
                MMagAfter[0] = 1;
                MMagBefore[0] = 1;
                MMagAfterFree[0] = false;
                MMagBeforeFree[0] = false;
                MDirAfter[1] = vec;
                MDirBefore[1] = vec;
                MDirBeforeFree[1] = false;
                MDirBeforeFree[1] = false;
                MMagAfter[1] = 1;
                MMagBefore[1] = 1;
                MMagAfterFree[1] = false;
                MMagBeforeFree[1] = false;
                return;
            }

            if (IsClosed == true)
                lastP = MNumPoints - 1;
            else
                lastP = MNumPoints - 2;

            for (int p = 0; p <= lastP; p++)
            {
                if (IsClosed == false && p == 0)
                {
                    ca = (MPoints[p + 1] - MPoints[p]);
                    ca.Normalize();
                    c2A = (MPoints[p + 2] - MPoints[p + 1]);
                    c2A.Normalize();
                    da = (ca + c2A);
                    d = ((ca * (Vector3D.DotProduct(da, ca) * 2)) - da);
                }
                else if (IsClosed == false && p == lastP)
                {
                    cb = (MPoints[p] - MPoints[p - 1]);
                    cb.Normalize();
                    c2B = (MPoints[p - 1] - MPoints[p - 2]);
                    c2B.Normalize();
                    db = (c2B + cb);
                    double tt = (Vector3D.DotProduct(db, cb) * 2);
                    d = ((cb * tt) - db);
                }
                else
                {
                    cb = (MPoints[p] - MPoints[PointIncrement(p, lastP, -1)]);
                    cb.Normalize();
                    ca = (MPoints[PointIncrement(p, lastP, +1)] - MPoints[p]);
                    ca.Normalize();
                    d = (cb + ca);
                    d.Normalize();

                    if (IsClosed == false && p == 1 && MNumPoints == 3)
                    {
                        c2B = (MPoints[PointIncrement(p, lastP, -1)] - MPoints[PointIncrement(p, lastP, -2)]);
                        c2B.Normalize();
                        c2A = (MPoints[PointIncrement(p, lastP, +2)] - MPoints[PointIncrement(p, lastP, +1)]);
                        c2A.Normalize();

                        db = (c2B + cb);
                        db.Normalize();

                        da = (ca + c2A);
                        da.Normalize();

                        wb = (db - cb).Length;
                        wa = (da - ca).Length;
                    }
                    else if (IsClosed == false && p == lastP - 1)
                    {
                        wa = (d - ca).Length;
                        c2B = (MPoints[PointIncrement(p, lastP, -1)] - MPoints[PointIncrement(p, lastP, -2)]);
                        c2B.Normalize();
                        db = (c2B + cb);
                        db.Normalize();
                        wb = (db - cb).Length;
                    }
                    else if (IsClosed == false && p == 1)
                    {
                        wb = (d - cb).Length;
                        c2A = (MPoints[p + 2] - MPoints[p + 1]);
                        c2A.Normalize();
                        da = (ca + c2A);
                        da.Normalize();
                        wa = (da - ca).Length;
                    }
                    else
                    {
                        c2B = (MPoints[PointIncrement(p, lastP, -1)] - MPoints[PointIncrement(p, lastP, -2)]);
                        c2B.Normalize();
                        c2A = (MPoints[PointIncrement(p, lastP, +2)] - MPoints[PointIncrement(p, lastP, +1)]);
                        c2A.Normalize();

                        db = (c2B + cb);
                        db.Normalize();

                        da = (ca + c2A);
                        da.Normalize();

                        wb = (db - cb).Length;
                        wa = (da - ca).Length;
                    }

                    if (wb < _eps && wa < 0.5)
                    {
                        wc = System.Math.Exp(-200 * (wa + wb));
                        d = ((cb * (wa + wc)) - (ca * (wb + wc)));
                    }
                    else
                    {
                        d = ((cb * wa) + (ca * wb));
                    }
                }

                if (d.X + d.Y + d.Z != 1)
                {
                    try
                    {
                        d = MPoints[p + 1] - MPoints[p];
                        d.Normalize();
                    }
                    catch { }
                }

                MDirAfterFree[p] = true;
                MDirBeforeFree[p] = true;
                MDirBefore[p] = d;
                MDirAfter[p] = d;
            }
            if (IsClosed == true)
            {
                MDirBefore[MNumPoints - 1] = MDirBefore[0];
                MDirAfter[MNumPoints - 1] = MDirAfter[0];
            }
        }


      public Point3D GetParameterCoordinate(double param)
      {
          if (param > MNumPoints) param = MNumPoints;

          if (param < 1 | param > MNumPoints) throw new Exception("Parameter out of range");
          RefreshClosedCurveData();
          Point3D coord = new Point3D();

          double cx = 0;
          double bx = 0;
          double ax = 0;
          double cy = 0;
          double by = 0;
          double ay = 0;
          double cz = 0;
          double bz = 0;
          double az = 0;
          Point3D p0 = new Point3D();
          Point3D p1 = new Point3D();
          Point3D p2 = new Point3D();
          Point3D p3 = new Point3D();
          int n1 = (int)(param - 1);
          int n2 = n1 + 1;
          p0 = MPoints[n1];
          p1 = new Point3D(p0.X + (MDirAfter[n1].X * MMagAfter[n1] / 3), p0.Y + (MDirAfter[n1].Y * MMagAfter[n1] / 3), p0.Z + (MDirAfter[n1].Z * MMagAfter[n1] / 3));
          p3 = MPoints[n2];
          p2 = new Point3D(p3.X - (MDirBefore[n2].X * MMagBefore[n2] / 3), p3.Y - (MDirBefore[n2].Y * MMagBefore[n2] / 3), p3.Z - (MDirBefore[n2].Z * MMagBefore[n2] / 3));
          param = param - (int)(param);
          cx = 3 * (p1.X - p0.X);
          bx = 3 * (p2.X - p1.X) - cx;
          ax = p3.X - p0.X - cx - bx;
          cy = 3 * (p1.Y - p0.Y);
          by = 3 * (p2.Y - p1.Y) - cy;
          ay = p3.Y - p0.Y - cy - by;
          cz = 3 * (p1.Z - p0.Z);
          bz = 3 * (p2.Z - p1.Z) - cz;
          az = p3.Z - p0.Z - cz - bz;
          coord.X = (ax * Math.Pow(param, 3)) + (bx * Math.Pow(param, 2)) + (cx * param) + p0.X;
          coord.Y = (ay * Math.Pow(param, 3)) + (by * Math.Pow(param, 2)) + (cy * param) + p0.Y;
          coord.Z = (az * Math.Pow(param, 3)) + (bz * Math.Pow(param, 2)) + (cz * param) + p0.Z;
          return coord;
      }

      private void RefreshClosedCurveData()
      {
          if (IsClosed == true)
          { 
           MPoints[MNumPoints - 1] = MPoints[0];

                MDirBefore[MNumPoints - 1] = MDirBefore[0];
                MMagBefore[MNumPoints - 1] = MMagBefore[0];
                MDirBeforeFree[MNumPoints - 1] = MDirBeforeFree[0];
                MMagBeforeFree[MNumPoints - 1] = MMagBeforeFree[0];
                MDirAfter[MNumPoints - 1] = MDirAfter[0];
                MMagAfter[MNumPoints - 1] = MMagAfter[0];
                MDirAfterFree[MNumPoints - 1] = MDirAfterFree[0];
                MMagAfterFree[MNumPoints - 1] = MMagAfterFree[0];
          }
      }

      public Point3DCollection GetDenseCurve (double pointDensity, int pointCount)
      {
          Point3D current;
          Point3D last;
        
          double stepOver = 0;
          double len = this.GetLength(pointDensity);
          stepOver = len / (pointCount - 1);

          double inc = pointDensity;
          double p = 1 + inc;
          double lastP = 1;


          Point3D[] points = new Point3D[pointCount];
          last = GetParameterCoordinate(1);
          points[0] = last;
          int ptCount = 1;

          while (p < (MNumPoints - pointDensity))
          {
              double subL = 0;
              double t = 0;
              double dl = 0;
              //do
              {
                  current = GetParameterCoordinate(p);
                  dl = GetDist(current, last);
                  last = current;

                  subL += dl;

              }

              points[ptCount] = last;
              p += inc;
              ptCount += 1;
          }
          if (ptCount < pointCount)
          {
              points[(pointCount - 1)] = last;
          }

          points[(pointCount - 1)] = last = MPoints[MNumPoints - 1];


          Point3DCollection ptOut = new Point3DCollection();

          foreach(var v in points )
          {
              ptOut.Add(v);
          }

          return ptOut ;

      }

      public double GetLength(double pointDensity)
      {
          double totLen = 0;


          double testLen = 0;
          double inc = pointDensity;
          double p = 1 + inc;
          Point3D lastP = MPoints[0];
          Point3D nextp;
          while (p < MNumPoints - 1)
          {
              nextp = GetParameterCoordinate(p);
              testLen += GetDist(nextp, lastP);

              lastP = nextp;

              p += inc;
          }
          totLen = testLen;
          return totLen;
      }

      public double GetDist(Point3D point1, Point3D point2)
      {
          double len1 = (point1 - point2).Length;
          return len1;
      }

      public Point3DCollection GetPolygonisedPoints(double paramRes)
      {
          Point3DCollection pts = new Point3DCollection();
          double p = 1;
          while (p <= MNumPoints)
          {
              if (p == MNumPoints)
                  pts.Add(MPoints[MNumPoints - 1]);
              else
                  pts.Add(GetParameterCoordinate(p));
              p += paramRes;
          }
          return new Point3DCollection(pts);
      }


    }
}


