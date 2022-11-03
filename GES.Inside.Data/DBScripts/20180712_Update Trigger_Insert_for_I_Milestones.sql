ALTER TRIGGER [dbo].[I_Milestones_Insert] 
ON [dbo].[I_Milestones] 
FOR INSERT 
AS 
 
 INSERT INTO dbo.I_Milestones_Audit ([I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId], [AuditDataState],[AuditDMLAction],[AuditUser],[AuditDateTime])
 SELECT [I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId],'New','Insert',SUSER_SNAME(),getdate()  FROM INSERTED  








