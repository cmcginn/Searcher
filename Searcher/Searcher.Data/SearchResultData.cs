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
    
    public partial class SearchResultData
    {
        public System.Guid Id { get; set; }
        public string PostId { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public string PostType { get; set; }
        public string PostTitle { get; set; }
        public string PostUrl { get; set; }
        public System.DateTime PostDateTime { get; set; }
        public Nullable<System.DateTime> AppliedDateTime { get; set; }
        public int KeywordScore { get; set; }
        public bool Hidden { get; set; }
    }
}
