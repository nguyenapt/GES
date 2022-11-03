
IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_PortfolioCompaniesImport' 
		  AND COLUMN_NAME = 'SustainalyticsID') 
ALTER TABLE [dbo].[I_PortfolioCompaniesImport] ADD [SustainalyticsID] [dbo].[IId] NULL


INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Internal Comments','INTERNAL-COMMENTS',36,1)

INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Internal Comments','INTERNAL-COMMENTS',61,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'INTERNAL-COMMENTS'),1);


DROP INDEX ix_I_GesCommentary_I_GesCaseReports_Id_includes ON [I_GesCommentary] ;
ALTER TABLE [I_GesCommentary] ALTER COLUMN [Description] nvarchar(max)
CREATE INDEX ix_I_GesCommentary_I_GesCaseReports_Id_includes ON [I_GesCommentary] (I_GesCommentary_Id, I_GesCaseReports_Id)


ALTER TABLE [I_Milestones] ALTER COLUMN [Description] nvarchar(max)
ALTER TABLE [I_Milestones_Audit] ALTER COLUMN [Description] nvarchar(max)
ALTER TABLE [I_GSSLink] ALTER COLUMN [Description] nvarchar(max)