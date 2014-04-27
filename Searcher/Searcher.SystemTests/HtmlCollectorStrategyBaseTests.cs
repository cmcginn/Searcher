using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Searcher.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Searcher.Core.Tests
{
    [TestClass]
    public abstract class HtmlCollectorStrategyBaseTests
    {

        private HtmlCollectorStrategyBase _strategy;

        [TestMethod]
        public abstract void CollectSearchQueriesTest();

        [TestMethod]
        public abstract void CollectSearchesTest();

        [TestMethod]
        public abstract void ScoreResultsTest();

        public abstract HtmlCollectorStrategyBase GetStrategy(); 
        public HtmlCollectorStrategyBase Strategy
        {
            get { return _strategy ?? (_strategy = GetStrategy()); }
        }


        public virtual void CollectSearchQueries(List<string> validation)
        {
            var actual = Strategy.CollectSearchQueries();
            validation.ForEach(x => Assert.IsTrue(actual.Contains(x)));
        }

 
        public virtual void CollectSearches()
        {
       
            Strategy.CollectSearchQueries();
            Strategy.CollectSearches();
            Assert.IsTrue(Strategy.SearchResults.Any());
        }


        public virtual void ScoreResults()
        {
            Strategy.CollectSearches();
            Assert.IsTrue(_strategy.SearchResults.Any(x => x.Uri != null));
            Strategy.ScoreResults();
            Assert.IsTrue(_strategy.SearchResults.Any(x => x.KeywordScore > 0));
        }
    }
}
