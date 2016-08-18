//---------------------------------------------------------------------------
//
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Limited Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/limitedpermissivelicense.mspx
// All other rights reserved.
//
// This file is part of the 3D Tools for Windows Presentation Foundation
// project.  For more information, see:
// 
// http://CodePlex.com/Wiki/View.aspx?ProjectName=3DTools
//
// The following article discusses the mechanics behind this
// trackball implementation: http://viewport3d.com/trackball.htm
//
// Reading the article is not required to use this sample code,
// but skimming it might be useful.
//
//---------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Markup; 

namespace ViewPortTools
{
    public class TrackballDecorator : Viewport3DDecorator
    {
        public TrackballDecorator()
        {
            // the transform that will be applied to the viewport 3d's camera
            _transform = new Transform3DGroup();
            _transform.Children.Add(_scale);
            _transform.Children.Add(new RotateTransform3D(_rotation));
            _transform.Children.Add(_translate);
            // used so that we always get events while activity occurs within
            // the viewport3D
            _eventSource = new Border();
            _eventSource.Background = Brushes.Transparent;

            PreViewportChildren.Add(_eventSource);
        }

        /// <summary>
        ///     A transform to move the camera or scene to the trackball's
        ///     current orientation and scale.
        /// </summary>
        public Transform3D Transform
        {
            get { return _transform; }
        }

        #region Event Handling



        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (IsMouseCaptured)
            {
                Mouse.Capture(this, CaptureMode.None);
            }
        }

