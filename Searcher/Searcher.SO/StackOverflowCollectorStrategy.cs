using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.Core;

namespace Searcher.SO
{
    public class StackOverflowCollectorStrategy : ICollectorStrategy
    {
        private List<String> _searchUrls;
        public List<string> SearchBaseUrls { get; set; }
        public List<string> SearchUrls
        {
            get { return _searchUrls ?? (_searchUrls = CollectSearchQueries()); }
        }
        public List<string> SearchTerms { get; set; }
        public Dictionary<string, int> ScoredKeywords { get; set; }
        public List<SearchResult> SearchResults { get; private set; }
        public List<string> CollectSearchQueries()
        {
            var result = new List<string>();
            SearchBaseUrls.ToList().ForEach(x => SearchTerms.ForEach(k => result.Add(String.Format("{0}/jobs?searchTerm={1}", x, System.Web.HttpUtility.UrlEncode(k)))));
            return result;
        }

        public void CollectSearches()
        {
            var searcher = new StackOverflowSearcher();
            SearchResults = new List<SearchResult>();

            SearchUrls.ForEach(x =>
            {
                var content = HtmlDocumentCollector.GetDocument(x).Result;
                var doc = HtmlDocumentCollector.GetHtmlDocument(content).Result;
                var nodes = searcher.GetSearchResultNodes(doc.DocumentNode).Result;
                nodes.ForEach(node =>
                {
                    var item = searcher.GetSearchResult(node).Result;
                    if (item != null)
                        SearchResults.Add(item);
                });
            });
        }

        public void ScoreResults()
        {
            var items = SearchResults.Where(x => x.Uri != null).GroupBy(x => x.Uri).Select(x => x.First()).ToList();
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
