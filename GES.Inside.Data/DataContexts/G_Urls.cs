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
    
    public partial class G_Urls
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public G_Urls()
        {
            this.G_ManagedDocumentItems = new HashSet<G_ManagedDocumentItems>();
            this.G_Urls1 = new HashSet<G_Urls>();
        }
    
        public long G_Urls_Id { get; set; }
        public Nullable<long> ParentG_Urls_Id { get; set; }
        public Nullable<long> G_ManagedDocuments_Id { get; set; }
        public string Url { get; set; }
        public System.DateTime Created { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_ManagedDocumentItems> G_ManagedDocumentItems { get; set; }
        public virtual G_ManagedDocuments G_ManagedDocuments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_Urls> G_Urls1 { get; set; }
        public virtual G_Urls G_Urls2 { get; set; }
    }
}
