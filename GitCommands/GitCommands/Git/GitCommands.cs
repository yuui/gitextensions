using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.IO;
using PatchApply;
using System.Diagnostics;

namespace GitCommands
{
    public class GitCommands
    {
        public static string FindGitWorkingDir(string startDir)
        {
            string dir = startDir;
            if (!dir.EndsWith("\\") && !dir.EndsWith("/"))
                dir += "\\";

            while (dir.LastIndexOfAny(new char[]{'\\', '/'}) > 0)
            {
                dir = dir.Substring(0, dir.LastIndexOfAny(new char[] { '\\', '/' }));

                if (Directory.Exists(dir + "\\" + ".git"))
                    return dir + "\\";
            }
            return startDir;
        }

        public string RunCmd(string cmd)
        {
            return RunCmd(cmd, "");
        }

        public void RunRealCmd(string cmd, string arguments)
        {
            //process used to execute external commands

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.RedirectStandardInput = false;

            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.FileName = cmd;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WorkingDirectory = Settings.WorkingDir;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            process.StartInfo.LoadUserProfile = true;

            process.Start();
            process.WaitForExit();
            process.Close();
            
        }

        public void RunRealCmdDetatched(string cmd, string arguments)
        {
            //process used to execute external commands

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.RedirectStandardInput = false;

            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.FileName = cmd;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WorkingDirectory = Settings.WorkingDir;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            process.StartInfo.LoadUserProfile = true;

            process.Start();


        }

        public void Run(string cmd, string arguments)
        {
            //process used to execute external commands

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = cmd;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WorkingDirectory = Settings.WorkingDir;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            process.StartInfo.LoadUserProfile = true;

            process.Start();
            //process.WaitForExit();

        }
        public System.Diagnostics.Process Process { get; set; }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public Process RunCmdAsync(string cmd, string arguments)
        {
            //process used to execute external commands
            Process = new System.Diagnostics.Process();
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.ErrorDialog = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardError = true;

            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.FileName = cmd;
            Process.StartInfo.Arguments = arguments;
            Process.StartInfo.WorkingDirectory = Settings.WorkingDir;
            Process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            Process.StartInfo.LoadUserProfile = true;
            Process.EnableRaisingEvents = true;

            Process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived);
            Process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_ErrorDataReceived);
            Output = "";
            ErrorOutput = "";

            Process.Exited += new EventHandler(process_Exited);
            Process.Start();

            Process.StandardInput.Write("achttien\r");

            Process.BeginErrorReadLine();
            Process.BeginOutputReadLine();

            return Process;
        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public string RunCmd(string cmd, string arguments)
        {
            //process used to execute external commands
            Process = new System.Diagnostics.Process();
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.ErrorDialog = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardError = true;

            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.FileName = cmd;
            Process.StartInfo.Arguments = arguments;
            Process.StartInfo.WorkingDirectory = Settings.WorkingDir;
            Process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            Process.StartInfo.LoadUserProfile = true;

            Process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived);
            Process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(process_ErrorDataReceived);
            Process.Exited += new EventHandler(process_Exited);

            Output = "";
            ErrorOutput = "";

            Process.Start();

            Process.BeginErrorReadLine();
            Process.BeginOutputReadLine();


            Process.WaitForExit();
            Process.Close();
            // Read the output stream first and then wait. 

            if (!string.IsNullOrEmpty(ErrorOutput))
            {
                return Output + ErrorOutput;
            }

