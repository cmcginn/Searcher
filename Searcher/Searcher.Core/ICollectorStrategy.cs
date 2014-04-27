using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Searcher.Core
{
    public interface ICollectorStrategy
    {
        List<string> SearchBaseUrls { get; set; }
        List<string> SearchUrls { get; }
        List<string> SearchTerms { get; set; }

        Dictionary<string, int> ScoredKeywords { get; set; }
        List<SearchResult> SearchResults { get; }
        List<String> CollectSearchQueries();
        SearcherBase Searcher { get; set; }
    
        void CollectSearches();

        void ScoreResults();
    }
}
