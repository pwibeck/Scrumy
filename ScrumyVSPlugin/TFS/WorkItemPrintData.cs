using System.Collections.ObjectModel;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class WorkItemPrintData
    {
        public WorkItemPrintData()
        {
            Rows = new Collection<Row>();
        }

        public string Type { get; set; }

        public int BackGroundColor { get; set; }

        public int TextColor { get; set; }

        public Collection<Row> Rows { get; set; }
    }
}