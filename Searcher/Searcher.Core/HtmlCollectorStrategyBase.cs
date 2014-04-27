using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Searcher.Core
{
    public abstract class HtmlCollectorStrategyBase : ICollectorStrategy
    {
        public List<string> SearchBaseUrls { get; set; }
        public List<string> SearchUrls { get; private set; }
        public List<string> SearchTerms { get; set; }
        public Dictionary<string, int> ScoredKeywords { get; set; }
        public List<SearchResult> SearchResults { get; private set; }
        public abstract List<string> CollectSearchQueries();
        public SearcherBase Searcher { get; set; }
        public void CollectSearches()
        {
           
            SearchResults = new List<SearchResult>();

            SearchUrls.ForEach(x =>
            {
                var content = HtmlDocumentCollector.GetDocument(x).Result;
                var doc = HtmlDocumentCollector.GetHtmlDocument(content).Result;
                var nodes = Searcher.GetSearchResultNodes(doc.DocumentNode).Result;
                nodes.ForEach(node =>
                {
                    var item = Searcher.GetSearchResult(node).Result;
                    if (item != null)
                        SearchResults.Add(item);
                });
            });
        }
        public virtual void ScoreResults()
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
