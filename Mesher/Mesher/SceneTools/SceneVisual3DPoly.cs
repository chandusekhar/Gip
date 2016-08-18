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
    public class SceneVisual3DPoly : SceneVisual3D 
    {
        private Point3DCollection   _points;
        private Color _polycolor = Colors.Red;

        public SceneVisual3DPoly(Point3DCollection points, Scene[] sceneobjects, Color color)
        {
            _points  = points;
            _polycolor = color;
            SetTheScene(sceneobjects);
           
        }

        public SceneVisual3DPoly(Point3DCollection points, Scene[] sceneobjects, Color color, Transform3D trans)
        {
            _points = points.Clone();
            KneeInnovation3D.EntityTools.Polygon3D.Transform(_points, trans);    

            _polycolor = color;
            SetTheScene(sceneobjects);

        }

        public  Point3DCollection  Points
        {
            get { return _points ; }
        }

        public Color Color
        {
            get { return _polycolor ; }
        }
   
        public override void Clear()
        {
            ClearGraphics();
            _points  = null;
        }

        protected override void RebuildGraphics()
        {
            if (_points == null || Scenecontrol == null) return;

            if (Scenegraphics == null)
            {
                Scenegraphics = new SceneModel3DGroups[Scenecontrol.Length];
                for (int v = 0; v < Scenegraphics.Length; v++)
                {

                    ViewPortTools.ScreenSpaceLines3D spacecurve =    EntityTools.Polygon3D. GetScreenSpaceLines3D(_points, _polycolor );

                    Scenegraphics[v] = new SceneModel3DGroups(spacecurve);
 
                }

            }
        }
    }
}
