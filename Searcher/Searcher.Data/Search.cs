//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Searcher.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Search
    {
        public Search()
        {
            this.SearchTerms = new HashSet<SearchTerm>();
        }
    
        public System.Guid Id { get; set; }
        public string SearchName { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public Nullable<System.DateTime> UpdatedOnUtc { get; set; }
    
        public virtual ICollection<SearchTerm> SearchTerms { get; set; }
    }
}
