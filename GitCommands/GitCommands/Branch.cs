using System;
using System.Collections.Generic;

using System.Text;

namespace GitCommands
{
    public class BranchDto
    {
        public string Name { get; set; }
        public string Result { get; set; }

        public BranchDto(string name)
        {
            this.Name = name;
        }
    }

    public class Branch
    {
        public BranchDto Dto { get; set; }
        public Branch(BranchDto dto)
        {
            this.Dto = dto;
        }

        public void Execute()
        {
            GitCommands gitCommands = new GitCommands();

            Dto.Result = gitCommands.RunCmd(Settings.GitDir + "git.exe", "branch " + Dto.Name);
        }
    }
}
