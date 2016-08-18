using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KICoCAD.EntityTools
{
    internal class MeshTags
    {
        public List<Face> FaceList { get; set; }

        internal void GetMeshUsingTags(MeshGeometry3D inputmesh, List<Face> facelist, string tag)
        {
            var points = facelist.Select(face =>  face.TagID  ==  tag).ToList();



        }

    }

    internal class Face
    {
        public int[] FaceNodes { get;  set; }
        public string TagID { get; set; }

        public Face(int[] face, string tag)
        {
            FaceNodes = face;
            TagID = tag;
        }
    }
}
