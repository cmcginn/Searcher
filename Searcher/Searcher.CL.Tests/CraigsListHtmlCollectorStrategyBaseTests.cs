using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Searcher.Core;
using Searcher.Core.Tests;

namespace Searcher.CL.Tests
{
    [TestClass]
    public class CraigsListHtmlCollectorStrategyBaseTests : HtmlCollectorStrategyBaseTests
    {

        [TestMethod()]
        public override void CollectSearchQueriesTest()
        {
            var validation = new List<string> { "http://bismarck.craigslist.org/search/sof?query+=C%23", "http://springfieldil.craigslist.org/search/sof?query+=ASP" };
            this.CollectSearchQueries(validation);
        }
        [TestMethod()]
        public override void CollectSearchesTest()
        {
            this.CollectSearches();
        }
        [TestMethod()]
        public override void ScoreResultsTest()
        {
            this.ScoreResults();
        }
        [TestMethod()]
        public override HtmlCollectorStrategyBase GetStrategy()
        {
            var result = new HtmlCollectorStrategyBase();
            result.ScoredKeywords = new Dictionary<string, int>();
            result.ScoredKeywords.Add("C#", 20);
            result.ScoredKeywords.Add("ASP", 20);
            result.SearchBaseUrls =
                TestData.SearchUrls.Split(System.Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries).Take(2).ToList();
            result.SearchTerms = new List<string> { "C#", "ASP" };
            result.Searcher = new CraigslistSearcher();
            result.Searcher.SearcherConfiguration = new SearcherConfiguration
            {
                SearchQueryFormatExpression = "{0}/search/sof?query+={1}",
                SearchResultKeyAttributeName = "data-pid",
                SearchResultsCityGroupRegex = "(<?city>[\\w. ]+)",
                SearchResultSourceUriLinkSelector = ".pl a",
                SearchResultsParentNodeXPathSelector = "//p['data-pid']",
                SearchResultsStateProvinceGroupRegex = "(<?stateprovince>[\\w. ]+)",
                SearchResultTitleLinkSelector = ".pl a"
            };
            //mocked document
            var mockDoc = new HtmlDocument();
            mockDoc.OptionOutputAsXml = true;
            mockDoc.LoadHtml(TestData.SearchResultsDocument);
            result.Searcher.DocumentNode = mockDoc.DocumentNode;
            return result;

        }
    }
}
