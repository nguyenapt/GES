CREATE PROCEDURE [dbo].[UpdateGesUNGPAssessmentForm_Audit] 	
	 @GesUNGPAssessmentFormId uniqueidentifier,
	 @IGesCaseReportsId IId,
	 @GesUNGPAssessmentFormAuditColumnsList  GesUNGPAssessmentForm_Audit_Columns_list READONLY,
	 @AuditDMLAction varchar(10),
	 @AuditUserId nvarchar(200)
AS

declare @GesUNGPAssessmentFormAuditId uniqueidentifier;
DECLARE @columns_list GesUNGPAssessmentForm_Audit_Columns_list;
DECLARE @ColumnName nvarchar(150)
DECLARE @OldValue nvarchar(max)
DECLARE @NewValue nvarchar(max)
DECLARE @AuditDataState varchar(10)
DECLARE @TransactionId uniqueidentifier
DECLARE @AuditUser nvarchar(200)

DECLARE ungpCur CURSOR FOR SELECT ColumnName, OldValue, NewValue, AuditDataState FROM @GesUNGPAssessmentFormAuditColumnsList

BEGIN
	set @GesUNGPAssessmentFormAuditId = NEWID();

	exec GetUserFullName @AuditUserId, @AuditUser output
		
	Insert into GesUNGPAssessmentForm_Audit(GesUNGPAssessmentForm_Audit_Id, GesUNGPAssessmentFormId, I_GesCaseReports_Id, AuditDMLAction, AuditUser, AuditDatetime)
	Values(@GesUNGPAssessmentFormAuditId, @GesUNGPAssessmentFormId, @IGesCaseReportsId,@AuditDMLAction, @AuditUser, GETDATE());

OPEN ungpCur

FETCH NEXT FROM ungpCur INTO @ColumnName, @OldValue, @NewValue, @AuditDataState

WHILE @@FETCH_STATUS = 0 BEGIN
    insert into GesUNGPAssessmentForm_Audit_Details(GesUNGPAssessmentForm_Audit_Details_Id, GesUNGPAssessmentForm_Audit_Id, ColumnName, OldValue ,NewValue, AuditDataState, AuditUser, AuditDatetime)
	Values(NEWID(), @GesUNGPAssessmentFormAuditId, @ColumnName, @OldValue, @NewValue, @AuditDataState, @AuditUser, GETDATE())

    FETCH NEXT FROM ungpCur INTO  @ColumnName, @OldValue, @NewValue, @AuditDataState
END

CLOSE ungpCur    
DEALLOCATE ungpCur

END
