using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.SO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Searcher.SO.Tests
{
    [TestClass()]
    public class StackOverflowCollectorStrategyTests
    {
        private static List<string> _searchUrls;

        [ClassInitialize]
        public static void Init(TestContext ctx)
        {
            _searchUrls = TestData.SearchUrls.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

        }

        [TestMethod()]
        public void CollectSearchQueriesTest()
        {
            var strategy = new StackOverflowCollectorStrategy();
            strategy.SearchBaseUrls = _searchUrls;
            strategy.SearchTerms = new List<string> { "C#,ASP,.NET" };
            var actual = strategy.CollectSearchQueries();
            Assert.IsTrue(actual.Contains("http://careers.stackoverflow.com/jobs?searchTerm=C%23%2cASP%2c.NET"));
            
        }

        [TestMethod()]
        public void CollectSearchesTest()
        {

            var strategy = new StackOverflowCollectorStrategy();

            strategy.SearchBaseUrls = _searchUrls;
            strategy.SearchTerms = new List<string> { "C#,ASP,.NET" };
            strategy.CollectSearchQueries();
            strategy.CollectSearches();

            Assert.IsTrue(strategy.SearchResults.Any());
        }

        [TestMethod()]
        public void ScoreResultsTest()
        {
            var strategy = new StackOverflowCollectorStrategy();
            var searchUrls =
             TestData.SearchUrls.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList().Take(2).ToList();
            strategy.SearchBaseUrls = searchUrls;
            strategy.SearchTerms = new List<string> { "C#,ASP,.NET" };
            strategy.ScoredKeywords = new Dictionary<string, int>();
            strategy.ScoredKeywords.Add("C#", 20);
            strategy.ScoredKeywords.Add("Telecommute", 100);

            strategy.CollectSearches();
            Assert.IsTrue(strategy.SearchResults.Any(x => x.Uri != null));
            strategy.ScoreResults();
            Assert.IsTrue(strategy.SearchResults.Any(x => x.KeywordScore > 0));
        }
    }
}
