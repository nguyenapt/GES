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
    
    public partial class G_LocalizedLongTexts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public G_LocalizedLongTexts()
        {
            this.G_LocalizedLongTextTranslations = new HashSet<G_LocalizedLongTextTranslations>();
        }
    
        public long G_LocalizedLongTexts_Id { get; set; }
        public bool New { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_LocalizedLongTextTranslations> G_LocalizedLongTextTranslations { get; set; }
    }
}
