ALTER TRIGGER [dbo].[I_Milestones_Update] 
ON [dbo].[I_Milestones] 
FOR UPDATE 
AS 

 DECLARE @IGesCaseReportsId as NVarchar(1024); 
 DECLARE @oldMilestonesDescription as NVarchar(1024);
 DECLARE @oldGesMilestoneTypesId as NVarchar(1024);
 DECLARE @newMilestonesDescription as NVarchar(1024);
 DECLARE @newGesMilestoneTypesId as NVarchar(1024);  
 DECLARE @audit_columns_list GesCaseReports_Audit_Columns_list;

 select @IGesCaseReportsId = [I_GesCaseReports_Id], @oldGesMilestoneTypesId = [GesMilestoneTypesId], @oldMilestonesDescription = [Description] from DELETED;
 select @IGesCaseReportsId = [I_GesCaseReports_Id], @newGesMilestoneTypesId = [GesMilestoneTypesId], @newMilestonesDescription = [Description] from INSERTED;

  If @oldGesMilestoneTypesId <> @newGesMilestoneTypesId
 Begin
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_Milestones','GesMilestoneTypesId', @oldGesMilestoneTypesId, @newGesMilestoneTypesId, 'Update')
 End

 If @oldMilestonesDescription <> @newMilestonesDescription
 Begin
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_Milestones','Description', @oldMilestonesDescription, @newMilestonesDescription, 'Update')
 End
 
 INSERT INTO dbo.I_Milestones_Audit([I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId], [AuditDataState],[AuditDMLAction],[AuditUser],[AuditDateTime])
 SELECT [I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId],'New','Update',SUSER_SNAME(),getdate()  FROM INSERTED  
 
 INSERT INTO dbo.I_Milestones_Audit ([I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId], [AuditDataState],[AuditDMLAction],[AuditUser],[AuditDateTime])
 SELECT [I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId],'Old','Update',SUSER_SNAME(),getdate()  FROM DELETED  
 

 EXEC GesUpdateGesCaseReportsAudit 'I_Milestones', @IGesCaseReportsId, @audit_columns_list, 'Update'