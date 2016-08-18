using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;


namespace KneeInnovation3D.EntityTools
{
    public partial class TriangleModel
    {
            public enum FilterOptions
            {
                FilterFirstHit,
                FilterClosest,
                FilterBelow,
                FilterAbove
            }

            private const double EPS = 0.0000001;
            private List<TriZone> zonecollection = null;
            private double zonesize = 0;
            private int zonecountX = 0;
            private int zonecountY = 0;
            private Rect3D bounds;
            private MeshGeometry3D meshmodel;

            public TriangleModel(MeshGeometry3D mesh)
             {
               meshmodel = mesh.Clone();
                bounds = mesh.Bounds;

             }

            public void ZoneTheModel(int zoneDensity)
            {
                try
                {

                    int numZones = numZones = (int) (((meshmodel.TriangleIndices.Count/3)*1.2/zoneDensity) + 1);
                    double zoneArea = (bounds.SizeX*bounds.SizeY)/numZones;
                    zonesize = System.Math.Sqrt(zoneArea);
                    zonecountX = (int) ((bounds.SizeX/zonesize) + 1);
                    zonecountY = (int) ((bounds.SizeY/zonesize) + 1);
                    numZones = zonecountX*zonecountY;
                    zonecollection = new List<TriZone>(numZones) {};
                    for (int i = 0; i < numZones; i++)
                    {
                        zonecollection.Add(new TriZone(zoneDensity));
                    }
                    int[] col = new int[3];
                    int[] row = new int[3];

                    for (int p = 0; p < meshmodel.TriangleIndices.Count -3; p++)
                    {


                        Vector3D vN =
                            KneeInnovation3D.EntityTools.MeshGeometryFunctions.CalculateNorms(meshmodel.Positions[meshmodel.TriangleIndices[p]],
                                meshmodel.Positions[meshmodel.TriangleIndices[p +1]], meshmodel.Positions[meshmodel.TriangleIndices[p +2]]);
                        TriangleVertex t = new TriangleVertex( meshmodel.Positions[meshmodel.TriangleIndices[p]], vN);
                        TriangleVertex t1 = new TriangleVertex(meshmodel.Positions[meshmodel.TriangleIndices[p +1]], vN);
                        TriangleVertex t2 = new TriangleVertex(meshmodel.Positions[meshmodel.TriangleIndices[p +2]], vN);

                        GetVertexInZone(t, ref col[0], ref row[0]);
                        GetVertexInZone(t1, ref col[1], ref row[1]);
                        GetVertexInZone(t2, ref col[2], ref row[2]);


                        System.Array.Sort(col);
                        System.Array.Sort(row);
                        for (int c = col[0]; c <= col[col.Length - 1]; c++)
                        {
                            for (int r = row[0]; r <= row[row.Length - 1]; r++)
                            {
                                int zn = (r*zonecountX) + c;
                                Triangle tri = new Triangle(t, t1, t2);
                                zonecollection[zn].Triangles.Add(tri);
                            }
                        }

                        p++;
                        p++;
                    }

                    for (int i = 0; i < numZones; i++)
                    {
                        zonecollection[i].Triangles.TrimExcess();
                    }
                }

                catch
                {
                    zonecollection = new List<TriZone> {};
                    throw;
                }
            }

            internal Point3D[] GetProjected(Point3D pointToProject, bool singular, double searchRadius)
            {
                Point3D[] intersections = new Point3D[100];
                int numIntersectionPoints = 0;
                if (zonecollection == null || zonecollection.Count == 0)
                    ZoneTheModel(0);
                int zone = GetPointZone(pointToProject);
                if (zone >= 0 && zone < zonecollection.Count)
                {
                    foreach (Triangle tri in zonecollection[zone].Triangles)
                    {
                        Point3D intPoint;
                        if (IntersectLineFace(pointToProject, tri, out intPoint))
                        {
                            intersections[numIntersectionPoints] = intPoint;
                            numIntersectionPoints++;
                            if (singular)
                                break;
                        }
                    }
                }
                Point3D[] returnedPoints = new Point3D[numIntersectionPoints];
                if (numIntersectionPoints > 0)
                {
                    Array.Copy(intersections, returnedPoints, numIntersectionPoints);
                }
                return returnedPoints;
            }

