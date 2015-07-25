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
            this.RowElements = new System.Windows.Forms.DataGridView();
            this.FontSelection = new System.Windows.Forms.ComboBox();
            this.FontLabel = new System.Windows.Forms.Label();
            this.AligmentSelection = new System.Windows.Forms.ComboBox();
            this.AlignmentLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RowElements)).BeginInit();
            this.SuspendLayout();
            // 
            // RowElements
            // 
            this.RowElements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RowElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RowElements.Location = new System.Drawing.Point(0, 29);
            this.RowElements.Margin = new System.Windows.Forms.Padding(2);
            this.RowElements.Name = "RowElements";
            this.RowElements.RowTemplate.Height = 24;
            this.RowElements.Size = new System.Drawing.Size(394, 142);
            this.RowElements.TabIndex = 9;            
            // 
            // FontSelection
            // 
            this.FontSelection.FormattingEnabled = true;
            this.FontSelection.Location = new System.Drawing.Point(182, 5);
            this.FontSelection.Margin = new System.Windows.Forms.Padding(2);
            this.FontSelection.Name = "FontSelection";
            this.FontSelection.Size = new System.Drawing.Size(86, 21);
            this.FontSelection.TabIndex = 8;
            this.FontSelection.SelectedIndexChanged += new System.EventHandler(this.FontSelectedIndexChanged);
            // 
            // FontLabel
            // 
            this.FontLabel.AutoSize = true;
            this.FontLabel.Location = new System.Drawing.Point(154, 8);
            this.FontLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new System.Drawing.Size(31, 13);
            this.FontLabel.TabIndex = 7;
            this.FontLabel.Text = "Font:";
            // 
            // AligmentSelection
            // 
            this.AligmentSelection.FormattingEnabled = true;
            this.AligmentSelection.Items.AddRange(new object[] {
            "Top",
            "Bottom"});
            this.AligmentSelection.Location = new System.Drawing.Point(59, 5);
            this.AligmentSelection.Margin = new System.Windows.Forms.Padding(2);
            this.AligmentSelection.Name = "AligmentSelection";
            this.AligmentSelection.Size = new System.Drawing.Size(92, 21);
            this.AligmentSelection.TabIndex = 6;
            this.AligmentSelection.SelectedIndexChanged += new System.EventHandler(this.AligmentSelectedIndexChanged);
            // 
            // AlignmentLabel
            // 
            this.AlignmentLabel.AutoSize = true;
            this.AlignmentLabel.Location = new System.Drawing.Point(7, 8);
            this.AlignmentLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AlignmentLabel.Name = "AlignmentLabel";
            this.AlignmentLabel.Size = new System.Drawing.Size(56, 13);
            this.AlignmentLabel.TabIndex = 5;
            this.AlignmentLabel.Text = "Alignment:";
            // 
            // RowItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RowElements);
            this.Controls.Add(this.FontSelection);
            this.Controls.Add(this.FontLabel);
            this.Controls.Add(this.AligmentSelection);
            this.Controls.Add(this.AlignmentLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RowItem";
            this.Size = new System.Drawing.Size(394, 171);
            ((System.ComponentModel.ISupportInitialize)(this.RowElements)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView RowElements;
        private ComboBox FontSelection;
        private Label FontLabel;
        private ComboBox AligmentSelection;
        private Label AlignmentLabel;
    }
}
