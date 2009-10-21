using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitCommands;

namespace Henk.GitExtensionsSccProvider
{
    public static class GitFileStatus
    {

        public static GitItemStatus GetFileStatus(string fileName)
        {
            if (fileName == null)
                return null;

            if (string.IsNullOrEmpty(Settings.WorkingDir))
                return null;

            fileName = fileName.Replace('\\', '/');
            
            List<GitItemStatus> fileStatusses = GitCommands.GitCommands.GetAllChangedFiles();

            foreach (GitItemStatus fileStatus in fileStatusses)
            {
                if (fileName.EndsWith(fileStatus.Name.Replace('\\', '/'), StringComparison.InvariantCultureIgnoreCase))
                {
                    return fileStatus;
                }
            }

            //return unchanged
            return new GitItemStatus(fileName, true, false, false, false);
        }

        
    }
}
