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
    
    public partial class G_LocalizedTextTranslations
    {
        public long G_LocalizedTextTranslations_Id { get; set; }
        public long G_LocalizedTexts_Id { get; set; }
        public long G_Cultures_Id { get; set; }
        public string Text { get; set; }
        public bool New { get; set; }
    
        public virtual G_Cultures G_Cultures { get; set; }
        public virtual G_LocalizedTexts G_LocalizedTexts { get; set; }
    }
}
