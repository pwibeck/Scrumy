using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using PeterWibeck.ScrumyVSPlugin.TFS;

namespace PeterWibeck.ScrumyVSPlugin
{
    public partial class SettingsForm : Form
    {
        private readonly TfsHelper tfsHelper;
        public Settings Settings = new Settings();

        public SettingsForm()
        {
            InitializeComponent();

            Settings.LoadSettings();
            BuildTabs();
        }

        public SettingsForm(TfsHelper tfsHelper)
        {
            InitializeComponent();

            this.tfsHelper = tfsHelper;
            Settings.LoadSettings();
            BuildTabs();
        }

        private void BuildTabs()
        {
            AdvanceDocument.Text = Settings.Document;

            TabControl.TabPages.Clear();
            foreach (var printWorkItem in Settings.PrintWorkItems)
            {
                var tab = new TabPage {Name = printWorkItem.Type, Text = printWorkItem.Type};

                var panel = new WorkItemTab(Settings.Fonts, tfsHelper)
                {
                    BackgroundColor = Color.FromArgb(printWorkItem.BackGroundColor),
                    TextColor = Color.FromArgb(printWorkItem.TextColor),
                    Rows = printWorkItem.Rows,
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Dock = DockStyle.Fill
                };
                tab.Controls.Add(panel);
                TabControl.TabPages.Add(tab);
                tab.Refresh();
            }
            
            TabControl.TabPages.Add(AdvancedTab);
            
            this.SetupFontTab();
        }

        private void SetupFontTab()
        {
            this.TabControl.TabPages.Add(this.FontsTab);
            int fontYPos = 30;
            foreach (var font in this.Settings.Fonts)
            {
                var fontTextBox = new TextBox
                                      {
                                          Location = new Point(7, fontYPos),
                                          Name = "FontLabel_" + font.Key,
                                          Size = new Size(100, 20),
                                          Text = font.Key
                                      };
                this.FontsTab.Controls.Add(fontTextBox);

                var fontLabel = new Label
                                    {
                                        AutoSize = true,
                                        Location = new Point(123, fontYPos),
                                        Name = "FontStyle_" + font.Key,
                                        Size = new Size(150, 13),
                                        Text = font.Value.Name + " " + font.Value.Size + " " + font.Value.Style
                                    };
                this.FontsTab.Controls.Add(fontLabel);

                var changeFontButton = new Button
                                           {
                                               Location = new Point(290, fontYPos),
                                               Name = "ChangeFontButton_" + font.Key,
                                               Size = new Size(75, 23),
                                               TabIndex = 4,
                                               Text = "Change font",
                                               UseVisualStyleBackColor = true
                                           };
                changeFontButton.Click += this.ChangeFontButtonClick;
                this.FontsTab.Controls.Add(changeFontButton);

                fontYPos += 30;
            }
        }

        private void ChangeFontButtonClick(object sender, EventArgs e)
        {
            var button = (Button) sender;

            string[] nameSplited = button.Name.Split('_');
            Font font;
            if (Settings.Fonts.TryGetValue(nameSplited[1], out font))
            {
                fontDialog1.Font = font;
                if (fontDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (Control control in FontsTab.Controls)
                    {
                        if (
                            string.Compare(control.Name, "FontStyle_" + nameSplited[1],
                                           StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            control.Text = fontDialog1.Font.Name + " " + fontDialog1.Font.Size + " " +
                                           fontDialog1.Font.Style;
                            break;
                        }
                    }

                    Settings.Fonts[nameSplited[1]] = fontDialog1.Font;
                }
            }
        }
        
        private void BtnOkClick(object sender, EventArgs e)
        {
            var copyOfSettings = new Settings();
            copyOfSettings.LoadSettings();

            bool settingsOk = false;
            if (string.Compare(AdvanceDocument.Text, copyOfSettings.Document, StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Copy work item data to copy of settings
                foreach (WorkItemPrintData printWorkItem in copyOfSettings.PrintWorkItems)
                {
                    foreach (TabPage tab in TabControl.TabPages)
                    {
                        if (tab.Name == printWorkItem.Type)
                        {
                            var workitem = tab.Controls[0] as WorkItemTab;
                            printWorkItem.BackGroundColor = workitem.BackgroundColor.ToArgb();
                            printWorkItem.TextColor = workitem.TextColor.ToArgb();
                            printWorkItem.Rows = workitem.Rows;
                        }
                    }
                }

                // Copy font data to copy of settings
                foreach (var font in Settings.Fonts)
                {
                    copyOfSettings.Fonts[font.Key] = font.Value;
                }

                // Update the font name if changed
                foreach (Control control in FontsTab.Controls)
                {
                    if (!control.Name.StartsWith("FontLabel_", StringComparison.OrdinalIgnoreCase))
                        continue;

                    string[] nameSplited = control.Name.Split('_');
                    if (string.Compare(control.Text, nameSplited[1], StringComparison.Ordinal) != 0)
                    {
                        Font font = copyOfSettings.Fonts[nameSplited[1]];
                        copyOfSettings.Fonts.Remove(nameSplited[1]);
                        copyOfSettings.Fonts.Add(control.Text, font);
                    }
                }

                try
                {
                    copyOfSettings.SaveSettings();
                    copyOfSettings.LoadSettings();
                    settingsOk = true;
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
                    settingsOk = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error:" + exception.Message, "Error when saving data", MessageBoxButtons.OK);
                }
            }

            if (settingsOk)
            {
                Close();
            }
            else
            {
                Settings.SaveSettings();
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnResetClick(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(
                    "This will reset your setting to default. Are you sure?",
                    "Reset to default settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Settings = new Settings();
                Settings.SaveSettings();
                BuildTabs();
            }
        }
    }
}