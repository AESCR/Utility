using System.Net.Http;
using BenchmarkDotNet.Attributes;
using Common.Utility.Extensions.HttpClient;
using Common.Utility.Http;

namespace Common.Benchmarks.HttpClientVsWebRequest
{
    /// <summary>
    /// 内存测试
    /// </summary>
    // [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    [HtmlExporter]
    [MemoryDiagnoser]//要显示GC和内存分配
    public class TestHttp
    {
        [Params("http://121.36.213.19:8017/")]
        public string Url;
        private HttpClient2 httpHelper;
        private HttpClient httpClient;
        private WebClient webClient;
        public TestHttp()
        {
            webClient=new WebClient();
            httpHelper=new HttpClient2();
            httpClient=new HttpClient();
        }
        [Benchmark]
        public void TestHttpHelper()
        {
            httpHelper.GetAsync(Url);
        }
        [Benchmark]
        public void TestHttpClient()
        {
            httpClient.DoGet(Url);
        }
        [Benchmark]
        public void TestWebRequest()
        {
            webClient.GetData(Url);
        }
    }
}