using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Core
{
    public class SearchResult
    {
        public string Key { get; set; }
        public DateTime PostDate { get; set; }
        public string LocationName { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationCityName { get; set; }
        public string Title { get; set; }
        public Uri Uri { get; set; }
        public string SearchResultContent { get; set; }
        public int KeywordScore { get; set; }
    }
}
