using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class DialogueModel
    {
        public DateTime? ContactDate { get; set; }
        public long? ContactDirectionId { get; set; }
        public long? ContactTypeId { get; set; }
        public string ContactTypeName { get; set; }
        public string JobTitle { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FileName { get; set; }
        public bool ClassA { get; set; }
    }

    public class DialogueAndLogModel
    {
        public IList<GES.Inside.Data.Models.DialogueModel> DialogueModels { get; set; }

        public bool ShowDocument { get; set; }
    }

    public class DialogueEditModel : DialogueModel
    {
        public long I_GesCompanySourceDialogues_Id { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        public long? G_Individuals_Id { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
        public string Text { get; set; }
        public long? G_ManagedDocuments_Id { get; set; }
        public bool ShowInCsc { get; set; }
        public bool ShowInReport { get; set; }
        public bool SendNotifications { get; set; }
        public string ContactFullName { get; set; }
        public string DialogueType { get; set; }
        public string ContactDirectionName { get; set; }        
    }

    public class DialogueCaseModel : DialogueEditModel
    {
        public long? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Issue { get; set; }
        public string Norm { get; set; }
    }
}
