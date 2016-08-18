 
using System.Windows.Media.Media3D;
 

namespace KneeInnovation3D.SceneControls
{
    public class SceneModel3DGroups
    {

        private ModelVisual3D _visuals;

        public ModelVisual3D Visual { get { return _visuals; } }

        private Model3DGroup _meshGroup;

        public Model3DGroup MeshEntities { get { return _meshGroup; } }

        public SceneModel3DGroups(ModelVisual3D visual)
        {
            _visuals = new ModelVisual3D();
            _meshGroup = new Model3DGroup();
            _visuals = visual;
            _visuals.Transform = Transform3D.Identity;  
 
        }

        public SceneModel3DGroups(MeshGeometry3D geometry, Material material, Transform3D trans)
        {

            _visuals = new ModelVisual3D();
            _meshGroup = new Model3DGroup();
            GeometryModel3D model = new GeometryModel3D(geometry, material);
        
            _meshGroup.Children.Add(model);
            _visuals.Content = _meshGroup;
            _visuals.Transform = trans;

        }

        public void Clear()
        {

            if (_visuals != null)
            {
                for (int i = _visuals.Children.Count - 1; i >= 0; i--)
                {
                    _visuals.Children.RemoveAt(i);
                }
            }
            _visuals = null;
            _meshGroup.Children.Clear();
        }

        public void SetMaterial(Material m)
        {
            foreach (var v in _meshGroup.Children)
            {
                if (v.GetType() == typeof(GeometryModel3D))
                {
                    GeometryModel3D geom = (GeometryModel3D)v;
                    geom.Material = m;
                    geom.BackMaterial = m;
                }
            }

        }

        public void SetTransform(Transform3D tran)
        {
            _visuals.Transform = tran;
        }
    }
}
