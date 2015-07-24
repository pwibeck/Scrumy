namespace PeterWibeck.ScrumyVSPlugin
{
    partial class WorkItemTab
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.BackgroundColorLabel = new System.Windows.Forms.Label();
            this.BackgroundColorPanel = new System.Windows.Forms.Panel();
            this.ChangeBackgroundColor = new System.Windows.Forms.Button();
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.ChangeTextColor = new System.Windows.Forms.Button();
            this.TextColorPanel = new System.Windows.Forms.Panel();
            this.TextColorLabel = new System.Windows.Forms.Label();
            this.LayoutLabel = new System.Windows.Forms.Label();
            this.RowDataPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // BackgroundColorLabel
            // 
            this.BackgroundColorLabel.AutoSize = true;
            this.BackgroundColorLabel.Location = new System.Drawing.Point(3, 9);
            this.BackgroundColorLabel.Name = "BackgroundColorLabel";
            this.BackgroundColorLabel.Size = new System.Drawing.Size(119, 17);
            this.BackgroundColorLabel.TabIndex = 0;
            this.BackgroundColorLabel.Text = "Background color";
            // 
            // BackgroundColorPanel
            // 
            this.BackgroundColorPanel.Location = new System.Drawing.Point(129, 8);
            this.BackgroundColorPanel.Name = "BackgroundColorPanel";
            this.BackgroundColorPanel.Size = new System.Drawing.Size(55, 23);
            this.BackgroundColorPanel.TabIndex = 1;
            // 
            // ChangeBackgroundColor
            // 
            this.ChangeBackgroundColor.Location = new System.Drawing.Point(200, 8);
            this.ChangeBackgroundColor.Name = "ChangeBackgroundColor";
            this.ChangeBackgroundColor.Size = new System.Drawing.Size(75, 23);
            this.ChangeBackgroundColor.TabIndex = 2;
            this.ChangeBackgroundColor.Text = "Change";
            this.ChangeBackgroundColor.UseVisualStyleBackColor = true;
            this.ChangeBackgroundColor.Click += new System.EventHandler(this.ChangeBackgroundColor_Click);
            // 
            // ChangeTextColor
            // 
            this.ChangeTextColor.Location = new System.Drawing.Point(200, 37);
            this.ChangeTextColor.Name = "ChangeTextColor";
            this.ChangeTextColor.Size = new System.Drawing.Size(75, 23);
            this.ChangeTextColor.TabIndex = 5;
            this.ChangeTextColor.Text = "Change";
            this.ChangeTextColor.UseVisualStyleBackColor = true;
            this.ChangeTextColor.Click += new System.EventHandler(this.ChangeTextColor_Click);
            // 
            // TextColorPanel
            // 
            this.TextColorPanel.Location = new System.Drawing.Point(129, 37);
            this.TextColorPanel.Name = "TextColorPanel";
            this.TextColorPanel.Size = new System.Drawing.Size(55, 23);
            this.TextColorPanel.TabIndex = 4;
            // 
            // TextColorLabel
            // 
            this.TextColorLabel.AutoSize = true;
            this.TextColorLabel.Location = new System.Drawing.Point(3, 38);
            this.TextColorLabel.Name = "TextColorLabel";
            this.TextColorLabel.Size = new System.Drawing.Size(70, 17);
            this.TextColorLabel.TabIndex = 3;
            this.TextColorLabel.Text = "Text color";
            // 
            // LayoutLabel
            // 
            this.LayoutLabel.AutoSize = true;
            this.LayoutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LayoutLabel.Location = new System.Drawing.Point(6, 72);
            this.LayoutLabel.Name = "LayoutLabel";
            this.LayoutLabel.Size = new System.Drawing.Size(57, 17);
            this.LayoutLabel.TabIndex = 6;
            this.LayoutLabel.Text = "Layout";
            // 
            // RowDataPanel
            // 
            this.RowDataPanel.Location = new System.Drawing.Point(9, 93);
            this.RowDataPanel.Name = "RowDataPanel";
            this.RowDataPanel.Size = new System.Drawing.Size(587, 220);
            this.RowDataPanel.TabIndex = 7;
            // 
            // WorkItemTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RowDataPanel);
            this.Controls.Add(this.LayoutLabel);
            this.Controls.Add(this.ChangeTextColor);
            this.Controls.Add(this.TextColorPanel);
            this.Controls.Add(this.TextColorLabel);
            this.Controls.Add(this.ChangeBackgroundColor);
            this.Controls.Add(this.BackgroundColorPanel);
            this.Controls.Add(this.BackgroundColorLabel);
            this.Name = "WorkItemTab";
            this.Size = new System.Drawing.Size(712, 348);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BackgroundColorLabel;
        private System.Windows.Forms.Panel BackgroundColorPanel;
        private System.Windows.Forms.Button ChangeBackgroundColor;
        private System.Windows.Forms.ColorDialog ColorDialog;
        private System.Windows.Forms.Button ChangeTextColor;
        private System.Windows.Forms.Panel TextColorPanel;
        private System.Windows.Forms.Label TextColorLabel;
        private System.Windows.Forms.Label LayoutLabel;
        private System.Windows.Forms.Panel RowDataPanel;
    }
}
