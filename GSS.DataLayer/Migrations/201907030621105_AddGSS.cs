namespace GSS.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddGSS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        CompanyVersion = c.Int(nullable: false),
                        PrincipleTemplateId = c.Int(),
                        IssueIndicatorTemplateId = c.Int(),
                        Type = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        PreviousValue = c.String(),
                        Author = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        InternalId = c.Guid(nullable: false),
                        ResearchEntityId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Isin = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false),
                        Country = c.String(nullable: false, maxLength: 50),
                        SubPeerGroup = c.String(nullable: false, maxLength: 100),
                        Website = c.String(nullable: false, maxLength: 255),
                        IsParkedForResearchSince = c.DateTime(),
                        UnGlobalCompactSignatorySince = c.DateTime(),
                        IsResearchEntityCandidate = c.Boolean(nullable: false),
                        Analyst = c.String(nullable: false, maxLength: 100),
                        Reviewer = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.ResearchEntityId)
                .Index(t => t.ResearchEntityId);
            
            CreateTable(
                "dbo.CompanyProfiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 100),
                        Assessment = c.Int(nullable: false),
                        HasSignificantAssessmentChange = c.Boolean(nullable: false),
                        AssessmentEffectiveSinceQuarterNumerator = c.Int(nullable: false),
                        AssessmentEffectiveSinceQuarterYear = c.Int(nullable: false),
                        PreviouslyNonCompliant = c.String(nullable: false, maxLength: 300),
                        OverallConclusion = c.String(nullable: false, maxLength: 500),
                        OutlookAssessment = c.Int(),
                        OutlookAssessmentEffectiveSinceQuarterNumerator = c.Int(),
                        OutlookAssessmentEffectiveSinceQuarterYear = c.Int(),
                        OutlookAssessmentComments = c.String(nullable: false, maxLength: 300),
                        Engagement = c.Int(),
                        EngagementAsOf = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Principles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TemplateId = c.Int(nullable: false),
                        CompanyProfileId = c.Guid(nullable: false),
                        Assessment = c.Int(nullable: false),
                        AssessmentEffectiveSinceQuarterNumerator = c.Int(nullable: false),
                        AssessmentEffectiveSinceQuarterYear = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyProfileId, cascadeDelete: true)
                .ForeignKey("dbo.PrincipleTemplates", t => t.TemplateId)
                .Index(t => t.TemplateId)
                .Index(t => t.CompanyProfileId);
            
            CreateTable(
                "dbo.PrincipleTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 3),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 300),
                        NormCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Headline = c.String(nullable: false),
                        Publisher = c.String(nullable: false, maxLength: 450),
                        PublicationDate = c.DateTime(nullable: false),
                        CompanyProfileId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyProfiles", t => t.CompanyProfileId, cascadeDelete: true)
                .Index(t => t.CompanyProfileId);
            
            CreateTable(
                "dbo.IssueIndicators",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TemplateId = c.Int(nullable: false),
                        PrincipleId = c.Guid(nullable: false),
                        Assessment = c.Int(nullable: false),
                        AssessmentEffectiveSinceQuarterNumerator = c.Int(nullable: false),
                        AssessmentEffectiveSinceQuarterYear = c.Int(nullable: false),
                        BasisForNonComplianceValue = c.Int(),
                        BasisForNonComplianceText = c.String(nullable: false, maxLength: 300),
                        OecdGuidelinesFlags = c.Int(nullable: false),
                        UnGuidingPrinciplesFlags = c.Int(nullable: false),
                        RelatedConventionsFlags = c.Binary(nullable: false, maxLength: 10),
                        Headline = c.String(nullable: false, maxLength: 250),
                        CaseSummary = c.String(nullable: false, maxLength: 1500),
                        Impact = c.String(nullable: false, maxLength: 300),
                        Management = c.String(nullable: false, maxLength: 300),
                        Conclusion = c.String(nullable: false, maxLength: 300),
                        IncidentConditionStatus = c.Boolean(),
                        IncidentConditionComment = c.String(nullable: false, maxLength: 800),
                        CompanyResponseToIncidentConditionStatus = c.Boolean(),
                        CompanyResponseToIncidentConditionComment = c.String(nullable: false, maxLength: 800),
                        CompanyLastContacted = c.DateTime(),
                        CompanyLastResponse = c.DateTime(),
                        CompanyContactComments = c.String(nullable: false, maxLength: 300),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Principles", t => t.PrincipleId, cascadeDelete: true)
                .ForeignKey("dbo.IssueIndicatorTemplates", t => t.TemplateId)
                .Index(t => t.TemplateId)
                .Index(t => t.PrincipleId);
            
            CreateTable(
                "dbo.ConclusionComments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false, maxLength: 1200),
                        SortKey = c.Int(nullable: false),
                        IssueIndicatorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IssueIndicators", t => t.IssueIndicatorId, cascadeDelete: true)
                .Index(t => t.IssueIndicatorId);
            
            CreateTable(
                "dbo.ImpactComments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false, maxLength: 1200),
                        SortKey = c.Int(nullable: false),
                        IssueIndicatorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IssueIndicators", t => t.IssueIndicatorId, cascadeDelete: true)
                .Index(t => t.IssueIndicatorId);
            
            CreateTable(
                "dbo.ManagementComments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false, maxLength: 1200),
                        SortKey = c.Int(nullable: false),
                        IssueIndicatorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IssueIndicators", t => t.IssueIndicatorId, cascadeDelete: true)
                .Index(t => t.IssueIndicatorId);
            
            CreateTable(
                "dbo.IssueIndicatorTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 250),
                        PrincipleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PrincipleTemplates", t => t.PrincipleId)
                .Index(t => t.PrincipleId);
            
            CreateTable(
                "dbo.OecdGuidelines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chapter = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RelatedConventions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UnGuidingPrinciples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IssueIndicators", "TemplateId", "dbo.IssueIndicatorTemplates");
            DropForeignKey("dbo.IssueIndicatorTemplates", "PrincipleId", "dbo.PrincipleTemplates");
            DropForeignKey("dbo.IssueIndicators", "PrincipleId", "dbo.Principles");
            DropForeignKey("dbo.ManagementComments", "IssueIndicatorId", "dbo.IssueIndicators");
            DropForeignKey("dbo.ImpactComments", "IssueIndicatorId", "dbo.IssueIndicators");
            DropForeignKey("dbo.ConclusionComments", "IssueIndicatorId", "dbo.IssueIndicators");
            DropForeignKey("dbo.Sources", "CompanyProfileId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.Principles", "TemplateId", "dbo.PrincipleTemplates");
            DropForeignKey("dbo.Principles", "CompanyProfileId", "dbo.CompanyProfiles");
            DropForeignKey("dbo.CompanyProfiles", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Companies", "ResearchEntityId", "dbo.Companies");
            DropIndex("dbo.IssueIndicatorTemplates", new[] { "PrincipleId" });
            DropIndex("dbo.ManagementComments", new[] { "IssueIndicatorId" });
            DropIndex("dbo.ImpactComments", new[] { "IssueIndicatorId" });
            DropIndex("dbo.ConclusionComments", new[] { "IssueIndicatorId" });
            DropIndex("dbo.IssueIndicators", new[] { "PrincipleId" });
            DropIndex("dbo.IssueIndicators", new[] { "TemplateId" });
            DropIndex("dbo.Sources", new[] { "CompanyProfileId" });
            DropIndex("dbo.Principles", new[] { "CompanyProfileId" });
            DropIndex("dbo.Principles", new[] { "TemplateId" });
            DropIndex("dbo.CompanyProfiles", new[] { "CompanyId" });
            DropIndex("dbo.Companies", new[] { "ResearchEntityId" });
            DropTable("dbo.UnGuidingPrinciples");
            DropTable("dbo.RelatedConventions");
            DropTable("dbo.OecdGuidelines");
            DropTable("dbo.IssueIndicatorTemplates");
            DropTable("dbo.ManagementComments");
            DropTable("dbo.ImpactComments");
            DropTable("dbo.ConclusionComments");
            DropTable("dbo.IssueIndicators");
            DropTable("dbo.Sources");
            DropTable("dbo.PrincipleTemplates");
            DropTable("dbo.Principles");
            DropTable("dbo.CompanyProfiles");
            DropTable("dbo.Companies");
            DropTable("dbo.ChangeRecords");
        }
    }
}
