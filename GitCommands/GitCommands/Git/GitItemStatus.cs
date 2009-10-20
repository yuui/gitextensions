using System;
using System.Collections.Generic;

using System.Text;

namespace GitCommands
{
    public class GitItemStatus
    {
        public GitItemStatus()
        {
            IsTracked = false;
            IsDeleted = false;
            IsChanged = false;
            IsNew = false;
        }

        public GitItemStatus(string name, bool isTracked, bool isChanged, bool isNew, bool isDeleted)
        {
            Name = name;
            IsTracked = isTracked;
            IsDeleted = isDeleted;
            IsChanged = isChanged;
            IsNew = isNew;
        }

        public string Name { get; set; }

        public string ChangeString 
        {
            get
            {
                if (!IsTracked) return "Untracked";
                else if (IsDeleted) return "Deleted";
                else if (IsChanged) return "Modified";
                else if (IsNew) return "New";
                else return "";
            }
        }

        public bool IsTracked { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsChanged { get; set; }
        public bool IsNew { get; set; }
    }
}
