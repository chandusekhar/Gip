using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Linq;
using System.Xml;
using System.IO;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
[assembly: InternalsVisibleTo("KICoCADTests")]

namespace KneeInnovation3D.EntityTools
{
   
    /// <summary>
    /// Provides helper methods for mesh geometries.
    /// </summary>
    public static class MeshGeometryFunctions
    {

        /// <summary>
        /// Calculates the normal vectors.
        /// </summary>
        /// <param name="mesh">
        /// The mesh.
        /// </param>
        /// <returns>
        /// Collection of normal vectors.
        /// </returns>
        public static Vector3DCollection CalculateNormals(MeshGeometry3D mesh)
        {
            return CalculateNormals(mesh.Positions, mesh.TriangleIndices);
        }

        /// <summary>
        /// Calculates the normal vectors.
        /// </summary>
        /// <param name="positions">
        /// The positions.
        /// </param>
        /// <param name="triangleIndices">
        /// The triangle indices.
        /// </param>
        /// <returns>
        /// Collection of normal vectors.
        /// </returns>
        public static Vector3DCollection CalculateNormals(IList<Point3D> positions, IList<int> triangleIndices)
        {
            var normals = new Vector3DCollection(positions.Count);
            for (int i = 0; i < positions.Count; i++)
            {
                normals.Add(new Vector3D());
            }

            for (int i = 0; i < triangleIndices.Count; i += 3)
            {
                int index0 = triangleIndices[i];
                int index1 = triangleIndices[i + 1];
                int index2 = triangleIndices[i + 2];
                var p0 = positions[index0];
                var p1 = positions[index1];
                var p2 = positions[index2];
                Vector3D u = p1 - p0;
                Vector3D v = p2 - p0;
                Vector3D w = Vector3D.CrossProduct(u, v);
                w.Normalize();
               
                if (double.IsNaN (w.X ) || double.IsNaN (w.Y ) || double.IsNaN (w.Z))
                {
                    int a = 0;
                }
                
                normals[index0] += w;
                normals[index1] += w;
                normals[index2] += w;

               
            }

            for (int i = 0; i < normals.Count; i++)
            {
                var w = normals[i];
                w.Normalize();
                normals[i] = w;
            }

            return normals;
        }

        /// <summary>
        /// Finds edges that are only connected to one triangle.
        /// </summary>
        /// <param name="mesh">
        /// A mesh geometry.
        /// </param>
        /// <returns>
        /// The edge indices for the edges that are only used by one triangle.
        /// </returns>
        public static Int32Collection FindBorderEdges(MeshGeometry3D mesh)
        {
            var dict = new Dictionary<ulong, int>();

            for (int i = 0; i < mesh.TriangleIndices.Count / 3; i++)
            {
                int i0 = i * 3;
                for (int j = 0; j < 3; j++)
                {
                    int index0 = mesh.TriangleIndices[i0 + j];
                    int index1 = mesh.TriangleIndices[i0 + ((j + 1) % 3)];
                    int minIndex = Math.Min(index0, index1);
                    int maxIndex = Math.Max(index1, index0);
                    ulong key = CreateKey((uint)minIndex, (uint)maxIndex);
                    if (dict.ContainsKey(key))
                    {
                        dict[key] = dict[key] + 1;
                    }
                    else
                    {
                        dict.Add(key, 1);
                    }
                }
            }

            var edges = new Int32Collection();
            foreach (var kvp in dict)
            {
                // find edges only used by 1 triangle
                if (kvp.Value == 1)
                {
                    uint i0, i1;
                    ReverseKey(kvp.Key, out i0, out i1);
                    edges.Add((int)i0);
                    edges.Add((int)i1);
                }
            }

            return edges;
        }

        /// <summary>
        /// Finds all edges in the mesh (each edge is only included once).
        /// </summary>
        /// <param name="mesh">
        /// A mesh geometry.
        /// </param>
        /// <returns>
        /// The edge indices (minimum index first).
        /// </returns>
        public static Int32Collection FindEdges(MeshGeometry3D mesh)
        {
            var edges = new Int32Collection();
            var dict = new HashSet<ulong>();

            for (int i = 0; i < mesh.TriangleIndices.Count / 3; i++)
            {
                int i0 = i * 3;
                for (int j = 0; j < 3; j++)
                {
                    int index0 = mesh.TriangleIndices[i0 + j];
                    int index1 = mesh.TriangleIndices[i0 + ((j + 1) % 3)];
                    int minIndex = Math.Min(index0, index1);
                    int maxIndex = Math.Max(index1, index0);
                    ulong key = CreateKey((uint)minIndex, (uint)maxIndex);
                    if (!dict.Contains(key))
                    {
                        edges.Add(minIndex);
                        edges.Add(maxIndex);
                        dict.Add(key);
                    }
                }
            }

            return edges;
        }

        /// <summary>
        /// Finds all edges where the angle between adjacent triangle normal vectors.
        /// is larger than minimumAngle
        /// </summary>
        /// <param name="mesh">
        /// A mesh geometry.
        /// </param>
        /// <param name="minimumAngle">
        /// The minimum angle between the normal vectors of two adjacent triangles (degrees).
        /// </param>
        /// <returns>
        /// The edge indices.
        /// </returns>
        public static Int32Collection FindSharpEdges(MeshGeometry3D mesh, double minimumAngle)
        {
            var edgeIndices = new Int32Collection();

            // the keys of the dictionary are created from the triangle indices of the edge
            var edgeNormals = new Dictionary<ulong, Vector3D>();

            for (int i = 0; i < mesh.TriangleIndices.Count / 3; i++)
            {
                int i0 = i * 3;
                var p0 = mesh.Positions[mesh.TriangleIndices[i0]];
                var p1 = mesh.Positions[mesh.TriangleIndices[i0 + 1]];
                var p2 = mesh.Positions[mesh.TriangleIndices[i0 + 2]];
                var n = Vector3D.CrossProduct(p1 - p0, p2 - p0);
                n.Normalize();
                for (int j = 0; j < 3; j++)
                {
                    int index0 = mesh.TriangleIndices[i0 + j];
                    int index1 = mesh.TriangleIndices[i0 + ((j + 1) % 3)];
                    int minIndex = Math.Min(index0, index1);
                    int maxIndex = Math.Max(index0, index1);
                    ulong key = CreateKey((uint)minIndex, (uint)maxIndex);
                    Vector3D value;
                    if (edgeNormals.TryGetValue(key, out value))
                    {
                        var n2 = value;
                        n2.Normalize();
                        double angle = 180 / Math.PI * Math.Acos(Vector3D.DotProduct(n, n2));
                        if (angle > minimumAngle)
                        {
                            edgeIndices.Add(minIndex);
                            edgeIndices.Add(maxIndex);
                        }
                    }
                    else
                    {
                        edgeNormals.Add(key, n);
                    }
                }
            }

            return edgeIndices;
        }

