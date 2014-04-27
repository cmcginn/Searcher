using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using Searcher.Core;
using HtmlAgilityPack;

namespace Searcher.CL
{
    public class CraigslistSearcher:SearcherBase
    {
        public override DateTime? GetSearchResultDateTime(HtmlNode item)
        {
            DateTime? result = null;
            var dtElement = item.CssSelect(".date").FirstOrDefault();

            if (dtElement != null)
            {
                DateTime test;
                if (DateTime.TryParse(dtElement.InnerText, out test))
                    result = test;
            }
            return result;
        }

        public override string GetSearchResultLocationName(HtmlNode item)
        {
            string result = null;
            var locationElement = item.CssSelect(".pnr").FirstOrDefault();

            if (locationElement != null && locationElement.Element("small") != null)
                result = locationElement.Element("small").InnerText.Trim();
            return result;
        }
    


    }
}
