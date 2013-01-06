using System;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class RowElementText : IRowElement
    {
        public String Data { get; set; }

        #region IRowElement Members

        public int MaxLength { get; set; }

        public string DateFormatting { get; set; }

        #endregion
    }
}