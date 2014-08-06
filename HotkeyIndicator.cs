﻿using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiveSplit
{
    public class HotkeyIndicator : IComponent
    {
        public float PaddingTop { get { return 0f; } }
        public float PaddingLeft { get { return 0f; } }
        public float PaddingBottom { get { return 0f; } }
        public float PaddingRight { get { return 0f; } }

        protected LineComponent Line { get; set; }

        public Color CurrentColor { get; set; }

        protected Color EnabledColor = Color.FromArgb(41, 204, 84);
        protected Color DisabledColor = Color.FromArgb(204, 55, 41);

        public GraphicsCache Cache { get; set; }

        public float VerticalHeight
        {
            get { return 3f; }
        }

        public float MinimumWidth
        {
            get { return 0; }
        }

        public float HorizontalWidth
        {
            get { return 3f; }
        }

        public float MinimumHeight
        {
            get { return 0; }
        }

        public HotkeyIndicator()
        {
            Line = new LineComponent(3, Color.White);
            Cache = new GraphicsCache();
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            var oldClip = g.Clip;
            var oldMatrix = g.Transform;
            var oldMode = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            g.Clip = new Region();
            Line.LineColor = CurrentColor;
            var scale = g.Transform.Elements.First();
            var newHeight = Math.Max((int)(3f * scale + 0.5f), 1) / scale;
            Line.VerticalHeight = newHeight;
            g.TranslateTransform(0, (3f - newHeight) / 2f);
            Line.DrawVertical(g, state, width, clipRegion);
            g.Clip = oldClip;
            g.Transform = oldMatrix;
            g.SmoothingMode = oldMode;
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            var oldClip = g.Clip;
            var oldMatrix = g.Transform;
            var oldMode = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            g.Clip = new Region();
            Line.LineColor = CurrentColor;
            var scale = g.Transform.Elements.First();
            var newWidth = Math.Max((int)(3f * scale + 0.5f), 1) / scale;
            g.TranslateTransform((3f - newWidth) / 2f, 0);
            Line.HorizontalWidth = newWidth;
            Line.DrawHorizontal(g, state, height, clipRegion);
            g.Clip = oldClip;
            g.Transform = oldMatrix;
            g.SmoothingMode = oldMode;
        }

        public string ComponentName
        {
            get { return "Hotkey Indicator"; }
        }


        public Control GetSettingsControl(LayoutMode mode)
        {
            return null;
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
        }


        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return document.CreateElement("HotkeyIndicatorSettings");
        }

        
        public IDictionary<string, Action> ContextMenuControls
        {
            get { return null; }
        }


        public void RenameComparison(string oldName, string newName)
        {
        }


        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            CurrentColor = state.Settings.GlobalHotkeysEnabled ? EnabledColor : DisabledColor;

            Cache.Restart();
            Cache["IndicatorColor"] = CurrentColor.ToArgb();

            if (invalidator != null && Cache.HasChanged)
            {
                if (mode == LayoutMode.Vertical)
                    invalidator.Invalidate(0, -1, width, height + 2);
                else
                    invalidator.Invalidate(-1, 0, width + 2, height);
            }
        }

        public void Dispose()
        {
        }
    }
}
