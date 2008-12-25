using System;
using System.Collections.Generic;

using System.Text;

namespace GitCommands
{
    public class CommitDto
    {
        public string Message { get; set; }
        public string Result { get; set; }

        public CommitDto(string message)
        {
            this.Message = message;
        }
    }

    public class Commit
    {
        public CommitDto Dto { get; set; }
        public Commit(CommitDto dto)
        {
            this.Dto = dto;
        }

        public void Execute()
        {
            GitCommands gitCommands = new GitCommands();

            Dto.Result = gitCommands.RunCmd(Settings.GitDir + "git.exe", "commit -m \"" + Dto.Message + "\"");
        }
    }
}
