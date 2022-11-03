IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseProfileEntitiesGroup' 
		  AND COLUMN_NAME = 'VisibleType') 
ALTER TABLE [dbo].[I_GesCaseProfileEntitiesGroup] ADD [VisibleType] int DEFAULT NULL

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseProfileEntities' 
		  AND COLUMN_NAME = 'VisibleType') 
ALTER TABLE [dbo].[I_GesCaseProfileEntities] ADD [VisibleType] int DEFAULT NULL

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesCaseProfileInvisibleEntities' 
		  AND COLUMN_NAME = 'InVisibleType') 
ALTER TABLE [dbo].[GesCaseProfileInvisibleEntities] ADD [InVisibleType] int DEFAULT NULL

GO

UPDATE I_GesCaseProfileEntitiesGroup SET VisibleType = 3

UPDATE I_GesCaseProfileEntities SET VisibleType = 3

UPDATE I_GesCaseProfileEntitiesGroup SET VisibleType = 1 WHERE GroupType = 'GUIDELINES-LIST'
UPDATE I_GesCaseProfileEntitiesGroup SET VisibleType = 1 WHERE GroupType = 'DESCRIPTION-NEW'
UPDATE I_GesCaseProfileEntitiesGroup SET VisibleType = 1 WHERE GroupType = 'ASSOCIATED-CORPORATIONS'
UPDATE I_GesCaseProfileEntitiesGroup SET VisibleType = 1 WHERE GroupType = 'SOURCES'
UPDATE I_GesCaseProfileEntitiesGroup SET VisibleType = 1 WHERE GroupType = 'SUPPLEMENTARY-READING'
UPDATE I_GesCaseProfileEntitiesGroup SET VisibleType = 1 WHERE GroupType = '360-INCIDENT-ANALYSIS'


UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = 'GUIDELINES-LIST'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = 'DESCRIPTION-NEW'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = 'ASSOCIATED-CORPORATIONS'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = 'SOURCES'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = 'SUPPLEMENTARY-READING'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = '360-INCIDENT-ANALYSIS-SUMMARY'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = '360-INCIDENT-ANALYSIS-DIALOGUE-AND-ANALYSIS'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = '360-INCIDENT-ANALYSIS-CONCLUSION'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = '360-INCIDENT-ANALYSIS-GUIDELINES-AND-CONVENTIONS'
UPDATE I_GesCaseProfileEntities SET VisibleType = 1 WHERE [Type] = '360-INCIDENT-ANALYSIS-SOURCES'


--
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Statistics','CLIENT-STATISTICS',27,2)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Endorsement','CLIENT-ENDORSEMENT',28,2)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Calendar','CLIENT-EVENT',29,2)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Document','CLIENT-DOCUMENT',30,2)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Company Related items','CLIENT-COMPANY-RELATED-ITEMS',31,2)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Recomendation for change','CLIENT-RECOMENDATION-FOR-CHANGE',32,2)
--



INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Statistics','CLIENT-STATISTICS',47,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-STATISTICS'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Endorsement','CLIENT-ENDORSEMENT',48,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-ENDORSEMENT'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Calendar','CLIENT-EVENT',49,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-EVENT'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Document','CLIENT-DOCUMENT',50,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-DOCUMENT'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Case','CLIENT-COMPANY-RELATED-ITEMS-CASE',51,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-COMPANY-RELATED-ITEMS'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Alert','CLIENT-COMPANY-RELATED-ITEMS-ALERT',52,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-COMPANY-RELATED-ITEMS'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Recomendation for change','CLIENT-RECOMENDATION-FOR-CHANGE',52,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-RECOMENDATION-FOR-CHANGE'),2);

UPDATE I_GesCaseProfileEntities SET VisibleType = 1 where [Type] = 'UNGP-ASSESSMENT-FORM'

INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Alert','CLIENT-ALERT',33,2)

INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Alert','CLIENT-ALERT-TEXT',54,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-ALERT'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Source','CLIENT-ALERT-SOURCE',55,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CLIENT-ALERT'),2);


INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Investor initiatives','OTHER-STAKEHOLDERS-INVESTOR-INITIATIVES',56,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'OTHER-STAKEHOLDERS'),2);

INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'UNGP Performance','UNGP-PERFORMANCE',46,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'UNGP-ASSESSMENT-FORM'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Details','UNGP-DETAILS',46,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'UNGP-ASSESSMENT-FORM'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Methodology','UNGP-METHODOLOGY',46,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'UNGP-ASSESSMENT-FORM'),2);

INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Milestone','CASE-INFORMATION-MILESTONE',7,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'),2);

INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Company','COMPANY-INFORMATION-COMPANY',50,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-INFO'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Isin','COMPANY-INFORMATION-ISIN',51,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-INFO'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Industry','COMPANY-INFORMATION-INDUSTRY',52,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-INFO'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Home Country','COMPANY-INFORMATION-HOME-COUNTRY',53,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-INFO'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'GRI ','COMPANY-INFORMATION-GRI',54,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-INFO'),2);
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'UN Global Compact','COMPANY-INFORMATION-UN-GLOBAL-COMPACT',55,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-INFO'),2);

INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order],[VisibleType]) VALUES (NEWID(),'Disclaimer','DISCLAIMER',34,2)
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id],[VisibleType]) VALUES (NEWID(),'Disclaimer','DISCLAIMER',56,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'DISCLAIMER'),2);