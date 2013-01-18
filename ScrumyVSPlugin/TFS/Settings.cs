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

        private string document;
        public string Document 
        {
            get { return this.document; }
            set
            {
                TextReader textReader = new StringReader(value);
                XDocument doc = XDocument.Load(textReader);
                LoadFonts(doc);
                LoadItems(doc);
                this.document = value;
            }
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
                                                     new object[] {new XAttribute("version", "1"), CreateFontsElement(), CreateXMLWorkItemsElement()}));
                doc.Save(stream.BaseStream);
            }
        }

        private XElement CreateXMLWorkItemsElement()
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
                                                         CreateXMLLayout(printWorkItem)
                                                     }));
            }

            return new XElement("workItems", xitemss.ToArray());
        }

        private XElement CreateXMLLayout(WorkItemPrintData workItemPrintData)
        {
            var xrows = new Collection<object>();
            foreach (Row row in workItemPrintData.Rows)
            {

                xrows.Add(new XElement("row", new object[]
                                                    {
                                                        new XAttribute("font", row.Font),
                                                        CreateXMLRowElements(row)
                                                    }));
            }

            return new XElement("layout", xrows.ToArray());
        }

        private object[] CreateXMLRowElements(Row row)
        {
            var xelements = new Collection<object>();
            foreach (IRowElement rowElement in row.RowElements)
            {
                if (rowElement is RowElementText)
                {
                    xelements.Add(new XElement("element", new object[]
                                                              {
                                                                  new XAttribute("type", "text"),
                                                                  ((RowElementText)rowElement).Data
                                                              }));
                }

                if (rowElement is RowElementField)
                {
                    xelements.Add(new XElement("element", new object[]
                                                              {
                                                                  new XAttribute("type", "field" ),
                                                                  ((RowElementField)rowElement).FieldName
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
                                                        new XElement("size", new XText(fontData.Value.Size.ToString(CultureInfo.InvariantCulture))),
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
                        LoadFonts(doc);
                        LoadItems(doc);
                        document = doc.ToString();
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
                                                  locationY = row.Attribute("locationY"),
                                                  rowLayout = row
                                              };
                        foreach (var row in xrow)
                        {
                            var r = new Row
                                        {
                                            Font = row.font,
                                            RowElements = CreateRowElements(row.rowLayout)
                                        };

                            if (row.locationY != null)
                            {
                                r.LocationY = row.locationY.Value;
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
                                            maxLenght = layout.Attribute("maxLenght"),
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
            LoadFonts(doc);
            LoadItems(doc);
            document = doc.ToString();
        }
    }
}