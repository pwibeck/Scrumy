using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class Bug
    {
        public Bug()
        {
            Priority = "X";
            Severity = "X";
            History = new Collection<Dictionary<string, object>>();
        }

        public Bug(int id, string title, string priority, string severity, string issue, string assignedTo,
                   string creationDate)
        {
            Id = id;
            Title = title;
            Priority = priority;
            Severity = severity;
            Issue = issue;
            AssignedTo = assignedTo;
            CreationDate = creationDate;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Priority { get; set; }

        public string Severity { get; set; }

        public string Issue { get; set; }

        public string AssignedTo { get; set; }

        public string CreationDate { get; set; }

        public Collection<Dictionary<string, object>> History { get; set; }
        public string Type { get; set; }
        public string BackGroundColor { get; set; }
        public string TextColor { get; set; }
    }
}