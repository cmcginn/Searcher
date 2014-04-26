using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScrapySharp.Extensions;
using Searcher.Core;
using Searcher.CL;

namespace Searcher.CL.Tests
{
    [TestClass]
    public class SearcherTests
    {
        private static HtmlDocument _document;
        private static ISearcher<HtmlNode> _target;
        private static List<HtmlNode> _nodes;


        [ClassInitialize]
        public static void Init(TestContext context)
        {

            _document = new HtmlDocument();
            _document.OptionOutputAsXml = true;
            _document.LoadHtml(TestData.SearchResultsDocument);
            _target = new CraigslistSearcher();
            _nodes = _target.GetSearchResultNodes(_document.DocumentNode).Result;
        }

        [TestMethod()]
        public void GetDateTimeTest()
        {
            Assert.IsTrue(_nodes.Any(x => _target.GetSearchResultDateTime(x).HasValue));
        }

        [TestMethod()]
        public void GetSearchResultKeyTest()
        {
            Assert.IsTrue(_nodes.Any(x => !String.IsNullOrEmpty(_target.GetSearchResultKey(x))));
        }

        [TestMethod()]
        public void GetSearchResultLocationNameTest()
        {
            Assert.IsTrue(_nodes.Any(x => !String.IsNullOrEmpty(_target.GetSearchResultLocationName(x))));
        }



        [TestMethod()]
        public void GetSearchResultCityNameTest()
        {
            Assert.IsTrue(_nodes.Any(x => !String.IsNullOrEmpty(_target.GetSearchResultCityName(x))));
        }

        [TestMethod()]
        public void GetSearchResultLocationStateProvinceTest()
        {
            Assert.IsTrue(_nodes.Any(x => !String.IsNullOrEmpty(_target.GetSearchResultLocationStateProvince(x))));
        }

        [TestMethod()]
        public void GetSearchResultTitleTest()
        {
            Assert.IsTrue(_nodes.Any(x => !String.IsNullOrEmpty(_target.GetSearchResultTitle(x))));
        }

        [TestMethod()]
        public void GetSearchResultUriTest()
        {
            Assert.IsTrue(_nodes.Any(x => _target.GetSearchResultUri(x)!=null));
        }

        [TestMethod()]
        public void GetSearchResultTest()
        {
            var html = TestData.JobPostingRow;
            var doc = new HtmlDocument();
            doc.OptionOutputAsXml = true;
            doc.LoadHtml(html);
            var actual = _target.GetSearchResult(doc.DocumentNode.CssSelect("p").First()).Result;
            Assert.IsInstanceOfType(actual, typeof (SearchResult));
        }

        [TestMethod()]
        public void GetSearchResultNodesTest()
        {
            var actual = _target.GetSearchResultNodes(_document.DocumentNode).Result;
            Assert.IsTrue(actual.Count > 0);
        }




    }
}
