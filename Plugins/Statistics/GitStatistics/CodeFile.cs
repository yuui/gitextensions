using System.IO;
using GitUIPluginInterfaces;
using System;

namespace GitStatistics
{
    /// <summary>
    ///   Represents a .NET file containing code: a .vb, .cs, .cpp, or .h file here.
    /// </summary>
    public class CodeFile
    {
        private readonly bool _isDesignerFile;
        protected int NumberCodeFiles;
        private bool _inCodeGeneratedRegion;
        private bool _inCommentBlock;
        private GitUIBaseEventArgs gitUiCommands;
        private string revision;

        internal CodeFile(string fullName, string revision, GitUIBaseEventArgs gitUiCommands)
        {
            File = fullName;
            _isDesignerFile = IsDesignerFile();
            IsTestFile = false;
            this.gitUiCommands = gitUiCommands;
            this.revision = revision;
        }

        public string File { get; private set; }

        protected internal int NumberLines { get; protected set; }

        protected internal int NumberBlankLines { get; protected set; }

        protected internal int NumberLinesInDesignerFiles { get; protected set; }

        protected internal int NumberCommentsLines { get; protected set; }

        internal bool IsTestFile { get; private set; }

        public string Extension
        {
            get
            {
                int extensionIndex = File.LastIndexOf('.');
                if (extensionIndex > 0)
                    return File.Substring(extensionIndex);

                return string.Empty;
            }
        }

        private bool IsDesignerFile()
        {
            return
                IsWebReferenceFile() ||
                File.Contains(".Designer.") ||
                File.Contains(".designer.");
        }

        private bool IsWebReferenceFile()
        {
            return File.Contains(@"\Web References\") &&
                   File.Equals("Reference.cs"); // Ugh
        }

        public void CountLines()
        {
            InitializeCountLines();

            string[] lines = gitUiCommands.GitCommands.RunGit(string.Format("show {0}:\"{1}\"", revision, File.Replace('\\', '/'))).Split(new char[] { '\0', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string line in lines)
                IncrementLineCountsFromLine(line.Trim());
        }

        private void InitializeCountLines()
        {
            SetLineCountsToZero();
            NumberCodeFiles = 1;
            _inCodeGeneratedRegion = false;
            _inCommentBlock = false;
        }

        protected void SetLineCountsToZero()
        {
            NumberLines = 0;
            NumberBlankLines = 0;
            NumberLinesInDesignerFiles = 0;
            NumberCommentsLines = 0;
            NumberCodeFiles = 0;
        }

        private void IncrementLineCountsFromLine(string line)
        {
            SetCodeBlockFlags(line);

            NumberLines++;
            if (_inCodeGeneratedRegion || _isDesignerFile)
                NumberLinesInDesignerFiles++;
            else if (line == "")
                NumberBlankLines++;
            else if (_inCommentBlock || line.StartsWith("'") || line.StartsWith(@"//"))
                NumberCommentsLines++;

            ResetCodeBlockFlags(line);
        }

        private void SetCodeBlockFlags(string line)
        {
            // The number of code-generated lines is an approximation at best, particularly
            // with VS 2003 code.  Change code here if you don't like the way it's working.
            // if (line.Contains("Designer generated code") // Might be cleaner
            if (line.StartsWith("#region Windows Form Designer generated code") ||
                line.StartsWith("#Region \" Windows Form Designer generated code") ||
                line.StartsWith("#region Component Designer generated code") ||
                line.StartsWith("#Region \" Component Designer generated code \"") ||
                line.StartsWith("#region Web Form Designer generated code") ||
                line.StartsWith("#Region \" Web Form Designer Generated Code \"")
                )
                _inCodeGeneratedRegion = true;
            if (line.StartsWith("/*"))
                _inCommentBlock = true;
            if (!_inCommentBlock && !_inCodeGeneratedRegion && (
                                                                   line.StartsWith("[Test")
                                                               ))
            {
                IsTestFile = true;
            }
        }

        private void ResetCodeBlockFlags(string line)
        {
            if (_inCodeGeneratedRegion && (line.Contains("#endregion") || line.Contains("#End Region")))
                _inCodeGeneratedRegion = false;
            if (_inCommentBlock && line.Contains("*/"))
                _inCommentBlock = false;
        }

        public bool CheckValidExtension(string fileName)
        {
            return true;
            //return fileName.EndsWith(".cs") || fileName.EndsWith(".vb") ||
            //    fileName.EndsWith(".cpp") || fileName.EndsWith(".h") || fileName.EndsWith(".hpp");
        }
    }
}