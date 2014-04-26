using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.Core;

namespace Searcher.CL
{
    public class CraigslistCollectorStrategy:ICollectorStrategy
    {

        private List<String> _searchUrls;


        public List<string> SearchBaseUrls { get; set; }
        public List<string> SearchTerms { get; set; }
        public Dictionary<string, int> ScoredKeywords { get; set; }
        public List<SearchResult> SearchResults { get; private set; }
        public List<string> SearchUrls
        {
            get { return _searchUrls ?? (_searchUrls = CollectSearchQueries()); }
        }

        public List<string> CollectSearchQueries()
        {
            var result = new List<string>();
            SearchBaseUrls.ToList().ForEach(x => SearchTerms.ForEach(k => result.Add(String.Format("{0}/search/sof?query+={1}", x, System.Web.HttpUtility.UrlEncode(k)))));
            return result;
        }
        public void CollectSearches()
        {

            var searcher = new CraigslistSearcher();
            SearchResults = new List<SearchResult>();
            SearchUrls.ForEach(x =>
            {
                var content = HtmlDocumentCollector.GetDocument(x).Result;
                var doc = HtmlDocumentCollector.GetHtmlDocument(content).Result;
                var nodes = searcher.GetSearchResultNodes(doc.DocumentNode).Result;
                nodes.ForEach(node =>
                {
                    var item = searcher.GetSearchResult(node).Result;
                    if(item != null)
                        SearchResults.Add(item);
                });
            });
        }
        public void ScoreResults()
        {
            
            var items = SearchResults.Where(x => x.Uri != null).ToList();
            Parallel.ForEach(items, (a) =>
            {
                var html = HtmlDocumentCollector.GetDocument(a.Uri.ToString()).Result;
                var doc = HtmlDocumentCollector.GetHtmlDocument(html).Result;
                var content = doc.DocumentNode.InnerText;
                ScoredKeywords.Select(x => x.Key).ToList().ForEach(kw =>
                {
                    if (content.Contains(kw))
                        a.KeywordScore += ScoredKeywords[kw];
                });
            });
        
        }
    }
}
