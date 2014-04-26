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
    public class CraigslistSearcher:ISearcher<HtmlNode>
    {
        public DateTime? GetSearchResultDateTime(HtmlNode item)
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

        public string GetSearchResultKey(HtmlNode item)
        {
            string result = null;
            if (item.Attributes.Contains("data-pid"))
                result =item.Attributes["data-pid"].Value.Trim();
            return result;
        }

        public string GetSearchResultLocationName(HtmlNode item)
        {
            string result = null;
            var locationElement = item.CssSelect(".pnr").FirstOrDefault();

            if (locationElement != null && locationElement.Element("small") != null)
                result = locationElement.Element("small").InnerText.Trim();
            return result;
        }

        public string GetSearchResultLocationStateProvince(HtmlNode item)
        {
            var result = "";
            var s = GetSearchResultLocationName(item);
            if (s != null)
            {
                var expr = new System.Text.RegularExpressions.Regex("[\\w. ]+");
                var matches = expr.Matches(s);
                if (matches.Count > 1)
                    result = matches[1].Value.Trim();
            }
            return result;
        }

        public string GetSearchResultCityName(HtmlNode item)
        {
            var result = "";
            var s = GetSearchResultLocationName(item);
            if (s != null)
            {
                var expr = new System.Text.RegularExpressions.Regex("[\\w. ]+");
                var matches = expr.Matches(s);
                if (matches.Count > 0)
                    result = matches[0].Value.Trim();
            }
            return result;
        }


        public string GetSearchResultTitle(HtmlNode item)
        {
            string result = null;
            var titleLink = item.CssSelect(".pl a").FirstOrDefault();

            if (titleLink != null)
                result = System.Web.HttpUtility.HtmlDecode(titleLink.InnerText);
            return result;
        }

        public Uri GetSearchResultUri(HtmlNode item)
        {
            Uri result = null;
            var pl = item.CssSelect(".pl a").FirstOrDefault();
            if (pl != null)
            {
                var href = pl.Attributes["href"];
                if (href != null)
                    Uri.TryCreate(href.Value.Trim(), UriKind.Absolute, out result);

            }
            return result;
        }

        public async Task<SearchResult> GetSearchResult(HtmlNode item)
        {
            SearchResult result = null;
            var dt = GetSearchResultDateTime(item);

            if (dt.HasValue)
            {
                var key = GetSearchResultKey(item);
                if (!String.IsNullOrEmpty(key))
                {
                    var source = GetSearchResultUri(item);
                    if (source != null)
                    {
                        var title = GetSearchResultTitle(item);
                        if (!String.IsNullOrEmpty(title))
                        {

                            var locationName = GetSearchResultLocationName(item);
                            var stateProvinceName = GetSearchResultLocationStateProvince(item);
                            var cityName = GetSearchResultCityName(item);
                            result = new SearchResult();
                            result.Key = key;
                            result.Title = title;
                            result.LocationName = locationName;
                            result.PostDate = dt.Value;
                            result.Uri = source;
                            result.LocationStateProvince = stateProvinceName;
                            result.LocationCityName = cityName;
                        }

                    }
                }

            }
            return result;
        }

        public async Task<List<HtmlNode>> GetSearchResultNodes(HtmlNode document)
        {
            var result = document.DescendantsAndSelf().Where(x => x.Attributes.Contains("data-pid")).ToList();
            return result;
        }
    }
}
