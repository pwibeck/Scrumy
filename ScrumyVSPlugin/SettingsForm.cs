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
                var tab = new TabPage {Name = printWorkItem.Type, Text = printWorkItem.Type};

                var backgroundColor = new Label
                                          {
                                              Name = "BGColor",
                                              Text = "Background Color",
                                              Location = new Point(6, 15),
                                              Size = new Size(92, 13)
                                          };
                tab.Controls.Add(backgroundColor);

                var backgroundColorPanel = new Panel
                                               {
                                                   Location = new Point(104, 7),
                                                   Name = "BGColorPanel",
                                                   Size = new Size(45, 22),
                                                   BackColor = Color.FromArgb(printWorkItem.BackGroundColor)
                                               };
                tab.Controls.Add(backgroundColorPanel);

                var changeBgColorButton = new Button
                                              {
                                                  Name = printWorkItem.Type + "_" + "BG",
                                                  Text = "Change",
                                                  Location = new Point(158, 6),
                                                  Size = new Size(75, 23),
                                                  UseVisualStyleBackColor = true
                                              };
                changeBgColorButton.Click += PickColorButtonClick;
                tab.Controls.Add(changeBgColorButton);

                var textColor = new Label
                                    {
                                        Name = "TextColor",
                                        Text = "Text Color",
                                        Location = new Point(6, 40),
                                        Size = new Size(55, 13)
                                    };
                tab.Controls.Add(textColor);

                var textColorPanel = new Panel
                                         {
                                             Location = new Point(66, 35),
                                             Name = "TextColorPanel",
                                             Size = new Size(45, 22),
                                             BackColor = Color.FromArgb(printWorkItem.TextColor)
                                         };
                tab.Controls.Add(textColorPanel);

                var changeTextColorButton = new Button
                                                {
                                                    Name = printWorkItem.Type + "_" + "TEXT",
                                                    Text = "Change",
                                                    Location = new Point(117, 35),
                                                    Size = new Size(75, 23),
                                                    UseVisualStyleBackColor = true
                                                };
                changeTextColorButton.Click += PickColorButtonClick;
                tab.Controls.Add(changeTextColorButton);

                var rowRawData = new TextBox
                                     {
                                         Location = new Point(9, 69),
                                         Multiline = true,
                                         Name = printWorkItem.Type + "_RowRawData",
                                         Text = printWorkItem.RowsRawData,
                                         Size = new Size(392, 202),
                                         TabIndex = 60,
                                         WordWrap = false,
                                         ScrollBars = ScrollBars.Both
                                     };
                rowRawData.TextChanged += RowRawDataTextChanged;
                tab.Controls.Add(rowRawData);

                TabControl.TabPages.Add(tab);
                tab.Refresh();
            }

            TabControl.TabPages.Add(FontsTab);
            TabControl.TabPages.Add(AdvancedTab);

            int fontYPos = 30;
            foreach (var font in settings.Fonts)
            {
                var fontTextBox = new TextBox
                                      {
                                          Location = new Point(7, fontYPos),
                                          Name = "FontLabel_" + font.Key,
                                          Size = new Size(100, 20),
                                          Text = font.Key
                                      };
                FontsTab.Controls.Add(fontTextBox);

                var fontLabel = new Label
                                    {
                                        AutoSize = true,
                                        Location = new Point(123, fontYPos),
                                        Name = "FontStyle_" + font.Key,
                                        Size = new Size(150, 13),
                                        Text = font.Value.Name + " " + font.Value.Size + " " + font.Value.Style
                                    };
                FontsTab.Controls.Add(fontLabel);

                var changeFontButton = new Button
                                           {
                                               Location = new Point(290, fontYPos),
                                               Name = "ChangeFontButton_" + font.Key,
                                               Size = new Size(75, 23),
                                               TabIndex = 4,
                                               Text = "Change font",
                                               UseVisualStyleBackColor = true
                                           };
                changeFontButton.Click += ChangeFontButtonClick;
                FontsTab.Controls.Add(changeFontButton);

                fontYPos += 30;
            }
        }

        private void RowRawDataTextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox) sender;

            string[] nameSplited = textBox.Name.Split('_');
            WorkItemPrintData workItem =
                settings.PrintWorkItems.Where(item => item.Type.Equals(nameSplited[0])).
                    ToArray()[0];
            workItem.RowsRawData = textBox.Text;
        }

        private void ChangeFontButtonClick(object sender, EventArgs e)
        {
            var button = (Button) sender;

            string[] nameSplited = button.Name.Split('_');
            Font font;
            if (settings.Fonts.TryGetValue(nameSplited[1], out font))
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

                    settings.Fonts[nameSplited[1]] = fontDialog1.Font;
                }
            }
        }

        private void PickColorButtonClick(object sender, EventArgs e)
        {
            var button = (Button) sender;

            string[] nameSplited = button.Name.Split('_');
            WorkItemPrintData workItem =
                settings.PrintWorkItems.Where(item => item.Type.Equals(nameSplited[0])).
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

            var settingsOk = false;
            if (string.Compare(AdvanceDocument.Text, copyOfSettings.Document, StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Copy work item data to copy of settings
                foreach (WorkItemPrintData printWorkItem in copyOfSettings.PrintWorkItems)
                {
                    foreach (WorkItemPrintData setting in settings.PrintWorkItems)
                    {
                        if (printWorkItem.Type.Equals(setting.Type))
                        {
                            printWorkItem.BackGroundColor = setting.BackGroundColor;
                            printWorkItem.TextColor = setting.TextColor;
                            printWorkItem.RowsRawData = setting.RowsRawData;
                        }
                    }
                }

                // Copy font data to copy of settings
                foreach (var font in settings.Fonts)
                {
                    copyOfSettings.Fonts[font.Key] = font.Value;
                }

                // Update the font name if changed
                foreach (Control control in FontsTab.Controls)
                {
                    if (control.Name.StartsWith("FontLabel_", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] nameSplited = control.Name.Split('_');
                        if (string.Compare(control.Text, nameSplited[1], StringComparison.Ordinal) != 0)
                        {
                            Font font = copyOfSettings.Fonts[nameSplited[1]];
                            copyOfSettings.Fonts.Remove(nameSplited[1]);
                            copyOfSettings.Fonts.Add(control.Text, font);
                        }
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