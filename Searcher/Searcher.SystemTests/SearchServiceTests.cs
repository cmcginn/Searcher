using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.Core;
using Searcher.Data;
using Searcher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Searcher.Services.Tests
{
    [TestClass()]
    public class SearchServiceTests
    {
        ISearchService GetTarget()
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
        public void SaveSearchTest_WhenInsert()
        {
            var target = GetTarget();
            var search = new SearchModel();
            search.SearchName = "Test";
            search.SearchTerms = new List<ISearchTerm>
            {
                new SearchTermModel{ Term="Testing", Score=5}
            };
            target.SaveSearch(search);
           
        }

        [TestMethod()]
        public void SaveSearchTest_WhenUpdate()
        {
            var target = GetTarget();
            var targetSearch = new SearchModel();
            targetSearch.SearchName = Guid.NewGuid().ToString();
            targetSearch.CreatedOnUtc = System.DateTime.UtcNow;
            var t1 = new SearchTermModel();
            t1.Term = "Static";
            t1.Score = 1;
            var t2 = new SearchTermModel();
            t2.Term = "Dynamic";
            t2.Score = 2;
            targetSearch.SearchTerms = new List<ISearchTerm>();
            targetSearch.SearchTerms.Add(t1);
            targetSearch.SearchTerms.Add(t2);
            target.SaveSearch(targetSearch);
            
            //check existing
            using (var ctx = new SearcherEntities())
            {
                var existing = ctx.Searches.Single(x => x.SearchName == targetSearch.SearchName);
                
                var existingT1 = existing.SearchTerms.Single(x => x.Term == t1.Term);
                var existingT2 = existing.SearchTerms.Single(x => x.Term == t2.Term);
                Assert.AreEqual(t1.Score, existingT1.Score, "First Term Scores do not match");
                Assert.AreEqual(t2.Score, existingT2.Score, "Second Term Scores do not match");

                targetSearch.Id = existing.Id;
         
            }
            var t3 = new SearchTermModel();
            t3.Term = "Brand New";
            t3.Score = 3;

            targetSearch.SearchTerms.Remove(t1);
            t2.Score = 100;
            targetSearch.SearchTerms.Add(t3);
            target.SaveSearch(targetSearch);
            using (var ctx = new SearcherEntities())
            {
                var existing = ctx.Searches.Single(x => x.Id == targetSearch.Id);
                var newTerm = existing.SearchTerms.Single(x => x.Term == t3.Term);
                var deleted = existing.SearchTerms.SingleOrDefault(x => x.Term == t1.Term);
                Assert.IsNull(deleted, "Deleted Term exists in Database");
                var existingTerm = existing.SearchTerms.SingleOrDefault(x => x.Term == t2.Term);
                Assert.IsTrue(existingTerm.Score == t2.Score, "Updated Score does not match");
            }
    
        }
        [ExpectedException(typeof(System.InvalidOperationException))]
        [TestMethod]
        public void SaveSearchTest_WhenDuplicateTerms()
        {
            var target = GetTarget();
            var targetSearch = new SearchModel();
            targetSearch.SearchName = Guid.NewGuid().ToString();
            var t1 = new SearchTermModel();
            t1.Term = "Duplicate";
            t1.Score = 1;
            var t2 = new SearchTermModel();
            t2.Term = "Duplicate";
            t2.Score = 2;
            targetSearch.SearchTerms = new List<ISearchTerm> {t1, t2};
            target.SaveSearch(targetSearch);
        }
    }
}
