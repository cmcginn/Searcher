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

    public abstract class SearcherBaseTests
    {
        protected HtmlDocument _document;
        protected SearcherBase _target;
        private List<HtmlNode> _nodes;

        public abstract List<HtmlNode> GetNodes();
        public abstract SearcherBase GetTarget();
        public abstract HtmlNode GetValidItemNode();
        public SearcherBase Target { get { return _target ?? (_target = GetTarget()); } }

        public List<HtmlNode> Nodes
        {
            get { return _nodes ?? (_nodes = GetNodes()); }
        }


        public abstract void GetSearchResultDateTimeTest();
        
        public abstract void GetSearchResultKeyTest();

        
        public abstract void GetSearchResultLocationNameTest();
     
        public abstract void GetSearchResultLocationStateProvinceTest();

        public abstract void GetSearchResultCityNameTest();
   
        public abstract void GetSearchResultTitleTest();
 
        public abstract void GetSearchResultUriTest();
    
        public abstract void GetSearchResultTest();
     
        public abstract void GetSearchResultNodesTest();
        public void GetSearchResultDateTime()
        {
            Assert.IsTrue(Nodes.Any(x => Target.GetSearchResultDateTime(x).HasValue));
        }


        public void GetSearchResultKey()
        {
            Assert.IsTrue(Nodes.Any(x => !String.IsNullOrEmpty(Target.GetSearchResultKey(x))));
        }

     
        public void GetSearchResultLocationName()
        {
            Assert.IsTrue(Nodes.Any(x => !String.IsNullOrEmpty(Target.GetSearchResultLocationName(x))));
        }

    
        public void GetSearchResultLocationStateProvince()
        {
            Assert.IsTrue(Nodes.Any(x => !String.IsNullOrEmpty(Target.GetSearchResultCityName(x))));
        }

        
        public void GetSearchResultCityName()
        {
            Assert.IsTrue(Nodes.Any(x => !String.IsNullOrEmpty(Target.GetSearchResultCityName(x))));
        }


        public void GetSearchResultTitle()
        {
            Assert.IsTrue(Nodes.Any(x => !String.IsNullOrEmpty(Target.GetSearchResultTitle(x))));
        }

    
        public void GetSearchResultUri()
        {
            Assert.IsTrue(Nodes.Any(x => Target.GetSearchResultUri(x) != null));
        }


        public void GetSearchResult()
        {

            var actual = Target.GetSearchResult(GetValidItemNode()).Result;
            Assert.IsInstanceOfType(actual, typeof(SearchResult));
        }

       
        public void GetSearchResultNodes()
        {
            var actual = Target.GetSearchResultNodes().Result;
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
