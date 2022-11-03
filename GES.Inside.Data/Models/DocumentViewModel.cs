using System;

namespace GES.Inside.Data.Models
{
    public class DocumentViewModel
    {
        public long G_ManagedDocuments_Id { get; set; }
        public string Name { get; set; }
        public long? G_ManagedDocumentApprovalStatuses_Id { get; set; }
        public long? G_ManagedDocumentRiskLevels_Id { get; set; }
        public long? G_DocumentManagementTaxonomies_Id { get; set; }
        public long? G_ManagedDocumentServices_Id { get; set; }
        public long? I_Msci_Id { get; set; }
        public long? I_Ftse_Id { get; set; }
        public DateTime? Created { get; set; }
        public string DateText { get; set; }
        public string Source { get; set; }
        public string Keywords { get; set; }
        public string Comment { get; set; }
        public long? NecI_Companies_Id { get; set; }
        public DateTime? Modified { get; set; }
        public long? ModifiedByG_Users_Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string DownloadUrl { get; set; }
        public long G_Uploads_Id { get; set; }
        public long? I_Companies_Id { get; set; }
        public string CompanyName { get; set; }        
        public long? I_GesCaseReports_Id { get; set; }
        public string ReportIncident { get; set; }
        public long I_GesCompanyDialogues_Id { get; set; }
        public string ServiceName { get; set; }
    }
}
