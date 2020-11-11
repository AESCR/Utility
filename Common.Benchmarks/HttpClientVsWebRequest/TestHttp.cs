using BenchmarkDotNet.Attributes;
using Common.Utility.Extensions.HttpClient;
using Common.Utility.HttpRequest;
using System.Net.Http;

namespace Common.Benchmarks.HttpClientVsWebRequest
{
    /// <summary>
    /// 内存测试
    /// </summary>
    // [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    /* [HtmlExporter]
     [MemoryDiagnoser]//要显示GC和内存分配*/

    public class TestHttp
    {
        private HttpClient httpClient;

        private HttpClient2 httpHelper;

        private WebClient webClient;

        [Params("http://121.36.213.19:8017/")]
        public string Url;

        public TestHttp()
        {
            webClient = new WebClient();
            httpHelper = new HttpClient2();
            httpClient = new HttpClient();
        }

        //[Benchmark]
        public void TestHttpClient()
        {
            httpClient.DoGet(Url);
        }

        //[Benchmark]
        public void TestHttpHelper()
        {
            httpHelper.GetAsync(Url);
        }

        // [Benchmark]
        public void TestWebRequest()
        {
            webClient.GetData(Url);
        }
    }
}