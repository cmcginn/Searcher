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
    
    public partial class SearchTerm
    {
        public System.Guid Id { get; set; }
        public System.Guid SearchId { get; set; }
        public string Term { get; set; }
        public int Score { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public Nullable<System.DateTime> UpdatedOnUtc { get; set; }
    
        public virtual Search Search { get; set; }
    }
}
