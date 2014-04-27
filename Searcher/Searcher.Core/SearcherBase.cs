using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Searcher.Core
{
    public abstract class SearcherBase : ISearcher<HtmlNode>
    {
        public SearcherConfiguration SearcherConfiguration { get; set; }
        public abstract DateTime? GetSearchResultDateTime(HtmlNode item);

        public virtual string GetSearchResultKey(HtmlNode item)
        {
            string result = null;
            if (item.Attributes.Contains(SearcherConfiguration.SearchResultKeyAttributeName))
                result = item.Attributes[SearcherConfiguration.SearchResultKeyAttributeName].Value.Trim();
            return result;
        }

        public abstract string GetSearchResultLocationName(HtmlNode item);

        public virtual string GetSearchResultLocationStateProvince(HtmlNode item)
        {
            var result = "";
            var s = GetSearchResultLocationName(item);
            if (s != null)
            {
                var expr = new System.Text.RegularExpressions.Regex(SearcherConfiguration.SearchResultsStateProvinceGroupRegex);
                var matches = expr.Match(s);
                if (matches.Groups["stateprovince"] != null)
                    result = matches.Groups["stateprovince"].Value.Trim();
            }
            return result;
        }
        public string GetSearchResultCityName(HtmlNode item)
        {
            var result = "";
            var s = GetSearchResultLocationName(item);
            if (s != null)
            {
                var expr = new System.Text.RegularExpressions.Regex(SearcherConfiguration.SearchResultsCityGroupRegex);
                var matches = expr.Match(s);
                if (matches.Groups["city"] != null)
                    result = matches.Groups["city"].Value.Trim();
            }
            return result;
        }

        public virtual string GetSearchResultTitle(HtmlNode item)
        {
            string result = null;
            var titleLink = item.CssSelect(SearcherConfiguration.SearchResultTitleLinkSelector).FirstOrDefault();

            if (titleLink != null)
                result = System.Web.HttpUtility.HtmlDecode(titleLink.InnerText);
            return result;
        }

        public virtual Uri GetSearchResultUri(HtmlNode item)
        {
            Uri result = null;
            var uriElement = item.CssSelect(SearcherConfiguration.SearchResultSourceUriLinkSelector).FirstOrDefault();
            if (uriElement != null)
            {
                var href = uriElement.Attributes["href"];
                if (href != null)
                    Uri.TryCreate(href.Value.Trim(), UriKind.Absolute, out result);

            }
            return result;
        }

        public virtual async Task<SearchResult> GetSearchResult(HtmlNode item)
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

        public virtual async Task<List<HtmlNode>> GetSearchResultNodes(HtmlNode document)
        {
            var result = document.SelectNodes(SearcherConfiguration.SearchResultsParentNodeXPathSelector).ToList();
            return result;
        }
    }
}
