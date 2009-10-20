using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitCommands;

namespace Henk.GitExtensionsSccProvider
{
    public class GitFileStatus
    {
        private static List<GitItemStatus> cachedFileStatusses = null;

        public static GitItemStatus GetFileStatus(string fileName)
        {
            GitCommands.Settings.GitDir = @"C:\Program Files (x86)\Git\cmd\";
            GitCommands.Settings.WorkingDir = fileName;

            //if (cachedFileStatusses == null)
                cachedFileStatusses = GitCommands.GitCommands.GetAllChangedFiles();

            foreach (GitItemStatus fileStatus in cachedFileStatusses)
            {
                if (fileName.EndsWith(fileStatus.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return fileStatus;
                }
            }

            //return unchanged
            return new GitItemStatus(fileName, true, false, false, false);
        }
    }
}
