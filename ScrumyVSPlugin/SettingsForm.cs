using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PeterWibeck.ScrumyVSPlugin.TFS;

namespace PeterWibeck.ScrumyVSPlugin
{
    public partial class SettingsForm : Form
    {
        public Settings settings = new Settings();

        public SettingsForm()
        {
            InitializeComponent();

            settings.LoadSettings();
            BuildTabs();
        }

        private void BuildTabs()
        {
            AdvanceDocument.Text = settings.Document;

            TabControl.TabPages.Clear();
            foreach (WorkItemPrintData printWorkItem in settings.PrintWorkItems)
            {
                var tab = new TabPage();
                tab.Name = printWorkItem.Type;
                tab.Text = printWorkItem.Type;

                var backgroundColor = new Label();
                backgroundColor.Name = "BGColor";
                backgroundColor.Text = "Background Color";
                backgroundColor.Location = new Point(6, 15);
                backgroundColor.Size = new Size(92, 13);
                tab.Controls.Add(backgroundColor);

                var backgroundColorPanel = new Panel();
                backgroundColorPanel.Location = new Point(104, 7);
                backgroundColorPanel.Name = "BGColorPanel";
                backgroundColorPanel.Size = new Size(45, 22);
                backgroundColorPanel.BackColor = Color.FromArgb(printWorkItem.BackGroundColor);
                tab.Controls.Add(backgroundColorPanel);

                var changeBGColorButton = new Button();
                changeBGColorButton.Name = printWorkItem.Type + "_" + "BG";
                changeBGColorButton.Text = "Change";
                changeBGColorButton.Location = new Point(158, 6);
                changeBGColorButton.Size = new Size(75, 23);
                changeBGColorButton.UseVisualStyleBackColor = true;
                changeBGColorButton.Click += pickColorButton_Click;
                tab.Controls.Add(changeBGColorButton);

                var textColor = new Label();
                textColor.Name = "TextColor";
                textColor.Text = "Text Color";
                textColor.Location = new Point(6, 40);
                textColor.Size = new Size(55, 13);
                tab.Controls.Add(textColor);

                var textColorPanel = new Panel();
                textColorPanel.Location = new Point(66, 35);
                textColorPanel.Name = "TextColorPanel";
                textColorPanel.Size = new Size(45, 22);
                textColorPanel.BackColor = Color.FromArgb(printWorkItem.TextColor);
                tab.Controls.Add(textColorPanel);

                var changeTextColorButton = new Button();
                changeTextColorButton.Name = printWorkItem.Type + "_" + "TEXT";
                changeTextColorButton.Text = "Change";
                changeTextColorButton.Location = new Point(117, 35);
                changeTextColorButton.Size = new Size(75, 23);
                changeTextColorButton.Click += pickColorButton_Click;
                changeTextColorButton.UseVisualStyleBackColor = true;
                tab.Controls.Add(changeTextColorButton);

                var rowRawData = new TextBox();
                rowRawData.Location = new Point(9, 69);
                rowRawData.Multiline = true;
                rowRawData.Name = "RowRawData";
                rowRawData.Text = printWorkItem.RowsRawData;
                rowRawData.Size = new Size(392, 202);
                rowRawData.TabIndex = 60;
                rowRawData.WordWrap = false;
                rowRawData.ScrollBars = ScrollBars.Both;
                tab.Controls.Add(rowRawData);

                TabControl.TabPages.Add(tab);
                tab.Refresh();
            }

            TabControl.TabPages.Add(FontsTab);
            TabControl.TabPages.Add(AdvancedTab);
        }

        private void pickColorButton_Click(object sender, EventArgs e)
        {
            var button = (Button) sender;

            string[] nameSplited = button.Name.Split('_');
            foreach (TabPage tabPage in TabControl.TabPages.Cast<TabPage>().Where(tabPage => tabPage.Name.Equals(nameSplited[0])))
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control.Name.Equals("TextColorPanel") && nameSplited[1].Equals("TEXT"))
                    {
                        ChangeColor((Panel) control);
                        break;
                    }

                    if (control.Name.Equals("BGColorPanel") && nameSplited[1].Equals("BG"))
                    {
                        ChangeColor((Panel) control);
                        break;
                    }
                }

                break;
            }
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            var copyOfSettings = new Settings();
            copyOfSettings.LoadSettings();

            bool settingsOK = false;
            if (string.Compare(AdvanceDocument.Text, copyOfSettings.Document, StringComparison.OrdinalIgnoreCase) == 0)
            {
                foreach (WorkItemPrintData printWorkItem in copyOfSettings.PrintWorkItems)
                {
                    foreach (Control control in from TabPage tabPage in TabControl.TabPages
                                                where tabPage.Name.Equals(printWorkItem.Type)
                                                from Control control in tabPage.Controls
                                                select control)
                    {
                        if (control.Name.Equals("BGColorPanel"))
                        {
                            printWorkItem.BackGroundColor = control.BackColor.ToArgb();
                        }

                        if (control.Name.Equals("TextColorPanel"))
                        {
                            printWorkItem.TextColor = control.BackColor.ToArgb();
                        }

                        if (control.Name.Equals("RowRawData"))
                        {
                            printWorkItem.RowsRawData = control.Text;
                        }
                    }
                }

                try
                {
                    copyOfSettings.SaveSettings();
                    copyOfSettings.LoadSettings();
                    settingsOK = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error:" + exception.Message, "Error when saving data", MessageBoxButtons.OK);
                }
            }
            else
            {
                try
                {
                    copyOfSettings.Document = AdvanceDocument.Text;
                    copyOfSettings.SaveSettings();
                    copyOfSettings.LoadSettings();
                    settingsOK = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error:" + exception.Message, "Error when saving data", MessageBoxButtons.OK);
                }
                
            }

            if (settingsOK)
            {
                Close();
            }
            else
            {
                settings.SaveSettings();
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangeColor(Panel panel)
        {
            colorDialog1.Color = panel.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel.BackColor = colorDialog1.Color;
            }
        }

        private void BtnResetClick(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(
                    "This will reset your setting to default. Are you sure?",
                    "Reset to default settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                settings = new Settings();
                settings.SaveSettings();
                BuildTabs();
            }
        }
    }
}