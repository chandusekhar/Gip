//---------------------------------------------------------------------------
//
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Limited Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/limitedpermissivelicense.mspx
// All other rights reserved.
//
// This file is part of the 3D Tools for Windows Presentation Foundation
// project.  For more information, see:
// 
// http://CodePlex.com/Wiki/View.aspx?ProjectName=3DTools
//
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Linq;

namespace ViewPortTools
{
    #region Mesh IValueConverters

    /// <summary>
    ///     Abstract base class for all type converters that have a MeshGeometry3D source
    /// </summary>
    public abstract class MeshConverter<TArgetType> : IValueConverter
    {
        public MeshConverter()
        {
        }

        /// <summary>
        ///     IValueConverter.Convert
        /// </summary>
        /// <param name="value">The binding source (should be a MeshGeometry3D)</param>
        /// <param name="targetType">The binding target</param>
        /// <param name="parameter">Optionaly parameter to the converter</param>
        /// <param name="culture">(ignored)</param>
        /// <returns>The converted value</returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (targetType != typeof(TArgetType))
            {
                throw new ArgumentException(String.Format("MeshConverter must target a {0}", typeof(TArgetType).Name));
            }

            MeshGeometry3D mesh = value as MeshGeometry3D;
            if (mesh == null)
            {
                throw new ArgumentException("MeshConverter can only convert from a MeshGeometry3D");
            }

            return Convert(mesh, parameter);
        }