        /// <summary>
        /// Creates a new mesh where no vertices are shared.
        /// </summary>
        /// <param name="input">
        /// The input mesh.
        /// </param>
        /// <returns>
        /// A new mesh.
        /// </returns>
        public static MeshGeometry3D NoSharedVertices(MeshGeometry3D input)
        {
            var p = new Point3DCollection();
            var ti = new Int32Collection();
            Vector3DCollection n = null;
            if (input.Normals != null && input.Normals .Count != 0)
            {
                n = new Vector3DCollection();
            }

            PointCollection tc = null;
            if (input.TextureCoordinates != null && input.TextureCoordinates .Count != 0)
            {
                tc = new PointCollection();
            }

            for (int i = 0; i < input.TriangleIndices.Count; i += 3)
            {
                int i0 = i;
                int i1 = i + 1;
                int i2 = i + 2;
                int index0 = input.TriangleIndices[i0];
                int index1 = input.TriangleIndices[i1];
                int index2 = input.TriangleIndices[i2];
                var p0 = input.Positions[index0];
                var p1 = input.Positions[index1];
                var p2 = input.Positions[index2];
                p.Add(p0);
                p.Add(p1);
                p.Add(p2);
                ti.Add(i0);
                ti.Add(i1);
                ti.Add(i2);
                if (n != null)
                {
                    n.Add(input.Normals[index0]);
                    n.Add(input.Normals[index1]);
                    n.Add(input.Normals[index2]);
                }

                if (tc != null)
                {
                    tc.Add(input.TextureCoordinates[index0]);
                    tc.Add(input.TextureCoordinates[index1]);
                    tc.Add(input.TextureCoordinates[index2]);
                }
            }

            return new MeshGeometry3D { Positions = p, TriangleIndices = ti, Normals = n, TextureCoordinates = tc };
        }

        /// <summary>
        /// Simplifies the specified mesh.
        /// </summary>
        /// <param name="mesh">
        /// The mesh.
        /// </param>
        /// <param name="eps">
        /// The tolerance.
        /// </param>
        /// <returns>
        /// A simplified mesh.
        /// </returns>
        public static MeshGeometry3D Simplify(MeshGeometry3D mesh, double eps)
        {
            // Find common positions
            var dict = new Dictionary<int, int>(); // map position index to first occurence of same position
            for (int i = 0; i < mesh.Positions.Count; i++)
            {
                for (int j = i + 1; j < mesh.Positions.Count; j++)
                {
                    if (dict.ContainsKey(j))
                    {
                        continue;
                    }

                    double l2 = (mesh.Positions[i] - mesh.Positions[j]).LengthSquared;
                    if (l2 < eps)
                    {
                        dict.Add(j, i);
                    }
                }
            }

            var p = new Point3DCollection();
            var ti = new Int32Collection();

            // create new positions array
            var newIndex = new Dictionary<int, int>(); // map old index to new index
            for (int i = 0; i < mesh.Positions.Count; i++)
            {
                if (!dict.ContainsKey(i))
                {
                    newIndex.Add(i, p.Count);
                    p.Add(mesh.Positions[i]);
                }
            }

            // Update triangle indices
            foreach (int index in mesh.TriangleIndices)
            {
                int j;
                ti.Add(dict.TryGetValue(index, out j) ? newIndex[j] : newIndex[index]);
            }

            var result = new MeshGeometry3D { Positions = p, TriangleIndices = ti };
            return result;
        }

        /// <summary>
        /// Simplifies the specified mesh.
        /// </summary>
        /// <param name="mesh">
        /// The mesh.
        /// </param>
        /// <param name="eps">
        /// The tolerance.
        /// </param>
        /// <returns>
        /// A simplified mesh.
        /// </returns>


        public static void Simplify(MeshGeometry3D targetmesh)
        {
            Dictionary<int, Point3D> initialList = new Dictionary<int, Point3D>(targetmesh.Positions.Count);
            for (int i = 0; i < targetmesh.Positions.Count; i++)
            {
                initialList.Add(i, targetmesh.Positions[i]);
            }

            List<int>[] triangleLists = new List<int>[targetmesh.Positions.Count];
            for (int i = 0; i < targetmesh.TriangleIndices.Count; i++)
            {
                if (triangleLists[targetmesh.TriangleIndices[i]] == null)
                {
                    triangleLists[targetmesh.TriangleIndices[i]] = new List<int> { };
                }

                triangleLists[targetmesh.TriangleIndices[i]].Add(i);
            }

            Int32Collection sortedIndices = targetmesh.TriangleIndices.Clone();
            System.Collections.Generic.IEnumerable <System.Linq.IGrouping<Point3D , System.Collections.Generic .KeyValuePair<int, Point3D >>> linqlist = from t in initialList group t by t.Value;

            foreach (var vertices in linqlist )
            {
                if (vertices.Count() > 1)
                {
                    int substitutionVal = vertices.First().Key;
                    foreach (var v in vertices)
                    {
                        if (v.Key != substitutionVal)
                        {
                            if (triangleLists[v.Key] != null )
                            {
                            for (int t = 0; t < triangleLists[v.Key].Count; t++)
                            {
                                if (triangleLists[substitutionVal] != null)
                                {
                                    triangleLists[substitutionVal].Add(triangleLists[v.Key][t]);
                                    sortedIndices[triangleLists[v.Key][t]] = substitutionVal;
                                }

                            }
                            }
                            triangleLists[v.Key] = null;
                        }
                    }
                }
            }

            Point3DCollection sortedPoints = new Point3DCollection(targetmesh.Positions.Count);
            for (int i = 0; i < triangleLists.Length; i++)
            {
                if (triangleLists[i] != null && triangleLists[i].Count > 0)
                {
                    sortedPoints.Add(targetmesh.Positions[i]);
                    int newidx = sortedPoints.Count - 1;
                    for (int t = 0; t < triangleLists[i].Count; t++)
                    {
                        sortedIndices[triangleLists[i][t]] = newidx;
                    }
                }
            }
            triangleLists = null;
            initialList.Clear();
            initialList = null;
            targetmesh.TriangleIndices.Clear();
            targetmesh.Positions.Clear();
            targetmesh.Positions = sortedPoints;
            targetmesh.TriangleIndices = sortedIndices;
        }

        public static MeshGeometry3D  AppendMeshGeometry3D( MeshGeometry3D parent,  MeshGeometry3D child)
        {
            int offset = parent.Positions.Count;
            for (int p = 0; p < child.Positions.Count; p++)
            {
                parent.Positions.Add(child.Positions[p]);
            }
            for (int i = 0; i < child.TriangleIndices.Count; i++)
            {
                parent.TriangleIndices.Add(offset + child.TriangleIndices[i]);
            }

            return parent;
        }

        public static void GetLineProjection(MeshGeometry3D mesh, Point3D origins, Vector3D direct, out Point3DCollection  results, out bool projFound)
        {
            results = new Point3DCollection();
            projFound = false;
            GeometryModel3D model = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.Red));
            ModelVisual3D modelVis = new ModelVisual3D();
            modelVis.Content = model;
           
                _projectedPointFound = false;
                VisualTreeHelper.HitTest(modelVis, null, new HitTestResultCallback(MyHitTestResult), new RayHitTestParameters(origins, direct));
                if (_projectedPointFound)
                {
                    results.Add(_lastProjectedPoint);
                    projFound = true;
                }
                else
                {
                    results.Add(new Point3D());
                    projFound = false;
                }
            
