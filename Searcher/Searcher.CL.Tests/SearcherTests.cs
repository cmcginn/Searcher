using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScrapySharp.Extensions;
using Searcher.Core;
using Searcher.CL;
using Searcher.Core.Tests;

namespace Searcher.CL.Tests
{
    [TestClass()]
    public class SearcherTests : SearcherBaseTests
    {
        public override List<HtmlNode> GetNodes()
        {
            var doc = new HtmlDocument();
            doc.OptionOutputAsXml = true;
            doc.LoadHtml(TestData.SearchResultsDocument);
            return doc.DocumentNode.SelectNodes("//p['data-pid']").ToList();
        }

        public override SearcherBase GetTarget()
        {
            var result = new CraigslistSearcher();

            result.SearcherConfiguration = new SearcherConfiguration
            {
                SearchQueryFormatExpression = "{0}/search/sof?query+={1}",
                SearchResultKeyAttributeName = "data-pid",
                SearchResultsCityGroupRegex = "(?<city>[\\w. ]+)",
                SearchResultSourceUriLinkSelector = ".pl a",
                SearchResultsParentNodeXPathSelector = "//p['data-pid']",
                SearchResultsStateProvinceGroupRegex = "(?:[\\w.,])([ ])(?<stateprovince>[A-Za-z]{2})",
                SearchResultTitleLinkSelector = ".pl a"
            };
            var doc = new HtmlDocument();
            doc.OptionOutputAsXml = true;
            doc.LoadHtml(TestData.SearchResultsDocument);
            result.DocumentNode = doc.DocumentNode;
            return result;
        }

        public override HtmlNode GetValidItemNode()
        {
            var doc = new HtmlDocument();
            doc.OptionOutputAsXml = true;
            doc.LoadHtml(TestData.JobPostingRow);
            return doc.DocumentNode.SelectNodes("//p['data-pid']").First();

        }
        [TestMethod]
        public override void GetSearchResultDateTimeTest()
        {
            this.GetSearchResultDateTime();
        }
        [TestMethod]
        public override void GetSearchResultKeyTest()
        {
            this.GetSearchResultKey();
        }
        [TestMethod]
        public override void GetSearchResultLocationNameTest()
        {
            this.GetSearchResultLocationName();
        }
        [TestMethod]
        public override void GetSearchResultLocationStateProvinceTest()
        {
            this.GetSearchResultLocationStateProvince();
        }
        [TestMethod]
        public override void GetSearchResultCityNameTest()
        {
            this.GetSearchResultCityName();
        }
        [TestMethod]
        public override void GetSearchResultTitleTest()
        {
            this.GetSearchResultTitle();
        }
        [TestMethod]
        public override void GetSearchResultUriTest()
        {
            this.GetSearchResultUri();
        }
        [TestMethod]
        public override void GetSearchResultTest()
        {
            this.GetSearchResult();
        }
        [TestMethod]
        public override void GetSearchResultNodesTest()
        {
            this.GetSearchResultNodes();
        }
    }
}
