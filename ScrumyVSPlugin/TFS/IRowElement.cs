namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public interface IRowElement
    {
        int MaxLength { get; set; }

        string DateFormatting { get; set; }
    }
}