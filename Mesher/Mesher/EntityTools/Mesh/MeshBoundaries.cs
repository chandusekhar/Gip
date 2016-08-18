using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Media.Media3D ;

using Edge = System.Tuple<int, int>;

namespace KICoCAD
{
   public  class MeshBoundary
    {

        public List<int[]> Triangles;

        public List<Edge> GetOutLineEdges()
        {
            var allEdges = new List<Edge>();
            foreach (var t in Triangles)
            {
                allEdges.Add(Tuple.Create(t[0], t[1]));
                allEdges.Add(Tuple.Create(t[1], t[2]));
                allEdges.Add(Tuple.Create(t[0], t[2]));

            }

            var groupByMultiplicity = allEdges.GroupBy(entry => entry, new EdgeComparer());
            var output = groupByMultiplicity.Where(grp => grp.Count() == 1).Select(grp => grp.Key).ToList();
            return output;
        }

        public  List<Point3DCollection> GetOpenBoundaries(MeshGeometry3D inputmesh)
        {

            MeshBoundary tb = new MeshBoundary();
            tb.Triangles = new List<int[]>();
            for (int i = 0; i < inputmesh.TriangleIndices.Count; i++)
            {
                int[] tri = new int[3];
                tri[0] = inputmesh.TriangleIndices[i];
                tri[1] = inputmesh.TriangleIndices[i + 1];
                tri[2] = inputmesh.TriangleIndices[i + 2];

                tb.Triangles.Add(tri);
                i++;
                i++;
            }

            List<System.Tuple<int, int>> t = tb.GetOutLineEdges();


            List<Point3DCollection> pc = new System.Collections.Generic.List<Point3DCollection>();

            Point3DCollection Bound = new Point3DCollection();

            for (int i = 0; i < t.Count; i++)
            {
                int a = t[i].Item1;
                int b = t[i].Item2;
                Bound.Add(inputmesh.Positions[a]);
                Bound.Add(inputmesh.Positions[b]);

            }

            Bound = ReorderUsingVector(Bound);
            pc.Add(Bound);

            return pc;
        }

        public static Point3DCollection ReorderUsingVector(Point3DCollection Input)
        {
            Point3D center = KneeInnovation3D.EntityTools.PointCollections.GetCenter(Input);   

            Point3DCollection reordered = new Point3DCollection();

            reordered.Add(Input[0]);

            Input.RemoveAt(0);

            double a = 1000;
            int f = -1;
            while (Input.Count > 1)
            {
                a = 1000;
                f = -1;

                if (Input.Count != 1)
                {
                    for (int i = Input.Count - 1; i >= 0; i--)
                    {
                        Vector3D v1 = center - Input[i];
                        Vector3D v2 = center - reordered[reordered.Count - 1];
                        double angle = Vector3D.AngleBetween(v1, v2);

                        if (angle < a)
                        {
                            a = angle;
                            f = i;
                        }
                    }

                    reordered.Add(Input[f]);
                    Input.RemoveAt(f);
                }
                else
                {
                    reordered.Add(Input[0]);
                    Input.RemoveAt(0);
                }
            }


            return reordered;
        }
    }

    class EdgeComparer : IEqualityComparer<Edge>
    {
        public bool Equals(Edge x, Edge y)
        {
            bool b1 = (x.Item1 == y.Item1 && x.Item2 == y.Item2);
            bool b2 = (x.Item2 == y.Item1 && x.Item1 == y.Item2);
            return b1 || b2;

        }

        public int GetHashCode(Edge obj)
        {
            return 0;
        }
    }

    
}
