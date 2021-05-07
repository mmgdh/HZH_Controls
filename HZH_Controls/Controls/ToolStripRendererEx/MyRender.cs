using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace HZH_Controls.Controls
{
    public class MyRender 
        : ToolStripRenderer
    {
        private static readonly int OffsetMargin = 24;
        private const int m_radius = 2;
        private const int m_borderRadius = 2;
        private string _menuLogoString = "";
        private ToolStripColorTable _colorTable;

        public MyRender()
            : base()
        {
        }

        public MyRender(
            ToolStripColorTable colorTable) : base()
        {
            _colorTable = colorTable;
        }

        public string MenuLogoString
        {
            get { return _menuLogoString; }
            set { _menuLogoString = value; }
        }

        protected virtual ToolStripColorTable ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new ToolStripColorTable();
                }
                return _colorTable;
            }
        }

        protected override void OnRenderToolStripBackground(
            ToolStripRenderEventArgs e)
        {           
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                RegionHelper.CreateRegion(toolStrip, bounds, m_borderRadius);
                using (SolidBrush brush = new SolidBrush(ColorTable.BackNormal))
                {
                    g.FillRectangle(brush, bounds);
                }
            }
            else if (toolStrip is MenuStrip)
            {              
                RenderHelper.RenderBackgroundInternal(
                    g,
                    bounds,
                    Color.White ,
                    ColorTable.Border,
                    RoundStyle.All,
                    2,
                    .35f,
                    false,
                    false);
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        protected override void OnRenderToolStripBorder(
            ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                {
                    using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(bounds, 8, RoundStyle.All, true))
                    {
                        using (Pen pen = new Pen(ColorTable.Border))
                        {
                            //path.Widen(pen);
                            g.DrawPath(pen, path);
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                        }
                    }
                }

                bounds.Inflate(-1, -1);
                using (GraphicsPath innerPath = GraphicsPathHelper.CreatePath(
                    bounds, 8, RoundStyle.All, true))
                {
                    using (Pen pen = new Pen(ColorTable.BackNormal))
                    {
                        g.DrawPath(pen, innerPath);
                    }
                }
            }
            else if (toolStrip is StatusStrip)
            {
                using (Pen pen = new Pen(ColorTable.Border))
                {
                    e.Graphics.DrawRectangle(
                        pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
                }
            }
            else if (toolStrip is MenuStrip)
            {
                base.OnRenderToolStripBorder(e);
            }
            else
            {
                using (Pen pen = new Pen(ColorTable.Border))
                {
                    g.DrawRectangle(
                        pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
                }
            }
        }

        protected override void OnRenderMenuItemBackground(
           ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripItem item = e.Item;

            if (!item.Enabled)
            {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

            if (toolStrip is MenuStrip)
            {
                if (item.Selected)
                {
                    RenderHelper.RenderBackgroundInternal(
                        g,
                        rect,
                        ColorTable.BackHover,
                        ColorTable.BackHover,
                        RoundStyle.All,
                        m_radius,
                        true,
                        true);
                }
                else if (item.Pressed)
                {
                    RenderHelper.RenderBackgroundInternal(
                       g,
                       rect,
                       ColorTable.BackPressed,
                       ColorTable.BackPressed,
                       RoundStyle.All,
                       m_radius,
                       true,
                       true);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
            else if (toolStrip is ToolStripDropDown)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);

                int offsetMargin = bDrawLogo ? OffsetMargin : 0;

                if (item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X += 4;
                }
                else
                {
                    rect.X += offsetMargin + 4;
                }
                rect.Width -= offsetMargin + 8;
                rect.Height--;

                if (item.Selected)
                {
                    RenderHelper.RenderBackgroundInternal(
                       g,
                       rect,
                       ColorTable.BackHover,
                       ColorTable.BackHover,
                       RoundStyle.All,
                       m_radius,
                       true,
                       true);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderItemText(
            ToolStripItemTextRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;

            e.TextColor = ColorTable.Fore;

            if (toolStrip is ToolStripDropDown &&
                e.Item is ToolStripMenuItem)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);

                int offsetMargin = bDrawLogo ? 18 : 0;

                Rectangle rect = e.TextRectangle;
                if (e.Item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X -= offsetMargin;
                }
                else
                {
                    rect.X += offsetMargin;
                }
                rect.Height = e.Item.Height;
                rect.Y = 0;
               
                e.TextRectangle = rect;
            }
            e.Graphics.DrawString(e.Text, e.TextFont, new SolidBrush(e.TextColor), e.TextRectangle, new StringFormat() { LineAlignment= StringAlignment.Center });
            //base.OnRenderItemText(e);
        }

        internal bool NeedDrawLogo(ToolStrip toolStrip)
        {
            ToolStripDropDown dropDown = toolStrip as ToolStripDropDown;
            bool bDrawLogo =
                (dropDown.OwnerItem != null &&
                dropDown.OwnerItem.Owner is MenuStrip) ||
                (toolStrip is ContextMenuStrip);
            //return bDrawLogo;
            return false;
        }
    }
}
