using System;
using System.Drawing;
using System.Windows.Forms;

namespace PeterWibeck.ScrumyVSPlugin
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Microsoft.TeamFoundation.WorkItemTracking.Client;

    using PeterWibeck.ScrumyVSPlugin.TFS;

    public partial class WorkItemTab : UserControl
    {
        private readonly Dictionary<string, Font> fonts;

        private readonly TfsHelper tfsHelper;

        public Color BackgroundColor
        {
            get
            {
                return this.BackgroundColorPanel.BackColor;
            }
            set
            {
                this.BackgroundColorPanel.BackColor = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return this.TextColorPanel.BackColor;
            }
            set
            {
                this.TextColorPanel.BackColor = value;
            }
        }

        public Collection<Row> Rows
        {
            set
            {
                RenderRows(value);
            }
        }

        public WorkItemTab(Dictionary<string, Font> fonts, TfsHelper tfsHelper)
        {
            this.fonts = fonts;
            this.tfsHelper = tfsHelper;
            InitializeComponent();
        }

        private void ChangeBackgroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = this.BackgroundColor;
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackgroundColor = ColorDialog.Color;
            }
        }

        private void ChangeTextColor_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = this.TextColor;
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                this.TextColor = ColorDialog.Color;
            }
        }

        private void RenderRows(Collection<Row> rows)
        {
            int rownumber = 0;
            foreach (Row row in rows)
            {
                RowItem rowItem = new RowItem(row, rownumber, fonts, tfsHelper);
                RowDataPanel.Controls.Add(rowItem);
                rownumber++;
            }
        }
    }
}
