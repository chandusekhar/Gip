using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KneeInnovation3D.SceneControls;
using KneeInnovation3D.EntityTools;
using ViewPortTools;

namespace Mesher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Scene[] SceneObjects;
        internal Scene ISOView;
        internal DiffuseMaterial SOLID_MATERIAL = new DiffuseMaterial(new SolidColorBrush(Colors.ForestGreen) { Opacity = 0.6 } );
          internal EmissiveMaterial SHINE = new EmissiveMaterial(new SolidColorBrush(Colors.Blue) { Opacity = 1 });

          LinearGradientBrush brush;
          SceneVisual3DMesh DisplayedMesh;

        public MainWindow()
        {
            InitializeComponent();

            MeshGeometry3D ShowMe = new MeshGeometry3D();

            ShowMe = AddMesh();
            

            ISOView = new Scene(new Vector3D(1, 0, 0), 30, new Vector3D(0, 0, -1), 90, true);
            ISOView.ShowInGrid(MainGrid, 0, 1, 0, 1);

            SceneObjects = new Scene[1];
            SceneObjects[0] = ISOView;

            MaterialGroup mt = new MaterialGroup();
            
            mt.Children.Add(SOLID_MATERIAL);
            
            SceneVisual3DMesh ThicknessMap = new SceneVisual3DMesh(ShowMe, SceneObjects, mt.Clone());
            //MainGrid.Children.Add(ThicknessMap);

            ThicknessMap.Show();

        }

        public MeshGeometry3D AddMesh()
        {
            MeshGeometry3D T = new MeshGeometry3D();

            //T = MeshGeometryFunctions.CreateDensifiedPlane(20, 100, 100, 0);

            Point3DCollection Arc1 = KneeInnovation3D.EntityTools.Polygon3D.GetArc(20, 180, 50, new Point3D(0, 0, 0));
            Point3DCollection Arc2 = KneeInnovation3D.EntityTools.Polygon3D.GetArc(50, 90, 70, new Point3D(0, 0, 20));

            





            return T;
        }


    }
}
