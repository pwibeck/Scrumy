using System;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class SettingsLoadException : Exception
    {
        public SettingsLoadException(string message) : base(message)
        {
        }
    }
}