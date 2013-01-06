using System.Collections.ObjectModel;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class Row
    {
        public Row()
        {
            LocationY = "Top";
            RowElements = new Collection<IRowElement>();
        }

        public string Font { get; set; }

        public string LocationY { get; set; }

        public Collection<IRowElement> RowElements { get; set; }
    }
}