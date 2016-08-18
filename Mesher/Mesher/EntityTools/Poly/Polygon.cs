// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polygon.cs" company="Helix 3D Toolkit">
//   http://EntityTools.codeplex.com, license: MIT
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Windows.Media;

namespace KneeInnovation3D.EntityTools
{
       
    /// <summary>
    /// Represents a 2D polygon.
    /// </summary>
    public class Polygon
    {
        //// http://softsurfer.com/Archive/algorithm_0101/algorithm_0101.htm

        /// <summary>
        /// The points.
        /// </summary>
        private PointCollection _points;

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
        public PointCollection Points
        {
            get
            {
                return this._points ?? (this._points = new PointCollection());
            }

            set
            {
                this._points = value;
            }
        }

        /// <summary>
        /// Triangulate the polygon by cutting ears
        /// </summary>
        /// <returns>An index collection.</returns>
        public Int32Collection Triangulate()
        {
            return EntityTools. CuttingEarsTriangulator.Triangulate(this._points);
        }


    }
}