        private bool _disabletrack = false;
        public bool DisableTrack { get { return _disabletrack; } set { _disabletrack = value; } }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsMouseCaptured)
            {
                Point currentPosition = e.GetPosition(this);

                // avoid any zero axis conditions
                if (currentPosition == _previousPosition2D) return;

                // Prefer tracking to zooming if both buttons are pressed.
                if (e.MiddleButton  == MouseButtonState.Pressed && Keyboard.IsKeyDown(Key.LeftShift))
                {
                    Translate(currentPosition);
                }
                else if (e.MiddleButton == MouseButtonState.Pressed && _disabletrack == false)
                {
                    Track(currentPosition);
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    Zoom(currentPosition);
                }

                _previousPosition2D = currentPosition;

                Viewport3D viewport3D = this.Viewport3D;
                if (viewport3D != null)
                {
                    if (viewport3D.Camera != null)
                    {
                        if (viewport3D.Camera.IsFrozen)
                        {
                            viewport3D.Camera = viewport3D.Camera.Clone();
                        }

                        if (viewport3D.Camera.Transform != _transform)
                        {
                            viewport3D.Camera.Transform = _transform;
                        }
                    }
                }
            }
        }
        public TranslateTransform3D _translate = new TranslateTransform3D();
        public Double TransScale = 6;
        private void Translate(Point currentPosition)
        {
            // Calculate the panning vector from screen(the vector component of the Quaternion
            // the division of the X and Y components scales the vector to the mouse movement
            Quaternion qV = new Quaternion(((_previousPosition2D.X - currentPosition.X) / TransScale),
            ((currentPosition.Y - _previousPosition2D.Y) / TransScale), 0, 0);

            // Get the current orientantion from the RotateTransform3D
            Quaternion q = new Quaternion(_rotation.Axis, _rotation.Angle);
            Quaternion qC = q;
            qC.Conjugate();

            // Here we rotate our panning vector about the the rotaion axis of any current rotation transform
            // and then sum the new translation with any exisiting translation
            qV = q * qV * qC;
            _translate.OffsetX += qV.X;
            _translate.OffsetY += qV.Y;
            _translate.OffsetZ += qV.Z;
        }



        #endregion Event Handling


        Point _previous;

        private void Track(Point currentPosition)
        {

            if (_previousPosition3D.X == 0 && _previousPosition3D.Y == 0 && _previousPosition3D.Z == 1)
            {
                _previousPosition3D = ProjectToTrackball(
                ActualWidth, ActualHeight, currentPosition);
            }
            Vector3D currentPosition3D = ProjectToTrackball(
             ActualWidth, ActualHeight, currentPosition);

          

            Vector3D axis = Vector3D.CrossProduct(_previousPosition3D, currentPosition3D);
            double angle = Vector3D.AngleBetween(_previousPosition3D, currentPosition3D);

    
            // quaterion will throw if this happens - sometimes we can get 3D positions that
            // are very similar, so we avoid the throw by doing this check and just ignoring
            // the event 
            if (axis.Length == 0) return;

            Quaternion delta = new Quaternion(axis, -angle);

            // Get the current orientantion from the RotateTransform3D
            AxisAngleRotation3D r = _rotation;
            Quaternion q = new Quaternion(_rotation.Axis, _rotation.Angle);

            // Compose the delta with the previous orientation
            q *= delta;

            // Write the new orientation back to the Rotation3D
            _rotation.Axis = q.Axis;
            _rotation.Angle = q.Angle;

            _previousPosition3D = currentPosition3D;
        }

        private Vector3D ProjectToTrackball(double width, double height, Point point)
        {
            double x = point.X / (width / 2);    // Scale so bounds map to [0,0] - [2,2]
            double y = point.Y / (height / 2);

            x = x - 1;                           // Translate 0,0 to the center
            y = 1 - y;                           // Flip so +Y is up instead of down

            double z2 = 1 - x * x - y * y;       // z^2 = 1 - x^2 - y^2
            double z = z2 > 0 ? Math.Sqrt(z2) : 0;

            return new Vector3D(x, y, z);
        }

        private void Zoom(Point currentPosition)
        {
            double yDelta = currentPosition.Y - _previousPosition2D.Y;

            double scale = Math.Exp(yDelta / 100);    // e^(yDelta/100) is fairly arbitrary.

            _scale.ScaleX *= scale;
            _scale.ScaleY *= scale;
            _scale.ScaleZ *= scale;
        }

        internal void SetZoomtoExtents(Rect3D rect)
        {


        }

        internal void SetZoom(double scale)
        {
            _scale.ScaleX = scale;
            _scale.ScaleY = scale;
            _scale.ScaleZ = scale;
        }

        //--------------------------------------------------------------------
        //
        // Private data
        //
        //--------------------------------------------------------------------

        private Point _previousPosition2D;
        private Vector3D _previousPosition3D = new Vector3D(0, 0, 1);

        private Transform3DGroup _transform;
        private ScaleTransform3D _scale = new ScaleTransform3D();
        private AxisAngleRotation3D _rotation = new AxisAngleRotation3D();

        private Border _eventSource;

        public delegate void HitTestHitFoundHandler(object sender, HitTestHitFoundEventArgs e);
        public event HitTestHitFoundHandler HitTestHitFound;
        public class HitTestHitFoundEventArgs : EventArgs
        {
            public List<RayMeshGeometry3DHitTestResult> HitTestResults;

            public HitTestHitFoundEventArgs(List<RayMeshGeometry3DHitTestResult> result)
            {
                this.HitTestResults = result;
            }
        }

        public bool OnlyHitTestHitTestVisible { get; set; }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            _previousPosition2D = e.GetPosition(this);
            _previousPosition3D = ProjectToTrackball(ActualWidth,
                                                     ActualHeight,
                                                     _previousPosition2D);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point pt = e.GetPosition(this);
                _hitTestResultList.Clear();

                if (OnlyHitTestHitTestVisible == true)
                {
                    VisualTreeHelper.HitTest(this.Viewport3D, new HitTestFilterCallback(MyHitTestFilter), new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(pt));
                }
                else
                {
                    VisualTreeHelper.HitTest(this, null, new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(pt));
                }

                if (HitTestHitFound != null)
                    HitTestHitFound.Invoke(this, new HitTestHitFoundEventArgs(_hitTestResultList));
            }

            if (Mouse.Captured == null)
            {
                Mouse.Capture(this, CaptureMode.Element);
            }
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

        internal enum MovementTypeList { Rotate, Pan, Zoom, Unkown };
        internal void UpdateAirMove(Point currentPosition, MovementTypeList movementType)
        {

            if (currentPosition == _previousPosition2D) return;

            ViewPortTools.TrackballDecorator.AirMovedEventArgs.EventTypeList type = AirMovedEventArgs.EventTypeList.Unkown;


            if (movementType == MovementTypeList.Pan)
            {
                Translate(currentPosition);
                type = AirMovedEventArgs.EventTypeList.Pan;
            }
            else if (movementType == MovementTypeList.Rotate && _disabletrack != true )
            {

                Track(currentPosition);
                type = AirMovedEventArgs.EventTypeList.Rotate;
            }
            else if (movementType == MovementTypeList.Zoom)
            {
                Zoom(currentPosition);
                type = AirMovedEventArgs.EventTypeList.Zoom;
            }

            _previousPosition2D = currentPosition;

            Viewport3D viewport3D = this.Viewport3D;
            if (viewport3D != null)
            {
                if (viewport3D.Camera != null)
                {
                    if (viewport3D.Camera.IsFrozen)
                    {
                        viewport3D.Camera = viewport3D.Camera.Clone();
                    }

                    if (viewport3D.Camera.Transform != _transform)
                    {
                        viewport3D.Camera.Transform = _transform;
                    }
                }
            }

            if (GestureMouseMoved != null && type != AirMovedEventArgs.EventTypeList.Unkown)
            {
                GestureMouseMoved.Invoke(new AirMovedEventArgs(type));
            }
        }

        public class AirMovedEventArgs : EventArgs
        {
            public enum EventTypeList { Rotate, Pan, Zoom, Unkown }

            public EventTypeList EventType { get; set; }

            public AirMovedEventArgs(EventTypeList eventType)
            {
                EventType = eventType;
            }
        }
        public delegate void AirMovedHandler(AirMovedEventArgs e);
        public event AirMovedHandler GestureMouseMoved;


        public class TouchEventArgs : EventArgs
        {
            public enum EventTypeList { Rotate, Pan, Zoom, Unkown }

            public EventTypeList EventType { get; set; }

            public TouchEventArgs(EventTypeList eventType)
            {
                EventType = eventType;
            }
        }
        public delegate void TouchMovedHandler(TouchEventArgs e);
        public event TouchMovedHandler GestureTouchMoved;

        internal void UpdateTouchPan(Point currentPosition)
        {
            if (currentPosition == _previousPosition2D) return;

            ViewPortTools.TrackballDecorator.AirMovedEventArgs.EventTypeList type = AirMovedEventArgs.EventTypeList.Unkown;

            Translate(currentPosition);

            _previousPosition2D = currentPosition;


        }



        internal void UpdateTouchMove(Point currentPosition, ScaleTransform3D scale)
        {
       
            if (currentPosition == _previousPosition2D) return;

            _scale.ScaleX *= scale.ScaleX;
            _scale.ScaleY *= scale.ScaleY;
            _scale.ScaleZ *= scale.ScaleZ;


          if (_disabletrack != true)   Track(currentPosition);

            ViewPortTools.TrackballDecorator.TouchEventArgs.EventTypeList type = TouchEventArgs.EventTypeList.Unkown;


            _previousPosition2D = currentPosition;

            Viewport3D viewport3D = this.Viewport3D;
            if (viewport3D != null)
            {
                if (viewport3D.Camera != null)
                {
                    if (viewport3D.Camera.IsFrozen)
                    {
                        viewport3D.Camera = viewport3D.Camera.Clone();
                    }

                    if (viewport3D.Camera.Transform != _transform)
                    {
                        viewport3D.Camera.Transform = _transform;
                    }
                }
            }

            if (GestureTouchMoved != null && type != TouchEventArgs.EventTypeList.Unkown)
            {
                GestureTouchMoved.Invoke(new TouchEventArgs(type));
            }
        }
        internal void ResetTouchMove()
        {
            _previousPosition3D = new Vector3D(0, 0, 1);
        }
    
        public Transform3DGroup Viewtransform;
        public TranslateTransform3D Viewtranslation = new TranslateTransform3D();
        public AxisAngleRotation3D Viewrotation = new AxisAngleRotation3D();
        public ScaleTransform3D Viewscale = new ScaleTransform3D();

       



        Viewport3D _currentVp = null;

        public Viewport3D CurrentViewport { get { return _currentVp; } set { _currentVp = value; } }

        public void SetViewDirection(Vector3D direction, double angle)
        {
            _rotation.Axis = direction;
            _rotation.Angle = angle;
            Viewport3D viewport3D = this.Viewport3D;
            if (viewport3D != null)
            {
                if (viewport3D.Camera != null)
                {

                    viewport3D.Camera = viewport3D.Camera.Clone();

                    if (viewport3D.Camera.Transform != _transform)
                    {
                        viewport3D.Camera.Transform = _transform;
                    }
                }
            }
            if (_currentVp != null && this.Viewport3D != null)
            {
                _currentVp.Camera.Transform = this.Viewport3D.Camera.Transform;
            }


        }
        public void SetViewDirection(Vector3D direction1, double angle1, Vector3D direction2, double angle2)
        {
            _rotation.Axis = direction1;
            _rotation.Angle = angle1;

            Quaternion delta = new Quaternion(direction2, angle2);
            Quaternion q = new Quaternion(_rotation.Axis, _rotation.Angle);
            q *= delta;
            _rotation.Axis = q.Axis;
            _rotation.Angle = q.Angle;

            Viewport3D viewport3D = this.Viewport3D;
            if (viewport3D != null)
            {
                if (viewport3D.Camera != null)
                {
                    if (viewport3D.Camera.IsFrozen)
                    {
                        viewport3D.Camera = viewport3D.Camera.Clone();
                    }

                    if (viewport3D.Camera.Transform != _transform)
                    {
                        viewport3D.Camera.Transform = _transform;
                    }
                }
            }
            if (_currentVp != null && this.Viewport3D != null)
            {
                _currentVp.Camera.Transform = this.Viewport3D.Camera.Transform;
            }

        }
    }
  
}
