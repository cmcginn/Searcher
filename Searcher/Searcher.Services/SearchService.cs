using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.CL;
using Searcher.Core;
using Searcher.Data;

namespace Searcher.Services
{
    public class SearchService:ISearchService
    {
        public List<SearchResult> GetSearchResults(string urlFilePath, List<string> searchTerms, Dictionary<string, int> scoredKeywords, SearcherBase searcher)
        {
            var collectorStrategy = new HtmlCollectorStrategyBase();
            var searchResultType = searcher.GetType().ToString();

            using (var ctx = new SearcherEntities())
            {
                collectorStrategy.ExcludeUrls = ctx.SearchResultDatas.Where(x=>x.PostType==searchResultType).Select(x => x.PostUrl).ToList();
            }
            collectorStrategy.ScoredKeywords = scoredKeywords;
            collectorStrategy.SearchBaseUrls = System.IO.File.ReadAllLines(urlFilePath).ToList();
            collectorStrategy.SearchTerms = searchTerms;
            if (searcher.GetType() == typeof (CraigslistSearcher))
                InitializeCraigslistSearcher(searcher);
            collectorStrategy.Searcher = searcher;
            collectorStrategy.CollectSearchQueries();
            collectorStrategy.CollectSearches();
            collectorStrategy.ScoreResults();
            PersistSearchResults(collectorStrategy.SearchResults, searchResultType);
            return collectorStrategy.SearchResults;
        }

        void InitializeCraigslistSearcher(SearcherBase searcher)
        {
            searcher.SearcherConfiguration = new SearcherConfiguration
            {
                SearchQueryFormatExpression = "{0}/search/sof?query+={1}",
                SearchResultKeyAttributeName = "data-pid",
                SearchResultsCityGroupRegex = "(?<city>[\\w. ]+)",
                SearchResultSourceUriLinkSelector = ".pl a",
                SearchResultsParentNodeXPathSelector = "//p['data-pid']",
                SearchResultsStateProvinceGroupRegex = "(<?stateprovince>[\\w. ]+)",
                SearchResultTitleLinkSelector = ".pl a"
            };

        }

        void PersistSearchResults(List<SearchResult> results,string postType)
        {
            var searchResultDatas = new List<SearchResultData>();
            results.ToList().ForEach(x =>
            {
                var searchResultData = new SearchResultData
                {
                    Id = Guid.NewGuid(),
                    CreatedOnUtc = System.DateTime.UtcNow,
                    PostDateTime = x.PostDate,
                    PostUrl = x.Uri.ToString(),
                    PostId = x.Key,
                    PostTitle = x.Title,
                    PostType = postType,
                    KeywordScore = x.KeywordScore
                    
                    

                };
                searchResultDatas.Add(searchResultData);

            });
            using (var ctx = new SearcherEntities())
            {

                ctx.SearchResultDatas.AddRange(searchResultDatas);
                ctx.SaveChanges();
            }
        }

        void AddSearch(ISearch target)
        {
            target.Id = Guid.NewGuid();
            target.CreatedOnUtc = System.DateTime.UtcNow;
          
            var search = new Search();
            search.Id = target.Id;
            search.CreatedOnUtc = target.CreatedOnUtc;
            search.SearchName = target.SearchName;
            target.SearchTerms.ForEach(x =>
            {
                x.Id = Guid.NewGuid();
                x.CreatedOnUtc = System.DateTime.UtcNow;
                search.SearchTerms.Add(new SearchTerm
                {
                    CreatedOnUtc = x.CreatedOnUtc,
                    Id = x.Id,
                    Score = x.Score,
                    Term = x.Term
                });
            });
            using (var ctx = new SearcherEntities())
            {
                ctx.Searches.Add(search);
                ctx.SaveChanges();
            }
        }

        void UpdateSearch(ISearch target)
        {
            using (var ctx = new SearcherEntities())
            {
                var original = ctx.Searches.Single(x => x.Id == target.Id);
                var targetTerms = target.SearchTerms.Select(x => x.Term.ToLower());
                var sourceTerms = original.SearchTerms.Select(x => x.Term.ToLower());
                //handle deletes
                original.SearchTerms.Where(x => !targetTerms.Contains(x.Term.ToLower()))
                    .ToList()
                    .ForEach(x =>
                    {
                        ctx.SearchTerms.Remove(x);
                    });
                
                //handle updates to existing 
                original.SearchTerms.ToList().ForEach(x =>
                {
                    var updated = target.SearchTerms.Single(n => n.Term.ToLower() == x.Term.ToLower());
                    if (updated.Score != x.Score)
                    {
                        x.Score = updated.Score;
                        x.UpdatedOnUtc = System.DateTime.UtcNow;
                    }
                });
                //handle inserts
                target.SearchTerms.Where(x => !sourceTerms.Contains(x.Term.ToLower())).ToList().ForEach(x =>
                {
                    var newTerm = new SearchTerm
                    {
                        Id = Guid.NewGuid(),
                        CreatedOnUtc = System.DateTime.UtcNow,
                        Score = x.Score,
                        Term = x.Term
                    };
                    original.SearchTerms.Add(newTerm);
                });
                ctx.SaveChanges();
            }
        }
        public void SaveSearch(ISearch search)
        {
            if (search.SearchTerms.GroupBy(x => x.Term).Any(x => x.Count() > 1))
                throw new System.InvalidOperationException("Cannot add duplicate terms to search");
            //throw new NotImplementedException();
            if (search.Id == Guid.Empty)
                AddSearch(search);
            else
                UpdateSearch(search);
        }
    }
}
