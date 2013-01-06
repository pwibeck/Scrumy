// Guids.cs
// MUST match guids.h
using System;

namespace PeterWibeck.ScrumyVSPlugin
{
    static class GuidList
    {
        public const string guidScrumyVSPluginPkgString = "b80ab99e-b18e-4f5a-86b8-4fb376b1e0c0";
        public const string guidScrumyVSPluginCmdSetString = "51374e59-60f0-4036-84b5-073542a4d04b";

        public static readonly Guid guidScrumyVSPluginCmdSet = new Guid(guidScrumyVSPluginCmdSetString);
    };
}