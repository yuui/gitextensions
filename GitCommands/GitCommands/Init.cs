using System;
using System.Collections.Generic;

using System.Text;

namespace GitCommands
{
    public class InitDto
    {
        public string Result { get; set; }

        public InitDto()
        {
        }
    }

    public class Init
    {
        public InitDto Dto { get; set; }
        public Init(InitDto dto)
        {
            this.Dto = dto;
        }

        public void Execute()
        {
            GitCommands gitCommands = new GitCommands();

            Dto.Result = gitCommands.RunCmd(Settings.GitDir + "git.exe", "init");
        }
    }
}
