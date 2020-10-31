using BenchmarkDotNet.Running;
using Common.Benchmarks.LogRedis;

namespace Common.Benchmarks
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(LogRedisTest).Assembly);

            // BenchmarkRunner.Run(typeof(Program).Assembly);
        }

        #endregion Private Methods
    }
}