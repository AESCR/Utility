using BenchmarkDotNet.Running;

namespace Common.Benchmarks
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }

        #endregion Private Methods
    }
}