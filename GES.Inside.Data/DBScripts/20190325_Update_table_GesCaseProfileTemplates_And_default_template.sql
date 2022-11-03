ALTER TABLE [GESClients].[dbo].[GesCaseProfileTemplates] ALTER COLUMN [I_EngagementTypes_Id] bigint NULL 
ALTER TABLE [GESClients].[dbo].[GesCaseProfileTemplates] ALTER COLUMN [I_GesCaseReportStatuses_Id] bigint NULL 

INSERT INTO [dbo].[GesCaseProfileTemplates]([GesCaseProfileTemplates_Id],[Name],[Description],[I_EngagementTypes_Id],[I_GesCaseReportStatuses_Id],[Created],[ModifiedByG_Users_Id])
     VALUES(NEWID(),'Default template','Default template',null,null,GETDATE(),null)