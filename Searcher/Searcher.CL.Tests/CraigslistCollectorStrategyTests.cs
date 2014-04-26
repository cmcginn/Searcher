using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.CL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Searcher.CL.Tests
{
    [TestClass()]
    public class CraigslistCollectorStrategyTests
    {

      
        private static List<string> _searchUrls;

        [ClassInitialize]
        public static void Init(TestContext ctx)
        {
            var searchUrls = TestData.SearchUrls.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            
        }

        [TestMethod()]
        public void CollectSearchesTest()
        {

            var strategy = new CraigslistCollectorStrategy();
            var searchUrls =
                TestData.SearchUrls.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList().Take(2).ToList();
            strategy.SearchBaseUrls = searchUrls;
            strategy.SearchTerms = new List<string> {"C#", "ASP"};
            strategy.ScoredKeywords = new Dictionary<string, int>();
            strategy.ScoredKeywords.Add("C#", 20);
            strategy.ScoredKeywords.Add("Telecommute", 100);
            strategy.CollectSearchQueries();
            strategy.CollectSearches();

            Assert.IsTrue(strategy.SearchResults.Any());
        }

        [TestMethod()]
        public void ScoreResultsTest()
        {
            var strategy = new CraigslistCollectorStrategy();
            var searchUrls =
             TestData.SearchUrls.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList().Take(2).ToList();
            strategy.SearchBaseUrls = searchUrls;
            strategy.SearchTerms = new List<string> { "C#", "ASP" };
            strategy.ScoredKeywords = new Dictionary<string, int>();
            strategy.ScoredKeywords.Add("C#", 20);
            strategy.ScoredKeywords.Add("Telecommute", 100);
            
            strategy.CollectSearches();
            Assert.IsTrue(strategy.SearchResults.Any(x => x.Uri != null));
            //Assert.IsNotNull(_strategy.SearchResults);
            strategy.ScoreResults();
            Assert.IsTrue(strategy.SearchResults.Any(x => x.KeywordScore > 0));
            
        }

        [TestMethod()]
        public void CollectSearchQueriesTest()
        {
            var strategy = new CraigslistCollectorStrategy();
            var searchUrls =
               TestData.SearchUrls.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            strategy.SearchBaseUrls = searchUrls;
            strategy.SearchTerms = new List<string> { "C#", "ASP" };
            var actual = strategy.CollectSearchQueries();
            Assert.IsTrue(actual.Contains("http://sanantonio.craigslist.org/search/sof?query+=C%23"));
            Assert.IsTrue(actual.Contains("http://sanantonio.craigslist.org/search/sof?query+=ASP"));
        }
    }
}
