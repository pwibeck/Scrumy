using System;
using System.Drawing;
using System.Globalization;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class PrintWorkItem
    {
        private readonly int height;
        private readonly WorkItem item;
        private readonly int padding;
        private readonly Settings settings;
        private readonly TfsHelper tfsHelper;
        private readonly int width;
        private readonly int x;
        private readonly int y;
        private RectangleF cardArea;
        private readonly Font copyrightFont = new Font("Calibri (Body)", 2);
        private RectangleF textPrintArea;

        public PrintWorkItem(WorkItem workItem, Settings settings, TfsHelper tfsHelper, int x, int y, int width,
                             int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            padding = 10;
            textPrintArea = new RectangleF(x + padding, y + padding, width - (padding*2), height - (padding*2));
            cardArea = new RectangleF(x, y, width, height);
            item = workItem;
            this.settings = settings;
            this.tfsHelper = tfsHelper;
        }

        public virtual void Draw(Graphics g)
        {
            string type = item.Fields["System.WorkItemType"].Value.ToString();
            WorkItemPrintData printData = null;
            foreach (WorkItemPrintData workItemPrintData in settings.PrintWorkItems)
            {
                if (String.Compare(workItemPrintData.Type, type, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    printData = workItemPrintData;
                    break;
                }
            }

            if (printData != null)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(printData.BackGroundColor)), cardArea);
                g.DrawRectangle(Pens.Black, x, y, width, height);
                DrawSpam(g);

                float printOffsetY = 0;
                foreach (Row row in printData.Rows)
                {
                    string rowText = string.Empty;
                    foreach (IRowElement rowElement in row.RowElements)
                    {
                        string elementText = string.Empty;
                        if (rowElement is RowElementText)
                        {
                            elementText = ((RowElementText) rowElement).Data;
                        }
                        else if (rowElement is RowElementField)
                        {
                            string fieldName = ((RowElementField) rowElement).FieldName;
                            if (TfsHelper.FieldExist(item, fieldName))
                            {
                                if (item.Fields[fieldName].Value != null)
                                {
                                    elementText = item.Fields[fieldName].Value.ToString();
                                }
                            }
                            else
                            {
                                elementText = "N/A";
                            }
                        }
                        else if (rowElement is RowElementRelatedItem)
                        {
                            foreach (object linked in item.Links)
                            {
                                var related = linked as RelatedLink;
                                if (related != null)
                                {
                                    WorkItem linkedItem = tfsHelper.GetWorkItem(related.RelatedWorkItemId);
                                    if (
                                        TfsHelper.FieldExist(linkedItem,
                                                             ((RowElementRelatedItem) rowElement).SearchField) &&
                                        TfsHelper.FieldExist(linkedItem,
                                                             ((RowElementRelatedItem) rowElement).ResultField))
                                    {
                                        if (linkedItem.Fields[((RowElementRelatedItem) rowElement).SearchField].Value !=
                                            null &&
                                            linkedItem.Fields[((RowElementRelatedItem) rowElement).ResultField].Value !=
                                            null)
                                        {
                                            if (String.Compare(
                                                linkedItem.Fields[((RowElementRelatedItem) rowElement).SearchField].
                                                    Value.ToString(),
                                                ((RowElementRelatedItem) rowElement).SearcData,
                                                StringComparison.OrdinalIgnoreCase) == 0)
                                            {
                                                elementText =
                                                    linkedItem.Fields[((RowElementRelatedItem) rowElement).ResultField].
                                                        Value.ToString();
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        elementText = "N/A";
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(rowElement.DateFormatting))
                        {
                            DateTime date;
                            if (DateTime.TryParse(elementText, out date))
                            {
                                elementText = date.ToString("d MMM", CultureInfo.GetCultureInfo("en-us"));
                            }
                        }

                        if (elementText.Length > rowElement.MaxLength)
                        {
                            elementText = elementText.Substring(0, rowElement.MaxLength - 3) + "...";
                        }

                        rowText += elementText;
                    }

                    Font printFont = settings.Fonts[row.Font];
                    float totalStringPrintWidth = g.MeasureString(rowText, printFont).Width;
                    float printAreaHeight = (float) Math.Floor((totalStringPrintWidth/textPrintArea.Width)) + 1;

                    if (String.Compare(row.LocationY, "Top", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        g.DrawString(rowText, printFont,
                                     new SolidBrush(Color.FromArgb(printData.TextColor)),
                                     new RectangleF(textPrintArea.Left, textPrintArea.Top + printOffsetY,
                                                    textPrintArea.Width,
                                                    printAreaHeight*printFont.GetHeight(g)));
                        printOffsetY += printAreaHeight*printFont.GetHeight(g);
                    }
                    else if (String.Compare(row.LocationY, "Bottom", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        g.DrawString(rowText, printFont,
                                     new SolidBrush(Color.FromArgb(printData.TextColor)),
                                     new RectangleF(textPrintArea.Left,
                                                    textPrintArea.Bottom - (printAreaHeight*printFont.GetHeight(g)),
                                                    textPrintArea.Width,
                                                    printAreaHeight*printFont.GetHeight(g)));
                    }
                }
            }
        }

        protected void DrawSpam(Graphics g)
        {
            g.DrawString("Scrumy, peterwib", copyrightFont, Brushes.Black, new PointF(x + width - 25, y + height - 5));
        }
    }
}