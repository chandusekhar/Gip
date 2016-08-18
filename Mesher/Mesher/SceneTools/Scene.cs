using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
namespace KneeInnovation3D.SceneControls
{

    ///
    /// The scene is  place holder for all view related elements icluding graphic entities and hit testing

    /// <summary>
    /// A control that contains a <see cref="Viewport3D" />, a <see cref="CameraController",  a <see cref="Entities",  a <see cref="WPFGridControl" ,  a <see cref="WPF Canvas"/>.
    /// </summary>
    public class Scene
    {
        public Viewport3D SceneViewport { get; set; }
        public OrthographicCamera SceneCamera { get; set; }
        public ContainerUIElement3D SceneElements { get; set; }
        public Model3DGroup SceneLights { get; set; }
        public Grid SceneMainControl { get; set; }
        public ViewPortTools.TrackballDecorator SceneTrackball { get; set; }

        private void SetupTheScene()
        {
            SceneMainControl = new Grid();
            SceneMainControl.Children.Add(new Rectangle() {  Stroke = Brushes.Black , StrokeThickness = 1 });


            SceneViewport = new Viewport3D();
            SceneViewport.ClipToBounds = true;

            SceneCamera = new OrthographicCamera(new Point3D(0, 20, 10000), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 3) { FarPlaneDistance = 1000000, Width = 150 };
            SceneViewport.Camera = SceneCamera;
            ContainerUIElement3D initialisingContainer = new ContainerUIElement3D() { IsHitTestVisible = false };
            ModelUIElement3D initialisingLightModel = new ModelUIElement3D();
            SceneLights = new Model3DGroup();
            SceneLights.Children.Add(new DirectionalLight(Colors.White, new Vector3D(0, 0, -1)));
           
            initialisingLightModel.Model = SceneLights;
            initialisingContainer.Children.Add(initialisingLightModel);
            SceneElements  = new ContainerUIElement3D();
            initialisingContainer.Children.Add(SceneElements);
            SceneTrackball = new ViewPortTools.TrackballDecorator();
            SceneViewport.Children.Add(initialisingContainer);
            SceneTrackball.Content = SceneViewport;
            SceneLights.Transform = SceneTrackball.Transform;
            SceneMainControl.Children.Add(SceneTrackball);

            SceneMainControl.IsManipulationEnabled = true;
            SceneMainControl.ManipulationDelta += SceneMainControl_ManipulationDelta;
            SceneMainControl.TouchUp += SceneMainControl_TouchUp;

            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
        }

        void SceneMainControl_TouchUp(object sender, TouchEventArgs e)
        {
            SceneTrackball.ResetTouchMove();
        }

        private void SetupTheSceneWithSpotLight()
        {
            SceneMainControl = new Grid();
            SceneMainControl.Children.Add(new Rectangle() { Stroke = Brushes.Black, StrokeThickness = 1 });


            SceneViewport = new Viewport3D();
            SceneViewport.ClipToBounds = true;

            SceneCamera = new OrthographicCamera(new Point3D(0, 20, 10000), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 3) { FarPlaneDistance = 1000000, Width = 150 };
            SceneViewport.Camera = SceneCamera;
            ContainerUIElement3D initialisingContainer = new ContainerUIElement3D() { IsHitTestVisible = false };
            ModelUIElement3D initialisingLightModel = new ModelUIElement3D();
            SceneLights = new Model3DGroup();

            SceneLights.Children.Add(new AmbientLight (Colors .DarkGray ));
            SceneLights.Children.Add(new DirectionalLight(Colors.Gray, new Vector3D(0.234, -0.114, -0.965)));

            initialisingLightModel.Model = SceneLights;
            initialisingContainer.Children.Add(initialisingLightModel);
            SceneElements = new ContainerUIElement3D();
            initialisingContainer.Children.Add(SceneElements);
            SceneTrackball = new ViewPortTools.TrackballDecorator();
            SceneViewport.Children.Add(initialisingContainer);
            SceneTrackball.Content = SceneViewport;
            SceneLights.Transform = SceneTrackball.Transform;
            SceneMainControl.Children.Add(SceneTrackball);

            SceneMainControl.IsManipulationEnabled = true;
            SceneMainControl.ManipulationDelta += SceneMainControl_ManipulationDelta;

            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
        }


        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            var touchPoints = e.GetTouchPoints(SceneViewport);

           // HitTest(touchPoints[0].Position);

            //if (touchPoints.Count >= 3 && touchPoints[0].Action == TouchAction.Move )
            //{

            //    lastposition = touchPoints[0].Position;

