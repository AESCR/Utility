using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utility.Autofac;
using Common.Utility.Random.Num;
using HtmlAgilityPack;

namespace Common.Utility.Random.UAgent
{
    public class RandomUAgent : ISingletonDependency
    {
        public RandomUAgent()
        {
           //TODo WebApi
        }
        /// <summary>
        /// 爬取UAgent
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<string>> SpiderUAgent()
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
            Dictionary<string, List<string>> spiderUAgent = new Dictionary<string, List<string>>();
            urls.AsParallel().ForAll(url =>
            {
                Console.WriteLine(url.Name + url.Url);
                List<string> stimuli = new List<string>();
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

                    stimuli.Add(u);
                }

                string name = url.Name;
                try
                {

                    spiderUAgent.Add(name, stimuli);
                   
                }
                catch (Exception e)
                {
                    return ;
                }
            });
            try
            {
                //redisCache.AddHash(MemoryEnum.UserAgent.GetMemoryKey(), spiderUAgent, TimeSpan.FromDays(30));
                return spiderUAgent;
            }
            catch (Exception)
            {
                return spiderUAgent;
            }
           
        }
    }
}
