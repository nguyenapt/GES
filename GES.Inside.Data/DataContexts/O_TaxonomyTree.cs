//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GES.Inside.Data.DataContexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class O_TaxonomyTree
    {
        public long O_TaxonomyTree_Id { get; set; }
        public Nullable<long> ParentO_Taxonomy_Id { get; set; }
        public long O_Taxonomy_Id { get; set; }
        public Nullable<byte> Depth { get; set; }
        public string Lineage { get; set; }
    }
}