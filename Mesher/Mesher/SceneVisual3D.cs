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
    public abstract class SceneVisual3D
    {
        protected Scene[] Scenecontrol = null;
        protected SceneModel3DGroups[] Scenegraphics = null;
        private Material _visualmaterial;
        private int[] _exclusionlist;

        public void SetMaterial(Material visualmaterial)
        {
            _visualmaterial = visualmaterial;


            if (Scenegraphics == null) return;
            foreach (var v in Scenegraphics)
                v.SetMaterial(visualmaterial);

            Hide();
            Show();

        }


        public void SetTransform(Transform3D transform)
        {
            if (Scenegraphics == null) return;
            foreach (var v in Scenegraphics)
                v.SetTransform(transform);

            Hide();
            if (_exclusionlist != null) Show(_exclusionlist);
            else Show();

        }

        public void Show()
        {
            if (Scenegraphics == null) RebuildGraphics();
            if (Scenegraphics == null) return;


            for (int i = 0; i < Scenecontrol.Length; i++)
            {
                if (Scenecontrol[i] != null)
                {
                    if (!Scenecontrol[i].SceneElements.Children.Contains(Scenegraphics[i].Visual))
                    {
                        if (IsTransparent())
                            Scenecontrol[i].SceneElements.Children.Add(Scenegraphics[i].Visual);
                        else
                            Scenecontrol[i].SceneElements.Children.Insert(0, Scenegraphics[i].Visual);

                        ViewPortTools.ScreenSpaceLines3D s = Scenegraphics[i].Visual as ViewPortTools.ScreenSpaceLines3D;
                        if (s != null) s.Render();

                    }
                }
            }
        }

        public void Show(int[] exclusionlist)
        {
            if (Scenegraphics == null) RebuildGraphics();
            if (Scenegraphics == null) return;
            _exclusionlist = exclusionlist;

            for (int i = 0; i < Scenecontrol.Length; i++)
            {
                if (!exclusionlist.Contains(i) && Scenecontrol[i] != null)
                {
                    if (!Scenecontrol[i].SceneElements.Children.Contains(Scenegraphics[i].Visual))
                    {
                        if (IsTransparent())
                            Scenecontrol[i].SceneElements.Children.Add(Scenegraphics[i].Visual);
                        else
                            Scenecontrol[i].SceneElements.Children.Insert(0, Scenegraphics[i].Visual);

                        ViewPortTools.ScreenSpaceLines3D s = Scenegraphics[i].Visual as ViewPortTools.ScreenSpaceLines3D;
                        if (s != null) s.Render();

                    }
                }
                else
                {
                    if (Scenecontrol[i] != null) Scenecontrol[i].SceneElements.Children.Remove(Scenegraphics[i].Visual);
                }
            }
        }

        public bool IsTransparent()
        {

            DiffuseMaterial dm = _visualmaterial as DiffuseMaterial;
            if (dm != null)
            {
                if (dm.Brush != null & dm.Brush.Opacity < 1) return true;
                else return false;
            }

            return false;
        }

        protected abstract void RebuildGraphics();

        public bool IsVisible
        {
            get
            {
                if (Scenegraphics == null) return false;
                if (Scenecontrol == null) return false;
                for (int i = 0; i < Scenecontrol.Length; i++)
                {
                    if (Scenecontrol[i].SceneElements.Children.Contains(Scenegraphics[i].Visual)) return true;
                }
                return false;
            }
        }

        public void Hide()
        {
            if (Scenegraphics == null || Scenecontrol == null) return;

            for (int c = 0; c < Scenecontrol.Length; c++)
            {
                if (Scenecontrol[c] != null)
                {
                    if (Scenecontrol[c].SceneElements.Children.Contains(Scenegraphics[c].Visual))
                        Scenecontrol[c].SceneElements.Children.Remove(Scenegraphics[c].Visual);
                }
            }
        }

        internal void ClearGraphics()
        {
            Hide();
            Scenecontrol = null;
            if (Scenegraphics != null)
            {
                foreach (var v in Scenegraphics)
                {
                    v.Clear();

                }

                Scenegraphics = null;
            }
        }

        public abstract void Clear();

        public Visual3D[] GetVisual3D()
        {
            if (Scenegraphics == null) return null;
            Visual3D[] v = new Visual3D[Scenegraphics.Length];
            for (int i = 0; i < Scenegraphics.Length; i++)
                v[i] = Scenegraphics[i].Visual;
            return v;
        }

        public bool ContainVisual(Visual3D visual)
        {
            if (Scenegraphics == null) return false;

            foreach (var v in Scenegraphics)
                if (v.Visual == visual) return true;

            return false;
        }

        public void SetTheScene(Scene[] scenes)
        {
            ClearGraphics();
            Scenecontrol = scenes;
            Show();
        }

        public Material Material
        {
            get
            {
                return _visualmaterial;
            }
        }

    }
}
