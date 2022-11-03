CREATE TRIGGER [dbo].[I_GesCompanyDialogues_Delete] 
   ON  [dbo].[I_GesCompanyDialogues]
   FOR DELETE
AS 
	DECLARE @IGesCaseReportsId IId;
	DECLARE @IGesCompanyDialoguesId IId;

	DECLARE @oldAction as NVarchar(Max);
	DECLARE @oldText as NVarchar(Max);
	DECLARE @oldNotes as NVarchar(Max);
	DECLARE @oldContactDirectionsId IId;
	DECLARE @oldManagedDocumentsId IId;
	DECLARE @audit_columns_list GesCaseReports_Audit_Columns_list;
	DECLARE @hasChangedValue as bit = 'false';

BEGIN
	select @IGesCompanyDialoguesId = [I_GesCaseReports_Id],
		   @IGesCaseReportsId = [I_GesCaseReports_Id],
		   @oldAction = [Action],
		   @oldText = [Text],
		   @oldNotes = [Notes],
		   @oldContactDirectionsId = [I_ContactDirections_Id],
		   @oldManagedDocumentsId = [G_ManagedDocuments_Id]		    
	from DELETED where [ShowInCsc] = 1 and [ClassA] = 1 and [I_ContactTypes_Id] = 1;
	
	if @oldAction IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Action', @oldAction, NULL, 'Delete');
		Set @hasChangedValue = 'true'
	end

	if @oldText IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Text', @oldText, NULL, 'Delete');
		Set @hasChangedValue = 'true';
	end

	if @oldNotes IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Notes', @oldNotes, NULL, 'Delete');
		Set @hasChangedValue = 'true';
	end

	if @oldContactDirectionsId IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','I_ContactDirections_Id', @oldContactDirectionsId, NULL, 'Delete');
		Set @hasChangedValue = 'true';
	end

	if @oldManagedDocumentsId IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','G_ManagedDocuments_Id', @oldManagedDocumentsId, NULL, 'Delete');
	    Set @hasChangedValue = 'true';
	end

	if(@hasChangedValue = 'true')
	Begin
		Exec GesUpdateGesCaseReportsAudit 'I_GesCompanyDialogues', @IGesCaseReportsId, @audit_columns_list, 'Delete'
	End

END
GO
