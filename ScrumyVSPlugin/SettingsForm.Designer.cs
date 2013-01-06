namespace PeterWibeck.ScrumyVSPlugin
{
    partial class SettingsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.FontsTab = new System.Windows.Forms.TabPage();
            this.AdvancedTab = new System.Windows.Forms.TabPage();
            this.btnReset = new System.Windows.Forms.Button();
            this.AdvanceDocument = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TabControl.SuspendLayout();
            this.AdvancedTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(343, 312);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(262, 312);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.FontsTab);
            this.TabControl.Controls.Add(this.AdvancedTab);
            this.TabControl.Location = new System.Drawing.Point(3, 3);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(415, 303);
            this.TabControl.TabIndex = 60;
            // 
            // FontsTab
            // 
            this.FontsTab.Location = new System.Drawing.Point(4, 22);
            this.FontsTab.Name = "FontsTab";
            this.FontsTab.Padding = new System.Windows.Forms.Padding(3);
            this.FontsTab.Size = new System.Drawing.Size(407, 277);
            this.FontsTab.TabIndex = 0;
            this.FontsTab.Text = "Fonts";
            this.FontsTab.UseVisualStyleBackColor = true;
            // 
            // AdvancedTab
            // 
            this.AdvancedTab.Controls.Add(this.label1);
            this.AdvancedTab.Controls.Add(this.btnReset);
            this.AdvancedTab.Controls.Add(this.AdvanceDocument);
            this.AdvancedTab.Location = new System.Drawing.Point(4, 22);
            this.AdvancedTab.Name = "AdvancedTab";
            this.AdvancedTab.Padding = new System.Windows.Forms.Padding(3);
            this.AdvancedTab.Size = new System.Drawing.Size(407, 277);
            this.AdvancedTab.TabIndex = 1;
            this.AdvancedTab.Text = "Advanced";
            this.AdvancedTab.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(323, 6);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 61;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnResetClick);
            // 
            // AdvanceDocument
            // 
            this.AdvanceDocument.Location = new System.Drawing.Point(3, 44);
            this.AdvanceDocument.Multiline = true;
            this.AdvanceDocument.Name = "AdvanceDocument";
            this.AdvanceDocument.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.AdvanceDocument.Size = new System.Drawing.Size(395, 230);
            this.AdvanceDocument.TabIndex = 0;
            this.AdvanceDocument.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "Any change here will overid all other changes";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 339);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Name = "SettingsForm";
            this.Text = "Scrumy Settings";
            this.TabControl.ResumeLayout(false);
            this.AdvancedTab.ResumeLayout(false);
            this.AdvancedTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage FontsTab;
        private System.Windows.Forms.TabPage AdvancedTab;
        private System.Windows.Forms.TextBox AdvanceDocument;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label1;
    }
}