            public Point3D[] GetAllPointIntersections(Point3D pointToProject, double searchRadius)
            {
                Point3D[] intersectionPoints = GetProjected(pointToProject, false, searchRadius);
                return intersectionPoints;
            }

            public Point3D GetProjectedPoint(Point3D pointToProject, FilterOptions options, double searchRadius)
            {
                Point3D[] intersectionPoints = GetProjected(pointToProject, options == FilterOptions.FilterFirstHit, searchRadius);
                if (intersectionPoints.Length != 0)
                {
                    double Nearest = 9e99;
                    int nearIdx = -1;
                    switch (options)
                    {
                        case FilterOptions.FilterClosest:
                            if (intersectionPoints.Length == 1)
                            {
                                nearIdx = 0;
                            }
                            else
                            {
                                for (int i = 0; i < intersectionPoints.Length; i++)
                                {
                                    double Dist = System.Math.Abs((intersectionPoints[i].Z - pointToProject.Z));
                                    if (Dist < Nearest)
                                    {
                                        Nearest = Dist;
                                        nearIdx = i;
                                    }
                                }
                            }
                            break;
                        case FilterOptions.FilterAbove:
                            for (int i = 0; i < intersectionPoints.Length; i++)
                            {
                                if (intersectionPoints[i].Z > pointToProject.Z)
                                {
                                    double Dist = System.Math.Abs((intersectionPoints[i].Z - pointToProject.Z));
                                    if (Dist < Nearest)
                                    {
                                        Nearest = Dist;
                                        nearIdx = i;
                                    }
                                }
                            }
                            break;
                        case FilterOptions.FilterBelow:
                            for (int i = 0; i < intersectionPoints.Length; i++)
                            {
                                if (intersectionPoints[i].Z < pointToProject.Z)
                                {
                                    double Dist = System.Math.Abs((intersectionPoints[i].Z - pointToProject.Z));
                                    if (Dist < Nearest)
                                    {
                                        Nearest = Dist;
                                        nearIdx = i;
                                    }
                                }
                            }
                            break;
                        default:
                            nearIdx = 0;
                            break;
                    }
                    if (nearIdx == -1)
                    {
                        Point3D invalidPoint = new Point3D();

                        return invalidPoint;
                    }
                    else
                    {
                        return intersectionPoints[nearIdx];
                    }
                }
                else
                {
                    Point3D invalidPoint = new Point3D();

                    return invalidPoint;
                }
            }

            public Point3DCollection GetProjectedPointList(Point3DCollection points, FilterOptions options, double searchRadius)
            {
                Point3DCollection projectedPoints = points.Clone();
                for (int i = 0; i < points.Count; i++)
                {
                    projectedPoints[i] = GetProjectedPoint(points[i], options, searchRadius);
                }
                return projectedPoints;
            }
    
            private void GetVertexInZone(TriangleVertex node, ref int column, ref int row)
            {
                double X, Y;
                X = node.Location.X - bounds.X;
                column = (int)(X / zonesize);
                Y = node.Location.Y - bounds.Y;
                row = (int)(Y / zonesize);
            }

            private class TriZone
            {
                List<Triangle> _triangles = null;

                public TriZone(int initialSize)
                {
                    int storeageSize = initialSize * 2;
                    _triangles = new List<Triangle>(storeageSize) { };
                }

                public List<Triangle> Triangles
                {
                    get { return _triangles; }
                    set { _triangles = value; }
                }
            }

            private int GetPointZone(Point3D point)
            {
                double X1, Y1;
                X1 = point.X - bounds.X;
                Y1 = point.Y - bounds.Y;
                int col, row;
                col = (int)(X1 / zonesize);
                row = (int)(Y1 / zonesize);
                return (row * zonecountX + col);
            }

