namespace PeterWibeck.ScrumyVSPlugin
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    partial class RowItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RowLabel = new System.Windows.Forms.LinkLabel();
            this.RowPanel = new System.Windows.Forms.Panel();
            this.RowElements = new System.Windows.Forms.DataGridView();
            this.FontSelection = new System.Windows.Forms.ComboBox();
            this.FontLabel = new System.Windows.Forms.Label();
            this.AligmentSelection = new System.Windows.Forms.ComboBox();
            this.AlignmentLabel = new System.Windows.Forms.Label();
            this.RowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RowElements)).BeginInit();
            this.SuspendLayout();
            // 
            // RowLabel
            // 
            this.RowLabel.AutoSize = true;
            this.RowLabel.Location = new System.Drawing.Point(3, 0);
            this.RowLabel.Name = "RowLabel";
            this.RowLabel.Size = new System.Drawing.Size(47, 17);
            this.RowLabel.TabIndex = 0;
            this.RowLabel.TabStop = true;
            this.RowLabel.Text = "Row #";
            this.RowLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RowLabel_LinkClicked);
            // 
            // RowPanel
            // 
            this.RowPanel.Controls.Add(this.RowElements);
            this.RowPanel.Controls.Add(this.FontSelection);
            this.RowPanel.Controls.Add(this.FontLabel);
            this.RowPanel.Controls.Add(this.AligmentSelection);
            this.RowPanel.Controls.Add(this.AlignmentLabel);
            this.RowPanel.Location = new System.Drawing.Point(6, 21);
            this.RowPanel.Name = "RowPanel";
            this.RowPanel.Size = new System.Drawing.Size(431, 181);
            this.RowPanel.TabIndex = 1;
            this.RowPanel.Visible = false;
            // 
            // RowElements
            // 
            this.RowElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RowElements.Location = new System.Drawing.Point(7, 25);
            this.RowElements.Name = "RowElements";
            this.RowElements.RowTemplate.Height = 24;
            this.RowElements.Size = new System.Drawing.Size(411, 150);
            this.RowElements.TabIndex = 4;
            this.RowElements.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.RowElementGridViewOnCellValidated);
            // 
            // FontSelection
            // 
            this.FontSelection.FormattingEnabled = true;
            this.FontSelection.Location = new System.Drawing.Point(237, 0);
            this.FontSelection.Name = "FontSelection";
            this.FontSelection.Size = new System.Drawing.Size(121, 24);
            this.FontSelection.TabIndex = 3;
            // 
            // FontLabel
            // 
            this.FontLabel.AutoSize = true;
            this.FontLabel.Location = new System.Drawing.Point(201, 4);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new System.Drawing.Size(40, 17);
            this.FontLabel.TabIndex = 2;
            this.FontLabel.Text = "Font:";
            // 
            // AligmentSelection
            // 
            this.AligmentSelection.FormattingEnabled = true;
            this.AligmentSelection.Items.AddRange(new object[] {
            "Top",
            "Bottom"});
            this.AligmentSelection.Location = new System.Drawing.Point(74, 0);
            this.AligmentSelection.Name = "AligmentSelection";
            this.AligmentSelection.Size = new System.Drawing.Size(121, 24);
            this.AligmentSelection.TabIndex = 1;
            // 
            // AlignmentLabel
            // 
            this.AlignmentLabel.AutoSize = true;
            this.AlignmentLabel.Location = new System.Drawing.Point(4, 4);
            this.AlignmentLabel.Name = "AlignmentLabel";
            this.AlignmentLabel.Size = new System.Drawing.Size(74, 17);
            this.AlignmentLabel.TabIndex = 0;
            this.AlignmentLabel.Text = "Alignment:";
            // 
            // RowItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RowPanel);
            this.Controls.Add(this.RowLabel);
            this.Name = "RowItem";
            this.Size = new System.Drawing.Size(526, 261);
            this.RowPanel.ResumeLayout(false);
            this.RowPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RowElements)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LinkLabel RowLabel;
        private Panel RowPanel;
        private Label AlignmentLabel;
        private ComboBox AligmentSelection;
        private Label FontLabel;
        private ComboBox FontSelection;
        private DataGridView RowElements;
    }
}
