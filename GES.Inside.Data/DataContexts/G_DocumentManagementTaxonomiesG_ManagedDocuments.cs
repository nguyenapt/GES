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
    
    public partial class G_DocumentManagementTaxonomiesG_ManagedDocuments
    {
        public long G_DocumentManagementTaxonomiesG_ManagedDocuments_Id { get; set; }
        public long G_DocumentManagementTaxonomies_Id { get; set; }
        public long G_ManagedDocuments_Id { get; set; }
    
        public virtual G_DocumentManagementTaxonomies G_DocumentManagementTaxonomies { get; set; }
        public virtual G_ManagedDocuments G_ManagedDocuments { get; set; }
    }
}
