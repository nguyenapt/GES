ALTER TRIGGER [dbo].[I_Milestones_Insert] 
ON [dbo].[I_Milestones] 
FOR INSERT 
AS 

 DECLARE @GesMilestoneTypesId as NVarchar(1024); 
 DECLARE @IGesCaseReportsId as NVarchar(1024); 
 DECLARE @MilestonesDescription as NVarchar(1024);
 DECLARE @audit_columns_list GesCaseReports_Audit_Columns_list;

 select @IGesCaseReportsId = [I_GesCaseReports_Id], @GesMilestoneTypesId = [GesMilestoneTypesId], @MilestonesDescription = [Description] from INSERTED;

 INSERT INTO dbo.I_Milestones_Audit ([I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId], [AuditDataState],[AuditDMLAction],[AuditUser],[AuditDateTime])
 SELECT [I_Milestones_Id],[I_GesCaseReports_Id],[Description],[MilestoneModified],[Created],[GesMilestoneTypesId],'New','Insert',SUSER_SNAME(),getdate()  FROM INSERTED  
 
 if(@GesMilestoneTypesId IS NOT NULL)
 Begin
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_Milestones','GesMilestoneTypesId', '', @GesMilestoneTypesId, 'Insert')
 End 

 if(@MilestonesDescription IS NOT NULL)
 Begin
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_Milestones','Description', '', @MilestonesDescription, 'Insert')
 End

 EXEC GesUpdateGesCaseReportsAudit 'I_Milestones', @IGesCaseReportsId, @audit_columns_list, 'Insert'
