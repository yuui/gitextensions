using System;
using System.Collections.Generic;

using System.Text;

namespace GitCommands
{
    public class PullDto
    {
        public string Result { get; set; }

        public PullDto()
        {
        }
    }

    public class Pull
    {
        public PullDto Dto { get; set; }
        public Pull(PullDto dto)
        {
            this.Dto = dto;
        }

        public void Execute()
        {
            GitCommands gitCommands = new GitCommands();

            Dto.Result = gitCommands.RunCmd(Settings.GitDir + "git.exe", "pull");
        }
    }
}
