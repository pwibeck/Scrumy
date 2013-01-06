using System.Collections.Generic;
using System.Drawing.Printing;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class ItemsDocument : PrintDocument
    {
        private readonly List<WorkItem> items = new List<WorkItem>();
        private readonly Settings settings;
        private bool hasMoreItemsToPrint;
        private IEnumerator<object> itemIterator;
        private TfsHelper tfsHelper;

        public ItemsDocument(Settings settings, TfsHelper tfsHelper)
        {
            this.settings = settings;
            this.tfsHelper = tfsHelper;
        }

        public void AddWorkItem(WorkItem item)
        {
            items.Add(item);
        }
        
        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);

            DefaultPageSettings.Landscape = true;
            itemIterator = items.GetEnumerator();
            hasMoreItemsToPrint = itemIterator.MoveNext();
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            int printHeight = DefaultPageSettings.PaperSize.Width - DefaultPageSettings.Margins.Left -
                              DefaultPageSettings.Margins.Right;
            int printWidth = DefaultPageSettings.PaperSize.Height - DefaultPageSettings.Margins.Top -
                             DefaultPageSettings.Margins.Bottom;
            int leftMargin = DefaultPageSettings.Margins.Left; //X
            int topMargin = DefaultPageSettings.Margins.Top; //Y
            int bottomMargin = DefaultPageSettings.Margins.Bottom; //Y
            int recWidth = printWidth/3;
            int recHeight = printHeight/2;

            bool done = false;
            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 1; y++)
                {
                    if (!hasMoreItemsToPrint)
                    {
                        done = true;
                        break;
                    }

                    object currentItem = itemIterator.Current;
                    var print = new PrintWorkItem((WorkItem)currentItem, settings, tfsHelper, leftMargin + recWidth*x,
                                                                topMargin + recHeight*y, recWidth, recHeight);
                    print.Draw(e.Graphics);
                    hasMoreItemsToPrint = itemIterator.MoveNext();
                }

                if (done)
                {
                    break;
                }
            }

            if (hasMoreItemsToPrint)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }
    }
}