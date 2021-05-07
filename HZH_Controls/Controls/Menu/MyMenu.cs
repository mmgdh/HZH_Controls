using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HZH_Controls.Controls.Menu
{
    public class MyMenu : ContextMenuStrip
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void InitializeComponent()
        {
            
            this.SuspendLayout();
            // 
            // MyMenu
            // 
            this.DropShadowEnabled = false;
            this.Font = new System.Drawing.Font("Nirmala UI", 11F);
            this.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.ShowImageMargin = false;
            this.ShowItemToolTips = false;
            this.Size = new System.Drawing.Size(36, 4);
            this.ResumeLayout(false);

        }
    }
}
