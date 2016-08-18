// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polygon3D.cs" company="Helix 3D Toolkit">
//   http://EntityTools.codeplex.com, license: MIT
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ViewPortTools;
using System.Linq;


namespace KneeInnovation3D.EntityTools
{

    /// <summary>
    /// Represents a 3D polygon.
    /// </summary>
    public class Polygon3D
    {
        /// <summary>
        /// The points.
        /// </summary>
        private IList<Point3D> _points;

        /// <summary>
        /// Initializes a new instance of the <see cref = "Polygon3D" /> class.
        /// </summary>
        public Polygon3D()
        {
            this._points = new List<Point3D>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon3D"/> class.
        /// </summary>
        /// <param name="pts">
        /// The PTS.
        /// </param>
        public Polygon3D(IList<Point3D> pts)
        {
            this._points = pts;
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
        public IList<Point3D> Points
        {
            get
            {
                return this._points;
            }

            set
            {
                this._points = value;
            }
        }

        //// http://en.wikipedia.org/wiki/Polygon_triangulation
        //// http://en.wikipedia.org/wiki/Monotone_polygon
        //// http://www.codeproject.com/KB/recipes/hgrd.aspx LGPL
        //// http://www.springerlink.com/content/g805787811vr1v9v/
        
        /// <summary>
        /// Flattens this polygon.
        /// </summary>
        /// <returns>
        /// The 2D polygon.
        /// </returns>
        public EntityTools .Polygon Flatten()
        {
            // http://forums.xna.com/forums/p/16529/86802.aspx
            // http://stackoverflow.com/questions/1023948/rotate-normal-vector-onto-axis-plane
            var up = this.GetNormal();
            up.Normalize();
            var right = Vector3D.CrossProduct(
                up, Math.Abs(up.X) > Math.Abs(up.Z) ? new Vector3D(0, 0, 1) : new Vector3D(1, 0, 0));
            var backward = Vector3D.CrossProduct(right, up);
            var m = new Matrix3D(
                backward.X, right.X, up.X, 0, backward.Y, right.Y, up.Y, 0, backward.Z, right.Z, up.Z, 0, 0, 0, 0, 1);

            // make first point origin
            var offs = m.Transform(this.Points[0]);
            m.OffsetX = -offs.X;
            m.OffsetY = -offs.Y;

            var polygon = new EntityTools.Polygon { Points = new PointCollection(this.Points.Count) };
            foreach (var p in this.Points)
            {
                var pp = m.Transform(p);
                polygon.Points.Add(new Point(pp.X, pp.Y));
            }

            return polygon;

          
        }

        /// <summary>
        /// Gets the normal of the polygon.
        /// </summary>
        /// <returns>
        /// The normal.
        /// </returns>
        public Vector3D GetNormal()
        {
            if (this.Points.Count < 3)
            {
                throw new InvalidOperationException("At least three points required in the polygon to find a normal.");
            }

            Vector3D v1 = this.Points[1] - this.Points[0];
            for (int i = 2; i < this.Points.Count; i++)
            {
                var n = Vector3D.CrossProduct(v1, this.Points[i] - this.Points[0]);
                if (n.LengthSquared > 1e-8)
                {
                    n.Normalize();
                    return n;
                }
            }

            throw new InvalidOperationException("Invalid polygon.");
        }

        /// <summary>
        /// Determines whether this polygon is planar.
        /// </summary>
        /// <returns>
        /// The is planar.
        /// </returns>
        public bool IsPlanar()
        {
            Vector3D v1 = this.Points[1] - this.Points[0];
            var normal = new Vector3D();
            for (int i = 2; i < this.Points.Count; i++)
            {
                var n = Vector3D.CrossProduct(v1, this.Points[i] - this.Points[0]);
                n.Normalize();
                if (i == 2)
                {
                    normal = n;
                }
                else if (Math.Abs(Vector3D.DotProduct(n, normal) - 1) > 1e-8)
                {
                    return false;
                }
            }

            return true;
        }

        public static void Transform(Point3DCollection Points, Transform3D transform)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = transform.Transform(Points[i]);
            }
        }

