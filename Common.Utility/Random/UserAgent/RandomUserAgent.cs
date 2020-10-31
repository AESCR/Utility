using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utility.Autofac;
using Common.Utility.MemoryCache.Model;
using Common.Utility.MemoryCache.Redis;
using Common.Utility.Random.Num;
using HtmlAgilityPack;

namespace Common.Utility.Random.UserAgent
{
    public class RandomUserAgent: ISingletonDependency
    {
        private readonly IRedisCache redisCache;
        Dictionary<string, List<string>> uagent = new Dictionary<string, List<string>>();
        private readonly RandomNum random=new RandomNum();

        public RandomUserAgent(IRedisCache redisCache)
        {
            this.redisCache = redisCache;
            SpiderUAgent();
        }

        public string GetUserAgent()
        {
            if (redisCache.Exists(MemoryEnum.UserAgent.GetMemoryKey()))
            {
                var result = redisCache.GetCollection(MemoryEnum.UserAgent.GetMemoryKey());
                var r= random.GetRandomInt(0, result.Length);
                return result[r];
            }
            else
            {
                Task.Run(SpiderUAgent);
                var result = SpiderChromeUAgent();
                var r = random.GetRandomInt(0, result.Count);
                if (result.Count==0)
                {
                    return "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36";
                }
                else
                {
                    return result[r];
                }
            }
        }

        private List<string> SpiderChromeUAgent()
        {
            Dictionary<string, List<string>> uagent = new Dictionary<string, List<string>>();
            var web = new HtmlWeb();
            List<string> ulist = new List<string>();
            HtmlDocument xdoc = web.Load("http://www.73207.com/useragent/pages/chrome/index.htm");
            var uNodes = xdoc.DocumentNode.SelectNodes("//*[@id='liste']//li/a");
            if (uNodes == null)
            {
                return ulist;
            }
            foreach (var node in uNodes)
            {
                var u = node?.InnerText;
                ulist.Add(u);
            }

            try
            {
                uagent.Add("Chrome", ulist);
                redisCache.AddHash(MemoryEnum.UserAgent.GetMemoryKey(), uagent, TimeSpan.FromDays(30));
                return ulist;
            }
            catch (Exception e)
            {
                return ulist;
            }
         
        }
        private Dictionary<string, List<string>> SpiderUAgent()
        {
            var web = new HtmlWeb();
            var xhtml = web.Load("http://www.73207.com/useragent/pages/useragentstring.php.htm");
            var anodes = xhtml.DocumentNode.SelectNodes("//table[@id='auswahl']//a");
            string host = "http://www.73207.com/useragent/pages/";
            List<dynamic> urls = new List<dynamic>();
            foreach (var htmlNode in anodes)
            {
                var name = htmlNode.InnerText;
                var url = htmlNode.GetAttributeValue("href", "");
                urls.Add(new { Name = name, Url = url });
            }
            Dictionary<string, List<string>> uagent = new Dictionary<string, List<string>>();
            urls.AsParallel().ForAll(url =>
            {
                Console.WriteLine(url.Name + url.Url);
                List<string> ulist = new List<string>();
                string ur = url.Url;
                if (string.IsNullOrEmpty(ur) != false) return;
                string tempUrl = host + ur;
                HtmlDocument xdoc = web.Load(tempUrl);
                var uNodes = xdoc.DocumentNode.SelectNodes("//*[@id='liste']//li/a");
                if (uNodes == null)
                {
                    return;
                }
                foreach (var node in uNodes)
                {
                    var u = node?.InnerText;

                    ulist.Add(u);
                }

                string name = url.Name;
                try
                {

                    uagent.Add(name, ulist);
                   
                }
                catch (Exception e)
                {
                    return ;
                }
            });
            try
            {
                redisCache.AddHash(MemoryEnum.UserAgent.GetMemoryKey(), uagent, TimeSpan.FromDays(30));
                return uagent;
            }
            catch (Exception)
            {
                return uagent;
            }
           
        }
    }
}