            return Output;
        }

        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        public event DataReceivedEventHandler DataReceived;
        public event EventHandler Exited;

        void process_Exited(object sender, EventArgs e)
        {
            if (Exited != null)
                Exited(this, e);
        }

        void process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            ErrorOutput += e.Data + "\n";
            if (DataReceived != null)
                DataReceived(this, e);
        }

        void process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Output += e.Data + "\n";
            if (DataReceived != null)
                DataReceived(this, e);
        }

        public Process CloneAsync(string source, string destination)
        {
            return RunCmdAsync(Settings.GitDir + "git.exe", "clone " + source + " " + destination);
        }

        public void RunGitK()
        {
            RunRealCmdDetatched("gitk", "");
        }

        public void RunGui()
        {
            Run(Settings.GitDir + "git.exe", "gui");
        }


        public void RunBash()
        {
            RunRealCmdDetatched("C:\\Windows\\System32\\cmd.exe", "/c \"" + Settings.GitDir + "sh.exe\" --login -i");
        }

        public string Reset()
        {
            return RunCmd(Settings.GitDir + "git.exe", "reset --hard");
        }

        public string Push(string path)
        {
            Directory.SetCurrentDirectory(Settings.WorkingDir);
            return RunCmd(Settings.GitDir + "git.exe", "push \"" + path + "\"");
        }

        public void PullAsync(string path, string branch)
        {
            Directory.SetCurrentDirectory(Settings.WorkingDir);

            RunCmdAsync(Settings.GitDir + "git.exe", "pull \"" + path + "\" \"" + branch + "\"");
        }

        public string Pull(string path, string branch)
        {
            Directory.SetCurrentDirectory(Settings.WorkingDir);

            string result = RunCmd(Settings.GitDir + "git.exe", "pull \"" + path + "\" \"" + branch + "\"");

            return result;
        }

        public string Resolved()
        {
            Directory.SetCurrentDirectory(Settings.WorkingDir);

            string result = RunCmd(Settings.GitDir + "git.exe", "am --3way --resolved");

            return result;
        }

        public string Skip()
        {
            Directory.SetCurrentDirectory(Settings.WorkingDir);

            string result = RunCmd(Settings.GitDir + "git.exe", "am --3way --skip");

            return result;
        }

        public string Abort()
        {
            Directory.SetCurrentDirectory(Settings.WorkingDir);

            string result = RunCmd(Settings.GitDir + "git.exe", "am --3way --abort");

            return result;
        }

        public string Patch(string patchFile)
        {
            Directory.SetCurrentDirectory(Settings.WorkingDir);

            string result = RunCmd(Settings.GitDir + "git.exe", "am --3way --signoff " + patchFile);

            return result;
        }

        public string GetSetting(string setting)
        {
            return RunCmd(Settings.GitDir + "git.exe", "config --get " + setting);
        }

        public void SetSetting(string setting, string value)
        {
            RunCmd(Settings.GitDir + "git.exe", "config --unset-all " + setting);
            RunCmd(Settings.GitDir + "git.exe", "config " + setting + " \"" + value + "\"");
        }

        public Patch GetSingleDiff(string from, string to, string filter)
        {
            PatchManager patchManager = new PatchManager();
            patchManager.LoadPatch(RunCmd(Settings.GitDir + "git.exe", "diff --ignore-submodules " + to + " " + from + " -- " + filter), false);

            return patchManager.patches.FirstOrDefault();
        }

        public List<Patch> GetDiff(string from, string to)
        {
            PatchManager patchManager = new PatchManager();
            patchManager.LoadPatch(RunCmd(Settings.GitDir + "git.exe", "diff " + from + " " + to), false);

            return patchManager.patches;
        }

        public List<string> GetDiffFiles(string from)
        {
            string result = RunCmd(Settings.GitDir + "git.exe", "diff --name-only " + from);

            string[] files = result.Split('\n');

            return files.ToList<string>();
        }

        public List<string> GetDiffFiles(string from, string to)
        {
            string result = RunCmd(Settings.GitDir + "git.exe", "diff --name-only " + from + " " + to);

            string[] files = result.Split('\n');

            return files.ToList<string>();
        }

        public List<GitItemStatus> GitStatus()
        {
            string status = RunCmd(Settings.GitDir + "git.exe", "status --untracked=all");

            string[] statusStrings = status.Split('\n');

            List<GitItemStatus> gitItemStatusList = new List<GitItemStatus>();

            foreach (string statusString in statusStrings)
            {
                if (statusString.StartsWith("#\tnew file:"))
                {
                    GitItemStatus itemStatus = new GitItemStatus();
                    itemStatus.IsNew = true;
                    itemStatus.IsTracked = true;
                    itemStatus.Name = statusString.Substring(statusString.LastIndexOf(':') + 1).Trim();
                    gitItemStatusList.Add(itemStatus);
                } else
                if (statusString.StartsWith("#\tdeleted:"))
                {
                    GitItemStatus itemStatus = new GitItemStatus();
                    itemStatus.IsDeleted = true;
                    itemStatus.IsTracked = true;
                    itemStatus.Name = statusString.Substring(statusString.LastIndexOf(':')+1).Trim();
                    gitItemStatusList.Add(itemStatus);
                } else
                    if (statusString.StartsWith("#\tmodified:"))
                {
                    GitItemStatus itemStatus = new GitItemStatus();
                    itemStatus.IsChanged = true;
                    itemStatus.IsTracked = true;
                    itemStatus.Name = statusString.Substring(statusString.LastIndexOf(':') + 1).Trim();
                    gitItemStatusList.Add(itemStatus);
                } else
                if (statusString.StartsWith("#\t"))
                {
                    GitItemStatus itemStatus = new GitItemStatus();
                    itemStatus.IsNew = true;
                    itemStatus.Name = statusString.Substring(2).Trim();
                    gitItemStatusList.Add(itemStatus);
                }
            }

            return gitItemStatusList;
        }

        public string GetCurrentChanges(string name)
        {
            //return RunCmd(Settings.GitDir + "git.exe", "diff --cached " + name);
            return RunCmd(Settings.GitDir + "git.exe", "diff " + name);
        }

        public List<GitRevision> GitRevisions()
        {
            return GitRevisions("");
        }

        public List<GitRevision> GitRevisions(string filter)
        {
            string tree;
            if (string.IsNullOrEmpty(filter))
                tree = RunCmd(Settings.GitDir + "git.exe", "rev-list --all --header --date-order");
            else
                tree = RunCmd(Settings.GitDir + "git.exe", "rev-list --header --topo-order " + filter);
            
            string[] itemsStrings = tree.Split('\n');

            List<GitRevision> revisions = new List<GitRevision>();

            for (int n = 0; n < itemsStrings.Count()-6;)
            {
                GitRevision revision = new GitRevision();
                revision.Guid = itemsStrings[n++].Trim('\0');
                revision.Name = revision.TreeGuid = itemsStrings[n++].Substring(4).Trim();
                while (itemsStrings[n].Contains("parent"))
                {
                    //Add parent
                    revision.ParentGuids.Add( itemsStrings[n++].Substring(6).Trim());
                }
                if (revision.ParentGuids.Count == 0)
                {
                    revision.ParentGuids.Add("0000000000000000000000000000000000000000");
                }
                revision.Author = itemsStrings[n++].Substring(6).Trim();
                revision.Committer = itemsStrings[n++].Substring(9).Trim();
                n++;

                while (itemsStrings.Count() > n + 1 &&
                    itemsStrings[n].Length > 0 &&
                    itemsStrings[n][0] == ' ')
                {
                    revision.Message += itemsStrings[n++].Trim() + "\n";
                }

                revisions.Add(revision);
            }

            return revisions;
        }

        public List<GitHead> GetHeads()
        {
            return GetHeads(true);
        }

        public string GetSelectedBranch()
        {
            string branches = RunCmd(Settings.GitDir + "git.exe", "branch");
            string[] branchStrings = branches.Split('\n');
            foreach (string branch in branchStrings)
            {
                if (branch.IndexOf('*') > -1)
                    return branch.Trim('*', ' ');
            }
            return "";
        }

        public List<GitHead> GetHeads(bool tags)
        {
            string tree;
            if (tags)
                tree = RunCmd(Settings.GitDir + "git.exe", "show-ref --dereference");
            else
                tree = RunCmd(Settings.GitDir + "git.exe", "show-ref --dereference --heads");
            string[] itemsStrings = tree.Split('\n');

            List<GitHead> heads = new List<GitHead>();

            foreach (string itemsString in itemsStrings)
            {
                if (itemsString.Length > 42)
                {
                    GitHead head = new GitHead();
                    head.Guid = itemsString.Substring(0, 40);
                    head.Name = itemsString.Substring(41).Trim();
                    if (head.Name.Length > 0 && head.Name.LastIndexOf("/") >1)
                        head.Name = head.Name.Substring(head.Name.LastIndexOf("/") + 1);

                    heads.Add(head);
                }
            }

            return heads;
        }

        public List<IGitItem> GetFileChanges(string file)
        {
            string tree = RunCmd(Settings.GitDir + "git.exe", "whatchanged --all --pretty=oneline " + file);

            string[] itemsStrings = tree.Split('\n');

            List<IGitItem> items = new List<IGitItem>();

            GitItem item = null;
            foreach (string itemsString in itemsStrings)
            {
                if (itemsString.Length > 43 && itemsString[0] != ':')
                {
                    item = new GitItem();

                    item.Guid = itemsString.Substring(0, 40);
                    item.Name = itemsString.Substring(41).Trim();

                    items.Add(item);
                }
                else
                {
                    if (item != null && itemsString.Length > 32)
                        item.Guid = itemsString.Substring(26, 7);
                        //item.Guid = itemsString.Substring(15, 7);
                }
            }

            return items;
        }


        public List<IGitItem> GetTree(string id)
        {
            string tree = RunCmd(Settings.GitDir + "git.exe", "ls-tree " + id);

            string [] itemsStrings = tree.Split('\n');

            List<IGitItem> items = new List<IGitItem>();

            foreach (string itemsString in itemsStrings)
            {
                GitItem item = new GitItem();

                if (itemsString.Length > 53)
                {

                    item.Mode = itemsString.Substring(0, 6);
                    item.ItemType = itemsString.Substring(7, 4);
                    item.Guid = itemsString.Substring(12, 40);
                    item.Name = itemsString.Substring(53).Trim();
                    item.FileName = item.Name;

                    //if (item.ItemType == "tree")
                    //    item.SubItems = GetTree(item.Guid);

                    items.Add(item);
                }
            }

            return items;
        }

        public string GetFileText(string id)
        {
            return RunCmd(Settings.GitDir + "git.exe", "cat-file blob " + id);
        }
    }
}
