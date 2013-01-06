using System;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class RowElementField : IRowElement
    {
        public String FieldName { get; set; }

        #region IRowElement Members

        public int MaxLength { get; set; }

        public string DateFormatting { get; set; }

        #endregion
    }
}