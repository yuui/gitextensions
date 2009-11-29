using System;
using System.Collections.Generic;

using System.Text;

namespace GitCommands
{
    public class CommitDto
    {
        public string Message { get; set; }
        public string Result { get; set; }
        public bool Amend { get; set; }

        public CommitDto(string message, bool amend)
        {
            this.Message = message;
            this.Amend = amend;
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
            if (Dto.Amend)
                GitCommands.RunCmd(Settings.GitCmd, "commit --amend -m \"" + Dto.Message + "\"");
            else
                GitCommands.RunCmd(Settings.GitCmd, "commit -m \"" + Dto.Message + "\"");
        }
    }
}
