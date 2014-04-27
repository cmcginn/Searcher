using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Core
{
    public class SearcherConfiguration
    {
        public string SearchResultKeyAttributeName { get; set; }
        public string SearchResultsStateProvinceGroupRegex { get; set; }

        public string SearchResultsCityGroupRegex { get; set; }

        public string SearchResultTitleLinkSelector { get; set; }

        public string SearchResultSourceUriLinkSelector { get; set; }

        public string SearchResultsParentNodeXPathSelector { get; set; }
    }
}
