using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Core
{
    public interface ISearchService
    {
        List<SearchResult> GetSearchResults(string urlFilePath, List<string> searchTerms,
            Dictionary<string, int> scoredKeywords, SearcherBase searcher);

        void SaveSearch(ISearch search);
    }
}
