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
            this.RowTabController = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // BackgroundColorLabel
            // 
            this.BackgroundColorLabel.AutoSize = true;
            this.BackgroundColorLabel.Location = new System.Drawing.Point(2, 7);
            this.BackgroundColorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BackgroundColorLabel.Name = "BackgroundColorLabel";
            this.BackgroundColorLabel.Size = new System.Drawing.Size(91, 13);
            this.BackgroundColorLabel.TabIndex = 0;
            this.BackgroundColorLabel.Text = "Background color";
            // 
            // BackgroundColorPanel
            // 
            this.BackgroundColorPanel.Location = new System.Drawing.Point(97, 6);
            this.BackgroundColorPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BackgroundColorPanel.Name = "BackgroundColorPanel";
            this.BackgroundColorPanel.Size = new System.Drawing.Size(41, 19);
            this.BackgroundColorPanel.TabIndex = 1;
            // 
            // ChangeBackgroundColor
            // 
            this.ChangeBackgroundColor.Location = new System.Drawing.Point(150, 6);
            this.ChangeBackgroundColor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ChangeBackgroundColor.Name = "ChangeBackgroundColor";
            this.ChangeBackgroundColor.Size = new System.Drawing.Size(56, 19);
            this.ChangeBackgroundColor.TabIndex = 2;
            this.ChangeBackgroundColor.Text = "Change";
            this.ChangeBackgroundColor.UseVisualStyleBackColor = true;
            this.ChangeBackgroundColor.Click += new System.EventHandler(this.ChangeBackgroundColor_Click);
            // 
            // ChangeTextColor
            // 
            this.ChangeTextColor.Location = new System.Drawing.Point(150, 30);
            this.ChangeTextColor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ChangeTextColor.Name = "ChangeTextColor";
            this.ChangeTextColor.Size = new System.Drawing.Size(56, 19);
            this.ChangeTextColor.TabIndex = 5;
            this.ChangeTextColor.Text = "Change";
            this.ChangeTextColor.UseVisualStyleBackColor = true;
            this.ChangeTextColor.Click += new System.EventHandler(this.ChangeTextColor_Click);
            // 
            // TextColorPanel
            // 
            this.TextColorPanel.Location = new System.Drawing.Point(97, 30);
            this.TextColorPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextColorPanel.Name = "TextColorPanel";
            this.TextColorPanel.Size = new System.Drawing.Size(41, 19);
            this.TextColorPanel.TabIndex = 4;
            // 
            // TextColorLabel
            // 
            this.TextColorLabel.AutoSize = true;
            this.TextColorLabel.Location = new System.Drawing.Point(2, 31);
            this.TextColorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TextColorLabel.Name = "TextColorLabel";
            this.TextColorLabel.Size = new System.Drawing.Size(54, 13);
            this.TextColorLabel.TabIndex = 3;
            this.TextColorLabel.Text = "Text color";
            // 
            // LayoutLabel
            // 
            this.LayoutLabel.AutoSize = true;
            this.LayoutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LayoutLabel.Location = new System.Drawing.Point(4, 58);
            this.LayoutLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LayoutLabel.Name = "LayoutLabel";
            this.LayoutLabel.Size = new System.Drawing.Size(45, 13);
            this.LayoutLabel.TabIndex = 6;
            this.LayoutLabel.Text = "Layout";
            // 
            // RowTabController
            // 
            this.RowTabController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RowTabController.Location = new System.Drawing.Point(0, 78);
            this.RowTabController.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RowTabController.Name = "RowTabController";
            this.RowTabController.SelectedIndex = 0;
            this.RowTabController.Size = new System.Drawing.Size(534, 205);
            this.RowTabController.TabIndex = 7;
            // 
            // WorkItemTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RowTabController);
            this.Controls.Add(this.LayoutLabel);
            this.Controls.Add(this.ChangeTextColor);
            this.Controls.Add(this.TextColorPanel);
            this.Controls.Add(this.TextColorLabel);
            this.Controls.Add(this.ChangeBackgroundColor);
            this.Controls.Add(this.BackgroundColorPanel);
            this.Controls.Add(this.BackgroundColorLabel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "WorkItemTab";
            this.Size = new System.Drawing.Size(534, 283);
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
        private System.Windows.Forms.TabControl RowTabController;
    }
}
