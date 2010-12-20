using System;
using System.Collections.Generic;
using System.IO;
using GitUIPluginInterfaces;
using System.Text.RegularExpressions;

namespace GitStatistics
{
    public class LineCounter
    {
        public event EventHandler LinesOfCodeUpdated;

        private GitUIBaseEventArgs gitUiCommands;

        public LineCounter(GitUIBaseEventArgs gitUiCommands)
        {
            this.gitUiCommands = gitUiCommands;
            LinesOfCodePerExtension = new Dictionary<string, int>();
        }

        public int NumberCommentsLines { get; private set; }
        public int NumberLines { get; private set; }
        public int NumberLinesInDesignerFiles { get; private set; }
        public int NumberTestCodeLines { get; private set; }
        public int NumberBlankLines { get; private set; }
        public int NumberCodeLines { get; private set; }
        public int NumberOfFiles { get; private set; }
        public int NumberOfFilesProcessed { get; private set; }
        public Dictionary<string, int> LinesOfCodePerExtension { get; private set; }

        private static bool DirectoryIsFiltered(FileSystemInfo dir, IEnumerable<string> directoryFilters)
        {
            foreach (var directoryFilter in directoryFilters)
            {
                if (dir.FullName.EndsWith(directoryFilter, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public void FindAndAnalyzeCodeFiles(string filePattern, string revision, string directoriesToIgnore)
        {
            NumberLines = 0;
            NumberBlankLines = 0;
            NumberLinesInDesignerFiles = 0;
            NumberCommentsLines = 0;
            NumberCodeLines = 0;
            NumberTestCodeLines = 0;
            NumberOfFilesProcessed = 0;
            NumberOfFiles = 1;

            var filters = filePattern.Split(';');
            var directoryFilter = directoriesToIgnore.Split(';');
            var lastUpdate = DateTime.Now;
            var timer = new TimeSpan(0,0,0,0,500);

            string [] files = gitUiCommands.GitCommands.RunGit("ls-tree --full-tree --name-only -r " + revision).Split(new char[] { '\0', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string [] extensions = filePattern.Split(',');

            NumberOfFiles = files.Length;

            var codeFiles = new List<CodeFile>();

            foreach (var file in files)
            {
                NumberOfFilesProcessed++;

                if (!HasMatchingExtension(extensions, file))
                    continue;
                //if (DirectoryIsFiltered(file.Directory, directoryFilter))
                //    continue;

                var codeFile = new CodeFile(file, revision, gitUiCommands);
                codeFile.CountLines();
                codeFiles.Add(codeFile);

                CalculateSums(codeFile);

                if (LinesOfCodeUpdated != null && DateTime.Now - lastUpdate > timer)
                {
                    lastUpdate = DateTime.Now;
                    LinesOfCodeUpdated(this, EventArgs.Empty);
                }
            }

            //Send 'changed' event when done
            if (LinesOfCodeUpdated != null)
                LinesOfCodeUpdated(this, EventArgs.Empty);
        }

        private static bool HasMatchingExtension(IEnumerable<string> extensions, string fileName)
        {
            foreach (string extension in extensions)
            {
                if (fileName.EndsWith("." + extension.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        private void CalculateSums(CodeFile codeFile)
        {
            NumberLines += codeFile.NumberLines;
            NumberBlankLines += codeFile.NumberBlankLines;
            NumberCommentsLines += codeFile.NumberCommentsLines;
            NumberLinesInDesignerFiles += codeFile.NumberLinesInDesignerFiles;

            var codeLines =
                codeFile.NumberLines -
                codeFile.NumberBlankLines -
                codeFile.NumberCommentsLines -
                codeFile.NumberLinesInDesignerFiles;

            var extension = codeFile.Extension.ToLower();

            if (!LinesOfCodePerExtension.ContainsKey(extension))
                LinesOfCodePerExtension.Add(extension, 0);

            LinesOfCodePerExtension[extension] += codeLines;
            NumberCodeLines += codeLines;

            if (codeFile.IsTestFile)
            {
                NumberTestCodeLines += codeLines;
            }


        }
    }
}