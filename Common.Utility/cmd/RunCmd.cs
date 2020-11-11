using System.Diagnostics;

namespace Common.Utility
{
    public class RunCmd
    {
        private readonly Process proc;

        public RunCmd()
        {
            proc = new Process();
        }

        private void sortProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                //this.BeginInvoke(new Action(() => { this.listBox1.Items.Add(e.Data); }));
            }
        }

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
    }
}