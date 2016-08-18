
namespace KneeInnovation3D.EntityTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media.Media3D;

  
    internal class ContourHelper
    {
        /// <summary>
        /// Provides the indices for the various <see cref="ContourFacetResult"/> cases.
        /// </summary>
        private static readonly IDictionary<ContourFacetResult, int[,]> ResultIndices
            = new Dictionary<ContourFacetResult, int[,]>
        {
            { ContourFacetResult.ZeroOnly, new[,] { { 0, 1 }, { 0, 2 } } },
            { ContourFacetResult.OneAndTwo, new[,] { { 0, 2 }, { 0, 1 } } },
            { ContourFacetResult.OneOnly, new[,] { { 1, 2 }, { 1, 0 } } },
            { ContourFacetResult.ZeroAndTwo, new[,] { { 1, 0 }, { 1, 2 } } },
            { ContourFacetResult.TwoOnly, new[,] { { 2, 0 }, { 2, 1 } } },
            { ContourFacetResult.ZeroAndOne, new[,] { { 2, 1 }, { 2, 0 } } },
        };

        /// <summary>
        /// The parameter 'a' of the plane equation.
        /// </summary>
        private readonly double _a;

        /// <summary>
        /// The parameter 'b' of the plane equation.
        /// </summary>
        private readonly double _b;

        /// <summary>
        /// The parameter 'c' of the plane equation.
        /// </summary>
        private readonly double _c;

        /// <summary>
        /// The parameter 'd' of the plane equation.
        /// </summary>
        private readonly double _d;

        /// <summary>
        /// The sides.
        /// </summary>
        private readonly double[] _sides = new double[3];

        /// <summary>
        /// The indices.
        /// </summary>
        private readonly int[] _indices = new int[3];

        /// <summary>
        /// Indicates whether the mesh uses texture coordinates.
        /// </summary>
        private readonly bool _hasTextureCoordinates;

        /// <summary>
        /// The original mesh positions.
        /// </summary>
        private readonly Point3D[] _meshPositions;

        /// <summary>
        /// The original mesh texture coordinates.
        /// </summary>
        private readonly Point[] _meshTextureCoordinates;

        /// <summary>
        /// The points.
        /// </summary>
        private readonly Point3D[] _points = new Point3D[3];

        /// <summary>
        /// The textures.
        /// </summary>
        private readonly Point[] _textures = new Point[3];

        /// <summary>
        /// The position count.
        /// </summary>
        private int _positionCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContourHelper"/> class.
        /// </summary>
        /// <param name="planeOrigin">
        /// The plane origin.
        /// </param>
        /// <param name="planeNormal">
        /// The plane normal.
        /// </param>
        /// <param name="originalMesh">
        /// The original mesh.
        /// </param>
        /// <param name="hasTextureCoordinates">
        /// Indicates whether texture coordinates need calculating.
        /// </param>
        public ContourHelper(Point3D planeOrigin, Vector3D planeNormal, MeshGeometry3D originalMesh, bool hasTextureCoordinates = false)
        {
            this._hasTextureCoordinates = hasTextureCoordinates;
            this._positionCount = originalMesh.Positions.Count;

            this._meshPositions = originalMesh.Positions.ToArray();
            this._meshTextureCoordinates = originalMesh.TextureCoordinates.ToArray();

            // Determine the equation of the plane as
            // ax + by + cz + d = 0
            var l = Math.Sqrt((planeNormal.X * planeNormal.X) + (planeNormal.Y * planeNormal.Y) + (planeNormal.Z * planeNormal.Z));
            this._a = planeNormal.X / l;
            this._b = planeNormal.Y / l;
            this._c = planeNormal.Z / l;
            this._d = -((planeNormal.X * planeOrigin.X) + (planeNormal.Y * planeOrigin.Y) + (planeNormal.Z * planeOrigin.Z));
        }

        /// <summary>
        /// The contour facet result.
        /// </summary>
        private enum ContourFacetResult
        {
            /// <summary>
            /// All of the points fall above the contour plane.
            /// </summary>
            None,

            /// <summary>
            /// Only the 0th point falls below the contour plane.
            /// </summary>
            ZeroOnly,

            /// <summary>
            /// The 1st and 2nd points fall below the contour plane.
            /// </summary>
            OneAndTwo,

            /// <summary>
            /// Only the 1st point falls below the contour plane.
            /// </summary>
            OneOnly,

            /// <summary>
            /// The 0th and 2nd points fall below the contour plane.
            /// </summary>
            ZeroAndTwo,

            /// <summary>
            /// Only the second point falls below the contour plane.
            /// </summary>
            TwoOnly,

            /// <summary>
            /// The 0th and 1st points fall below the contour plane.
            /// </summary>
            ZeroAndOne,

            /// <summary>
            /// All of the points fall below the contour plane.
            /// </summary>
            All
        }

        /// <summary>
        /// Create a contour slice through a 3 vertex facet.
        /// </summary>
        /// <param name="index0">
        /// The 0th point index.
        /// </param>
        /// <param name="index1">
        /// The 1st point index.
        /// </param>
        /// <param name="index2">
        /// The 2nd point index.
        /// </param>
        /// <param name="positions">
        /// Any new positions that are created, when the contour plane slices through the vertex.
        /// </param>
        /// <param name="textureCoordinates">
        /// Any new texture coordinates that are created, when the contour plane slices through the vertex.
        /// </param>
        /// <param name="triangleIndices">
        /// All triangle indices that are created, when 1 or more points fall below the contour plane.
        /// </param>
        public void ContourFacet(
            int index0,
            int index1,
            int index2,
            out Point3D[] positions,
            out Point[] textureCoordinates,
            out int[] triangleIndices)
        {
            this.SetData(index0, index1, index2);

            var facetResult = this.GetContourFacet();
            switch (facetResult)
            {
                case ContourFacetResult.ZeroOnly:
                    triangleIndices = new[] { index0, this._positionCount++, this._positionCount++ };
                    break;
                case ContourFacetResult.OneAndTwo:
                    triangleIndices = new[] { index1, index2, this._positionCount, this._positionCount++, this._positionCount++, index1 };
                    break;
                case ContourFacetResult.OneOnly:
                    triangleIndices = new[] { index1, this._positionCount++, this._positionCount++ };
                    break;
                case ContourFacetResult.ZeroAndTwo:
                    triangleIndices = new[] { index2, index0, this._positionCount, this._positionCount++, this._positionCount++, index2 };
                    break;
                case ContourFacetResult.TwoOnly:
                    triangleIndices = new[] { index2, this._positionCount++, this._positionCount++ };
                    break;
                case ContourFacetResult.ZeroAndOne:
                    triangleIndices = new[] { index0, index1, this._positionCount, this._positionCount++, this._positionCount++, index0 };
                    break;
                case ContourFacetResult.All:
                    positions = new Point3D[0];
                    textureCoordinates = new Point[0];
                    triangleIndices = new[] { index0, index1, index2 };
                    return;
                default:
                    positions = new Point3D[0];
                    textureCoordinates = new Point[0];
                    triangleIndices = new int[0];
                    return;
            }

            var facetIndices = ResultIndices[facetResult];
            positions = new[]
            {
                this.CreateNewPosition(facetIndices[0, 0], facetIndices[0, 1]),
                this.CreateNewPosition(facetIndices[1, 0], facetIndices[1, 1])
            };
            if (this._hasTextureCoordinates)
            {
                textureCoordinates = new[]
                {
                    this.CreateNewTexture(facetIndices[0, 0], facetIndices[0, 1]),
                    this.CreateNewTexture(facetIndices[1, 0], facetIndices[1, 1])
                };
            }
            else
            {
                textureCoordinates = new Point[0];
            }
        }

        /// <summary>
        /// Calculates a new point coordinate.
        /// </summary>
        /// <param name="firstPoint">
        /// The first point coordinate.
        /// </param>
        /// <param name="secondPoint">
        /// The second point coordinate.
        /// </param>
        /// <param name="firstSide">
        /// The first side.
        /// </param>
        /// <param name="secondSide">
        /// The second side.
        /// </param>
        /// <returns>The new coordinate.</returns>
        private static double CalculatePoint(double firstPoint, double secondPoint, double firstSide, double secondSide)
        {
            return firstPoint - (firstSide * (secondPoint - firstPoint) / (secondSide - firstSide));
        }

        /// <summary>
        /// Gets the <see cref="ContourFacetResult"/> for the current facet.
        /// </summary>
        /// <returns>a facet result.</returns>
        private ContourFacetResult GetContourFacet()
        {
            if (this.IsSideAlone(0))
            {
                return this._sides[0] > 0 ? ContourFacetResult.ZeroOnly : ContourFacetResult.OneAndTwo;
            }

            if (this.IsSideAlone(1))
            {
                return this._sides[1] > 0 ? ContourFacetResult.OneOnly : ContourFacetResult.ZeroAndTwo;
            }

            if (this.IsSideAlone(2))
            {
                return this._sides[2] > 0 ? ContourFacetResult.TwoOnly : ContourFacetResult.ZeroAndOne;
            }

            if (this.AllSidesBelowContour())
            {
                return ContourFacetResult.All;
            }

            return ContourFacetResult.None;
        }

        /// <summary>
        /// Initializes the facet data and calculates the <see cref="_sides"/> values from the specified triangle indices. 
        /// </summary>
        /// <param name="index0">The first triangle index of the facet.</param>
        /// <param name="index1">The second triangle index of the facet.</param>
        /// <param name="index2">The third triangle index of the facet.</param>
        private void SetData(int index0, int index1, int index2)
        {
            this._indices[0] = index0;
            this._indices[1] = index1;
            this._indices[2] = index2;

            this._points[0] = this._meshPositions[index0];
            this._points[1] = this._meshPositions[index1];
            this._points[2] = this._meshPositions[index2];

            if (this._hasTextureCoordinates)
            {
                this._textures[0] = this._meshTextureCoordinates[index0];
                this._textures[1] = this._meshTextureCoordinates[index1];
                this._textures[2] = this._meshTextureCoordinates[index2];
            }

            this._sides[0] = (this._a * this._points[0].X) + (this._b * this._points[0].Y) + (this._c * this._points[0].Z) + this._d;
            this._sides[1] = (this._a * this._points[1].X) + (this._b * this._points[1].Y) + (this._c * this._points[1].Z) + this._d;
            this._sides[2] = (this._a * this._points[2].X) + (this._b * this._points[2].Y) + (this._c * this._points[2].Z) + this._d;
        }

        /// <summary>
        /// Calculates the position at the plane intersection for the side specified by two triangle indices.
        /// </summary>
        /// <param name="index0">The first index.</param>
        /// <param name="index1">The second index.</param>
        /// <returns>The interpolated position.</returns>
        private Point3D CreateNewPosition(int index0, int index1)
        {
            var firstPoint = this._points[index0];
            var secondPoint = this._points[index1];
            var firstSide = this._sides[index0];
            var secondSide = this._sides[index1];
            return new Point3D(
                CalculatePoint(firstPoint.X, secondPoint.X, firstSide, secondSide),
                CalculatePoint(firstPoint.Y, secondPoint.Y, firstSide, secondSide),
                CalculatePoint(firstPoint.Z, secondPoint.Z, firstSide, secondSide));
        }

        /// <summary>
        /// Calculates the texture coordinate at the plane intersection for the side specified by two triangle indices.
        /// </summary>
        /// <param name="index0">The first index.</param>
        /// <param name="index1">The second index.</param>
        /// <returns>The interpolated texture coordinate.</returns>
        private Point CreateNewTexture(int index0, int index1)
        {
            var firstTexture = this._textures[index0];
            var secondTexture = this._textures[index1];
            var firstSide = this._sides[index0];
            var secondSide = this._sides[index1];

            return new Point(
                CalculatePoint(firstTexture.X, secondTexture.X, firstSide, secondSide),
                CalculatePoint(firstTexture.Y, secondTexture.Y, firstSide, secondSide));
        }

        /// <summary>
        /// Determines whether the vertex at the specified index is at the opposite side of the other two vertices.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns><c>true</c> if the vertex is on its own side.</returns>
        private bool IsSideAlone(int index)
        {
            Func<int, int> getNext = i => i + 1 > 2 ? 0 : i + 1;

            var firstSideIndex = getNext(index);
            var secondSideIndex = getNext(firstSideIndex);
            return this._sides[index] * this._sides[firstSideIndex] < 0
                && this._sides[index] * this._sides[secondSideIndex] < 0;
        }

        /// <summary>
        /// Determines whether all sides of the facet are below the contour.
        /// </summary>
        /// <returns><c>true</c> if all sides are below the contour.</returns>
        private bool AllSidesBelowContour()
        {
            return this._sides[0] >= 0
                && this._sides[1] >= 0
                && this._sides[2] >= 0;
        }
    }
}