using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitUI
{
    public partial class FormPull : Form
    {
        public FormPull()
        {
            InitializeComponent();
        }

        private void BrowseSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                PullSource.Text = dialog.SelectedPath;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Mergetool_Click(object sender, EventArgs e)
        {
            new GitCommands.GitCommands().RunRealCmd(GitCommands.Settings.GitDir + "git.exe", "mergetool --tool=kdiff3");

            if (MessageBox.Show("Resolved all conflicts? Commit?", "Conflicts solved", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Output.Text += "\n";
                FormCommit form = new FormCommit();
                form.ShowDialog();
            }
        }

        private void PullSource_TextChanged(object sender, EventArgs e)
        {
            Branches.DataSource = null;
        }

        private void Branches_DropDown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PullSource.Text))
            {
                Branches.DataSource = null;
                return;
            }

            string realWorkingDir = GitCommands.Settings.WorkingDir;

            try
            {
                GitCommands.Settings.WorkingDir = PullSource.Text;
                Branches.DisplayMember = "Name";
                Branches.DataSource = new GitCommands.GitCommands().GetHeads(false);
            }
            finally
            {
                GitCommands.Settings.WorkingDir = realWorkingDir;
            }
        }

        private void Pull_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PullSource.Text))
            {
                MessageBox.Show("Please select a source directory");
                return;
            }

            try
            {
                Output.Text = "";

                Pull.Enabled = false;
                BrowseSource.Enabled = false;
                Mergetool.Enabled = false;

                GitCommands.GitCommands gitCommands = new GitCommands.GitCommands();
                gitCommands.DataReceived += new System.Diagnostics.DataReceivedEventHandler(gitCommands_DataReceived);
                gitCommands.Exited += new EventHandler(gitCommands_Exited);
                gitCommands.PullAsync(PullSource.Text, Branches.SelectedText);

                Output.Text = "Start pull \n";
            }
            catch
            {
            }

        }

        // This method is passed in to the SetTextCallBack delegate
        // to set the Text property of textBox1.
        private void SetText(string text)
        {
            this.Output.Text += "\n" + text;
        }

        void gitCommands_DataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (Output.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { e.Data });
            }
            else
            {
                SetText(e.Data);
            }
        }

        void gitCommands_Exited(object sender, EventArgs e)
        {
            DoneCallback d = new DoneCallback(Done);
            this.Invoke(d, new object[] { });
        }
        private void Done()
        {
            this.Pull.Enabled = true;
            BrowseSource.Enabled = true;
            Mergetool.Enabled = true;
        }

    }
}