        /// <summary>
        ///     IValueConverter.ConvertBack
        /// 
        ///     Not implemented
        /// </summary>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Subclasses should override this to do conversion
        /// </summary>
        /// <param name="mesh">The mesh source</param>
        /// <param name="parameter">Optional converter argument</param>
        /// <returns>The converted value</returns>
        public abstract object Convert(MeshGeometry3D mesh, object parameter);
    }

    /// <summary>
    /// MeshTextureCoordinateConverter
    /// 
    /// A MeshConverter that returns a PointCollection and takes an optional direction argument
    /// </summary>
    public abstract class MeshTextureCoordinateConverter : MeshConverter<PointCollection>
    {
        public MeshTextureCoordinateConverter()
        {
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="mesh">The source mesh</param>
        /// <param name="parameter">The optional parameter</param>
        /// <returns>The converted value</returns>
        public override object Convert(MeshGeometry3D mesh, object parameter)
        {
            string paramAsString = parameter as string;
            if (parameter != null && paramAsString == null)
            {
                throw new ArgumentException("Parameter must be a string.");
            }

            // Default to the positive Y axis
            Vector3D dir = MathUtils.YAxis;

            if (paramAsString != null)
            {
                dir = Vector3D.Parse(paramAsString);
                MathUtils.TryNormalize(ref dir);
            }

            return Convert(mesh, dir);
        }

        /// <summary>
        ///     Subclasses should override this to do conversion
        /// </summary>
        /// <param name="mesh">The source mesh</param>
        /// <param name="dir">The normalized direction parameter</param>
        /// <returns></returns>
        public abstract object Convert(MeshGeometry3D mesh, Vector3D dir);
    }

    /// <summary>
    ///     IValueConverter that generates texture coordinates for a plane.
    /// </summary>
    public class PlanarTextureCoordinateGenerator : MeshTextureCoordinateConverter
    {
        public PlanarTextureCoordinateGenerator()
        {
        }

        public override object Convert(MeshGeometry3D mesh, Vector3D dir)
        {
            return MeshUtils.GeneratePlanarTextureCoordinates(mesh, dir);
        }
    }

    /// <summary>
    ///     IValueConverter that generates texture coordinates for a sphere.
    /// </summary>
    public class SphericalTextureCoordinateGenerator : MeshTextureCoordinateConverter
    {
        public SphericalTextureCoordinateGenerator()
        {
        }

        public override object Convert(MeshGeometry3D mesh, Vector3D dir)
        {
            return MeshUtils.GenerateSphericalTextureCoordinates(mesh, dir);
        }
    }

    /// <summary>
    ///     IValueConverter that generates texture coordinates for a cylinder
    /// </summary>
    public class CylindricalTextureCoordinateGenerator : MeshTextureCoordinateConverter
    {
        public CylindricalTextureCoordinateGenerator()
        {
        }

        public override object Convert(MeshGeometry3D mesh, Vector3D dir)
        {
            return MeshUtils.GenerateCylindricalTextureCoordinates(mesh, dir);
        }
    }

    #endregion

    public static class MeshUtils
    {
        #region Texture Coordinate Generation Methods

        /// <summary>
        ///     Generates texture coordinates as if mesh were a cylinder.
        /// 
        ///     Notes: 
        ///         1) v is flipped for you automatically
        ///         2) 'mesh' is not modified. If you want the generated coordinates
        ///            to be assigned to mesh, do:
        /// 
        ///            mesh.TextureCoordinates = GenerateCylindricalTextureCoordinates(mesh, foo)
        /// 
        /// </summary>
        /// <param name="mesh">The mesh</param>
        /// <param name="dir">The axis of rotation for the cylinder</param>
        /// <returns>The generated texture coordinates</returns>
        public static PointCollection GenerateCylindricalTextureCoordinates(MeshGeometry3D mesh, Vector3D dir)
        {
            if (mesh == null)
            {
                return null;
            }

            Rect3D bounds = mesh.Bounds;
            int count = mesh.Positions.Count;
            PointCollection texcoords = new PointCollection(count);
            IEnumerable<Point3D> positions = TransformPoints(ref bounds, mesh.Positions, ref dir);

            foreach (Point3D vertex in positions)
            {
                texcoords.Add(new Point(
                    GetUnitCircleCoordinate(-vertex.Z, vertex.X),
                    1.0 - GetPlanarCoordinate(vertex.Y, bounds.Y, bounds.SizeY)
                    ));
            }

            return texcoords;
        }

        /// <summary>
        ///     Generates texture coordinates as if mesh were a sphere.
        /// 
        ///     Notes: 
        ///         1) v is flipped for you automatically
        ///         2) 'mesh' is not modified. If you want the generated coordinates
        ///            to be assigned to mesh, do:
        /// 
        ///            mesh.TextureCoordinates = GenerateSphericalTextureCoordinates(mesh, foo)
        /// 
        /// </summary>
        /// <param name="mesh">The mesh</param>
        /// <param name="dir">The axis of rotation for the sphere</param>
        /// <returns>The generated texture coordinates</returns>
        public static PointCollection GenerateSphericalTextureCoordinates(MeshGeometry3D mesh, Vector3D dir)
        {
            if (mesh == null)
            {
                return null;
            }

            Rect3D bounds = mesh.Bounds;
            int count = mesh.Positions.Count;
            PointCollection texcoords = new PointCollection(count);
            IEnumerable<Point3D> positions = TransformPoints(ref bounds, mesh.Positions, ref dir);

            foreach (Point3D vertex in positions)
            {
                // Don't need to do 'vertex - center' since TransformPoints put us
                // at the origin
                Vector3D radius = new Vector3D(vertex.X, vertex.Y, vertex.Z);
                MathUtils.TryNormalize(ref radius);

                texcoords.Add(new Point(
                    GetUnitCircleCoordinate(-radius.Z, radius.X),
                    1.0 - (Math.Asin(radius.Y) / Math.PI + 0.5)
                    ));
            }

            return texcoords;
        }

        /// <summary>
        ///     Generates texture coordinates as if mesh were a plane.
        /// 
        ///     Notes: 
        ///         1) v is flipped for you automatically
        ///         2) 'mesh' is not modified. If you want the generated coordinates
        ///            to be assigned to mesh, do:
        /// 
        ///            mesh.TextureCoordinates = GeneratePlanarTextureCoordinates(mesh, foo)
        /// 
        /// </summary>
        /// <param name="mesh">The mesh</param>
        /// <param name="dir">The normal of the plane</param>
        /// <returns>The generated texture coordinates</returns>
        public static PointCollection GeneratePlanarTextureCoordinates(MeshGeometry3D mesh, Vector3D dir)
        {
            if (mesh == null)
            {
                return null;
            }

            Rect3D bounds = mesh.Bounds;
            int count = mesh.Positions.Count;
            PointCollection texcoords = new PointCollection(count);
            IEnumerable<Point3D> positions = TransformPoints(ref bounds, mesh.Positions, ref dir);

            foreach (Point3D vertex in positions)
            {
                // The plane is looking along positive Y, so Z is really Y

                texcoords.Add(new Point(
                    GetPlanarCoordinate(vertex.X, bounds.X, bounds.SizeX),
                    GetPlanarCoordinate(vertex.Z, bounds.Z, bounds.SizeZ)
                    ));
            }

            return texcoords;
        }

        #endregion

        internal static double GetPlanarCoordinate(double end, double start, double width)
        {
            return (end - start) / width;
        }

        internal static double GetUnitCircleCoordinate(double y, double x)
        {
            return Math.Atan2(y, x) / (2.0 * Math.PI) + 0.5;
        }

        /// <summary>
        ///     Finds the transform from 'dir' to '<0, 1, 0>' and transforms 'bounds'
        ///     and 'points' by it.
        /// </summary>
        /// <param name="bounds">The bounds to transform</param>
        /// <param name="points">The vertices to transform</param>
        /// <param name="dir">The orientation of the mesh</param>
        /// <returns>
        ///     The transformed points. If 'dir' is already '<0, 1, 0>' then this
        ///     will equal 'points.'
        /// </returns>
        internal static IEnumerable<Point3D> TransformPoints(ref Rect3D bounds, Point3DCollection points, ref Vector3D dir)
        {
            if (dir == MathUtils.YAxis)
            {
                return points;
            }

            Vector3D rotAxis = Vector3D.CrossProduct(dir, MathUtils.YAxis);
            double rotAngle = Vector3D.AngleBetween(dir, MathUtils.YAxis);
            Quaternion q;

            if (rotAxis.X != 0 || rotAxis.Y != 0 || rotAxis.Z != 0)
            {
                Debug.Assert(rotAngle != 0);

                q = new Quaternion(rotAxis, rotAngle);
            }
            else
            {
                Debug.Assert(dir == -MathUtils.YAxis);

                q = new Quaternion(MathUtils.XAxis, rotAngle);
            }

            Vector3D center = new Vector3D(
                bounds.X + bounds.SizeX / 2,
                bounds.Y + bounds.SizeY / 2,
                bounds.Z + bounds.SizeZ / 2
                );

            Matrix3D t = Matrix3D.Identity;
            t.Translate(-center);
            t.Rotate(q);

            int count = points.Count;
            Point3D[] transformedPoints = new Point3D[count];

            for (int i = 0; i < count; i++)
            {
                transformedPoints[i] = t.Transform(points[i]);
            }

            // Finally, transform the bounds too
            bounds = MathUtils.TransformBounds(bounds, t);

            return transformedPoints;
        }


        public static List<Point3D> FindAdjacentNeighbors(Point3D[] v, int[] t, Point3D vertex)
        {
            List<Point3D> adjacentV = new List<Point3D>();
            List<int> facemarker = new List<int>();
            int facecount = 0;

            // Find matching vertices
            for (int i = 0; i < v.Length; i++)
                if (Math.Round(vertex.X, 2) == Math.Round(v[i].X, 2) &&
                    Math.Round(vertex.Y, 2) == Math.Round(v[i].Y, 2) &&
                    Math.Round(vertex.Z, 2) == Math.Round(v[i].Z, 2))
                {
                    int v1 = 0;
                    int v2 = 0;
                    bool marker = false;

                    // Find vertex indices from the triangle array
                    for (int k = 0; k < t.Length - 3; k = k + 3)
                        if (facemarker.Contains(k) == false)
                        {
                            v1 = 0;
                            v2 = 0;
                            marker = false;

                            if (i == t[k])
                            {
                                v1 = t[k + 1];
                                v2 = t[k + 2];
                                marker = true;
                            }

                            if (i == t[k + 1])
                            {
                                v1 = t[k];
                                v2 = t[k + 2];
                                marker = true;
                            }

                            if (i == t[k + 2])
                            {
                                v1 = t[k];
                                v2 = t[k + 1];
                                marker = true;
                            }

                            facecount++;
                            if (marker)
                            {
                                // Once face has been used mark it so it does not get used again
                                facemarker.Add(k);

                                // Add non duplicate vertices to the list
                                try
                                {
                                    if (IsVertexExist(adjacentV, v[v1]) == false)
                                    {
                                        adjacentV.Add(v[v1]);
                                        //Debug.Log("Adjacent vertex index = " + v1);
                                    }

                                    if (v2 < v.Length - 1)
                                    {
                                        if (IsVertexExist(adjacentV, v[v2]) == false)
                                        {
                                            adjacentV.Add(v[v2]);
                                            //Debug.Log("Adjacent vertex index = " + v2);
                                        }
                                    }
                                }
                                catch
                                { }
                                marker = false;
                            }
                        }
                }

            //Debug.Log("Faces Found = " + facecount);

            return adjacentV;
        }


        // Finds a set of adjacent vertices indexes for a given vertex
        // Note the success of this routine expects only the set of neighboring faces to eacn contain one vertex corresponding
        // to the vertex in question
        public static List<int> FindAdjacentNeighborIndexes(Point3D[] v, int[] t, Point3D vertex)
        {
            List<int> adjacentIndexes = new List<int>();
            List<Point3D> adjacentV = new List<Point3D>();
            List<int> facemarker = new List<int>();
            int facecount = 0;

            // Find matching vertices
            for (int i = 0; i < v.Length; i++)
                if (Math.Round(vertex.X, 2) == Math.Round(v[i].X, 2) &&
                    Math.Round(vertex.Y, 2) == Math.Round(v[i].Y, 2) &&
                    Math.Round(vertex.Z, 2) == Math.Round(v[i].Z, 2))
                {
                    int v1 = 0;
                    int v2 = 0;
                    bool marker = false;

                    // Find vertex indices from the triangle array
                    for (int k = 0; k < t.Length - 3; k = k + 3)
                        if (facemarker.Contains(k) == false)
                        {
                            v1 = 0;
                            v2 = 0;
                            marker = false;

                            if (i == t[k])
                            {
                                v1 = t[k + 1];
                                v2 = t[k + 2];
                                marker = true;
                            }

                            if (i == t[k + 1])
                            {
                                v1 = t[k];
                                v2 = t[k + 2];
                                marker = true;
                            }

                            if (i == t[k + 2])
                            {
                                v1 = t[k];
                                v2 = t[k + 1];
                                marker = true;
                            }

                            facecount++;
                            if (marker)
                            {
                                // Once face has been used mark it so it does not get used again
                                facemarker.Add(k);

                                // Add non duplicate vertices to the list
                                if (IsVertexExist(adjacentV, v[v1]) == false)
                                {
                                    adjacentV.Add(v[v1]);
                                    adjacentIndexes.Add(v1);
                                    //Debug.Log("Adjacent vertex index = " + v1);
                                }

                                if (v2 < v.Length - 1)
                                {
                                    if (IsVertexExist(adjacentV, v[v2]) == false)
                                    {
                                        adjacentV.Add(v[v2]);
                                        adjacentIndexes.Add(v2);
                                        //Debug.Log("Adjacent vertex index = " + v2);
                                    }
                                    marker = false;
                                }
                            }
                        }
                }

            //Debug.Log("Faces Found = " + facecount);

            return adjacentIndexes;
        }

        // Does the vertex v exist in the list of vertices
        static bool IsVertexExist(List<Point3D> adjacentV, Point3D v)
        {
            bool marker = false;
            foreach (Point3D vec in adjacentV)
                if (Math.Round(vec.X, 2) == Math.Round(v.X, 2) && Math.Round(vec.Y, 2) == Math.Round(v.Y, 2) && Math.Round(vec.Z, 2) == Math.Round(v.Z, 2))
                {
                    marker = true;
                    break;
                }



            return marker;
        }


        public static Int32Collection[] GetConnectedMeshRegionsUsingSeadPoint(MeshGeometry3D mesh, int SeadIndex)
        {
            Int32Collection[] ConnectedList = new Int32Collection[mesh.Positions.Count];
            Dictionary<int, int> TriIndices = new Dictionary<int, int> { };
            for (int i = 0; i < mesh.TriangleIndices.Count; i++)
                TriIndices.Add(i, mesh.TriangleIndices[i]);

            var SortedGroups = from o in TriIndices group o by o.Value;

            //var SeadedIndex = from KeyValuePair<int, int> Connections in SortedGroups
            //    where Connections.Key .Equals(0)
            //    select Connections;


            foreach (var vGroup in SortedGroups)
            {
                foreach (var v in vGroup)
                {


                    int idx = v.Value;
                    if (ConnectedList[idx] == null)
                        ConnectedList[idx] = new Int32Collection();




                    int Face = 3 * (v.Key / 3);
                    int Indices1 = Face;
                    int Indices2 = Face + 1;
                    int Indices3 = Face + 2;

                    bool alreadyInList = false;
                    if (ConnectedList[idx] != null && ConnectedList[idx].Count != 0)
                    {
                        int[] tri = new int[3];
                        tri[0] = Indices1;
                        tri[1] = Indices2;
                        tri[2] = Indices3;
                        for (int i = 0; i < ConnectedList[idx].Count; i++)
                        {
                            if (tri[0] == (ConnectedList[idx][i]) && tri[1] == (ConnectedList[idx][i + 1]) && tri[2] == (ConnectedList[idx][i + 2]))
                            {
                                alreadyInList = true;
                            }

                            i++;
                            i++;
                        }
                    }
                    if (alreadyInList == false)
                    {
                        ConnectedList[idx].Add(Face);
                        ConnectedList[idx].Add(Face + 1);
                        ConnectedList[idx].Add(Face + 2);
                    }
                }
            }
            TriIndices = null;
            return ConnectedList;
        }

    }
}
