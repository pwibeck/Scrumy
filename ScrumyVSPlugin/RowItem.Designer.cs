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
            this.RowElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RowElements.Location = new System.Drawing.Point(12, 36);
            this.RowElements.Name = "RowElements";
            this.RowElements.RowTemplate.Height = 24;
            this.RowElements.Size = new System.Drawing.Size(511, 168);
            this.RowElements.TabIndex = 9;
            // 
            // FontSelection
            // 
            this.FontSelection.FormattingEnabled = true;
            this.FontSelection.Location = new System.Drawing.Point(242, 6);
            this.FontSelection.Name = "FontSelection";
            this.FontSelection.Size = new System.Drawing.Size(113, 24);
            this.FontSelection.TabIndex = 8;
            // 
            // FontLabel
            // 
            this.FontLabel.AutoSize = true;
            this.FontLabel.Location = new System.Drawing.Point(206, 10);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new System.Drawing.Size(40, 17);
            this.FontLabel.TabIndex = 7;
            this.FontLabel.Text = "Font:";
            // 
            // AligmentSelection
            // 
            this.AligmentSelection.FormattingEnabled = true;
            this.AligmentSelection.Items.AddRange(new object[] {
            "Top",
            "Bottom"});
            this.AligmentSelection.Location = new System.Drawing.Point(79, 6);
            this.AligmentSelection.Name = "AligmentSelection";
            this.AligmentSelection.Size = new System.Drawing.Size(121, 24);
            this.AligmentSelection.TabIndex = 6;
            // 
            // AlignmentLabel
            // 
            this.AlignmentLabel.AutoSize = true;
            this.AlignmentLabel.Location = new System.Drawing.Point(9, 10);
            this.AlignmentLabel.Name = "AlignmentLabel";
            this.AlignmentLabel.Size = new System.Drawing.Size(74, 17);
            this.AlignmentLabel.TabIndex = 5;
            this.AlignmentLabel.Text = "Alignment:";
            // 
            // RowItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RowElements);
            this.Controls.Add(this.FontSelection);
            this.Controls.Add(this.FontLabel);
            this.Controls.Add(this.AligmentSelection);
            this.Controls.Add(this.AlignmentLabel);
            this.Name = "RowItem";
            this.Size = new System.Drawing.Size(526, 210);
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
