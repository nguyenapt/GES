DROP TABLE [GesUNGPAssessmentForm_Audit_Details];
DROP TABLE GesUNGPAssessmentForm_Audit;
DROP TYPE GesUNGPAssessmentForm_Audit_Columns_list;

CREATE TABLE [dbo].[GesUNGPAssessmentForm_Audit](
	[GesUNGPAssessmentForm_Audit_Id] [uniqueidentifier] NOT NULL,
	[GesUNGPAssessmentFormId] [uniqueidentifier] NOT NULL,
	[I_GesCaseReports_Id] BIGINT NOT NULL,
	[AuditDMLAction] [varchar](10)  NULL,
	[AuditUser] [nvarchar](128) NULL,
	[AuditDatetime] [datetime]  NULL,
CONSTRAINT [GesUNGPAssessmentForm_Audit_pk] PRIMARY KEY CLUSTERED 
(
	[GesUNGPAssessmentForm_Audit_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[GesUNGPAssessmentForm_Audit_Details](
	[GesUNGPAssessmentForm_Audit_Details_Id] [uniqueidentifier] NOT NULL,
	[GesUNGPAssessmentForm_Audit_Id] [uniqueidentifier] NOT NULL,
	[ColumnName] [nvarchar](500) NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
	[AuditDataState] [varchar](10)  NULL,
	[AuditUser] [nvarchar](128) NULL,
	[AuditDatetime] [datetime]  NULL,

CONSTRAINT [GesUNGPAssessmentForm_Audit_Details_pk] PRIMARY KEY CLUSTERED 
(
	[GesUNGPAssessmentForm_Audit_Details_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[GesUNGPAssessmentForm_Audit_Details]  WITH CHECK ADD  CONSTRAINT [GesUNGPAssessmentForm_Audit_Details_GesUNGPAssessmentForm_Audit] FOREIGN KEY([GesUNGPAssessmentForm_Audit_Id])
REFERENCES [dbo].[GesUNGPAssessmentForm_Audit] ([GesUNGPAssessmentForm_Audit_Id])
GO

ALTER TABLE [dbo].[GesUNGPAssessmentForm_Audit_Details] CHECK CONSTRAINT [GesUNGPAssessmentForm_Audit_Details_GesUNGPAssessmentForm_Audit]
GO

CREATE TYPE [dbo].[GesUNGPAssessmentForm_Audit_Columns_list] AS TABLE(
	[ColumnName] [nvarchar](150) NOT NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
	[AuditDataState] [varchar](10) NOT NULL
)
GO

CREATE TABLE [dbo].[GesUNGPAssessmentFormResource_Audit](
	[GesUNGPAssessmentFormResource_Audit_Id] [uniqueidentifier] NOT NULL,
	[GesUNGPAssessmentFormId] [uniqueidentifier] NOT NULL,
	[GesUNGPAssessmentFormResourcesId] [uniqueidentifier] NOT NULL,
	[AuditDMLAction] [varchar](10)  NULL,
	[AuditUser] [nvarchar](128) NULL,
	[AuditDatetime] [datetime]  NULL,
	CONSTRAINT [GesUNGPAssessmentFormResource_Audit_pk] PRIMARY KEY CLUSTERED
		(
			[GesUNGPAssessmentFormResource_Audit_Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[GesUNGPAssessmentFormResource_Audit_Details](
	[GesUNGPAssessmentFormResource_Audit_Details_Id] [uniqueidentifier] NOT NULL,
	[GesUNGPAssessmentFormResource_Audit_Id] [uniqueidentifier] NOT NULL,
	[ColumnName] [nvarchar](500) NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
	[AuditDataState] [varchar](10)  NULL,
	[AuditUser] [nvarchar](128) NULL,
	[AuditDatetime] [datetime]  NULL,

	CONSTRAINT [GesUNGPAssessmentFormResource_Audit_Details_pk] PRIMARY KEY CLUSTERED
		(
			[GesUNGPAssessmentFormResource_Audit_Details_Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[GesUNGPAssessmentFormResource_Audit_Details]  WITH CHECK ADD  CONSTRAINT [GesUNGPAssessmentFormResource_Audit_Details_GesUNGPAssessmentFormResource_Audit] FOREIGN KEY([GesUNGPAssessmentFormResource_Audit_Id])
REFERENCES [dbo].[GesUNGPAssessmentFormResource_Audit] ([GesUNGPAssessmentFormResource_Audit_Id])
GO

ALTER TABLE [dbo].[GesUNGPAssessmentFormResource_Audit_Details] CHECK CONSTRAINT [GesUNGPAssessmentFormResource_Audit_Details_GesUNGPAssessmentFormResource_Audit]
GO


