using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using Searcher.Core;

namespace Searcher.SO
{
    public class StackOverflowSearcher : ISearcher<HtmlNode>
    {
        public DateTime? GetSearchResultDateTime(HtmlNode item)
        {
            DateTime? result = null;
            var dtElement = item.CssSelect(".posted.top").FirstOrDefault();
            if (dtElement != null)
            {
                var regex = new System.Text.RegularExpressions.Regex("[0-9]+");
                var match = regex.Match(dtElement.InnerText);
                if (match.Success)
                {
                    int test = 0;
                    if (int.TryParse(match.Value, out test))
                    {
                        var period = "";
                        if (dtElement.InnerText.Trim().Contains("weeks"))
                            result = System.DateTime.Now.Date.AddDays((test*-1)*7);
                        else if(dtElement.InnerText.Trim().Contains("days"))
                            result = System.DateTime.Now.Date.AddDays(test * -1);
                        else if (dtElement.InnerText.Trim().Contains("hours") || dtElement.InnerText.Trim().Contains("minutes"))
                            result = System.DateTime.Now.Date;
                    }
                }
            }
            return result;
        }

        public string GetSearchResultKey(HtmlNode item)
        {
            string result = null;
            if (item.Attributes.Contains("data-jobid"))
                result = item.Attributes["data-jobid"].Value.Trim();
            return result;
        }

        public string GetSearchResultLocationName(HtmlNode item)
        {
            string result = null;
            var locationElement = item.CssSelect("p.location").FirstOrDefault();

            if (locationElement != null)
            {
                var replacement = locationElement.CssSelect("span.employer").FirstOrDefault();
                result = locationElement.InnerText;
                if (replacement != null)
                    result = result.Replace(replacement.InnerText, "");
                result = result.Trim();
            }
            return result;
        }

        public string GetSearchResultLocationStateProvince(HtmlNode item)
        {
            var result = "";
            var locationName = GetSearchResultLocationName(item);
            if (!String.IsNullOrEmpty(locationName))
            {
                var regex = new System.Text.RegularExpressions.Regex("([A-Za-z]+, )(?<sp>[A-Z]{2})");
                var matches = regex.Match(locationName);
                if (matches.Groups["sp"] != null)
                    result = matches.Groups["sp"].Value.Trim();
            }

            return result;
        }

        public string GetSearchResultCityName(HtmlNode item)
        {
            var result = "";
            var locationName = GetSearchResultLocationName(item);
            if (!String.IsNullOrEmpty(locationName))
            {
                var regex = new System.Text.RegularExpressions.Regex("(?<city>[A-Za-z]+)(?:,)");
                var matches = regex.Match(locationName);
                if (matches.Groups["city"] != null)
                    result = matches.Groups["city"].Value.Trim();
            }

            return result;
        }

        public string GetSearchResultTitle(HtmlNode item)
        {
            var result = "";
            var titleElement = item.CssSelect("a.job-link").FirstOrDefault();
            if (titleElement != null)
                result = titleElement.InnerText.Trim();
            return result;
        }

        public Uri GetSearchResultUri(HtmlNode item)
        {
            Uri result = null;
            var linkElement = item.CssSelect("a.job-link").FirstOrDefault();
            if (linkElement != null)
            {
                if (linkElement.Attributes.Contains("href"))
                    result = new Uri(String.Format("http://careers.stackoverflow.com{0}",linkElement.Attributes["href"].Value));
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
            var result = document.DescendantsAndSelf().Where(x => x.Attributes.Contains("data-jobid")).ToList();
            return result;
        }
    }
}
