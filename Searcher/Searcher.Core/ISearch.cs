using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Core
{
    public interface ISearch
    {
        Guid Id { get; set; }
        string SearchName { get; set; }

        DateTime CreatedOnUtc { get; set; }
        DateTime? UpdatedOnUtc { get; set; }

        List<ISearchTerm> SearchTerms { get; set; }
    }
}
