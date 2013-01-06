using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.WorkItemTracking;
using PeterWibeck.ScrumyVSPlugin.TFS;


namespace PeterWibeck.ScrumyVSPlugin
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0.6", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad("{e13eedef-b531-4afe-9725-28a69fa4f896}")]
    //[ProvideLoadKey("Standard", "1.0.4", "ScrumyVSPlugin", "Peter Wibeck", 104)]
    [Guid(GuidList.guidScrumyVSPluginPkgString)]
    public sealed class ScrumyVSPluginPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public ScrumyVSPluginPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the menu item.
                CommandID menuCommandScrumySettingsID = new CommandID(GuidList.guidScrumyVSPluginCmdSet, (int)PkgCmdIDList.cmdScrumySettings);
                MenuCommand menuItemScrumySettings = new MenuCommand(ScrumySettingsCallback, menuCommandScrumySettingsID);
                mcs.AddCommand(menuItemScrumySettings);

                CommandID menuCommandPreviewItemID = new CommandID(GuidList.guidScrumyVSPluginCmdSet, (int)PkgCmdIDList.cmdPreviewItem);
                MenuCommand menuItemPreviewItem = new MenuCommand(PreviewItemCallback, menuCommandPreviewItemID);
                mcs.AddCommand(menuItemPreviewItem);

                CommandID menuCommandPrintItemID = new CommandID(GuidList.guidScrumyVSPluginCmdSet, (int)PkgCmdIDList.cmdPrintItem);
                MenuCommand menuItemPrintItem = new MenuCommand(PrintItemCallback, menuCommandPrintItemID);
                mcs.AddCommand(menuItemPrintItem);

                CommandID menuCommandAnalyzeBugsID = new CommandID(GuidList.guidScrumyVSPluginCmdSet, (int)PkgCmdIDList.cmdAnalyzeBugs);
                MenuCommand menuItemAnalyzeBugs = new MenuCommand(AnalyzeBugsCallback, menuCommandAnalyzeBugsID);
                mcs.AddCommand(menuItemAnalyzeBugs);
            }
        }

        #endregion

        private void AnalyzeBugsCallback(object sender, EventArgs e)
        {
            TeamFoundationServerExt tfsExt = (TeamFoundationServerExt)Dte.GetObject("Microsoft.VisualStudio.TeamFoundation.TeamFoundationServerExt");

            IWorkItemTrackingDocument doc = GetActiveDocument();
            int[] selectedItemIds = ((IResultsDocument)doc).SelectedItemIds;
            doc.Release(this);

            TfsHelper tfsHelper = new TfsHelper(new Uri(tfsExt.ActiveProjectContext.DomainUri), tfsExt.ActiveProjectContext.ProjectName);
            //foreach (var workItem in tfsHelper.GetWorkItems(selectedItemIds))
            //{
            //    Bug bug = workItem as Bug;

            //    if(bug != null)
            //    {

            //    }
            //}
        }

        private void ScrumySettingsCallback(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        private void PreviewItemCallback(object sender, EventArgs e)
        {
            TeamFoundationServerExt tfsExt = (TeamFoundationServerExt)Dte.GetObject("Microsoft.VisualStudio.TeamFoundation.TeamFoundationServerExt");

            IWorkItemTrackingDocument doc = GetActiveDocument();
            int[] selectedItemIds = ((IResultsDocument)doc).SelectedItemIds;
            doc.Release(this);

            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.WindowState = FormWindowState.Maximized;
            TfsHelper tfsHelper = new TfsHelper(new Uri(tfsExt.ActiveProjectContext.DomainUri), tfsExt.ActiveProjectContext.ProjectName);
            Settings settings = new Settings();
            settings.LoadSettings();
            ItemsDocument printDoc = new ItemsDocument(settings, tfsHelper);
            foreach (var workItem in tfsHelper.GetWorkItems(selectedItemIds))
            {
                printDoc.AddWorkItem(workItem);
            }

            previewDialog.Document = printDoc;
            if (previewDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    previewDialog.Document.Print();
                }
                catch (Win32Exception)
                {
                    IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                    Guid clsid = Guid.Empty;
                    int result;
                    Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                               0,
                               ref clsid,
                               "Preview item error",
                               "Not possible to preview. Try to restart 'Print Spooler'",
                               string.Empty,
                               0,
                               OLEMSGBUTTON.OLEMSGBUTTON_OK,
                               OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                               OLEMSGICON.OLEMSGICON_INFO,
                               0,        // false
                               out result));
                }
            }
        }

        private void PrintItemCallback(object sender, EventArgs e)
        {
            TeamFoundationServerExt tfsExt = (TeamFoundationServerExt)Dte.GetObject("Microsoft.VisualStudio.TeamFoundation.TeamFoundationServerExt");

            IWorkItemTrackingDocument doc = GetActiveDocument();
            int[] selectedItemIds = ((IResultsDocument)doc).SelectedItemIds;
            doc.Release(this);

            PrintDialog printDialog = new PrintDialog();
            TfsHelper tfsHelper = new TfsHelper(new Uri(tfsExt.ActiveProjectContext.DomainUri), tfsExt.ActiveProjectContext.ProjectName);
            Settings settings = new Settings();
            settings.LoadSettings();
            ItemsDocument printDoc = new ItemsDocument(settings, tfsHelper);
            foreach (var workItem in tfsHelper.GetWorkItems(selectedItemIds))
            {
                printDoc.AddWorkItem(workItem);
            }

            printDialog.Document = printDoc;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDialog.Document.Print();
                }
                catch (Win32Exception)
                {
                    IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                    Guid clsid = Guid.Empty;
                    int result;
                    Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                               0,
                               ref clsid,
                               "Print item error",
                               "Not possible to print. Try to restart 'Print Spooler'",
                               string.Empty,
                               0,
                               OLEMSGBUTTON.OLEMSGBUTTON_OK,
                               OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                               OLEMSGICON.OLEMSGICON_INFO,
                               0,        // false
                               out result));
                }
            }
        }

        public EnvDTE.DTE Dte
        {
            get
            {
                return GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            }
        }

        public DocumentService DocService
        {
            get
            {
                return (DocumentService)GetService(typeof(DocumentService));
            }
        }

        private object _lockToken = new object();

        public IWorkItemTrackingDocument GetActiveDocument()
        {
            if (Dte.ActiveDocument != null)
            {
                IWorkItemTrackingDocument doc = DocService.FindDocument(Dte.ActiveDocument.FullName, _lockToken);
                if (doc != null)
                {
                    doc.Release(_lockToken);
                }

                return doc;
            }

            return null;
        }

    }
}
