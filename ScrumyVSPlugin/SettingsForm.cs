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

        private void AlignmentSelectionSelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
                return;

            int row = int.Parse(comboBox.Name.Split('_')[2]);
            string itemType = comboBox.Name.Split('_')[1];
            WorkItemPrintData item =
                Settings.PrintWorkItems.FirstOrDefault(workItemPrintData => workItemPrintData.Type.Equals(itemType));

            if (item != null)
            {
                item.Rows[row].Alignment = comboBox.SelectedItem.ToString();
            }
        }

        private void FontSelectionSelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
                return;

            int row = int.Parse(comboBox.Name.Split('_')[2]);
            string itemType = comboBox.Name.Split('_')[1];
            WorkItemPrintData item =
                Settings.PrintWorkItems.FirstOrDefault(workItemPrintData => workItemPrintData.Type.Equals(itemType));

            if (item != null)
            {
                item.Rows[row].Font = comboBox.SelectedItem.ToString();
            }
        }

        private static void RowLableLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var label = sender as LinkLabel;
            if (label == null)
                return;

            int row = int.Parse(label.Name.Split('_')[1]);

            // Find first row link postion
            int startY =
                (from Control s in label.Parent.Controls where s.Name.Equals("RowLink_0") select s.Location.Y).
                    FirstOrDefault();

            // Reset row link positions
            foreach (
                Control s in
                    from Control s in label.Parent.Controls
                    where s.Name.StartsWith("RowLink_") && !s.Name.Equals("RowLink_0")
                    select s)
            {
                int rowNumber = int.Parse(s.Name.Split('_')[1]);
                s.Location = new Point(s.Location.X, startY + (rowNumber*20));
            }

            // Set new positions
            foreach (Control s in label.Parent.Controls)
            {
                if (s.Name.StartsWith("RowElementPanel_"))
                {
                    s.Visible = s.Name.EndsWith("_" + row);
                }

                if (s.Name.StartsWith("RowLink_"))
                {
                    int rowNumber = int.Parse(s.Name.Split('_')[1]);
                    if (row < rowNumber)
                    {
                        s.Location = new Point(s.Location.X, s.Location.Y + 220);
                    }
                }
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

        private void PickColorButtonClick(object sender, EventArgs e)
        {
            var button = (Button) sender;

            string[] nameSplited = button.Name.Split('_');
            WorkItemPrintData workItem =
                Settings.PrintWorkItems.Where(item => item.Type.Equals(nameSplited[0])).
                    ToArray()[0];
            TabPage tabPage =
                TabControl.TabPages.Cast<TabPage>().Where(page => page.Name.Equals(nameSplited[0])).ToArray()[0];

            foreach (Control control in tabPage.Controls)
            {
                if (control.Name.Equals("TextColorPanel") && nameSplited[1].Equals("TEXT"))
                {
                    ChangeColor((Panel) control);
                    workItem.TextColor = control.BackColor.ToArgb();
                    break;
                }

                if (control.Name.Equals("BGColorPanel") && nameSplited[1].Equals("BG"))
                {
                    ChangeColor((Panel) control);
                    workItem.BackGroundColor = control.BackColor.ToArgb();
                    break;
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
                    foreach (
                        WorkItemPrintData setting in
                            Settings.PrintWorkItems.Where(setting => printWorkItem.Type.Equals(setting.Type)))
                    {
                        printWorkItem.BackGroundColor = setting.BackGroundColor;
                        printWorkItem.TextColor = setting.TextColor;
                        printWorkItem.Rows = setting.Rows;
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
                Settings = new Settings();
                Settings.SaveSettings();
                BuildTabs();
            }
        }

        #region Nested type: RowElementAttributes

        private enum RowElementAttributes
        {
            MaxeLength,
            Text,
            Field,
            DateFormat,
            SearchField,
            SearchData,
            ResultField
        }

        #endregion

        #region Nested type: RowElementType

        private enum RowElementType
        {
            Text,
            Field,
            RelatedItem
        }

        #endregion
    }
}