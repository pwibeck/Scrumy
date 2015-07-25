using System.Windows.Forms;

namespace PeterWibeck.ScrumyVSPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Microsoft.TeamFoundation.WorkItemTracking.Client;

    using PeterWibeck.ScrumyVSPlugin.TFS;

    public partial class RowItem : UserControl
    {
        private readonly Dictionary<string, Font> fonts;

        private readonly TfsHelper tfsHelper;

        private Row row;

        public Row Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
                RenderRow();
            }
        }

        public RowItem(Dictionary<string, Font> fonts, TfsHelper tfsHelper)
        {
            this.InitializeComponent();
            this.fonts = fonts;
            this.tfsHelper = tfsHelper;
        }

        private void RenderRow()
        {
            this.AligmentSelection.SelectedItem = this.row.Alignment;

            foreach (var font in this.fonts)
            {
                this.FontSelection.Items.Add(font.Key);
            }

            this.FontSelection.SelectedItem = this.row.Font;

            this.RowElements.CellValueChanged -= this.CellValueChanged;
            SetupColumnsToGrid();
            AddDataToGrid();
            this.RowElements.CellValueChanged += this.CellValueChanged;
        }

        private void SetupColumnsToGrid()
        {
            this.RowElements.Columns.Clear();

            var typeColumn = new DataGridViewComboBoxColumn { Name = "Type", HeaderText = "Type" };
            typeColumn.Items.Add(RowElementType.Text.ToString());
            typeColumn.Items.Add(RowElementType.Field.ToString());
            typeColumn.Items.Add(RowElementType.RelatedItem.ToString());
            this.RowElements.Columns.Add(typeColumn);
            this.RowElements.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = RowElementAttributes.MaxeLength.ToString(),
                HeaderText = RowElementAttributes.MaxeLength.ToString()
            });
            this.RowElements.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = RowElementAttributes.DateFormat.ToString(),
                HeaderText = RowElementAttributes.DateFormat.ToString()
            });
            this.RowElements.Columns.Add(new DataGridViewTextBoxColumn
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
            foreach (FieldDefinition field in this.tfsHelper.FieldDefinitions)
            {
                fieldColumn.Items.Add(field.ReferenceName);
            }
            fieldColumn.Items.Add("N/A");
            this.RowElements.Columns.Add(fieldColumn);

            var searchFieldColumn = new DataGridViewComboBoxColumn
            {
                Name = RowElementAttributes.SearchField.ToString(),
                HeaderText = RowElementAttributes.SearchField.ToString(),
                Width = 200
            };
            foreach (FieldDefinition field in this.tfsHelper.FieldDefinitions)
            {
                searchFieldColumn.Items.Add(field.ReferenceName);
            }
            searchFieldColumn.Items.Add("N/A");
            this.RowElements.Columns.Add(searchFieldColumn);

            this.RowElements.Columns.Add(new DataGridViewTextBoxColumn
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
            foreach (FieldDefinition field in this.tfsHelper.FieldDefinitions)
            {
                resultFieldColumn.Items.Add(field.ReferenceName);
            }
            resultFieldColumn.Items.Add("N/A");
            this.RowElements.Columns.Add(resultFieldColumn);
        }

        private void AddDataToGrid()
        {
            this.RowElements.Rows.Clear();

            foreach (IRowElement element in this.row.RowElements)
            {
                var rowElementText = element as RowElementText;
                if (rowElementText != null)
                {
                    this.RowElements.Rows.Add(RowElementType.Text.ToString(), element.MaxLength,
                                                element.DateFormatting, rowElementText.Data, "", "", "", "");
                }

                var rowElementField = element as RowElementField;
                if (rowElementField != null)
                {
                    this.RowElements.Rows.Add(RowElementType.Field.ToString(), element.MaxLength,
                                                element.DateFormatting, "", rowElementField.FieldName, "", "",
                                                "");
                }

                var rowElementRelatedItem = element as RowElementRelatedItem;
                if (rowElementRelatedItem != null)
                {
                    this.RowElements.Rows.Add(RowElementType.RelatedItem.ToString(), element.MaxLength,
                                                element.DateFormatting, "", "",
                                                rowElementRelatedItem.SearchField,
                                                rowElementRelatedItem.SearcData,
                                                rowElementRelatedItem.ResultField);
                }
            }

            for (int i = 0; i < this.RowElements.Rows.Count; i++)
            {
                FormatRowElementGrid(i, this.RowElements);
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

        private void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null)
                return;

            if (grid.Columns[e.ColumnIndex].Name.Equals("Type"))
            {
                FormatRowElementGrid(e.RowIndex, grid);
            }
            
            DataGridViewRow row = grid.Rows[e.RowIndex];
            string type = GetValueForColumn(row, grid, "Type");
                
            if (type.Equals(RowElementType.Text.ToString()))
            {
                this.row.RowElements[e.RowIndex] = new RowElementText();
                int maxLenght;
                if (Int32.TryParse(GetValueForColumn(row, grid, RowElementAttributes.MaxeLength.ToString()),
                                    out maxLenght))
                {
                    this.row.RowElements[e.RowIndex].MaxLength = maxLenght;
                }

                ((RowElementText)this.row.RowElements[e.RowIndex]).Data = GetValueForColumn(row, grid,
                                                                        RowElementAttributes.Text.ToString());
            }

            if (type.Equals(RowElementType.Field.ToString()))
            {
                this.row.RowElements[e.RowIndex] = new RowElementField();
                int maxLenght;
                if (Int32.TryParse(GetValueForColumn(row, grid, RowElementAttributes.MaxeLength.ToString()),
                                    out maxLenght))
                {
                    this.row.RowElements[e.RowIndex].MaxLength = maxLenght;
                }

                this.row.RowElements[e.RowIndex].DateFormatting = GetValueForColumn(row, grid, RowElementAttributes.DateFormat.ToString());
                ((RowElementField)this.row.RowElements[e.RowIndex]).FieldName = GetValueForColumn(row, grid,
                                                                                RowElementAttributes.Field.ToString());
            }

            if (type.Equals(RowElementType.RelatedItem.ToString()))
            {
                this.row.RowElements[e.RowIndex] = new RowElementRelatedItem();
                int maxLenght;
                if (Int32.TryParse(GetValueForColumn(row, grid, RowElementAttributes.MaxeLength.ToString()),
                                    out maxLenght))
                {
                    this.row.RowElements[e.RowIndex].MaxLength = maxLenght;
                }

                this.row.RowElements[e.RowIndex].DateFormatting = GetValueForColumn(row, grid, RowElementAttributes.DateFormat.ToString());
                ((RowElementRelatedItem)this.row.RowElements[e.RowIndex]).SearchField = GetValueForColumn(row, grid,
                                                                                        RowElementAttributes.
                                                                                            SearchField.ToString());
                ((RowElementRelatedItem)this.row.RowElements[e.RowIndex]).SearcData = GetValueForColumn(row, grid,
                                                                                    RowElementAttributes.SearchData.
                                                                                        ToString());
                ((RowElementRelatedItem)this.row.RowElements[e.RowIndex]).ResultField = GetValueForColumn(row, grid,
                                                                                        RowElementAttributes.
                                                                                            ResultField.ToString());
            }
        }

        private static void FormatRowElementGrid(int rowIndex, DataGridView grid)
        {
            string type = String.Empty;
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
                            cell.Value = String.Empty;
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
                            cell.Value = String.Empty;
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
                            cell.Value = String.Empty;
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
                            cell.Value = String.Empty;
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
                            cell.Value = String.Empty;
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
                            cell.Value = String.Empty;
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
                            cell.Value = String.Empty;
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

        private static string GetValueForColumn(DataGridViewRow row, DataGridView grid, string column)
        {
            foreach (
                DataGridViewCell cell in
                    row.Cells.Cast<DataGridViewCell>().Where(
                        cell => grid.Columns[cell.ColumnIndex].Name.Equals(column) && cell.Value != null))
            {
                return cell.Value.ToString();
            }

            return String.Empty;
        }

        private void AligmentSelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
                return;

            this.row.Alignment = comboBox.SelectedItem.ToString();

        }

        private void FontSelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
                return;

            this.row.Font = comboBox.SelectedItem.ToString();
        }        
    }
}
