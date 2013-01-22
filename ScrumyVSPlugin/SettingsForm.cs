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
            foreach (WorkItemPrintData printWorkItem in Settings.PrintWorkItems)
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

                var layoutLabel = new Label
                                      {
                                          Location = new Point(9, 60),
                                          Text = "Layout",
                                          Size = new Size(392, 13),
                                          Font =
                                              new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point,
                                                       ((0)))
                                      };
                tab.Controls.Add(layoutLabel);


                int rownumber = 0;
                foreach (Row row in printWorkItem.Rows)
                {
                    var rowLable = new LinkLabel
                                       {
                                           Location = new Point(9, 80 + rownumber*20),
                                           Text = string.Format("Row {0}", rownumber),
                                           Size = new Size(400, 13),
                                           Name = "RowLink_" + rownumber
                                       };
                    rowLable.LinkClicked += RowLableLinkClicked;
                    tab.Controls.Add(rowLable);

                    var rowElementPanel = new Panel
                                              {
                                                  Name = "RowElementPanel_" + printWorkItem.Type + "_" + rownumber,
                                                  Location = new Point(9, 97 + rownumber*20),
                                                  Size = new Size(500, 220),
                                                  Visible = false
                                              };
                    tab.Controls.Add(rowElementPanel);

                    rowElementPanel.Controls.Add(new Label
                                                     {Text = "Alignment:", Location = new Point(0, 2), AutoSize = true});
                    var alignmentSelection = new ComboBox
                                                 {
                                                     Name = "AlignmentForRow_" + printWorkItem.Type + "_" + rownumber,
                                                     Location = new Point(55, 0),
                                                 };
                    alignmentSelection.Items.Add("Top");
                    alignmentSelection.Items.Add("Bottom");
                    alignmentSelection.SelectedItem = row.Alignment;
                    alignmentSelection.SelectedIndexChanged += AlignmentSelectionSelectedIndexChanged;
                    rowElementPanel.Controls.Add(alignmentSelection);

                    rowElementPanel.Controls.Add(new Label
                                                     {Text = "Font:", Location = new Point(180, 2), AutoSize = true});
                    var fontSelection = new ComboBox
                                            {
                                                Name = "FontForRow_" + printWorkItem.Type + "_" + rownumber,
                                                Location = new Point(220, 0),
                                            };
                    foreach (var font in Settings.Fonts)
                    {
                        fontSelection.Items.Add(font.Key);
                    }
                    fontSelection.SelectedItem = row.Font;
                    fontSelection.SelectedIndexChanged += FontSelectionSelectedIndexChanged;
                    rowElementPanel.Controls.Add(fontSelection);

                    var rowElementGridView = new DataGridView
                                                 {
                                                     Location = new Point(0, 23),
                                                     Size = new Size(500, 200),
                                                     Name = "DataGridRow_" + printWorkItem.Type + "_" + rownumber,
                                                     ScrollBars = ScrollBars.Both,
                                                 };

                    rowElementGridView.CellValidated += RowElementGridViewOnCellValidated;
                    var typeColumn = new DataGridViewComboBoxColumn {Name = "Type", HeaderText = "Type"};
                    typeColumn.Items.Add(RowElementType.Text.ToString());
                    typeColumn.Items.Add(RowElementType.Field.ToString());
                    typeColumn.Items.Add(RowElementType.RelatedItem.ToString());
                    rowElementGridView.Columns.Add(typeColumn);
                    rowElementGridView.Columns.Add(new DataGridViewTextBoxColumn
                                                       {
                                                           Name = RowElementAttributes.MaxeLength.ToString(),
                                                           HeaderText = RowElementAttributes.MaxeLength.ToString()
                                                       });
                    rowElementGridView.Columns.Add(new DataGridViewTextBoxColumn
                                                       {
                                                           Name = RowElementAttributes.DateFormat.ToString(),
                                                           HeaderText = RowElementAttributes.DateFormat.ToString()
                                                       });
                    rowElementGridView.Columns.Add(new DataGridViewTextBoxColumn
                                                       {
                                                           Name = RowElementAttributes.Text.ToString(),
                                                           HeaderText = RowElementAttributes.Text.ToString()
                                                       });

                    var fieldColumn = new DataGridViewComboBoxColumn
                                          {
                                              Name = RowElementAttributes.Field.ToString(),
                                              HeaderText = RowElementAttributes.Field.ToString(),
                                              Width = 200
                                          };
                    foreach (FieldDefinition field in tfsHelper.FieldDefinitions)
                    {
                        fieldColumn.Items.Add(field.ReferenceName);
                    }
                    fieldColumn.Items.Add("N/A");
                    rowElementGridView.Columns.Add(fieldColumn);

                    var searchFieldColumn = new DataGridViewComboBoxColumn
                                                {
                                                    Name = RowElementAttributes.SearchField.ToString(),
                                                    HeaderText = RowElementAttributes.SearchField.ToString(),
                                                    Width = 200
                                                };
                    foreach (FieldDefinition field in tfsHelper.FieldDefinitions)
                    {
                        searchFieldColumn.Items.Add(field.ReferenceName);
                    }
                    searchFieldColumn.Items.Add("N/A");
                    rowElementGridView.Columns.Add(searchFieldColumn);

                    rowElementGridView.Columns.Add(new DataGridViewTextBoxColumn
                                                       {
                                                           Name = RowElementAttributes.SearchData.ToString(),
                                                           HeaderText = RowElementAttributes.SearchData.ToString()
                                                       });

                    var resultFieldColumn = new DataGridViewComboBoxColumn
                                                {
                                                    Name = RowElementAttributes.ResultField.ToString(),
                                                    HeaderText = RowElementAttributes.ResultField.ToString(),
                                                    Width = 200
                                                };
                    foreach (FieldDefinition field in tfsHelper.FieldDefinitions)
                    {
                        resultFieldColumn.Items.Add(field.ReferenceName);
                    }
                    resultFieldColumn.Items.Add("N/A");
                    rowElementGridView.Columns.Add(resultFieldColumn);

                    rowElementPanel.Controls.Add(rowElementGridView);

                    foreach (IRowElement element in row.RowElements)
                    {
                        var rowElementText = element as RowElementText;
                        if (rowElementText != null)
                        {
                            rowElementGridView.Rows.Add(RowElementType.Text.ToString(), element.MaxLength,
                                                        element.DateFormatting, rowElementText.Data, "", "", "", "");
                        }

                        var rowElementField = element as RowElementField;
                        if (rowElementField != null)
                        {
                            rowElementGridView.Rows.Add(RowElementType.Field.ToString(), element.MaxLength,
                                                        element.DateFormatting, "", rowElementField.FieldName, "", "",
                                                        "");
                        }

                        var rowElementRelatedItem = element as RowElementRelatedItem;
                        if (rowElementRelatedItem != null)
                        {
                            rowElementGridView.Rows.Add(RowElementType.RelatedItem.ToString(), element.MaxLength,
                                                        element.DateFormatting, "", "",
                                                        rowElementRelatedItem.SearchField,
                                                        rowElementRelatedItem.SearcData,
                                                        rowElementRelatedItem.ResultField);
                        }
                    }

                    for (int i = 0; i < rowElementGridView.Rows.Count; i++)
                    {
                        FormatRowElementGrid(i, rowElementGridView);
                    }

                    rownumber++;
                }

                TabControl.TabPages.Add(tab);
                tab.Refresh();
            }

            TabControl.TabPages.Add(FontsTab);
            TabControl.TabPages.Add(AdvancedTab);

            int fontYPos = 30;
            foreach (var font in Settings.Fonts)
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

        private void RowElementGridViewOnCellValidated(object sender,
                                                       DataGridViewCellEventArgs dataGridViewCellValidatingEventArgs)
        {
            var grid = sender as DataGridView;
            if (grid == null)
                return;

            if (grid.Columns[dataGridViewCellValidatingEventArgs.ColumnIndex].Name.Equals("Type"))
            {
                FormatRowElementGrid(dataGridViewCellValidatingEventArgs.RowIndex, grid);
            }

            string itemType = grid.Name.Split('_')[1];
            int rowNumber = int.Parse(grid.Name.Split('_')[2]);
            WorkItemPrintData item =
                Settings.PrintWorkItems.FirstOrDefault(workItemPrintData => workItemPrintData.Type.Equals(itemType));
            if (item == null)
                return;

            Row rowData = item.Rows[rowNumber];
            rowData.RowElements.Clear();
            foreach (DataGridViewRow row in grid.Rows)
            {
                string type = GetValueForColumn(row, grid, "Type");

                IRowElement rowElement = null;
                if (type.Equals(RowElementType.Text.ToString()))
                {
                    rowElement = new RowElementText();
                    int maxLenght;
                    if (int.TryParse(GetValueForColumn(row, grid, RowElementAttributes.MaxeLength.ToString()),
                                     out maxLenght))
                    {
                        rowElement.MaxLength = maxLenght;
                    }

                    ((RowElementText) rowElement).Data = GetValueForColumn(row, grid,
                                                                           RowElementAttributes.Text.ToString());
                }

                if (type.Equals(RowElementType.Field.ToString()))
                {
                    rowElement = new RowElementField();
                    int maxLenght;
                    if (int.TryParse(GetValueForColumn(row, grid, RowElementAttributes.MaxeLength.ToString()),
                                     out maxLenght))
                    {
                        rowElement.MaxLength = maxLenght;
                    }

                    rowElement.DateFormatting = GetValueForColumn(row, grid, RowElementAttributes.DateFormat.ToString());
                    ((RowElementField) rowElement).FieldName = GetValueForColumn(row, grid,
                                                                                 RowElementAttributes.Field.ToString());
                }

                if (type.Equals(RowElementType.RelatedItem.ToString()))
                {
                    rowElement = new RowElementRelatedItem();
                    int maxLenght;
                    if (int.TryParse(GetValueForColumn(row, grid, RowElementAttributes.MaxeLength.ToString()),
                                     out maxLenght))
                    {
                        rowElement.MaxLength = maxLenght;
                    }

                    rowElement.DateFormatting = GetValueForColumn(row, grid, RowElementAttributes.DateFormat.ToString());
                    ((RowElementRelatedItem) rowElement).SearchField = GetValueForColumn(row, grid,
                                                                                         RowElementAttributes.
                                                                                             SearchField.ToString());
                    ((RowElementRelatedItem) rowElement).SearcData = GetValueForColumn(row, grid,
                                                                                       RowElementAttributes.SearchData.
                                                                                           ToString());
                    ((RowElementRelatedItem) rowElement).ResultField = GetValueForColumn(row, grid,
                                                                                         RowElementAttributes.
                                                                                             ResultField.ToString());
                }

                rowData.RowElements.Add(rowElement);
            }
        }

        private static string GetValueForColumn(DataGridViewRow row, DataGridView grid, string column)
        {
            foreach (
                DataGridViewCell cell in
                    row.Cells.Cast<DataGridViewCell>().Where(
                        cell => grid.Columns[cell.ColumnIndex].Name.Equals(column) && cell.Value != null))
            {
                return cell.Value.ToString();
            }

            return string.Empty;
        }

        private static void FormatRowElementGrid(int rowIndex, DataGridView grid)
        {
            string type = string.Empty;
            foreach (
                DataGridViewCell cell in
                    grid.Rows[rowIndex].Cells.Cast<DataGridViewCell>().Where(
                        cell => grid.Columns[cell.ColumnIndex].Name.Equals("Type") && cell.Value != null))
            {
                type = cell.Value.ToString();
                break;
            }


            foreach (DataGridViewCell cell in grid.Rows[rowIndex].Cells)
            {
                if (grid.Columns[cell.ColumnIndex].Name.Equals(RowElementAttributes.DateFormat.ToString()))
                {
                    if (type.Equals(RowElementType.Field.ToString()))
                    {
                        if (cell.Value != null && cell.Value.ToString().Equals("N/A"))
                        {
                            cell.Value = string.Empty;
                        }

                        cell.ReadOnly = false;
                    }
                    else if (type.Equals(RowElementType.Text.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.RelatedItem.ToString()))
                    {
                        if (cell.Value != null && cell.Value.ToString().Equals("N/A"))
                        {
                            cell.Value = string.Empty;
                        }

                        cell.ReadOnly = false;
                    }
                }

                if (grid.Columns[cell.ColumnIndex].Name.Equals(RowElementAttributes.Field.ToString()))
                {
                    if (type.Equals(RowElementType.Field.ToString()))
                    {
                        if (cell.Value != null && cell.Value.ToString().Equals("N/A"))
                        {
                            cell.Value = string.Empty;
                        }

                        cell.ReadOnly = false;
                    }
                    else if (type.Equals(RowElementType.Text.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.RelatedItem.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                }

                if (grid.Columns[cell.ColumnIndex].Name.Equals(RowElementAttributes.ResultField.ToString()))
                {
                    if (type.Equals(RowElementType.Field.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.Text.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.RelatedItem.ToString()))
                    {
                        if (cell.Value != null && cell.Value.ToString().Equals("N/A"))
                        {
                            cell.Value = string.Empty;
                        }

                        cell.ReadOnly = false;
                    }
                }

                if (grid.Columns[cell.ColumnIndex].Name.Equals(RowElementAttributes.SearchData.ToString()))
                {
                    if (type.Equals(RowElementType.Field.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.Text.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.RelatedItem.ToString()))
                    {
                        if (cell.Value != null && cell.Value.ToString().Equals("N/A"))
                        {
                            cell.Value = string.Empty;
                        }

                        cell.ReadOnly = false;
                    }
                }

                if (grid.Columns[cell.ColumnIndex].Name.Equals(RowElementAttributes.SearchField.ToString()))
                {
                    if (type.Equals(RowElementType.Field.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.Text.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.RelatedItem.ToString()))
                    {
                        if (cell.Value != null && cell.Value.ToString().Equals("N/A"))
                        {
                            cell.Value = string.Empty;
                        }

                        cell.ReadOnly = false;
                    }
                }

                if (grid.Columns[cell.ColumnIndex].Name.Equals(RowElementAttributes.Text.ToString()))
                {
                    if (type.Equals(RowElementType.Field.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                    else if (type.Equals(RowElementType.Text.ToString()))
                    {
                        if (cell.Value != null && cell.Value.ToString().Equals("N/A"))
                        {
                            cell.Value = string.Empty;
                        }

                        cell.ReadOnly = false;
                    }
                    else if (type.Equals(RowElementType.RelatedItem.ToString()))
                    {
                        cell.Value = "N/A";
                        cell.ReadOnly = true;
                    }
                }
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