using BenchmarkDotNet.Running;
using Common.Benchmarks.RedisTest;

namespace Common.Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(TestRedis).Assembly);

            // BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}