            modelVis.Content = null;
            model = null;
            modelVis = null;
        }

        public static void GetLineProjections(MeshGeometry3D mesh, Point3DCollection origins, Vector3D direct, out Point3DCollection results, out bool[] projsFound)
        {
            results = new Point3DCollection(origins.Count);
            projsFound = new bool[origins.Count];
            GeometryModel3D model = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.Red));
            ModelVisual3D modelVis = new ModelVisual3D();
            modelVis.Content = model;
            for (int i = 0; i < origins.Count; i++)
            {
                _projectedPointFound = false;
                VisualTreeHelper.HitTest(modelVis, null, new HitTestResultCallback(MyHitTestResult), new RayHitTestParameters(origins[i], direct));
                if (_projectedPointFound)
                {
                    results.Add(_lastProjectedPoint);
                    projsFound[i] = true;
                }
                else
                {
                    results.Add(new Point3D());
                    projsFound[i] = false;
                }
            }
            modelVis.Content = null;
            model = null;
            modelVis = null;
        }

        public static MeshGeometry3D CreateSphere(Point3D center, double radius, int slices, int stacks)
        {
            Point3DCollection pts = new Point3DCollection(1000);
            for (int stack = 0; stack <= stacks; stack++)
            {
                double phi = Math.PI / 2 - stack * Math.PI / stacks;
                double y = radius * Math.Sin(phi);
                double scale = -radius * Math.Cos(phi);

                for (int slice = 0; slice <= slices; slice++)
                {
                    double theta = slice * 2 * Math.PI / slices;
                    double x = scale * Math.Sin(theta);
                    double z = scale * Math.Cos(theta);
                    Vector3D normal = new Vector3D(x, y, z);
                    pts.Add(normal + center);
                }
            }
            Int32Collection tris = new Int32Collection(1000);
            for (int stack = 0; stack < stacks; stack++)
                for (int slice = 0; slice < slices; slice++)
                {
                    int n = slices + 1;

                    if (stack != 0)
                    {
                        tris.Add((stack + 0) * n + slice);
                        tris.Add((stack + 1) * n + slice);
                        tris.Add((stack + 0) * n + slice + 1);
                    }

                    if (stack != stack - 1)
                    {
                        tris.Add((stack + 0) * n + slice + 1);
                        tris.Add((stack + 1) * n + slice);
                        tris.Add((stack + 1) * n + slice + 1);
                    }
                }
            MeshGeometry3D plane = new MeshGeometry3D();
            plane.Positions = pts;
            plane.TriangleIndices = tris;
            return plane;
        }

        public static MeshGeometry3D CreatePlane(double w, double h, Point3D cp, Vector3D dir)
        {
            Point3DCollection pts = new Point3DCollection(8);
            pts.Add(new Point3D(-w / 2, -h / 2, 0));
            pts.Add(new Point3D(w / 2, -h / 2, 0));
            pts.Add(new Point3D(w / 2, h / 2, 0));
            pts.Add(new Point3D(-w / 2, h / 2, 0));
            Int32Collection ids = new Int32Collection(6) { 0, 1, 2, 2, 3, 0 };
            Transform3D trans =  EntityTools.Polygon3D .TransformtoCordSys(cp, dir);
            MeshGeometry3D plane = new MeshGeometry3D();
            plane.Positions = pts;
            plane.TriangleIndices = ids;
            EntityTools.Polygon3D.Transform(plane.Positions , trans);
            return plane;
        }

        public static MeshGeometry3D CreateDensifiedPlane(int resolution, double length, double width, double zPosition)
        {
            MeshGeometry3D DensifiedPlane = new MeshGeometry3D();

            for (int j = 0; j < resolution; j++)
            {
                double currentheight = -length / 2 + j * length / (resolution - 1);

                for (int i = 0; i < resolution; i++)
                {
                    DensifiedPlane.Positions.Add(new Point3D((-width / 2 + i * width / (resolution - 1)), currentheight,
                        zPosition));
                }
            }

            for (int x = 0; x < DensifiedPlane.Positions.Count - (resolution); x++)
            {
                bool skip = false;
                if ((x + 1) % resolution == 0) skip = true;

                if (skip) skip = false;
                else
                {
                    DensifiedPlane.TriangleIndices.Add(x);
                    DensifiedPlane.TriangleIndices.Add(x + resolution);
                    DensifiedPlane.TriangleIndices.Add(x + 1);

                    DensifiedPlane.TriangleIndices.Add(x + resolution);
                    DensifiedPlane.TriangleIndices.Add(x + resolution + 1);
                    DensifiedPlane.TriangleIndices.Add(x + 1);
                }

            }

            return DensifiedPlane;
        }
        



        public static MeshGeometry3D CreateWrapPlane(double w, double h, Point3D cp, Vector3D dir)
        {
            Point3DCollection pts = new Point3DCollection(8);
            pts.Add(new Point3D(-w / 2, -h / 2, 0));
            pts.Add(new Point3D(w / 2, -h / 2, 0));
            pts.Add(new Point3D());
            pts.Add(new Point3D(w / 2, h / 2, 0));
            pts.Add(new Point3D(-w / 2, h / 2, 0));



            Int32Collection ids = new Int32Collection(12) { 0, 1, 2,  1, 3, 2, 3,  4, 2, 4, 0, 2 };
            Transform3D trans = EntityTools.Polygon3D.TransformtoCordSys(cp, dir);
            MeshGeometry3D plane = new MeshGeometry3D();
            plane.Positions = pts;
            plane.TriangleIndices = ids;
            EntityTools.Polygon3D.Transform(plane.Positions, trans);
            return plane;
        }

        private static bool _projectedPointFound = false;
        private static Point3D _lastProjectedPoint;
        private static List<RayMeshGeometry3DHitTestResult> _hitTestResultList = new List<RayMeshGeometry3DHitTestResult> { };
        public static HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            if (result != null && result.GetType() == typeof(RayMeshGeometry3DHitTestResult))
            {
                RayMeshGeometry3DHitTestResult res = (RayMeshGeometry3DHitTestResult)result;
                _lastProjectedPoint = res.PointHit;
                _projectedPointFound = true;
            }
            return HitTestResultBehavior.Stop;
        }

        public static Vector3D CalculateNorms(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(
                p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            v0.Normalize();
            Vector3D v1 = new Vector3D(
                p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            v1.Normalize();
            Vector3D cp = Vector3D.CrossProduct(v0, v1);
            return cp;
        }
 
        /// <summary>
        /// Validates the specified mesh.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <returns>Validation report or null if no issues were found.</returns>
        public static string Validate(MeshGeometry3D mesh)
        {
            var sb = new StringBuilder();
            if (mesh.Normals != null && mesh.Normals.Count != 0 && mesh.Normals.Count != mesh.Positions.Count)
            {
                sb.AppendLine("Wrong number of normal vectors");
            }

            if (mesh.TextureCoordinates != null && mesh.TextureCoordinates.Count != 0
                && mesh.TextureCoordinates.Count != mesh.Positions.Count)
            {
                sb.AppendLine("Wrong number of TextureCoordinates");
            }

            if (mesh.TriangleIndices.Count % 3 != 0)
            {
                sb.AppendLine("TriangleIndices not complete");
            }

            for (int i = 0; i < mesh.TriangleIndices.Count; i++)
            {
                int index = mesh.TriangleIndices[i];
                if (index < 0 || index >= mesh.Positions.Count)
                {
                    sb.AppendFormat("Wrong index {0} in triangle {1} vertex {2}", index, i / 3, i % 3);
                    sb.AppendLine();
                }
            }

            return sb.Length > 0 ? sb.ToString() : null;
        }

        /// <summary>
        /// Cuts the mesh with the specified plane.
        /// </summary>
        /// <param name="mesh">
        /// The mesh.
        /// </param>
        /// <param name="plane">
        /// The plane origin.
        /// </param>
        /// <param name="normal">
        /// The plane normal.
        /// </param>
        /// <returns>
        /// The <see cref="MeshGeometry3D"/>.
        /// </returns>
        public static MeshGeometry3D Cut(MeshGeometry3D mesh, Point3D plane, Vector3D normal, bool cap)
        {
            var hasTextureCoordinates = mesh.TextureCoordinates != null && mesh.TextureCoordinates.Count > 0;
            var meshBuilder = new MeshBuilder(false, hasTextureCoordinates);
            var contourHelper = new ContourHelper(plane, normal, mesh, hasTextureCoordinates);
            foreach (var position in mesh.Positions)
            {
                meshBuilder.Positions.Add(position);
            }

            if (hasTextureCoordinates)
            {
                foreach (var textureCoordinate in mesh.TextureCoordinates)
                {
                    meshBuilder.TextureCoordinates.Add(textureCoordinate);
                }
            }


            var segments = new List<Point3D>();

            for (var i = 0; i < mesh.TriangleIndices.Count; i += 3)
            {
                var index0 = mesh.TriangleIndices[i];
                var index1 = mesh.TriangleIndices[i + 1];
                var index2 = mesh.TriangleIndices[i + 2];

                Point3D[] positions;
                Point[] textureCoordinates;
                int[] triangleIndices;

                contourHelper.ContourFacet(index0, index1, index2, out positions, out textureCoordinates, out triangleIndices);
   
                foreach (var p in positions)
                {
                    meshBuilder.Positions.Add(p);
                    segments.AddRange(positions);
                }

                foreach (var tc in textureCoordinates)
                {
                    meshBuilder.TextureCoordinates.Add(tc);
                }

                foreach (var ti in triangleIndices)
                {
                    meshBuilder.TriangleIndices.Add(ti);
                }
            }


            MeshGeometry3D outputMesh = meshBuilder.ToMesh();




            if (cap == true)
            {
                List<Point3DCollection > boundarycurves = CombineSegments(segments, 0.01);

                foreach (var v in boundarycurves)
                {
                    MeshGeometry3D capMesh = EntityTools.CuttingEarsTriangulator.GetCap(v, plane, normal);
                    if ( capMesh .TriangleIndices != null) EntityTools.Mesh3D.Append(outputMesh, capMesh);
                }

            }


            return outputMesh;
        }


        public static List<Point3DCollection> GetPolyCrossSection(MeshGeometry3D mesh, Point3D plane, Vector3D normal, bool cap)
        {
            var hasTextureCoordinates = mesh.TextureCoordinates != null && mesh.TextureCoordinates.Count > 0;
            var meshBuilder = new MeshBuilder(false, hasTextureCoordinates);
            var contourHelper = new ContourHelper(plane, normal, mesh, hasTextureCoordinates);
            foreach (var position in mesh.Positions)
            {
                meshBuilder.Positions.Add(position);
            }

            if (hasTextureCoordinates)
            {
                foreach (var textureCoordinate in mesh.TextureCoordinates)
                {
                    meshBuilder.TextureCoordinates.Add(textureCoordinate);
                }
            }


            var segments = new List<Point3D>();

            for (var i = 0; i < mesh.TriangleIndices.Count; i += 3)
            {
                var index0 = mesh.TriangleIndices[i];
                var index1 = mesh.TriangleIndices[i + 1];
                var index2 = mesh.TriangleIndices[i + 2];

                Point3D[] positions;
                Point[] textureCoordinates;
                int[] triangleIndices;

                contourHelper.ContourFacet(index0, index1, index2, out positions, out textureCoordinates, out triangleIndices);

                foreach (var p in positions)
                {
                    meshBuilder.Positions.Add(p);
                    segments.AddRange(positions);
                }

            }

                List<Point3DCollection> boundarycurves = CombineSegments(segments, 0.01);


                return boundarycurves;
        }


        public static MeshGeometry3D Cut(MeshGeometry3D mesh, Point3D plane, Vector3D normal, bool cap, out MeshGeometry3D outcap)
        {
            var hasTextureCoordinates = mesh.TextureCoordinates != null && mesh.TextureCoordinates.Count > 0;
            var meshBuilder = new MeshBuilder(false, hasTextureCoordinates);
            var contourHelper = new ContourHelper(plane, normal, mesh, hasTextureCoordinates);
            foreach (var position in mesh.Positions)
            {
                meshBuilder.Positions.Add(position);
            }

            if (hasTextureCoordinates)
            {
                foreach (var textureCoordinate in mesh.TextureCoordinates)
                {
                    meshBuilder.TextureCoordinates.Add(textureCoordinate);
                }
            }


            var segments = new List<Point3D>();

            for (var i = 0; i < mesh.TriangleIndices.Count; i += 3)
            {
                var index0 = mesh.TriangleIndices[i];
                var index1 = mesh.TriangleIndices[i + 1];
                var index2 = mesh.TriangleIndices[i + 2];

                Point3D[] positions;
                Point[] textureCoordinates;
                int[] triangleIndices;

                contourHelper.ContourFacet(index0, index1, index2, out positions, out textureCoordinates, out triangleIndices);

                foreach (var p in positions)
                {
                    meshBuilder.Positions.Add(p);
                    segments.AddRange(positions);
                }

                foreach (var tc in textureCoordinates)
                {
                    meshBuilder.TextureCoordinates.Add(tc);
                }

                foreach (var ti in triangleIndices)
                {
                    meshBuilder.TriangleIndices.Add(ti);
                }
            }


            MeshGeometry3D outputMesh = meshBuilder.ToMesh();


            outcap = new MeshGeometry3D();


            if (cap == true)
            {
                List<Point3DCollection> boundarycurves = CombineSegments(segments, 0.01);

                foreach (var v in boundarycurves)
                {
                    MeshGeometry3D capMesh = EntityTools.CuttingEarsTriangulator.GetCap(v, plane, normal);
                    if (capMesh.TriangleIndices != null)
                    {
                        EntityTools.Mesh3D.Append(outputMesh, capMesh);
                        EntityTools.Mesh3D.Append(outcap, capMesh);
                    }
                }

            }


            return outputMesh;
        }

        public static MeshGeometry3D Cut(MeshGeometry3D mesh, Point3D plane, Vector3D normal, bool cap, out double highestpoint)
        {
            var hasTextureCoordinates = mesh.TextureCoordinates != null && mesh.TextureCoordinates.Count > 0;
            var meshBuilder = new MeshBuilder(false, hasTextureCoordinates);
            var contourHelper = new ContourHelper(plane, normal, mesh, hasTextureCoordinates);
            foreach (var position in mesh.Positions)
            {
                meshBuilder.Positions.Add(position);
            }

            if (hasTextureCoordinates)
            {
                foreach (var textureCoordinate in mesh.TextureCoordinates)
                {
                    meshBuilder.TextureCoordinates.Add(textureCoordinate);
                }
            }


            var segments = new List<Point3D>();

            for (var i = 0; i < mesh.TriangleIndices.Count; i += 3)
            {
                var index0 = mesh.TriangleIndices[i];
                var index1 = mesh.TriangleIndices[i + 1];
                var index2 = mesh.TriangleIndices[i + 2];

                Point3D[] positions;
                Point[] textureCoordinates;
                int[] triangleIndices;

                contourHelper.ContourFacet(index0, index1, index2, out positions, out textureCoordinates, out triangleIndices);

                foreach (var p in positions)
                {
                    meshBuilder.Positions.Add(p);
                    segments.AddRange(positions);
                }

                foreach (var tc in textureCoordinates)
                {
                    meshBuilder.TextureCoordinates.Add(tc);
                }

                foreach (var ti in triangleIndices)
                {
                    meshBuilder.TriangleIndices.Add(ti);
                }
            }


            MeshGeometry3D outputMesh = meshBuilder.ToMesh();
            highestpoint = -1000;


            if (cap == true)
            {
                List<Point3DCollection> boundarycurves = CombineSegments(segments, 0.01);

                foreach (var v in boundarycurves)
                {
                    if ((KneeInnovation3D.EntityTools.PointCollections.Extents(v).Z +  KneeInnovation3D.EntityTools.PointCollections.Extents(v).SizeZ)  >  highestpoint ) highestpoint = (KneeInnovation3D.EntityTools.PointCollections.Extents(v).Z +  KneeInnovation3D.EntityTools.PointCollections.Extents(v).SizeZ);

                    MeshGeometry3D capMesh = EntityTools.CuttingEarsTriangulator.GetCap(v, plane, normal);
                    if (capMesh.TriangleIndices != null) EntityTools.Mesh3D.Append(outputMesh, capMesh);
                }

            }


            return outputMesh;
        }
        /// <summary>
        /// Gets the contour segments.
        /// </summary>
        /// <param name="mesh">
        /// The mesh.
        /// </param>
        /// <param name="plane">
        /// The plane origin.
        /// </param>
        /// <param name="normal">
        /// The plane normal.
        /// </param>
        /// <returns>
        /// The segments of the contour.
        /// </returns>
        public static IList<Point3D> GetContourSegments(MeshGeometry3D mesh, Point3D plane, Vector3D normal)
        {
            var segments = new List<Point3D>();
            var contourHelper = new ContourHelper(plane, normal, mesh);
            for (int i = 0; i < mesh.TriangleIndices.Count; i += 3)
            {
                Point3D[] positions;
                Point[] textureCoordinates;
                int[] triangleIndices;

                contourHelper.ContourFacet(
                    mesh.TriangleIndices[i],
                    mesh.TriangleIndices[i + 1],
                    mesh.TriangleIndices[i + 2],
                    out positions,
                    out textureCoordinates,
                    out triangleIndices);
                segments.AddRange(positions);
            }

            return segments;
        }
 

         //<summary>
         //Combines the segments.
         //</summary>
         //<param name="segments">
         //The segments.
         //</param>
         //<param name="eps">
         //The tolerance.
         //</param>
         //<returns>
         //Enumerated connected contour curves.
         //</returns>
        public static List<Point3DCollection > CombineSegments(IList<Point3D> segments, double eps)
        {
            // This is a simple, slow, naïve method - should be improved:
            // http://stackoverflow.com/questions/1436091/joining-unordered-line-segments
            
            List<Point3DCollection> pc = new List<Point3DCollection>();
            var curve = new Point3DCollection();
            int curveCount = 0;

            int segmentCount = segments.Count;
            int segment1 = -1, segment2 = -1;
            while (segmentCount > 0)
            {
                if (curveCount > 0)
                {
                    // Find a segment that is connected to the head of the contour
                    segment1 = FindConnectedSegment(segments, curve[0], eps);
                    if (segment1 >= 0)
                    {
                        if (segment1 % 2 == 1)
                        {
                            curve.Insert(0, segments[segment1 - 1]);
                            segments.RemoveAt(segment1 - 1);
                            segments.RemoveAt(segment1 - 1);
                        }
                        else
                        {
                            curve.Insert(0, segments[segment1 + 1]);
                            segments.RemoveAt(segment1);
                            segments.RemoveAt(segment1);
                        }

                        curveCount++;
                        segmentCount -= 2;
                    }

                    // Find a segment that is connected to the tail of the contour
                    segment2 = FindConnectedSegment(segments, curve[curveCount - 1], eps);
                    if (segment2 >= 0)
                    {
                        if (segment2 % 2 == 1)
                        {
                            curve.Add(segments[segment2 - 1]);
                            segments.RemoveAt(segment2 - 1);
                            segments.RemoveAt(segment2 - 1);
                        }
                        else
                        {
                            curve.Add(segments[segment2 + 1]);
                            segments.RemoveAt(segment2);
                            segments.RemoveAt(segment2);
                        }

                        curveCount++;
                        segmentCount -= 2;
                    }
                }

                if ((segment1 < 0 && segment2 < 0) || segmentCount == 0)
                {
                    if (curveCount > 0)
                    {
                        pc.Add (curve);
                        curve = new Point3DCollection();
                        curveCount = 0;
                    }

                    if (segmentCount > 0)
                    {
                        curve.Add(segments[0]);
                         curve.Add(segments[1]);
                        curveCount += 2;
                        segments.RemoveAt(0);
                        segments.RemoveAt(0);
                        segmentCount -= 2;
                    }
                }
            }

            return pc;
        }

        /// <summary>
        /// Combines the segments.
        /// </summary>
        /// <param name="segments">
        /// The segments.
        /// </param>
        /// <param name="eps">
        /// The tolerance.
        /// </param>
        /// <returns>
        /// Enumerated connected contour curves.
        /// </returns>
        public static List<IList<Point3D>> CombineSegmentLists(IList<Point3D> segments, double eps)
        {
            // This is a simple, slow, naïve method - should be improved:
            // http://stackoverflow.com/questions/1436091/joining-unordered-line-segments

            List<IList<Point3D>> pc = new List<IList<Point3D>>();
            var curve = new Point3DCollection();
            int curveCount = 0;

            int segmentCount = segments.Count;
            int segment1 = -1, segment2 = -1;
            while (segmentCount > 0)
            {
                if (curveCount > 0)
                {
                    // Find a segment that is connected to the head of the contour
                    segment1 = FindConnectedSegment(segments, curve[0], eps);
                    if (segment1 >= 0)
                    {
                        if (segment1 % 2 == 1)
                        {
                            curve.Insert(0, segments[segment1 - 1]);
                            segments.RemoveAt(segment1 - 1);
                            segments.RemoveAt(segment1 - 1);
                        }
                        else
                        {
                            curve.Insert(0, segments[segment1 + 1]);
                            segments.RemoveAt(segment1);
                            segments.RemoveAt(segment1);
                        }

                        curveCount++;
                        segmentCount -= 2;
                    }

                    // Find a segment that is connected to the tail of the contour
                    segment2 = FindConnectedSegment(segments, curve[curveCount - 1], eps);
                    if (segment2 >= 0)
                    {
                        if (segment2 % 2 == 1)
                        {
                            curve.Add(segments[segment2 - 1]);
                            segments.RemoveAt(segment2 - 1);
                            segments.RemoveAt(segment2 - 1);
                        }
                        else
                        {
                            curve.Add(segments[segment2 + 1]);
                            segments.RemoveAt(segment2);
                            segments.RemoveAt(segment2);
                        }

                        curveCount++;
                        segmentCount -= 2;
                    }
                }

                if ((segment1 < 0 && segment2 < 0) || segmentCount == 0)
                {
                    if (curveCount > 0)
                    {
                        pc.Add(curve);
                        curve = new Point3DCollection();
                        curveCount = 0;
                    }

                    if (segmentCount > 0)
                    {
                        curve.Add(segments[0]);
                        curve.Add(segments[1]);
                        curveCount += 2;
                        segments.RemoveAt(0);
                        segments.RemoveAt(0);
                        segmentCount -= 2;
                    }
                }
            }

            return pc;
        }

    
    
        /// <summary>
        /// Create a 64-bit key from two 32-bit indices
        /// </summary>
        /// <param name="i0">
        /// The i 0.
        /// </param>
        /// <param name="i1">
        /// The i 1.
        /// </param>
        /// <returns>
        /// The create key.
        /// </returns>
        private static ulong CreateKey(uint i0, uint i1)
        {
            return ((ulong)i0 << 32) + i1;
        }

        /// <summary>
        /// Extract two 32-bit indices from the 64-bit key
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="i0">
        /// The i 0.
        /// </param>
        /// <param name="i1">
        /// The i 1.
        /// </param>
        private static void ReverseKey(ulong key, out uint i0, out uint i1)
        {
            i0 = (uint)(key >> 32);
            i1 = (uint)((key << 32) >> 32);
        }

        /// <summary>
        /// Finds the nearest connected segment to the specified point.
        /// </summary>
        /// <param name="segments">
        /// The segments.
        /// </param>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="eps">
        /// The tolerance.
        /// </param>
        /// <returns>
        /// The index of the nearest point.
        /// </returns>
        public static int FindConnectedSegment(IList<Point3D> segments, Point3D point, double eps)
        {
            double best = eps;
            int result = -1;
            for (int i = 0; i < segments.Count; i++)
            {
                double ls0 = (point - segments[i]).LengthSquared;
                if (ls0 < best)
                {
                    result = i;
                    best = ls0;
                }
            }

            return result;
        }

        public static double GetProjectedVolume(MeshGeometry3D meshmodel)
        {
            double vol = 0;
            for (int i = 0; i < meshmodel.TriangleIndices.Count; i += 3)
            {
                double x1, x2, x3, y1, y2, y3, z1, z2, z3;
                x1 = meshmodel.Positions[meshmodel.TriangleIndices[i]].X;
                x2 = meshmodel.Positions[meshmodel.TriangleIndices[i + 1]].X;
                x3 = meshmodel.Positions[meshmodel.TriangleIndices[i + 2]].X;
                y1 = meshmodel.Positions[meshmodel.TriangleIndices[i]].Y;
                y2 = meshmodel.Positions[meshmodel.TriangleIndices[i + 1]].Y;
                y3 = meshmodel.Positions[meshmodel.TriangleIndices[i + 2]].Y;
                z1 = meshmodel.Positions[meshmodel.TriangleIndices[i]].Z;
                z2 = meshmodel.Positions[meshmodel.TriangleIndices[i + 1]].Z;
                z3 = meshmodel.Positions[meshmodel.TriangleIndices[i + 2]].Z;
                double v = (z1 + z2 + z3) * ((x1 * y2) - (x2 * y1) + (x2 * y3) - (x3 * y2) + (x3 * y1) - (x1 * y3)) / 6;
                vol += v;
            }
            return vol;
        }


        internal static void WriteMeshGeometryXml(XmlTextWriter x, MeshGeometry3D mesh)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                WriteToDatStream(mesh, ms);
                ms.Flush();
                byte[] data = ms.GetBuffer();
                x.WriteBase64(data, 0, data.Length);
            }
        }

        internal static MeshGeometry3D GetXmlMeshGeometry3D(XmlTextReader reader)
        {
            string tempfile = "";
            try
            {
                string currentNode = reader.Name;
                byte[] buffer = new byte[1000];

                tempfile = System.IO.Path.GetTempFileName();
                using (System.IO.FileStream fs = new System.IO.FileStream(tempfile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                    {
                        do
                        {
                            int cnt = reader.ReadBase64(buffer, 0, 1000);
                            bw.Write(buffer, 0, cnt);

                        } while (reader.Name == currentNode);
                    }

                    return FromDatFile (tempfile);
                }

            }
            catch
            {
                return null;
            }
            finally
            {
                if (tempfile != "" && System.IO.File.Exists(tempfile))
                    System.IO.File.Delete(tempfile);
            }
        }

        public static MeshGeometry3D FromDatFile(string filename)
        {
            if (!System.IO.File.Exists(filename)) throw new Exception("File not found.");
            BinaryReader br = null;
            try
            {
                br = new BinaryReader(System.IO.File.OpenRead(filename));
                try
                {
                    br.BaseStream.Seek(0, SeekOrigin.Begin);

                    bool headerCheck = false;
                    int headerSize = 0;
                    int nextChar;
                    while (headerCheck == false)
                    {
                        nextChar = br.Read();
                        if (nextChar == -1) throw new Exception("Invalid header in Mesh file");
                        else if (nextChar == 0)
                        {
                            headerSize++;
                            headerCheck = true;
                        }
                        else
                        {
                            headerSize++;
                            if (headerSize == 256) headerCheck = true;
                        }
                    }
                    br.BaseStream.Seek(0, SeekOrigin.Begin);
                    br.BaseStream.Position = headerSize;
                    int fileFlags = (int)br.ReadUInt32();
                    string fileVersion = (string)br.ReadString();
                    if (fileVersion != "KICOdatV1") throw new Exception("Unsupport Mesh File version.");
                    int blockCount = (int)br.ReadUInt32();

                    int fFlag = (int)br.ReadUInt32();
                    int nodeCount = (int)br.ReadUInt32();
                    int triCount = (int)br.ReadUInt32();
                    if (blockCount <= 0) throw new Exception("Invalid block count found in file.");
                    if (nodeCount <= 0) throw new Exception("Invalid node count found in file.");
                    if (triCount <= 0) throw new Exception("Invalid triangle count found in file.");
                    bool asFloat;
                    if ((fileFlags & 1) > 0)
                        asFloat = true;
                    else
                        asFloat = false;

                    int ndOffset = 0;
                    Point3DCollection points = new Point3DCollection(nodeCount);
                    Int32Collection indeces = new Int32Collection(triCount * 3);
                    Vector3DCollection normals = new Vector3DCollection(nodeCount);
                    bool hasnormals = true;
                    for (int blk = 0; blk < blockCount; blk++)
                    {
                        int blockFlags = (int)br.ReadUInt32();
                        int blockNodeCount = (int)br.ReadUInt32();
                        int blockTriCount = (int)br.ReadUInt32();
                        bool blockWithVectors;
                        if ((blockFlags & 1) > 0)
                            blockWithVectors = true;
                        else
                            blockWithVectors = false;
                        double x, y, z, I, j, k;

                        for (int n = 0; n < blockNodeCount; n++)
                        {
                            if (asFloat)
                            {
                                x = (double)br.ReadSingle();
                                y = (double)br.ReadSingle();
                                z = (double)br.ReadSingle();
                            }
                            else
                            {
                                x = (double)br.ReadDouble();
                                y = (double)br.ReadDouble();
                                z = (double)br.ReadDouble();
                            }
                            if (blockWithVectors)
                            {
                                if (asFloat)
                                {
                                    I = (double)br.ReadSingle();
                                    j = (double)br.ReadSingle();
                                    k = (double)br.ReadSingle();
                                }
                                else
                                {
                                    I = (double)br.ReadDouble();
                                    j = (double)br.ReadDouble();
                                    k = (double)br.ReadDouble();
                                }
                                normals.Add(new Vector3D(I, j, k));
                            }
                            else
                            {
                                hasnormals = false;
                            }
                            points.Add(new Point3D(x, y, z));
                        }
                        int node1Index;
                        int node2Index;
                        int node3Index;
                        for (int t = 0; t < blockTriCount; t++)
                        {
                            if (blockNodeCount < 65536)
                            {
                                node1Index = (int)br.ReadUInt16();
                                node2Index = (int)br.ReadUInt16();
                                node3Index = (int)br.ReadUInt16();
                            }
                            else
                            {
                                node1Index = (int)br.ReadUInt32();
                                node2Index = (int)br.ReadUInt32();
                                node3Index = (int)br.ReadUInt32();
                            }
                            indeces.Add(node1Index + ndOffset);
                            indeces.Add(node2Index + ndOffset);
                            indeces.Add(node3Index + ndOffset);
                        }
                        ndOffset += blockNodeCount;
                        short blockVersion = (short)br.ReadUInt16();

                    }
                    if (!hasnormals)
                    {
                        normals.Clear();
                        normals = null;
                    }

                    MeshGeometry3D model = new MeshGeometry3D();
                    model.Positions = points;
                    model.TriangleIndices = indeces;
                    if (hasnormals)
                        model.Normals = normals;
                    return model;
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Error reading Mesh file.\n{0}", e.Message));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (br != null)
                {
                    br.Close();
                    br = null;
                }
            }
        }

        public static void WriteToDatStream(MeshGeometry3D mesh, Stream fs)
        {
            try
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    string header = "MeshFile ";
                    if (header.Length > 255) header = header.Substring(0, 255);
                    header += '\0';
                    bw.Write(header);
                    bw.Write((UInt32)0);
                    bw.Write((String)"KICOdatV1");
                    bw.Write((UInt32)1);
                    bw.Write((UInt32)1);
                    bw.Write((UInt32)mesh.Positions.Count);
                    bw.Write((UInt32)(mesh.TriangleIndices.Count / 3));
                    UInt32 blockFlags = 0;
                    bw.Write(blockFlags);
                    bw.Write((UInt32)mesh.Positions.Count);
                    bw.Write((UInt32)(mesh.TriangleIndices.Count / 3));
                    bool lowNumV = (mesh.Positions.Count <= 65536);
                    for (int i = 0; i < mesh.Positions.Count; i++)
                    {
                        bw.Write(mesh.Positions[i].X);
                        bw.Write(mesh.Positions[i].Y);
                        bw.Write(mesh.Positions[i].Z);
                    }
                    for (int i = 0; i < mesh.TriangleIndices.Count; i += 3)
                    {
                        int idx1 = i;
                        int idx2 = i + 1;
                        int idx3 = i + 2;
                        if (lowNumV)
                        {
                            bw.Write((UInt16)mesh.TriangleIndices[idx1]);
                            bw.Write((UInt16)mesh.TriangleIndices[idx2]);
                            bw.Write((UInt16)mesh.TriangleIndices[idx3]);
                        }
                        else
                        {
                            bw.Write((UInt32)mesh.TriangleIndices[idx1]);
                            bw.Write((UInt32)mesh.TriangleIndices[idx2]);
                            bw.Write((UInt32)mesh.TriangleIndices[idx3]);
                        }
                    }
                    bw.Write((UInt16)1000);
                }
            }
            catch
            {
            }
        }

        internal static MeshGeometry3D FilterMeshBasedOnZmax(MeshGeometry3D inputmesh, double Zmax)
        {

            if (inputmesh == null) return null;


            Point3DCollection points = new Point3DCollection();
            Int32Collection tris = new Int32Collection();


            int count = 0;


            for (int i = 0; i < inputmesh.TriangleIndices.Count; i++)
            {
                if (inputmesh.Positions[inputmesh.TriangleIndices[i]].Z < Zmax )
                {

                    points.Add(inputmesh.Positions[inputmesh.TriangleIndices[i]]);
                    points.Add(inputmesh.Positions[inputmesh.TriangleIndices[i + 1]]);
                    points.Add(inputmesh.Positions[inputmesh.TriangleIndices[i + 2]]);

                    tris.Add(count);
                    tris.Add(count + 1);
                    tris.Add(count + 2);

                    count++;
                    count++;
                    count++;

                }

                i++;
                i++;

            }

            MeshGeometry3D outputMesh = new MeshGeometry3D();
            outputMesh.Positions = points;
            outputMesh.TriangleIndices = tris;

            KneeInnovation3D.EntityTools.MeshGeometryFunctions.Simplify(outputMesh);

            return outputMesh;


        }

        internal static MeshGeometry3D FilterMeshBasedOnZone(MeshGeometry3D inputmesh, Rect3D boundbox)
        {

            if (inputmesh == null) return null;


            Point3DCollection points = new Point3DCollection();
            Int32Collection tris = new Int32Collection();


            int count = 0;


            for (int i = 0; i < inputmesh.TriangleIndices.Count ; i++)
            {
                if (inputmesh.Positions[inputmesh.TriangleIndices[i]].X > boundbox.Location.X - (boundbox.SizeX /2) && inputmesh.Positions[inputmesh.TriangleIndices[i]].X < boundbox.Location.X + (boundbox.SizeX /2) && inputmesh.Positions[inputmesh.TriangleIndices[i]].Y > boundbox.Location.Y - (boundbox.SizeY /2) && inputmesh.Positions[inputmesh.TriangleIndices[i]].Y < boundbox.Location.Y + (boundbox.SizeY / 2) && inputmesh.Positions[inputmesh.TriangleIndices[i]].Z > boundbox.Location.Z - (boundbox.SizeZ /2) && inputmesh.Positions[inputmesh.TriangleIndices[i]].Z < boundbox.Location.Z + (boundbox.SizeZ / 2))
                {

                    points.Add(inputmesh.Positions[inputmesh.TriangleIndices[i]]);
                    points.Add(inputmesh.Positions[inputmesh.TriangleIndices[i + 1]]);
                    points.Add(inputmesh.Positions[inputmesh.TriangleIndices[i + 2]]);

                    tris.Add(count);
                    tris.Add(count + 1);
                    tris.Add(count + 2);

                    count++;
                    count++;
                    count++;

                }

                i++;
                i++;
        
            }

            MeshGeometry3D outputMesh = new MeshGeometry3D();
            outputMesh.Positions = points;
            outputMesh.TriangleIndices = tris;

            KneeInnovation3D.EntityTools.MeshGeometryFunctions.Simplify(outputMesh);

            return outputMesh;


        }

        internal static Point3D Barycentric(Point3D a, Point3D b, Point3D c)
        {

           Point3D mp =  ViewPortTools.MathUtils.GetMidPoint(a, b);
           Point3D mp2 = ViewPortTools.MathUtils.GetMidPoint(c, mp);

            return mp2;
        }

        internal static List<double> CalculateDistanceBetweenMeshesWithFilter(MeshGeometry3D ScannedMesh, MeshGeometry3D TargetMesh)
        {
            MeshGeometry3D filteredMesh = MeshGeometryFunctions.FilterMeshBasedOnZone(TargetMesh, ScannedMesh.Bounds);
            MeshGeometry3D reversedMesh = filteredMesh.Clone();
            Mesh3D.ReverseNormals(reversedMesh);

            List<double> TotalError = new List<double>(ScannedMesh.TriangleIndices.Count);
            for (int jj = 0; jj < TotalError.Capacity; jj++)
            {
                TotalError.Add(0);
            }

            List<double> ErrorList = new List<double>();

            for (int i = 0; i < ScannedMesh.TriangleIndices.Count; i++)
            {

                Vector3D vN =  MeshGeometryFunctions.CalculateNorms(ScannedMesh.Positions[ScannedMesh.TriangleIndices[i]],
                    ScannedMesh.Positions[ScannedMesh.TriangleIndices[i + 1]],
                    ScannedMesh.Positions[ScannedMesh.TriangleIndices[i + 2]]);

                Point3D bcP = Barycentric(ScannedMesh.Positions[ScannedMesh.TriangleIndices[i]],
                    ScannedMesh.Positions[ScannedMesh.TriangleIndices[i + 1]],
                    ScannedMesh.Positions[ScannedMesh.TriangleIndices[i + 2]]);

                Point3DCollection pc;
                bool tempbool = false;
                KneeInnovation3D.EntityTools.MeshGeometryFunctions.GetLineProjection(reversedMesh , bcP, vN , out pc, out tempbool);

                if (tempbool == false)
                {
                    KneeInnovation3D.EntityTools.MeshGeometryFunctions.GetLineProjection(filteredMesh, bcP, vN * -1, out pc, out tempbool);
                    
                }
              //MeshGeometryFunctions.GetLineProjection(CTScan, ScannedMesh.Positions[i], ScannedMeshNorms[i] * -1, out temp, out tempbool);
             if (tempbool)
             {
                 double dist = bcP.DistanceTo(pc[0]);



                 TotalError[ScannedMesh.TriangleIndices[i]] = dist;
                 TotalError[ScannedMesh.TriangleIndices[i + 1]] = dist;
                 TotalError[ScannedMesh.TriangleIndices[i + 2]] = dist;


             }
             else
             {
                 TotalError[ScannedMesh.TriangleIndices[i]] = 100;
                 TotalError[ScannedMesh.TriangleIndices[i + 1]] = 100;
                 TotalError[ScannedMesh.TriangleIndices[i + 2]] = 100;
             }

                i++;
                i++;
            }


            return TotalError;
        }

        internal static List<double> CalculateNormalErrorBetweenMeshes(MeshGeometry3D ScannedMesh, MeshGeometry3D CTScan)
        {
            //List<double> TotalError = new List<double>();


           MeshGeometry3D filteredmesh =  MeshGeometryFunctions.FilterMeshBasedOnZone(CTScan, ScannedMesh.Bounds);
            List<double> TotalError = new List<double>(ScannedMesh.TriangleIndices.Count);
            for (int jj = 0; jj < TotalError.Capacity; jj++)
            {
                TotalError.Add(0);
            }


            Vector3DCollection ScannedMeshNorms = MeshGeometryFunctions.CalculateNormals(ScannedMesh.Positions, ScannedMesh.TriangleIndices);
       
            MeshGeometry3D reversemesh = CTScan.Clone();
           
            Mesh3D.ReverseNormals(reversemesh);

            //MeshGeometry3D CTScanF = CTScan.Clone();
            //MeshGeometry3D ScannedMeshF = ScannedMesh.Clone();
            //MeshGeometry3D reversemeshF = reversemesh.Clone();
            //Vector3DCollection ScannedMeshNormsF = ScannedMeshNorms.Clone();

            //CTScanF.Freeze();
            //ScannedMeshF.Freeze();
            //reversemeshF.Freeze();
            //ScannedMeshNormsF.Freeze();

            //ConcurrentDictionary<int, double> Results = new ConcurrentDictionary<int, double>();
            //int count = -1;

            //Parallel.For(0, ScannedMeshF.Positions.Count, ctr =>
            //{
            //    int t = Interlocked.Increment(ref count);

            //    Point3DCollection temp = new Point3DCollection();
            //    bool tempbool;

            //    GetLineProjection(reversemeshF, ScannedMeshF.Positions[t], ScannedMeshNormsF[t], out temp, out tempbool);


            //    if (tempbool)
            //    {
            //        double dist = -1 * ScannedMeshF.Positions[t].DistanceTo(temp[0]);


            //        for (int p = 0; p < ScannedMeshF.TriangleIndices.Count - 2; p += 3)
            //        {
            //            if (ScannedMeshF.TriangleIndices[p] == t)
            //            {
            //                Results.GetOrAdd(p, dist);
            //                Results.GetOrAdd(p + 1, dist);
            //                Results.GetOrAdd(p + 2, dist);

            //            }
            //        }
            //    }
            //    else
            //    {
            //        GetLineProjection(CTScanF, ScannedMeshF.Positions[t], ScannedMeshNormsF[t] * -1, out temp,
            //            out tempbool);

            //        if (tempbool)
            //        {
            //            double dist = ScannedMeshF.Positions[t].DistanceTo(temp[0]);

            //            for (int p = 0; p < ScannedMeshF.TriangleIndices.Count - 2; p += 3)
            //            {
            //                if (ScannedMeshF.TriangleIndices[p] == t)
            //                {
            //                    Results.GetOrAdd(p, dist);
            //                    Results.GetOrAdd(p + 1, dist);
            //                    Results.GetOrAdd(p + 2, dist);
            //                }
            //            }
            //        }
            //    }
            //});



            for (int i = 0; i < ScannedMesh.Positions.Count; i++)
            {
                Point3DCollection temp = new Point3DCollection();
                bool tempbool;
                KneeInnovation3D.EntityTools.MeshGeometryFunctions.GetLineProjection(reversemesh, ScannedMesh.Positions[i], ScannedMeshNorms[i], out temp, out tempbool);
                if (tempbool)
                {
                    double dist = -1 * ScannedMesh.Positions[i].DistanceTo(temp[0]);

                    if (dist < -10 || dist > 10)
                    {
                        string s = "stop";
                    }

                    for (int p = 0; p < ScannedMesh.TriangleIndices.Count - 2; p += 3)
                    {
                        if (ScannedMesh.TriangleIndices[p] == i)
                        {
                            TotalError[p] = dist;
                            TotalError[p + 1] = dist;
                            TotalError[p + 2] = dist;
                        }
                    }
                }
                else
                {
                    MeshGeometryFunctions.GetLineProjection(CTScan, ScannedMesh.Positions[i], ScannedMeshNorms[i] * -1, out temp, out tempbool);
                    if (tempbool)
                    {
                        double dist = ScannedMesh.Positions[i].DistanceTo(temp[0]);

                        if (dist < -10 || dist > 10)
                        {
                            string s = "stop";
                        }

                        for (int p = 0; p < ScannedMesh.TriangleIndices.Count - 2; p += 3)
                        {
                            if (ScannedMesh.TriangleIndices[p] == i)
                            {
                                TotalError[p] = dist;
                                TotalError[p + 1] = dist;
                                TotalError[p + 2] = dist;
                            }
                        }
                    }
                    else
                    {
                        for (int p = 0; p < ScannedMesh.TriangleIndices.Count - 2; p += 3)
                        {
                            if (ScannedMesh.TriangleIndices[p] == i)
                            {
                                TotalError[p] = 100;
                                TotalError[p + 1] = 100;
                                TotalError[p + 2] = 100;
                            }
                        }
                    }
                }
               


                {
                    
                }

            }

         
            //Results.OrderBy(x => x.Key);
            //TotalError.AddRange(Results.Values);

           //System.Windows.MessageBox.Show("Total loop time:" + totalsecondloop.ToString(), "the time", MessageBoxButton.OK,MessageBoxImage.Hand,MessageBoxResult.OK,MessageBoxOptions.DefaultDesktopOnly);

            return TotalError;


        }
    }
}