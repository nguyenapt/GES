ALTER TRIGGER [dbo].[I_Milestones_Delete] 
ON [dbo].[I_Milestones] 
FOR DELETE 
AS 
 
 INSERT INTO dbo.I_Milestones_Audit ([I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId], [AuditDataState],[AuditDMLAction],[AuditUser],[AuditDateTime])
 SELECT [I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId],'Old','Delete',SUSER_SNAME(),getdate()  FROM DELETED  
