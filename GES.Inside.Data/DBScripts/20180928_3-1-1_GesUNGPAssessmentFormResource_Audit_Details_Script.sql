
CREATE TABLE [dbo].[GesUNGPAssessmentFormResource_Audit_Details](
	[GesUNGPAssessmentFormResource_Audit_Details_Id] [uniqueidentifier] NOT NULL,
	[GesUNGPAssessmentFormResource_Audit_Id] [uniqueidentifier] NOT NULL,
	[ColumnName] [nvarchar](500) NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
	[AuditDataState] [varchar](10) NULL,
	[AuditUser] [nvarchar](128) NULL,
	[AuditDatetime] [datetime] NULL,
	[ColumnNameDescription] [nvarchar](500) NULL,
 CONSTRAINT [GesUNGPAssessmentFormResource_Audit_Details_pk] PRIMARY KEY CLUSTERED 
(
	[GesUNGPAssessmentFormResource_Audit_Details_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[GesUNGPAssessmentFormResource_Audit_Details]  WITH CHECK ADD  CONSTRAINT [GesUNGPAssessmentFormResource_Audit_Details_GesUNGPAssessmentFormResource_Audit] FOREIGN KEY([GesUNGPAssessmentFormResource_Audit_Id])
REFERENCES [dbo].[GesUNGPAssessmentFormResource_Audit] ([GesUNGPAssessmentFormResource_Audit_Id])
GO

ALTER TABLE [dbo].[GesUNGPAssessmentFormResource_Audit_Details] CHECK CONSTRAINT [GesUNGPAssessmentFormResource_Audit_Details_GesUNGPAssessmentFormResource_Audit]
GO

