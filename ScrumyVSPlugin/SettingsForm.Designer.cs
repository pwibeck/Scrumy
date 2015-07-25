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
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AdvancedTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.AdvanceDocument = new System.Windows.Forms.TextBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.TabControl.SuspendLayout();
            this.FontsTab.SuspendLayout();
            this.AdvancedTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(458, 421);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(377, 421);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.FontsTab);
            this.TabControl.Controls.Add(this.AdvancedTab);
            this.TabControl.Location = new System.Drawing.Point(3, 3);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(541, 412);
            this.TabControl.TabIndex = 60;
            // 
            // FontsTab
            // 
            this.FontsTab.Controls.Add(this.label4);
            this.FontsTab.Controls.Add(this.label2);
            this.FontsTab.Location = new System.Drawing.Point(4, 22);
            this.FontsTab.Name = "FontsTab";
            this.FontsTab.Padding = new System.Windows.Forms.Padding(3);
            this.FontsTab.Size = new System.Drawing.Size(533, 386);
            this.FontsTab.TabIndex = 0;
            this.FontsTab.Text = "Fonts";
            this.FontsTab.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(126, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Style";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name";
            // 
            // AdvancedTab
            // 
            this.AdvancedTab.Controls.Add(this.label1);
            this.AdvancedTab.Controls.Add(this.btnReset);
            this.AdvancedTab.Controls.Add(this.AdvanceDocument);
            this.AdvancedTab.Location = new System.Drawing.Point(4, 22);
            this.AdvancedTab.Name = "AdvancedTab";
            this.AdvancedTab.Padding = new System.Windows.Forms.Padding(3);
            this.AdvancedTab.Size = new System.Drawing.Size(533, 386);
            this.AdvancedTab.TabIndex = 1;
            this.AdvancedTab.Text = "Advanced";
            this.AdvancedTab.UseVisualStyleBackColor = true;
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
            this.AdvanceDocument.Size = new System.Drawing.Size(524, 336);
            this.AdvanceDocument.TabIndex = 0;
            this.AdvanceDocument.WordWrap = false;
            // 
            // fontDialog1
            // 
            this.fontDialog1.Color = System.Drawing.SystemColors.ControlText;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 446);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Name = "SettingsForm";
            this.Text = "Scrumy Settings";
            this.TabControl.ResumeLayout(false);
            this.FontsTab.ResumeLayout(false);
            this.FontsTab.PerformLayout();
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FontDialog fontDialog1;
    }
}