
CREATE TRIGGER [dbo].[I_GesCompanyDialogues_Insert]
   ON  [dbo].[I_GesCompanyDialogues]
   FOR INSERT
AS 
DECLARE @IGesCaseReportsId IId;
DECLARE @IGesCompanyDialoguesId IId;


DECLARE @newAction as NVarchar(Max);
DECLARE @newText as NVarchar(Max);
DECLARE @newNotes as NVarchar(Max);
DECLARE @newContactDirectionsId IId;
DECLARE @newManagedDocumentsId IId;
DECLARE @audit_columns_list GesCaseReports_Audit_Columns_list;
DECLARE @hasChangedValue as bit = 'false';

BEGIN
	select @IGesCompanyDialoguesId = [I_GesCaseReports_Id],
		   @IGesCaseReportsId = [I_GesCaseReports_Id],
		   @newAction = [Action],
		   @newText = [Text],
		   @newNotes = [Notes],
		   @newContactDirectionsId = [I_ContactDirections_Id],
		   @newManagedDocumentsId = [G_ManagedDocuments_Id]		    
	from INSERTED where [ShowInCsc] = 1 and [ClassA] = 1 and [I_ContactTypes_Id] = 1;

	if @newAction IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Action', NULL, @newAction, 'Insert');
		Set @hasChangedValue = 'true'
	end

	if @newText IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Text', NULL, @newText, 'Insert');
		Set @hasChangedValue = 'true';
	end

	if @newNotes IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Notes', NULL, @newNotes, 'Insert');
		Set @hasChangedValue = 'true';
	end

	if @newContactDirectionsId IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','I_ContactDirections_Id', NULL, @newContactDirectionsId, 'Insert');
		Set @hasChangedValue = 'true';
	end

	if @newManagedDocumentsId IS NOT NULL
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','G_ManagedDocuments_Id', NULL, @newManagedDocumentsId, 'Insert');
	    Set @hasChangedValue = 'true';
	end

	if(@hasChangedValue = 'true')
	Begin
		Exec GesUpdateGesCaseReportsAudit 'I_GesCompanyDialogues', @IGesCaseReportsId, @audit_columns_list, 'Insert'
	End

END
GO
