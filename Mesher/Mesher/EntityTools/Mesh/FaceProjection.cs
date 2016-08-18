using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media.Media3D.Converters;
using KneeInnovation3D.EntityTools;

namespace KICoCAD.EntityTools.Mesh
{
    class FaceProjection
    {
    }

    public class TriFace
    {
        public Point3D[] PointsInFace;
        public Vector3D  Norm;
        public int Num;
    
    
        public TriFace(Point3D[] vertices, int facenumber)
        {
            PointsInFace[0] = vertices[0];
            PointsInFace[1] = vertices[1];
            PointsInFace[2] = vertices[2];
            Num = facenumber;

            Vector3D  v1 = (vertices[1] - vertices[0]);
            Vector3D  v2 = (vertices[2] - vertices[1]);

            v1.Normalize();
            v2.Normalize();
            Norm = Vector3D.CrossProduct(v1, v2);


        }

        public void ReverseNormal()
        {
            Array.Reverse(PointsInFace );
            if (Norm != null)
                Norm *= -1;
        }

        private bool  QuickCheckDirection(Point3D point1, Point3D  point2, Point3D  point3, Vector3D  norm)
        {
            double Checki, Checkj, Checkk;
            double CheckProduct;
            
            Checki = (((point2.Y  - point1.Y) * (point3.Z - point1.Z)) - ((point3.Y  - point1.Y) * (point2.Z - point1.Z)));
            Checkj = (((point2.Z - point1.Z) * (point3.X - point1.X)) - ((point3.Z - point1.Z) * (point2.X - point1.X)));
            Checkk = (((point2.X - point1.X) * (point3.Y  - point1.Y)) - ((point3.X - point1.X) * (point2.Y - point1.Y)));

            Vector3D checkV = new Vector3D(Checki, Checkj, Checkk);

     
            CheckProduct = Vector3D.DotProduct(norm, checkV);
  
            if (CheckProduct < 0) return false;
            else return true;
        }

        private bool  CheckForIntersections(Point3D pt1, Point3D  pt2, Point3D pt3, Point3D  linept, Vector3D  vect,
                                        ref Point3D  pt_int, ref double tOut, bool usePreparedNormal)
        {
            double V1x, V1y, V1z;
            double V2x, V2y, V2z;
            Vector3D normaldir = new Vector3D();
            double CheckProduct;
            double t;

            if (!usePreparedNormal || Norm == null)
            {
                
                V1x = pt2.X - pt1.X;
                V1y = pt2.Y- pt1.Y;
                V1z = pt2.Z - pt1.Z;
 
                V2x = pt3.X - pt2.X;
                V2y = pt3.Y - pt2.Y;
                V2z = pt3.Z - pt2.Z;
  
                normaldir.X = V1y * V2z - V1z * V2y;
                normaldir.Y = V1z * V2x - V1x * V2z;
                normaldir.Z = V1x * V2y - V1y * V2x;
            }
            else
            {
                normaldir = Norm;
            }

    
            CheckProduct = normaldir.X * vect.X  + normaldir.Y * vect.Y  + normaldir.Z * vect.Z ;

            if (System.Math.Abs(CheckProduct) > 0)
            {
              
                t = -(normaldir.X * (linept.X  - pt1.X) + normaldir.Y * (linept.Y  - pt1.Y) + normaldir.Z  * (linept.Z  - pt1.Z)) /
                       (normaldir.X * vect.X  + normaldir.Y * vect.Y + normaldir.Z * vect.Z );

               
                if (t < 0) return false;

                pt_int.X = linept.X + vect.X * t;
                pt_int.Y = linept.Y  + vect.Y  * t;
                pt_int.Z = linept.Z + vect.Z * t;
                tOut = t;

                if (QuickCheckDirection(pt1, pt2, pt_int, normaldir))
                {
                    if (QuickCheckDirection(pt2, pt3, pt_int, normaldir))
                    {
                        if (QuickCheckDirection(pt3, pt1, pt_int, normaldir))
                        {
                           
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool Intersect_Face(Point3D origin , Vector3D direction , ref Point3D  intersectionout , ref double tOut, bool usenormal)
        {
            bool intersectionfound = CheckForIntersections(PointsInFace[0], PointsInFace[1], PointsInFace[2], origin, direction, ref intersectionout, ref tOut, usenormal);
            return intersectionfound;
        }

        public bool Intersect_Face(Point3D  orig,  Vector3D  dir, out Point3D intersection)
        {
            Point3D vertex1 = new Point3D();
            Point3D vertex2 = new Point3D();
            Vector3D vectori = new Vector3D();
            Vector3D vectorj = new Vector3D();
            Vector3D vectork = new Vector3D();
            intersection = new Point3D();
            double determinant, inv_determinant;

            vertex1.X = PointsInFace[1].X - PointsInFace[0].X;
            vertex1.Y = PointsInFace[1].Y - PointsInFace[0].Y;
            vertex1.Z = PointsInFace[1].Z - PointsInFace[0].Z;
            vertex2.X = PointsInFace[2].X - PointsInFace[0].X;
            vertex2.Y = PointsInFace[2].Y - PointsInFace[0].Y;
            vertex2.Z = PointsInFace[2].Z - PointsInFace[0].Z;

            vectori.X = dir.Y * vertex2.Z - dir.Z * vertex2.Y;
            vectori.Y = dir.Z * vertex2.X - dir.X * vertex2.Z;
            vectori.Z = dir.X * vertex2.Y - dir.Y * vertex2.X;

            determinant = vertex1.X * vectori.X + vertex1.Y * vectori.Y + vertex1.Z * vectori.Z;

            if (determinant < 0.0000001)
                return false ;

            vectorj.X = orig.X - PointsInFace[0].X;
            vectorj.Y = orig.Y - PointsInFace[0].Y;
            vectorj.Z = orig.Z - PointsInFace[0].Z;

            double Yintersection = vectorj.X * vectori.X + vectorj.Y * vectori.Y + vectorj.Z * vectori.Z;
            if (Yintersection < 0 || Yintersection > determinant)
                return false;

            vectork.X  = vectorj.Y  * vertex1.Z  - vectorj.Z  * vertex1.Y;
            vectork.Y  = vectorj.Z  * vertex1.X  - vectorj.X  * vertex1.Z ;
            vectork.Z  = vectorj.X  * vertex1.Y  - vectorj.Y  * vertex1.X ;

            double Zintersection = dir.X * vectork.X + dir.Y  * vectork.Y  + dir.Z  * vectork.Z ;
            if (Zintersection < 0 || Yintersection + Zintersection > determinant)
                return false;

            double Xintersection = vertex2.X  * vectork.X  + vertex2.Y * vectork.Y  + vertex2.Z * vectork.Z ;
            inv_determinant = 1 / determinant;
            Xintersection *= inv_determinant;
            Yintersection *= inv_determinant;
            Zintersection *= inv_determinant;
 

            return true;
        }
    }
}
