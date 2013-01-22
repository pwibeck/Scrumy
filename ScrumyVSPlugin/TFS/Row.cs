using System.Collections.ObjectModel;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class Row
    {
        public Row()
        {
            Alignment = "Top";
            RowElements = new Collection<IRowElement>();
        }

        public string Font { get; set; }

        public string Alignment { get; set; }

        public Collection<IRowElement> RowElements { get; set; }
    }
}