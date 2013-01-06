namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class RowElementRelatedItem : IRowElement
    {
        public string SearchField { get; set; }

        public string ResultField { get; set; }

        public string SearcData { get; set; }

        #region IRowElement Members

        public int MaxLength { get; set; }

        public string DateFormatting { get; set; }

        #endregion
    }
}