        public static void InverseTransform(Point3DCollection Points, Transform3D transform)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = transform.Inverse.Transform(Points[i]);
            }
        }

        public static void Scale(Point3DCollection Points, double x, double y, double z)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = new Point3D(Points[i].X * x, Points[i].Y * y, Points[i].Z * z);
            }
        }

        public static Rect3D Extents(Point3DCollection Points)
        {
            double minX = 9e99;
            double maxX = -9e99;
            double minY = 9e99;
            double maxY = -9e99;
            double minZ = 9e99;
            double maxZ = -9e99;
            foreach (Point3D p in Points)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.Z < minZ) minZ = p.Z;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
                if (p.Z > maxZ) maxZ = p.Z;
            }
            return new Rect3D(minX, minY, minZ, maxX - minX, maxY - minY, maxZ - minZ);
        }

        public static Point3D GetCenter(Point3DCollection Points)
        {
            Rect3D bounds = Extents(Points);
            return new Point3D(bounds.X + (bounds.SizeX / 2), bounds.Y + (bounds.SizeY / 2), bounds.Z + (bounds.SizeZ / 2));
        }


        public static bool IsClosed(Point3DCollection Points)
        {
            return ((Points[0] - Points[Points.Count - 1]).Length < 0.0001);
        }

        public static void Reverse(Point3DCollection curve)
        {
            Point3DCollection copied = curve.Clone();
            curve.Clear();
            foreach (Point3D p in copied.Reverse())
                curve.Add(p);
            copied = null;
        } 

        public static Transform3D TransformtoCordSys(Point3D o, Vector3D x, Vector3D y, Vector3D z)
        {
            Matrix3D mat = new Matrix3D(x.X, x.Y, x.Z, 0, y.X, y.Y, y.Z, 0, z.X, z.Y, z.Z, 0, o.X, o.Y, o.Z, 1);
            return (Transform3D)(new MatrixTransform3D(mat));
        }

        public static ScreenSpaceLines3D GetScreenSpaceLines3D(Point3DCollection pcol, Color mat)
        {
            ScreenSpaceLines3D spacerlines = new ScreenSpaceLines3D();
            spacerlines.Thickness = 2;
            spacerlines.Color = mat;

            for (int i = 1; i < pcol.Count; i++)
            {
                spacerlines.Points.Add(pcol[i - 1]);
                spacerlines.Points.Add(pcol[i]);
            }

            return spacerlines;
        }

        public static Transform3D TransformtoCordSys(Point3D origin, Vector3D zaxis)
        {
            Vector3D zAxisLocal = zaxis;

            Vector3D xAxisLocal, yAxisLocal;
            if (System.Math.Abs(zAxisLocal.Z) >= System.Math.Abs(zAxisLocal.X) && System.Math.Abs(zAxisLocal.Z) >= System.Math.Abs(zAxisLocal.Y))
            {
                xAxisLocal = new Vector3D(1, 0, -1 * zAxisLocal.Y / zAxisLocal.Z);
            }
            else if (System.Math.Abs(zAxisLocal.Y) >= System.Math.Abs(zAxisLocal.X) && System.Math.Abs(zAxisLocal.Y) >= System.Math.Abs(zAxisLocal.Z))
            {
                xAxisLocal = new Vector3D(1, -1 * zAxisLocal.Z / zAxisLocal.Y, 0);
            }
            else if (zAxisLocal.X != 0)
            {
                xAxisLocal = new Vector3D(-1 * zAxisLocal.Z / zAxisLocal.X, 0, 1);
            }
            else
            {
                xAxisLocal = new Vector3D(1, 0, 0);
            }

            yAxisLocal = Vector3D.CrossProduct(zAxisLocal, xAxisLocal);
            xAxisLocal = Vector3D.CrossProduct(yAxisLocal, zAxisLocal);
            xAxisLocal.Normalize();
            yAxisLocal.Normalize();

            return TransformtoCordSys(origin, xAxisLocal, yAxisLocal, zAxisLocal);
        }

        public static bool Ispointinside(Point3DCollection mPoints, Point3D pt)
        {
            int j;

            bool oddnodes = false;

            for (int i = 0; i < mPoints.Count - 2; i++)
            {
                j = i + 1;
                if ((mPoints[i].Y < pt.Y && mPoints[j].Y >= pt.Y) || (mPoints[j].Y < pt.Y && mPoints[i].Y >= pt.Y))
                {
                    if (mPoints[i].X + (pt.Y - mPoints[i].Y) / (mPoints[j].Y - mPoints[i].Y) * (mPoints[j].X - mPoints[i].X) < pt.X)
                    {
                        oddnodes = true;
                    }

                }
            }

            return oddnodes;
        }

        public static Point3DCollection GetCircle(double rad, Point3D centre)
        {
            Point3DCollection pc = new Point3DCollection();

            Point3D p = new Point3D(rad, 0, 0);
            RotateTransform3D r = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 10));
            pc.Add(p);
            for (int i = 0; i < 36; i++)
            {
                p = r.Transform(p);
                pc.Add(p);
            }

            TranslateTransform3D t = new TranslateTransform3D(centre.X, centre.Y, centre.Z);
            Transform(pc, t);

            return pc;
        }

        public static Point3DCollection GetArc(double rad, double angle, int res, Point3D centre)
        {
            Point3DCollection pc = new Point3DCollection();

            Point3D p = new Point3D(rad, 0, 0);
            RotateTransform3D r = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), angle/res));
            pc.Add(p);
            for (int i = 0; i < res; i++)
            {
                p = r.Transform(p);
                pc.Add(p);
            }

            TranslateTransform3D t = new TranslateTransform3D(centre.X, centre.Y, centre.Z);
            Transform(pc, t);

            return pc;
        }



        public static Point3DCollection ExtendLineBothDirections(Point3DCollection inputline, double offsetamount)
        {
            Vector3D v1 =  inputline[0] - inputline[inputline.Count - 1];
            Vector3D v2 = inputline[inputline.Count - 1] - inputline[0];
            v1.Normalize();
            v2.Normalize();

            Point3D startPoint = inputline[0];
            Point3D endPoint = inputline[inputline.Count - 1];
            startPoint.Offset(offsetamount * v1.X, offsetamount * v1.Y, offsetamount * v1.Z);
            inputline.Insert (0, startPoint);
            endPoint.Offset(offsetamount * v2.X, offsetamount * v2.Y, offsetamount * v2.Z);
            inputline.Add(endPoint);

            return inputline;

        }
        public static List<Point3DCollection> GetCrossSection(MeshGeometry3D m1, Vector3D cutVect, Point3D origin)
        {
            ContourHelper ContHelp = new ContourHelper(new Point3D(0, 0, -0.1 * cutVect.Z), cutVect, m1);
            var segments = new List<Point3D>();

            for (var i = 0; i < m1.TriangleIndices.Count; i += 3)
            {
                var index0 = m1.TriangleIndices[i];
                var index1 = m1.TriangleIndices[i + 1];
                var index2 = m1.TriangleIndices[i + 2];

                Point3D[] positions;
                Point[] textureCoordinates;
                int[] triangleIndices;

                ContHelp.ContourFacet(index0, index1, index2, out positions, out textureCoordinates, out triangleIndices);

                foreach (var p in positions)
                {
                    segments.AddRange(positions);
                }
            }

            List<Point3DCollection> Simple = KneeInnovation3D.EntityTools.MeshGeometryFunctions.CombineSegments(
                segments, 0.01);

            List<Point3DCollection> SimplifySimple = new List<Point3DCollection>();

            foreach (var p in Simple)
            {
                IEnumerable<Point3D> temp = new Point3DCollection();
                temp = p.Take(p.Count / 2);
                SimplifySimple.Add(new Point3DCollection(temp));
            }

            return SimplifySimple;

        }

        public static Point3DCollection GetDensifiedLine(Point3D P1, Point3D P2, int NumberOfPoints)
        {
            double length = (P1 - P2).Length;
            double increment =  length/NumberOfPoints;

            Vector3D vV = P1 - P2;
            vV.Normalize();

            Point3DCollection LineOut = new Point3DCollection();
            LineOut.Add(P1);

            for(int i= 0; i < NumberOfPoints; i++ )
            {
                Point3D Po = P1;
                Po.Offset((-increment*i)*vV.X, (-increment*i)*vV.Y, (-increment*i)*vV.Z);
                LineOut.Add(Po);
            }

            return LineOut; 
        }

    }
}