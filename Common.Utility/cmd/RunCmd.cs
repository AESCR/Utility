using System.Diagnostics;

namespace Common.Utility
{
    public class RunCmd
    {
        #region Private Fields

        private readonly Process proc;

        #endregion Private Fields

        #region Public Constructors

        public RunCmd()
        {
            proc = new Process();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Exe(string cmd)
        {
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            // proc.OutputDataReceived += new DataReceivedEventHandler(sortProcess_OutputDataReceived);
            proc.Start();
            var cmdWriter = proc.StandardInput;
            proc.BeginOutputReadLine();
            if (!string.IsNullOrEmpty(cmd)) cmdWriter.WriteLine(cmd);
            cmdWriter.Close();
            proc.Close();
        }

        #endregion Public Methods

        #region Private Methods

        private void sortProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                //this.BeginInvoke(new Action(() => { this.listBox1.Items.Add(e.Data); }));
            }
        }

        #endregion Private Methods
    }
}