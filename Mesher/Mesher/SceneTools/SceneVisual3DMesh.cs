using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using KneeInnovation3D.SceneControls;

namespace KneeInnovation3D.SceneControls
{
    public class SceneVisual3DMesh : SceneVisual3D 
    {
        private MeshGeometry3D _mesh;
        private Transform3D _trans;
        private Material _mat;

        public SceneVisual3DMesh(MeshGeometry3D mesh, Scene[] sceneobjects, Material material)
        {
            _mesh = mesh;
            _mat = material.Clone(); 
            SetTheScene(sceneobjects);
            SetMaterial(material);
            _trans = Transform3D.Identity;
        }

        public SceneVisual3DMesh(MeshGeometry3D mesh, Scene[] sceneobjects, Material material, Transform3D transform)
        {
            _mesh = mesh.Clone ();
            _trans = transform;
            _mat = material.Clone();
            SetTheScene(sceneobjects);
            SetMaterial(material);

        }

        public MeshGeometry3D Mesh
        {
            get { return _mesh; }
        }


        public Transform3D  Trans
        {
            set { _trans = value; SetTransform(_trans); }
            get { return _trans; }
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
                    Scenegraphics[v] = new SceneModel3DGroups(_mesh,  _mat, _trans );
                    
                }
            }
        }
    }
}
