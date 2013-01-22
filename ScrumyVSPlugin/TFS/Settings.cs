using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class Settings
    {
        private const string SettingFileName = "ScrumySettings.xml";
        private string document;
        private IsolatedStorageFileAdapter.StoreType storeType;

        public Settings()
        {
            Init(IsolatedStorageFileAdapter.StoreType.Assembly);
        }

        public Settings(IsolatedStorageFileAdapter.StoreType storeType)
        {
            Init(storeType);
        }

        public Dictionary<string, Font> Fonts { get; set; }

        public Collection<WorkItemPrintData> PrintWorkItems { get; set; }

        public string Document
        {
            get { return document; }
            set
            {
                TextReader textReader = new StringReader(value);
                XDocument doc = XDocument.Load(textReader);
                LoadSettings(doc);
            }
        }

        private void LoadSettings(XDocument doc)
        {
            LoadFonts(doc);
            LoadItems(doc);
            document = doc.ToString();
        }

        protected void Init(IsolatedStorageFileAdapter.StoreType store)
        {
            storeType = store;
            Fonts = new Dictionary<string, Font>();
            PrintWorkItems = new Collection<WorkItemPrintData>();
            ResetSettings();
        }

        public void SaveSettings()
        {
            SaveSettings(new IsolatedStorageFileAdapter(storeType));
        }

        public void SaveSettings(IIsolatedStorageFile store)
        {
            if (store.FileExists(SettingFileName))
            {
                store.DeleteFile(SettingFileName);
            }

            using (IIsolatedStorageFileStream stream = store.CreateStream(SettingFileName, FileMode.Create))
            {
                var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                        new XElement("settings",
                                                     new object[]
                                                         {
                                                             new XAttribute("version", "1"), CreateFontsElement(),
                                                             CreateXmlWorkItemsElement()
                                                         }));
                doc.Save(stream.BaseStream);
            }
        }

        private XElement CreateXmlWorkItemsElement()
        {
            var xitemss = new Collection<object>();
            foreach (WorkItemPrintData printWorkItem in PrintWorkItems)
            {
                xitemss.Add(new XElement("item", new object[]
                                                     {
                                                         new XAttribute("type", printWorkItem.Type),
                                                         new XAttribute("bgColor",
                                                                        printWorkItem.BackGroundColor.ToString(
                                                                            CultureInfo.InvariantCulture)),
                                                         new XAttribute("txtColor",
                                                                        printWorkItem.TextColor.ToString(
                                                                            CultureInfo.InvariantCulture)),
                                                         CreateXmlLayout(printWorkItem)
                                                     }));
            }

            return new XElement("workItems", xitemss.ToArray());
        }

        private XElement CreateXmlLayout(WorkItemPrintData workItemPrintData)
        {
            var xrows = new Collection<object>();
            foreach (Row row in workItemPrintData.Rows)
            {
                xrows.Add(new XElement("row", new object[]
                                                  {
                                                      new XAttribute("font", row.Font),
                                                      new XAttribute("alignment", row.Alignment),
                                                      CreateXmlRowElements(row)
                                                  }));
            }

            return new XElement("layout", xrows.ToArray());
        }

        private static object[] CreateXmlRowElements(Row row)
        {
            var xelements = new Collection<object>();
            foreach (IRowElement rowElement in row.RowElements)
            {
                var rowElementText = rowElement as RowElementText;
                if (rowElementText != null)
                {
                    xelements.Add(new XElement("element", new object[]
                                                              {
                                                                  new XAttribute("type", "text"),
                                                                  new XAttribute("maxLength", rowElement.MaxLength),
                                                                  rowElementText.Data
                                                              }));
                }

                var rowElementField = rowElement as RowElementField;
                if (rowElementField != null)
                {
                    xelements.Add(new XElement("element", new object[]
                                                              {
                                                                  new XAttribute("type", "field"),
                                                                  new XAttribute("maxLength", rowElement.MaxLength),
                                                                  new XAttribute("dateFormatting",
                                                                                 rowElement.DateFormatting),
                                                                  rowElementField.FieldName
                                                              }));
                }

                var rowElementRelatedItem = rowElement as RowElementRelatedItem;
                if (rowElementRelatedItem != null)
                {
                    xelements.Add(new XElement("element", new object[]
                                                              {
                                                                  new XAttribute("type", "relatedItem"),
                                                                  new XAttribute("maxLength",
                                                                                 rowElementRelatedItem.MaxLength),
                                                                  new XAttribute("dateFormatting",
                                                                                 rowElementRelatedItem.DateFormatting),
                                                                  new XAttribute("searchField",
                                                                                 rowElementRelatedItem.SearchField),
                                                                  new XAttribute("searchData",
                                                                                 rowElementRelatedItem.SearcData),
                                                                  new XAttribute("resultField",
                                                                                 rowElementRelatedItem.ResultField)
                                                              }));
                }
            }

            return xelements.ToArray();
        }

        private XElement CreateFontsElement()
        {
            var xfonts = new Collection<object>();
            foreach (var fontData in Fonts)
            {
                xfonts.Add(new XElement("font", new object[]
                                                    {
                                                        new XAttribute("name", fontData.Key),
                                                        new XElement("familyName",
                                                                     new XText(fontData.Value.FontFamily.Name)),
                                                        new XElement("size",
                                                                     new XText(
                                                                         fontData.Value.Size.ToString(
                                                                             CultureInfo.InvariantCulture))),
                                                        new XElement("style", new XText(fontData.Value.Style.ToString()))
                                                    }));
            }

            return new XElement("fonts", xfonts.ToArray());
        }

        public void LoadSettings()
        {
            LoadSettings(new IsolatedStorageFileAdapter(storeType));
        }

        public void LoadSettings(IIsolatedStorageFile store)
        {
            if (store.FileExists(SettingFileName))
            {
                bool failedToReadFile = false;
                using (IIsolatedStorageFileStream stream = store.CreateStream(SettingFileName, FileMode.Open))
                {
                    try
                    {
                        XDocument doc = XDocument.Load(stream.BaseStream);
                        LoadSettings(doc);
                    }
                    catch (SettingsLoadException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        failedToReadFile = true;
                        ResetSettings();
                    }
                }

                if (failedToReadFile)
                {
                    store.DeleteFile(SettingFileName);
                }
            }
            else
            {
                ResetSettings();
            }
        }

        private void LoadItems(XDocument doc)
        {
            try
            {
                PrintWorkItems = new Collection<WorkItemPrintData>();
                var xitem = from item in doc.Descendants("item")
                            select new
                                       {
                                           type = item.Attribute("type").Value,
                                           bgcolor = int.Parse(item.Attribute("bgColor").Value),
                                           txtcolor = int.Parse(item.Attribute("txtColor").Value),
                                           layout = item.Element("layout")
                                       };

                foreach (var item in xitem)
                {
                    var printWorkItem = new WorkItemPrintData
                                            {
                                                Type = item.type,
                                                BackGroundColor = item.bgcolor,
                                                TextColor = item.txtcolor,
                                                RowsRawData = item.layout.ToString()
                                            };
                    try
                    {
                        var xrow = from row in item.layout.Descendants("row")
                                   select new
                                              {
                                                  font = row.Attribute("font").Value,
                                                  alignment = row.Attribute("alignment"),
                                                  rowLayout = row
                                              };
                        foreach (var row in xrow)
                        {
                            var r = new Row
                                        {
                                            Font = row.font,
                                            RowElements = CreateRowElements(row.rowLayout)
                                        };

                            if (row.alignment != null)
                            {
                                r.Alignment = row.alignment.Value;
                            }

                            printWorkItem.Rows.Add(r);
                        }
                    }
                    catch (Exception e)
                    {
                        if (!(e is SettingsLoadException))
                        {
                            throw new SettingsLoadException("Error loading workitem:" + printWorkItem.Type);
                        }

                        throw;
                    }

                    PrintWorkItems.Add(printWorkItem);
                }
            }
            catch (Exception e)
            {
                if (!(e is SettingsLoadException))
                {
                    throw new SettingsLoadException("Error loading workitem setting");
                }

                throw;
            }
        }

        private void LoadFonts(XDocument doc)
        {
            try
            {
                Fonts = new Dictionary<string, Font>();
                var xfonts = from item in doc.Descendants("font")
                             select new
                                        {
                                            name = item.Attribute("name").Value,
                                            familyName = item.Element("familyName").Value,
                                            size = float.Parse(item.Element("size").Value),
                                            style = item.Element("style").Value
                                        };
                foreach (var font in xfonts)
                {
                    FontStyle style;
                    if (!Enum.TryParse(font.style, true, out style))
                    {
                        style = FontStyle.Regular;
                    }

                    Fonts.Add(font.name, new Font(font.familyName, font.size, style));
                }
            }
            catch (Exception e)
            {
                if (!(e is SettingsLoadException))
                {
                    throw new SettingsLoadException("Error loading fonts setting");
                }

                throw;
            }
        }

        private static Collection<IRowElement> CreateRowElements(XElement row)
        {
            var rowElements = new Collection<IRowElement>();

            var xrowLayout = from layout in row.Elements()
                             select new
                                        {
                                            type = layout.Attribute("type"),
                                            maxLenght = layout.Attribute("maxLength"),
                                            dateFormatting = layout.Attribute("dateFormatting"),
                                            data = layout
                                        };

            foreach (var element in xrowLayout)
            {
                IRowElement rowElement;

                string type = string.Empty;
                if (element.type != null)
                {
                    type = element.type.Value;
                }

                switch (type.ToUpperInvariant())
                {
                    case "TEXT":
                        rowElement = new RowElementText();
                        if (element.data != null)
                        {
                            ((RowElementText) rowElement).Data = element.data.Value;
                        }
                        break;
                    case "FIELD":
                        rowElement = new RowElementField();
                        if (element.data != null)
                        {
                            ((RowElementField) rowElement).FieldName = element.data.Value;
                        }
                        break;
                    case "RELATEDITEM":
                        rowElement = new RowElementRelatedItem();
                        ((RowElementRelatedItem) rowElement).SearchField = element.data.Attribute("searchField").Value;
                        ((RowElementRelatedItem) rowElement).SearcData = element.data.Attribute("searchData").Value;
                        ((RowElementRelatedItem) rowElement).ResultField = element.data.Attribute("resultField").Value;
                        break;
                    default:
                        throw new Exception("Could not find type");
                }

                if (element.dateFormatting != null)
                {
                    rowElement.DateFormatting = element.dateFormatting.Value;
                }
                else
                {
                    rowElement.DateFormatting = string.Empty;
                }

                rowElement.MaxLength = element.maxLenght != null ? int.Parse(element.maxLenght.Value) : int.MaxValue;

                rowElements.Add(rowElement);
            }

            return rowElements;
        }

        public void ResetSettings()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            XDocument doc =
                XDocument.Load(assembly.GetManifestResourceStream("PeterWibeck.ScrumyVSPlugin.TFS.SettingsDefault.xml"));
            LoadSettings(doc);
        }
    }
}