            //    SceneTrackball.UpdateTouchPan(touchPoints[0].Position);
            //    //this.TouchLine.X1 = touchPoints[0].Position.X;
            //    //this.TouchLine.X2 = touchPoints[1].Position.X;
            //    //this.TouchLine.Y1 = touchPoints[0].Position.Y;
            //    //this.TouchLine.Y2 = touchPoints[1].Position.Y;
            //}
        }
      
        Point _lastposition;

        void SceneMainControl_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
       
           double x = e.DeltaManipulation.Scale.X;
     
           Point delta = e.ManipulationOrigin;
           _lastposition = delta;
           SceneTrackball.UpdateTouchMove(delta,new  ScaleTransform3D (x,x,x));

        }

     

        public Scene(Vector3D viewDirection, double viewAngle)
        {
            SetupTheScene();
            SceneTrackball.SetViewDirection(viewDirection, viewAngle);
 
        }
        public Scene(Vector3D viewDirection1, double viewAngle1, Vector3D viewDirection2, double viewAngle2)
        {
            SetupTheScene();
            SceneTrackball.SetViewDirection(viewDirection1, viewAngle1, viewDirection2 , viewAngle2 );

        }

        public Scene(Vector3D viewDirection, double viewAngle, bool directionallighting)
        {
            SetupTheSceneWithSpotLight();
            SceneTrackball.SetViewDirection(viewDirection, viewAngle);

        }
        public Scene(Vector3D viewDirection1, double viewAngle1, Vector3D viewDirection2, double viewAngle2, bool directionallighting)
        {
            SetupTheSceneWithSpotLight();
            SceneTrackball.SetViewDirection(viewDirection1, viewAngle1, viewDirection2, viewAngle2);

        }


        public void SetSceneDirection(Vector3D viewDirection, double viewAngle)
        {
            SceneTrackball.SetViewDirection(viewDirection, viewAngle);
        }

        public void SetSceneDirection(Vector3D viewDirection1, double viewAngle1, Vector3D  viewDirection2, double viewAngle2)
        {
            SceneTrackball.SetViewDirection(viewDirection1, viewAngle1, viewDirection2, viewAngle2);
        }

        public void SceneHitTester(MouseEventArgs e)
        { 
        
        }

        //same as trackballdecorator hit testing

        private List<RayMeshGeometry3DHitTestResult> _hitTestResultList = new List<RayMeshGeometry3DHitTestResult> { };

        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            if (result != null && result.GetType() == typeof(RayMeshGeometry3DHitTestResult))
            {
                RayMeshGeometry3DHitTestResult res = (RayMeshGeometry3DHitTestResult)result;

                _hitTestResultList.Add(res);

            }
            // Set the behavior to return visuals at all z-order levels.
            return HitTestResultBehavior.Continue;
        }

        public HitTestFilterBehavior MyHitTestFilter(DependencyObject o)
        {
            if (o.GetType() == typeof(Viewport3DVisual))
            {
                return HitTestFilterBehavior.Continue;
            }
            else if (o.GetType() == typeof(ModelUIElement3D))
            {
                ModelUIElement3D j = (ModelUIElement3D)o;
                bool d = (bool)j.GetValue(UIElement3D.IsHitTestVisibleProperty);

                if (d == false)
                {
                    return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
                }
                else
                {
                    return HitTestFilterBehavior.Continue;
                }
            }
            else
            {
                return HitTestFilterBehavior.ContinueSkipSelf;
            }
        }

        public void HitTest(MouseEventArgs e)
        {
            if (e == null || SceneTrackball == null) return;
            Point pt = e.GetPosition(SceneTrackball);
            HitTest(pt);
        }

        public void HitTest(Point position)
        {
            _hitTestResultList.Clear();
            if (SceneTrackball == null) return;
            VisualTreeHelper.HitTest(SceneTrackball, new HitTestFilterCallback(MyHitTestFilter), new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(position));
        }

        public void ShowInGrid(Grid thisGrid, int row, int rowspan, int column, int columnspan)
        {
            if (thisGrid == null) return;

            if (SceneMainControl != null)
            {
                Grid parentControl = SceneMainControl.Parent as Grid;
               if (parentControl != null) parentControl.Children.Remove(SceneMainControl);
            }
  
                Grid.SetRow(SceneMainControl, row);
                Grid.SetColumn(SceneMainControl, column);
                Grid.SetRowSpan(SceneMainControl, rowspan);
                Grid.SetColumnSpan(SceneMainControl, columnspan);
                thisGrid.Children.Insert(0, SceneMainControl);
         
        }
    }
}
