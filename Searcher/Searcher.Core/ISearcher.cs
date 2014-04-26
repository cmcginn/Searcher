using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Core
{
    public interface ISearcher<T>
    {
        DateTime? GetSearchResultDateTime(T item);
        string GetSearchResultKey(T item);
        string GetSearchResultLocationName(T item);

        string GetSearchResultLocationStateProvince(T item);

        string GetSearchResultCityName(T item);

        string GetSearchResultTitle(T item);

        Uri GetSearchResultUri(T item);

        Task<SearchResult> GetSearchResult(T item);

        Task<List<T>> GetSearchResultNodes(T document);
    }
}
