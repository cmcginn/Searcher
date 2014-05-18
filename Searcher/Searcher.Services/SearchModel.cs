using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.Core;

namespace Searcher.Services
{
    public class SearchModel:ISearch
    {
        public Guid Id { get; set; }

        public string SearchName { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        public List<ISearchTerm> SearchTerms { get; set; }
    }
}
