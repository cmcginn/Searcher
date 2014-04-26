using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Searcher.Core
{
    public class HtmlDocumentCollector
    {
        public static async Task<string> GetDocument(string url)
        {
            var result = "";
            using (var wc = new System.Net.WebClient())
            {
                result = wc.DownloadString(url);
            }
            return result;
        }

        public static async Task<HtmlDocument> GetHtmlDocument(string html)
        {
            var result = new HtmlAgilityPack.HtmlDocument();
            result.OptionOutputAsXml = true;
            result.LoadHtml(html);
            return result;
        }
    }
}
