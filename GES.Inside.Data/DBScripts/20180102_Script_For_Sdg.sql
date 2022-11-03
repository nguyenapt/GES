/****** Object:  Table [dbo].[Sdg]    Script Date: 2/1/2018 11:21:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sdg](
	[Sdg_Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Sdg_Name] [NVARCHAR](500) NOT NULL,
	[Sdg_Link] [NVARCHAR](500) NULL,
	[DocumentId] [UNIQUEIDENTIFIER] NULL,
	[Created] [DATETIME] NULL,
	[Modified] [DATETIME] NULL,
 CONSTRAINT [PK_Sdg] PRIMARY KEY CLUSTERED 
(
	[Sdg_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Sdg]  WITH CHECK ADD  CONSTRAINT [FK_Sdg_GesDocument] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[GesDocument] ([DocumentId])
GO

ALTER TABLE [dbo].[Sdg] CHECK CONSTRAINT [FK_Sdg_GesDocument]
GO

/****** Object:  Table [dbo].[GesCaseReportSdg]    Script Date: 2/1/2018 11:21:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GesCaseReportSdg](
	[GesCaseReport_Sdg_Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[GesCaseReport_Id] [BIGINT] NOT NULL,
	[Sdg_Id] [BIGINT] NOT NULL,
	[SortOrder] [INT] NULL,
 CONSTRAINT [PK_GesCaseReportSdg] PRIMARY KEY CLUSTERED 
(
	[GesCaseReport_Sdg_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GesCaseReportSdg]  WITH CHECK ADD  CONSTRAINT [FK_GesCaseReportSdg_I_GesCaseReports] FOREIGN KEY([GesCaseReport_Id])
REFERENCES [dbo].[I_GesCaseReports] ([I_GesCaseReports_Id])
GO

ALTER TABLE [dbo].[GesCaseReportSdg] CHECK CONSTRAINT [FK_GesCaseReportSdg_I_GesCaseReports]
GO

ALTER TABLE [dbo].[GesCaseReportSdg]  WITH CHECK ADD  CONSTRAINT [FK_GesCaseReportSdg_Sdg] FOREIGN KEY([Sdg_Id])
REFERENCES [dbo].[Sdg] ([Sdg_Id])
GO

ALTER TABLE [dbo].[GesCaseReportSdg] CHECK CONSTRAINT [FK_GesCaseReportSdg_Sdg]
GO


/****** Add Document Service For SDG ******/
IF NOT EXISTS (SELECT * FROM GesDocumentService WHERE Name = 'Sdg')
INSERT	INTO dbo.GesDocumentService
        ( Name, SortOrder )
VALUES  ( N'Sdg', 4 )