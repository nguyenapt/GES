
CREATE TRIGGER [dbo].[I_GesCompanyDialogues_Update]
   ON  [dbo].[I_GesCompanyDialogues]
   FOR UPDATE
AS 
DECLARE @IGesCaseReportsId IId;
DECLARE @IGesCompanyDialoguesId IId;

DECLARE @oldAction as NVarchar(Max);
DECLARE @newAction as NVarchar(Max);
DECLARE @oldText as NVarchar(Max);
DECLARE @newText as NVarchar(Max);
DECLARE @oldNotes as NVarchar(Max);
DECLARE @newNotes as NVarchar(Max);
DECLARE @oldContactDirectionsId IId;
DECLARE @newContactDirectionsId IId;
DECLARE @oldManagedDocumentsId IId;
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

	select @IGesCompanyDialoguesId = [I_GesCaseReports_Id],
		   @IGesCaseReportsId = [I_GesCaseReports_Id],
		   @oldAction = [Action],
		   @oldText = [Text],
		   @oldNotes = [Notes],
		   @oldContactDirectionsId = [I_ContactDirections_Id],
		   @oldManagedDocumentsId = [G_ManagedDocuments_Id]		    
	from DELETED where [ShowInCsc] = 1 and [ClassA] = 1 and [I_ContactTypes_Id] = 1;

	if @newAction <> @oldAction
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Action', @oldAction, @newAction, 'Update');
		Set @hasChangedValue = 'true'
	end

	if @newText <> @oldText
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Text', @oldText, @newText, 'Update');
		Set @hasChangedValue = 'true';
	end

	if @newNotes <> @oldNotes
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','Notes', @oldNotes, @newNotes, 'Update');
		Set @hasChangedValue = 'true';
	end

	if @newContactDirectionsId <> @oldContactDirectionsId
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','I_ContactDirections_Id', @oldContactDirectionsId, @newContactDirectionsId, 'Update');
		Set @hasChangedValue = 'true';
	end

	if @newManagedDocumentsId <> @oldManagedDocumentsId
	begin
		INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_GesCompanyDialogues','G_ManagedDocuments_Id', @oldManagedDocumentsId, @newManagedDocumentsId, 'Update');
	    Set @hasChangedValue = 'true';
	end

	if(@hasChangedValue = 'true')
	Begin
		Exec GesUpdateGesCaseReportsAudit 'I_GesCompanyDialogues', @IGesCaseReportsId, @audit_columns_list, 'Update'
	End

END
GO
