ALTER TRIGGER [dbo].[I_Milestones_Update] 
ON [dbo].[I_Milestones] 
FOR UPDATE 
AS 
 
 INSERT INTO dbo.I_Milestones_Audit([I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId], [AuditDataState],[AuditDMLAction],[AuditUser],[AuditDateTime])
 SELECT [I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId],'New','Update',SUSER_SNAME(),getdate()  FROM INSERTED  
 
 INSERT INTO dbo.I_Milestones_Audit ([I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId], [AuditDataState],[AuditDMLAction],[AuditUser],[AuditDateTime])
 SELECT [I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId],'Old','Update',SUSER_SNAME(),getdate()  FROM DELETED  




