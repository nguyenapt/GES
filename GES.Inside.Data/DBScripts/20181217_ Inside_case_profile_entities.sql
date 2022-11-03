drop table [GesCaseProfileInvisibleEntities]
drop table [I_GesCaseProfileEntities];
drop table [I_GesCaseProfileEntitiesGroup]
drop table [GesCaseProfileTemplates];

CREATE TABLE [I_GesCaseProfileEntitiesGroup]
(
 [I_GesCaseProfileEntitiesGroup_Id] uniqueidentifier NOT NULL ,
 [Name]                    nvarchar(500) NOT NULL,
 [GroupType]               nvarchar(500) NOT NULL,
 [Order]				   int
 CONSTRAINT [PK_I_GesCaseProfileEntitiesGroup] PRIMARY KEY CLUSTERED ([I_GesCaseProfileEntitiesGroup_Id] ASC)
);

-- ************************************** [I_GesCaseProfileEntities]

CREATE TABLE [I_GesCaseProfileEntities]
(
 [GesCaseProfileEntity_Id] uniqueidentifier NOT NULL ,
 [Name]                    nvarchar(250) NOT NULL ,
 [Type]                    nvarchar(150) NULL ,
 [Description]             nvarchar(500) NULL ,
 [Order]				   int,
 [I_GesCaseProfileEntitiesGroup_Id]       uniqueidentifier NOT NULL
 CONSTRAINT [I_GesCaseProfileEntitiesGroup_Id] PRIMARY KEY CLUSTERED ([GesCaseProfileEntity_Id] ASC),
 CONSTRAINT [FK_34] FOREIGN KEY ([I_GesCaseProfileEntitiesGroup_Id])  REFERENCES [I_GesCaseProfileEntitiesGroup]([I_GesCaseProfileEntitiesGroup_Id])
);

-- ************************************** [GesCaseProfileTemplates]

CREATE TABLE [GesCaseProfileTemplates]
(
 [GesCaseProfileTemplates_Id] uniqueidentifier NOT NULL ,
 [Name]                       nvarchar(250) NOT NULL ,
 [Description]                nvarchar(500) NULL ,
 [I_EngagementTypes_Id]       bigint NOT NULL ,
 [I_GesCaseReportStatuses_Id] bigint NOT NULL ,
 [Created]                    datetime NULL ,
 [ModifiedByG_Users_Id]       int NULL ,

 CONSTRAINT [PK_GesCaseProfileTemplates] PRIMARY KEY CLUSTERED ([GesCaseProfileTemplates_Id] ASC),
 CONSTRAINT [FK_17] FOREIGN KEY ([I_EngagementTypes_Id])  REFERENCES [I_EngagementTypes]([I_EngagementTypes_Id]),
 CONSTRAINT [FK_20] FOREIGN KEY ([I_GesCaseReportStatuses_Id])  REFERENCES [I_GesCaseReportStatuses]([I_GesCaseReportStatuses_Id])
);


CREATE NONCLUSTERED INDEX [fkIdx_17] ON [GesCaseProfileTemplates] 
 (
  [I_EngagementTypes_Id] ASC
 )
 

CREATE NONCLUSTERED INDEX [fkIdx_20] ON [GesCaseProfileTemplates] 
 (
  [I_GesCaseReportStatuses_Id] ASC
 )


-- ************************************** [GesCaseProfileInvisibleEntities]

CREATE TABLE [GesCaseProfileInvisibleEntities]
(
 [GesCaseProfileInvisibleEntity_Id] uniqueidentifier NOT NULL ,
 [GesCaseProfileTemplates_Id]       uniqueidentifier NOT NULL ,
 [GesCaseProfileEntity_Id]        uniqueidentifier NOT NULL ,

 CONSTRAINT [PK_I_GesCaseProfileEntities] PRIMARY KEY CLUSTERED ([GesCaseProfileInvisibleEntity_Id] ASC),
 CONSTRAINT [FK_32] FOREIGN KEY ([GesCaseProfileTemplates_Id])  REFERENCES [GesCaseProfileTemplates]([GesCaseProfileTemplates_Id]),
 CONSTRAINT [FK_35] FOREIGN KEY ([GesCaseProfileEntity_Id])  REFERENCES [I_GesCaseProfileEntities]([GesCaseProfileEntity_Id])
);

CREATE NONCLUSTERED INDEX [fkIdx_32] ON [GesCaseProfileInvisibleEntities] 
 (
  [GesCaseProfileTemplates_Id] ASC
 )


CREATE NONCLUSTERED INDEX [fkIdx_35] ON [GesCaseProfileInvisibleEntities] 
 (
  [GesCaseProfileEntity_Id] ASC
 )



INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Company info','COMPANY-INFO',1)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Case information','CASE-INFORMATION',2)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Contact information','GES-CONTACT-INFORMATION',3)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'SDGs','SDGS',4)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Guidelines','GUIDELINES-LIST',5)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Conventions','CONVENTIONS',6)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Kpis','KPIS',7)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Summary','SUMMARY',8)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Guidelines','GUIDELINES',9)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Full description','FULL-DESCRIPTION',10)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Description new','DESCRIPTION-NEW',11)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Conclusion','CONCLUSION',12)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Company dialogue','COMPANY-DIALOGUE',13)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Source dialogue','SOURCE-DIALOGUE',14)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'GES commentary','GES-COMMENTARY',15)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'News','NEWS',16)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Engagement information','ENGAGEMENT-INFORMATION',17)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Discussion points','DISCUSSION-POINTS',18)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Other stakeholders','OTHER-STAKEHOLDERS',19)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Associated Corporations','ASSOCIATED-CORPORATIONS',20)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Sources','SOURCES',21)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'References','REFERENCES',22)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'Supplementary Reading','SUPPLEMENTARY-READING',23)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'360 Incident Analysis','360-INCIDENT-ANALYSIS',24)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'360 Closing Incident Analysis','360-CLOSING-INCIDENT-ANALYSIS',25)
INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order]) VALUES (NEWID(),'UNGP assessment form','UNGP-ASSESSMENT-FORM',26)


INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Company info','COMPANY-INFO',1,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-INFO'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Entry date','ENTRY-DATE',2,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Norm','NORM',3,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Location','LOCATION',4,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Recommendation','RECOMMENDATION',5,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Case information Conclusion','CASE-INFORMATION-CONCLUSION',6,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Engagement type','ENGAGEMENT-TYPE',7,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Response','RESPONSE',8,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Progress','PROGRESS',9,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Development','DEVELOPMENT',10,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CASE-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'GES contact information','GES-CONTACT-INFORMATION',11,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'GES-CONTACT-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'SDGs','SDGS',12,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'SDGS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Guidelines list','GUIDELINES-LIST',13,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'GUIDELINES-LIST'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Conventions','CONVENTIONS',14,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CONVENTIONS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Kpis','KPIS',15,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'KPIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Summary','SUMMARY',16,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'SUMMARY'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Guidelines','GUIDELINES',17,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'GUIDELINES'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Full description','FULL-DESCRIPTION',18,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'FULL-DESCRIPTION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Description new','DESCRIPTION-NEW',19,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'DESCRIPTION-NEW'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Conclusion','CONCLUSION',20,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'CONCLUSION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Company dialogue','COMPANY-DIALOGUE',21,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-DIALOGUE'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Company dialogue new','COMPANY-DIALOGUE-NEW',22,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-DIALOGUE'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Company dialogue log','COMPANY-DIALOGUE-LOG',23,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'COMPANY-DIALOGUE'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Source dialogue','SOURCE-DIALOGUE',24,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'SOURCE-DIALOGUE'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Source dialogue new','SOURCE-DIALOGUE-NEW',25,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'SOURCE-DIALOGUE'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Source dialogue log','SOURCE-DIALOGUE-LOG',26,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'SOURCE-DIALOGUE'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'GES commentary','GES-COMMENTARY',27,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'GES-COMMENTARY'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'News','NEWS',28,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'NEWS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Change objective','CHANGE-OBJECTIVE',29,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'ENGAGEMENT-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Milestone','MILESTONE',30,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'ENGAGEMENT-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Next step','NEXT-STEP',31,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'ENGAGEMENT-INFORMATION'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Discussion points','DISCUSSION-POINTS',32,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'DISCUSSION-POINTS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Other stakeholders','OTHER-STAKEHOLDERS',33,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'OTHER-STAKEHOLDERS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Associated Corporations','ASSOCIATED-CORPORATIONS',34,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'ASSOCIATED-CORPORATIONS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Sources','SOURCES',35,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'SOURCES'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'References','REFERENCES',36,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'REFERENCES'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Supplementary Reading','SUPPLEMENTARY-READING',37,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'SUPPLEMENTARY-READING'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Summary','360-INCIDENT-ANALYSIS-SUMMARY',38,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = '360-INCIDENT-ANALYSIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),' Dialogue And Analysis','360-INCIDENT-ANALYSIS-DIALOGUE-AND-ANALYSIS',39,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = '360-INCIDENT-ANALYSIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Conclusion','360-INCIDENT-ANALYSIS-CONCLUSION',40,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = '360-INCIDENT-ANALYSIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Guidelines and conventions','360-INCIDENT-ANALYSIS-GUIDELINES-AND-CONVENTIONS',41,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = '360-INCIDENT-ANALYSIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),' Sources','360-INCIDENT-ANALYSIS-SOURCES',42,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = '360-INCIDENT-ANALYSIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),' Summary','360-CLOSING-INCIDENT-ANALYSIS-SUMMARY',43,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = '360-CLOSING-INCIDENT-ANALYSIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Dialogue And Analysis','360-CLOSING-INCIDENT-ANALYSIS-DIALOGUE-AND-ANALYSIS',44,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = '360-CLOSING-INCIDENT-ANALYSIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'Conclusion','360-CLOSING-INCIDENT-ANALYSIS-CONCLUSION',45,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = '360-CLOSING-INCIDENT-ANALYSIS'));
INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id]) VALUES (NEWID(),'UNGP assessment form','UNGP-ASSESSMENT-FORM',46,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'UNGP-ASSESSMENT-FORM'));
