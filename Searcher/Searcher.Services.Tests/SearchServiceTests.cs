using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Searcher.Services.Tests
{
    [TestClass()]
    public class SearchServiceTests
    {
        SearchService GetTarget()
        {
            var result = new SearchService();
            return result;
        }
        [TestMethod()]
        public void GetSearchResultsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveSearchTest()
        {
            var s = new SearchModel();
            s.SearchName = "Test";
            s.SearchTerms = new List<SearchTermModel>();
            s.SearchTerms.Add()
            //Assert.Fail();
        }
    }
}
