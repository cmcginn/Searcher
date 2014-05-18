using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Searcher.Core;

namespace Searcher.Services
{
    public class SearchTermModel:ISearchTerm
    {
        public Guid Id { get; set; }

        public string Term { get; set; }

        public int Score { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }
    }
}
