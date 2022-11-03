CREATE TABLE [dbo].[GesUNGPAssessmentFormResource_Audit](
	[GesUNGPAssessmentFormResource_Audit_Id] [uniqueidentifier] NOT NULL,
	[GesUNGPAssessmentFormId] [uniqueidentifier] NOT NULL,
	[GesUNGPAssessmentFormResourcesId] [uniqueidentifier] NOT NULL,
	[AuditDMLAction] [varchar](10) NULL,
	[AuditUser] [nvarchar](128) NULL,
	[AuditDatetime] [datetime] NULL,
 CONSTRAINT [GesUNGPAssessmentFormResource_Audit_pk] PRIMARY KEY CLUSTERED 
(
	[GesUNGPAssessmentFormResource_Audit_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

