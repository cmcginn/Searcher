using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Core
{
    public interface ISearchTerm
    {
        Guid Id { get; set; }
        string Term { get; set; }
        int Score { get; set; }
        DateTime CreatedOnUtc { get; set; }
        DateTime? UpdatedOnUtc { get; set; }
    }
}
