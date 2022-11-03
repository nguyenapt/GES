using System;
using System.Collections.Generic;
using com.sun.tools.javac.util;

namespace GES.Inside.Data.Models
{
    public class OrganizationViewModel
    {
        public long G_Organizations_Id { get; set; }
        public long? OwnerG_Organizations_Id { get; set; }
        public long? G_Industries_Id { get; set; }
        public string Created { get; set; }
        public DateTime? Modified { get; set; }
        public long? ModifiedByG_Users_Id { get; set; }
        public string LicenseKey { get; set; }
        public string Name { get; set; }
        public string OrgNr { get; set; }
        public string Domain { get; set; }
        public string WebsiteUrl { get; set; }
        public bool? WebsiteExists { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public Guid? CountryId { get; set; }
        public long? G_Languages_Id { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingAddress3 { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingCity { get; set; }
        public long? BillingG_Countries_Id { get; set; }
        public long? G_PaymentMethods_Id { get; set; }
        public string PostgiroNumber { get; set; }
        public string BankgiroNumber { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string LogoUrl { get; set; }
        public string Comment { get; set; }
        public string OurApproach { get; set; }
        public long? G_SaleStates_Id { get; set; }
        public int CreditPaymentTerms { get; set; }
        public long? G_SalesRatings_Id { get; set; }
        public int? Employees { get; set; }
        public string Members { get; set; }
        public decimal? TotalAssets { get; set; }
        public string Equity { get; set; }
        public string Property { get; set; }
        public string Fi { get; set; }
        public string SalesInterest { get; set; }
        public DateTime? SalesFirstContact { get; set; }
        public DateTime? SalesLastContact { get; set; }
        public DateTime? SalesNextContact { get; set; }
        public string SalesMeetingReport { get; set; }
        public string SalesComment { get; set; }
        public string ReferredBy { get; set; }
        public string DeliveryDate { get; set; }
        public bool Customer { get; set; }
        public long? G_OrganizationsI_ClientStatuses_Id { get; set; }
        public long? I_ClientProgressStatuses_Id { get; set; }
        public int? FreeHours { get; set; }
        public decimal? StandardPrice { get; set; }
        public long? I_Companies_Id { get; set; }
        public string ModifyString { get; set; }
        
        public IList<OrganizationsServicesViewModel> OrganizationsServicesViewModels { get; set; } 
    }
}