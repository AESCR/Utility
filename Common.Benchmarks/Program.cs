using BenchmarkDotNet.Running;
using Common.Benchmarks.LogRedis;
using Common.Benchmarks.RedisTest;

namespace Common.Benchmarks
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(TestRedis).Assembly);

            // BenchmarkRunner.Run(typeof(Program).Assembly);
        }

        #endregion Private Methods
    }
}