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
    
    public partial class G_Individuals
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public G_Individuals()
        {
            this.E_MessageContentG_Individuals = new HashSet<E_MessageContentG_Individuals>();
            this.G_Bills = new HashSet<G_Bills>();
            this.G_ClientDialogue = new HashSet<G_ClientDialogue>();
            this.R_EmailSource = new HashSet<R_EmailSource>();
            this.R_ListsG_Individuals = new HashSet<R_ListsG_Individuals>();
            this.G_IndividualsG_Services = new HashSet<G_IndividualsG_Services>();
            this.G_Users1 = new HashSet<G_Users>();
            this.I_GesCaseReportsG_Individuals = new HashSet<I_GesCaseReportsG_Individuals>();
            this.I_GesCompanyDialogues = new HashSet<I_GesCompanyDialogues>();
            this.I_GesCompanyDialoguesG_Individuals = new HashSet<I_GesCompanyDialoguesG_Individuals>();
            this.I_GesCompanyWatcher = new HashSet<I_GesCompanyWatcher>();
            this.I_GesSourceDialogues = new HashSet<I_GesSourceDialogues>();
            this.I_GesSourceDialoguesG_Individuals = new HashSet<I_GesSourceDialoguesG_Individuals>();
            this.I_IndividualListsG_Individuals = new HashSet<I_IndividualListsG_Individuals>();
            this.T_Activites = new HashSet<T_Activites>();
            this.GesCaseReportSignUp = new HashSet<GesCaseReportSignUp>();
        }
    
        public long G_Individuals_Id { get; set; }
        public Nullable<long> G_Organizations_Id { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<long> ModifiedByG_Users_Id { get; set; }
        public Nullable<long> W_HitTypes_Id { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public Nullable<bool> HtmlEmail { get; set; }
        public string Phone { get; set; }
        public string WorkPhone { get; set; }
        public string Fax { get; set; }
        public string MobilePhone { get; set; }
        public Nullable<long> G_Genders_Id { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingAddress3 { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingCity { get; set; }
        public Nullable<long> BillingG_Countries_Id { get; set; }
        public decimal BillingManualAdjustment { get; set; }
        public string BillingManualAdjustmentMotivation { get; set; }
        public Nullable<long> G_PaymentMethods_Id { get; set; }
        public Nullable<long> F_CreditReportStates_Id { get; set; }
        public long G_TimeZones_Id { get; set; }
        public Nullable<long> CultureG_Languages_Id { get; set; }
        public Nullable<long> CultureG_Countries_Id { get; set; }
        public string Comment { get; set; }
        public Nullable<long> G_Departments_Id { get; set; }
        public Nullable<long> G_IndividualsGroups_Id { get; set; }
        public Nullable<long> Picture_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<E_MessageContentG_Individuals> E_MessageContentG_Individuals { get; set; }
        public virtual F_CreditReportStates F_CreditReportStates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_Bills> G_Bills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_ClientDialogue> G_ClientDialogue { get; set; }
        public virtual G_Genders G_Genders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_EmailSource> R_EmailSource { get; set; }
        public virtual G_Languages G_Languages { get; set; }
        public virtual G_PaymentMethods G_PaymentMethods { get; set; }
        public virtual G_TimeZones G_TimeZones { get; set; }
        public virtual G_Users G_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_ListsG_Individuals> R_ListsG_Individuals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_IndividualsG_Services> G_IndividualsG_Services { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_Users> G_Users1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesCaseReportsG_Individuals> I_GesCaseReportsG_Individuals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesCompanyDialogues> I_GesCompanyDialogues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesCompanyDialoguesG_Individuals> I_GesCompanyDialoguesG_Individuals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesCompanyWatcher> I_GesCompanyWatcher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesSourceDialogues> I_GesSourceDialogues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesSourceDialoguesG_Individuals> I_GesSourceDialoguesG_Individuals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_IndividualListsG_Individuals> I_IndividualListsG_Individuals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_Activites> T_Activites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GesCaseReportSignUp> GesCaseReportSignUp { get; set; }
        public virtual G_Organizations G_Organizations { get; set; }
    }
}