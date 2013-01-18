using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    /// <summary>
    /// Interface to TFS server.
    /// </summary>
    public class TfsHelper
    {
        private readonly Uri teamCollectionUrl;

        private readonly string teamProject;
        private WorkItemStore store;
        private TfsTeamProjectCollection tfs;

        /// <summary>
        /// Tfs Connector.
        /// </summary>        
        /// <param name="teamCollectionUrl">The URL to server</param>
        /// <param name="teamProject">The project</param>
        public TfsHelper(Uri teamCollectionUrl, string teamProject)
        {
            this.teamCollectionUrl = teamCollectionUrl;
            Connect();
            this.teamProject = teamProject;
        }

        public FieldDefinitionCollection FieldDefinitions
        {
            get { return store.FieldDefinitions; }
        }

        /// <summary>
        /// Get Work Item
        /// </summary>
        /// <param name="workItemid">The work item id</param>
        public WorkItem GetWorkItem(int workItemid)
        {
            WorkItem workItem = store.GetWorkItem(workItemid);
            return workItem;
        }


        public List<WorkItem> GetWorkItems(int[] workItemIds)
        {
            var result = new List<WorkItem>();
            if (workItemIds.Length == 0)
            {
                return result;
            }

            string idList = string.Empty;
            foreach (int id in workItemIds)
            {
                idList += id + ",";
            }

            idList = idList.Remove(idList.LastIndexOf(",", StringComparison.Ordinal));

            string query = string.Format(CultureInfo.CurrentCulture,
                                         "SELECT [System.Id], [System.Title], [System.WorkItemType] FROM WorkItems WHERE [System.TeamProject] = '{0}' AND [System.Id] IN ({1})",
                                         teamProject, idList);
            WorkItemCollection wic = store.Query(query);
            foreach (WorkItem item in wic)
            {
                result.Add(item);
            }

            return result;
        }

        public Bug GetBug(WorkItem item)
        {
            var bug = new Bug();
            bug.Id = int.Parse(item.Fields["System.Id"].Value.ToString());
            bug.Title = item.Fields["System.Title"].Value.ToString();
            if (FieldExist(item, "Microsoft.VSTS.Common.Priority"))
                bug.Priority = item.Fields["Microsoft.VSTS.Common.Priority"].Value.ToString();
            if (FieldExist(item, "Microsoft.VSTS.Common.Severity"))
                bug.Severity = item.Fields["Microsoft.VSTS.Common.Severity"].Value.ToString();
            bug.Issue = item.Fields["Microsoft.VSTS.Common.Issue"].Value.ToString();
            bug.AssignedTo = item.Fields["System.AssignedTo"].Value.ToString();
            DateTime creationDate = DateTime.Parse(item.Fields["System.CreatedDate"].Value.ToString(),
                                                   CultureInfo.CurrentCulture);
            bug.CreationDate = creationDate.ToString("d MMM", CultureInfo.GetCultureInfo("en-us"));

            Dictionary<string, object> revision_prev = new Dictionary<string, object>();
            foreach (Revision rev in item.Revisions)
            {
                Dictionary<string, object> revision = new Dictionary<string, object>();
                foreach (Field field in item.Fields)
                {
                    bool addField = true;
                    if (revision_prev.ContainsKey(field.ReferenceName))
                    {
                        var objPrev = revision_prev[field.ReferenceName];
                        var objNew = rev.Fields[field.Name].Value;

                        if (objPrev != null && objNew != null)
                        {
                            if (objNew.Equals(objPrev))
                                addField = false;
                        }
                        else
                        {
                            if (objPrev != objNew)
                                addField = false;
                        }

                    }

                    if(addField)
                    {
                        revision.Add(field.ReferenceName, rev.Fields[field.Name].Value);
                    }
                }

                if (revision.Count > 0)
                {
                    bug.History.Add(revision);
                    revision_prev = revision;
                }
            }

            return bug;
        }

        public static bool FieldExist(WorkItem item, string fieldname)
        {
            try
            {
                object obj = item.Fields[fieldname].Value;
            }
            catch (FieldDefinitionNotExistException)
            {
                return false;
            }

            return true;
        }

        private void Connect()
        {
            tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(teamCollectionUrl);
            store = (WorkItemStore) tfs.GetService(typeof (WorkItemStore));
        }

        /// <summary>
        /// Execute work item query
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Collection of work items</returns>
        public WorkItemCollection ExecuteQuery(string query)
        {
            return store.Query(query);
        }
    }
}