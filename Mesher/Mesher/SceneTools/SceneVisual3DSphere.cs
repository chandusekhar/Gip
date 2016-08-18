using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using KneeInnovation3D.SceneControls;
using EntityTools.Wpf;

namespace KneeInnovation3D.SceneControls
{
    public class SceneVisual3DSphere : SceneVisual3D 
    {
        private MeshGeometry3D _mesh;
        private Transform3D _trans;
        private Material _mat;
        private Point3D _location;
        private double _rad;

        public SceneVisual3DSphere(Point3D centrePoint, double radius, Scene[] sceneobjects, Material material, Transform3D transform)
        {
            _rad = radius;
            _location = centrePoint;

            _mesh = EntityTools. MeshGeometryFunctions.CreateSphere(_location, _rad, 15, 15);         
            _trans = transform ;


            //EntityTools.Mesh3D.Transform(_mesh, _trans);

            _mat = material.Clone(); 
            SetTheScene(sceneobjects);
            SetMaterial(material);

        }


        public  MeshGeometry3D Mesh
        {
            get { return _mesh; }
        }

   

        public override void Clear()
        {
            ClearGraphics();
            _mesh = null;
        }

        protected override void RebuildGraphics()
        {
            if (_mesh == null || Scenecontrol == null) return;

            if (Scenegraphics == null)
            {
                Scenegraphics = new SceneModel3DGroups[Scenecontrol.Length];
                for (int v = 0; v < Scenegraphics.Length; v++)
                {
                    Scenegraphics[v] = new SceneModel3DGroups(_mesh, _mat, _trans );
                 
                }
            }
        }
    }
}
