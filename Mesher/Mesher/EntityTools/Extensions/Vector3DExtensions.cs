// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector3DExtensions.cs" company="Helix 3D Toolkit">
//   http://EntityTools.codeplex.com, license: MIT
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace KneeInnovation3D.EntityTools
{
    using System.Windows.Media.Media3D;

    /// <summary>
    /// Extension methods for <see cref="Vector3D"/>.
    /// </summary>
    public static class Vector3DExtensions
    {
        /// <summary>
        /// Find a <see cref="Vector3D"/> that is perpendicular to the given <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="n">
        /// The input vector.
        /// </param>
        /// <returns>
        /// A perpendicular vector.
        /// </returns>
        public static Vector3D FindAnyPerpendicular(this Vector3D n)
        {
            n.Normalize();
            Vector3D u = Vector3D.CrossProduct(new Vector3D(0, 1, 0), n);
            if (u.LengthSquared < 1e-3)
            {
                u = Vector3D.CrossProduct(new Vector3D(1, 0, 0), n);
            }

            return u;
        }

        /// <summary>
        /// Determines whether the specified vector is undefined (NaN,NaN,NaN).
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>
        /// <c>true</c> if the specified vector is undefined; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUndefined(this Vector3D v)
        {
            return double.IsNaN(v.X) && double.IsNaN(v.Y) && double.IsNaN(v.Z);
        }

        /// <summary>
        /// Convert a <see cref="Vector3D"/> to a <see cref="Point3D"/>.
        /// </summary>
        /// <param name="n">
        /// The input vector.
        /// </param>
        /// <returns>
        /// A point.
        /// </returns>
        public static Point3D ToPoint3D(this Vector3D n)
        {
            return new Point3D(n.X, n.Y, n.Z);
        }

        public static double  GetAngleBetweenTwoVectorsover360(Vector3D v1, Vector3D v2)
        {
            double angle =  System.Math.Atan2(v1.X , v1.Y) -  System.Math.Atan2 (v2.X, v2.Y );
            angle = angle * 360 / (2 * System.Math.PI );
            if (angle < 0)
            {
                angle = angle + 360;
            }

            return angle;

        }
    }
}