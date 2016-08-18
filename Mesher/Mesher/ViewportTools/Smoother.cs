using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
 
namespace OrthoDesigner.Math
{
   
    using System.Collections;
    using System.Collections.Generic;

    /*
        MeshSmoothTest
 
        Laplacian Smooth Filter, HC-Smooth Filter
 
        MarkGX, Jan 2011
    */
    public class SmoothFilter
    {
        /*
            Standard Laplacian Smooth Filter
        */
        public static Point3D[]LaplacianFilter(Point3D[]sv, int[] t)
        {
            Point3D[] wv = new Point3D[sv.Length];
            List<Point3D> adjacentVertices = new List<Point3D>();

            float dx = 0.0f;
            float dy = 0.0f;
            float dz = 0.0f;

            for (int vi = 0; vi < sv.Length; vi++)
            {
                // Find the sv neighboring vertices
                adjacentVertices = ViewPortTools.MeshUtils.FindAdjacentNeighbors(sv, t, sv[vi]);

                if (adjacentVertices.Count != 0)
                {
                    dx = 0.0f;
                    dy = 0.0f;
                    dz = 0.0f;

                    //Debug.Log("Vertex Index Length = "+vertexIndexes.Length);
                    // Add the vertices and divide by the number of vertices
                    for (int j = 0; j < adjacentVertices.Count; j++)
                    {
                        dx += (float)adjacentVertices[j].X;
                        dy += (float)adjacentVertices[j].Y;
                        dz += (float)adjacentVertices[j].Z;
                    }

                    wv[vi].X = dx / adjacentVertices.Count;
                    wv[vi].Y = dy / adjacentVertices.Count;
                    wv[vi].Z = dz / adjacentVertices.Count;
                }
            }

            return wv;
        }

        /*
            HC (Humphrey’s Classes) Smooth Algorithm - Reduces Shrinkage of Laplacian Smoother
 
            Where sv - original points
                    pv - previous points,
                    alpha [0..1] influences previous points pv, e.g. 0
                    beta  [0..1] e.g. > 0.5
        */
        public static Point3D[]HcFilter(Point3D[]sv, Point3D[]pv, int[] t, float alpha, float beta)
        {
            Point3D[] wv = new Point3D[sv.Length];
            Point3D[] bv = new Point3D[sv.Length];



            // Perform Laplacian Smooth
            wv = LaplacianFilter(sv, t);

            // Compute Differences
            for (int i = 0; i < wv.Length; i++)
            {
                bv[i].X = wv[i].X - (alpha * sv[i].X + (1 - alpha) * sv[i].X);
                bv[i].Y = wv[i].Y - (alpha * sv[i].Y + (1 - alpha) * sv[i].Y);
                bv[i].Z = wv[i].Z - (alpha * sv[i].Z + (1 - alpha) * sv[i].Z);
            }

            List<int> adjacentIndexes = new List<int>();

            float dx = 0.0f;
            float dy = 0.0f;
            float dz = 0.0f;

            for (int j = 0; j < bv.Length; j++)
            {
                adjacentIndexes.Clear();

                // Find the bv neighboring vertices
                adjacentIndexes = ViewPortTools.MeshUtils.FindAdjacentNeighborIndexes(sv, t, sv[j]);

                dx = 0.0f;
                dy = 0.0f;
                dz = 0.0f;

                for (int k = 0; k < adjacentIndexes.Count; k++)
                {
                    dx += (float )bv[adjacentIndexes[k]].X;
                    dy += (float)bv[adjacentIndexes[k]].Y;
                    dz += (float)bv[adjacentIndexes[k]].Z;

                }

                wv[j].X -= beta * bv[j].X + ((1 - beta) / adjacentIndexes.Count) * dx;
                wv[j].Y -= beta * bv[j].Y + ((1 - beta) / adjacentIndexes.Count) * dy;
                wv[j].Z -= beta * bv[j].Z + ((1 - beta) / adjacentIndexes.Count) * dz;
            }

            return wv;
        }
    }
}