            private bool IntersectLineFace(Point3D LinePoint, Triangle Face, out Point3D Intersection)
            {
                int sign = 0;

                Point3D tpt1 = Face.Nodes[0].Location ;
                Point3D tpt2 = Face.Nodes[1].Location ;
                Point3D tpt3 = Face.Nodes[2].Location ;

                Vector3D lvec = new Vector3D(0, 0, -1);

                Intersection = new Point3D();

                if (LinePoint.X < tpt1.X && LinePoint.X < tpt2.X && LinePoint.X < tpt3.X) return false;
                if (LinePoint.X > tpt1.X && LinePoint.X > tpt2.X && LinePoint.X > tpt3.X) return false;
                if (LinePoint.Y < tpt1.Y && LinePoint.Y < tpt2.Y && LinePoint.Y < tpt3.Y) return false;
                if (LinePoint.Y > tpt1.Y && LinePoint.Y > tpt2.Y && LinePoint.Y > tpt3.Y) return false;

                Vector3D edge1_vec = (tpt2 - tpt1);
                edge1_vec.Normalize();
                Vector3D edge1_norm = Vector3D.CrossProduct (  edge1_vec , lvec);
                Vector3D to_poi_vec = (LinePoint - tpt2);
                double dp = Vector3D.DotProduct (to_poi_vec , edge1_norm);
                if (dp < 0) sign = -1;
                else if (dp > 0) sign = 1;

                Vector3D edge2_vec = tpt3 - tpt2;
                Vector3D edge2_norm =  Vector3D.CrossProduct (edge2_vec , lvec);
                dp =  Vector3D.DotProduct (to_poi_vec , edge2_norm);
                if (sign != 0)
                {
                    if (dp < 0 && sign > 0) return false;
                    if (dp > 0 && sign < 0) return false;
                }
                else
                {
                    if (dp < 0) sign = -1;
                    else if (dp > 0) sign = 1;
                }

                Vector3D edge3_vec = tpt1 - tpt3;
                Vector3D edge3_norm = Vector3D.CrossProduct (edge3_vec , lvec);
                to_poi_vec = LinePoint - tpt1;
                dp = Vector3D.DotProduct (to_poi_vec ,edge3_norm);
                if (sign != 0)
                {
                    if (dp < 0 && sign > 0) return false;
                    else if (dp > 0 && sign < 0) return false;
                }

                Vector3D tnorm = Vector3D.CrossProduct(edge1_vec , edge2_vec);
                tnorm.Normalize();
                if (tnorm.X == 0 && tnorm.Y == 0 && tnorm.Z == 0) return false;
                double dp2 = Vector3D.DotProduct (lvec , tnorm);
                if (System.Math.Abs(dp2) < EPS) return false;
                Vector3D intVec = tpt1 - LinePoint;
                double par = Vector3D.DotProduct(intVec , tnorm) / dp2;
                Intersection = LinePoint + (lvec * par);

                return true;
            }
        }

        public class Triangle
        {
            TriangleVertex[] vetices = new TriangleVertex[3];
       
            int[] index = new int[3];

          
            public Triangle(TriangleVertex nd1, TriangleVertex nd2, TriangleVertex nd3,
                int node1Index, int node2Index, int node3Index)
            {
                vetices[0] = nd1;
                vetices[1] = nd2;
                vetices[2] = nd3;
                index = new int[3];
                index[0] = node1Index;
                index[1] = node2Index;
                index[2] = node3Index;
            }

            public Triangle(TriangleVertex nd1, TriangleVertex nd2, TriangleVertex nd3)
            {
                vetices[0] = nd1;
                vetices[1] = nd2;
                vetices[2] = nd3;
                index = new int[0];
            }

            public int[] NodeIndeces
            {
                get { return index; }
            }

            public TriangleVertex[] Nodes
            {
                get { return vetices; }
            }
        }

        public class TriangleVertex
        {
            Point3D location;
            Vector3D vector;

            public TriangleVertex(Point3D position, Vector3D normal)
            {
                location = position;
                vector = normal;
            }

            public Point3D Location
            {
                get { return location; }
                set { location = value; }
            }

            public Vector3D Normal
            {
                get { return vector; }
                set { vector = value; }
            }

        }
    }

