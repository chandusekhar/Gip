﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Input;
using System.Windows.Markup;

namespace ViewPortTools
{
    /// <summary>
    ///     Trackport3D loads a Model3D from a xaml file and displays it.  The user
    ///     may rotate the view by dragging the mouse with the left mouse button.
    ///     Dragging with the right mouse button will zoom in and out.
    /// 
    ///     Trackport3D is primarily an example of how to use the Trackball utility
    ///     class, but it may be used as a custom control in your own applications.
    /// </summary>
    public partial class Trackport3D : UserControl
    {
        private Trackball _trackball = new Trackball();
        private readonly ScreenSpaceLines3D _wireframe = new ScreenSpaceLines3D();

        public Trackport3D()
        {
            InitializeComponent();

            this.Viewport.Children.Add(_wireframe);
            this.Camera.Transform = _trackball.Transform;
            this.Headlight.Transform = _trackball.Transform;
        }

        /// <summary>
        ///     Loads and displays the given Xaml file.  Expects the root of
        ///     the Xaml file to be a Model3D.
        /// </summary>
        public void LoadModel(System.IO.Stream fileStream)
        {
            _model = (Model3D)XamlReader.Load(fileStream);

            SetupScene();
        }

        public Color HeadlightColor
        {
            get { return this.Headlight.Color; }
            set { this.Headlight.Color = value; }
        }

        public Color AmbientLightColor
        {
            get { return this.AmbientLight.Color; }
            set { this.AmbientLight.Color = value; }
        }

        public ViewMode ViewMode
        {
            get { return _viewMode; }
            set
            {
                _viewMode = value;
                SetupScene();
            }
        }

        private void SetupScene()
        {
            switch (ViewMode)
            {
                case ViewMode.Solid:
                    this.Root.Content = _model;
                    this._wireframe.Points.Clear();
                    break;

                case ViewMode.Wireframe:
                    this.Root.Content = null;
                    this._wireframe.MakeWireframe(_model);
                    break;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Viewport3Ds only raise events when the mouse is over the rendered 3D geometry.
            // In order to capture events whenever the mouse is over the client are we use a
            // same sized transparent Border positioned on top of the Viewport3D.
            _trackball.EventSource = CaptureBorder;
        }

        private ViewMode _viewMode;
        private Model3D _model;
    }
}