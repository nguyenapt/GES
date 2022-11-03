IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'I_GSSLink')
	SET NOEXEC ON

CREATE TABLE [dbo].[I_GSSLink](
	[I_GSSLink_Id] [uniqueidentifier] NOT NULL,
	[I_GesCaseReports_Id] [dbo].[IId] NULL,
	[Description] [dbo].[NDescription] NULL,
	[GSSLinkModified] [datetime] NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [PK_I_GSSLink] PRIMARY KEY CLUSTERED 
(
	[I_GSSLink_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[I_GSSLink] ADD  CONSTRAINT [DF_I_GSSLink_Created]  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[I_GSSLink]  WITH NOCHECK ADD  CONSTRAINT [FK_I_GSSLink_I_GesCaseReports] FOREIGN KEY([I_GesCaseReports_Id])
REFERENCES [dbo].[I_GesCaseReports] ([I_GesCaseReports_Id])
ON DELETE CASCADE

GO

ALTER TABLE [dbo].[I_GSSLink] NOCHECK CONSTRAINT [FK_I_GSSLink_I_GesCaseReports]

GO

INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Gss Link','GSS-LINK',35,3)

INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Gss Link','GSS-LINK',60,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'GSS-LINK'),3);