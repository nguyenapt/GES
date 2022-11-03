CREATE  PROCEDURE [dbo].[UpdateGesUNGPAssessmentFormResource_Audit] 	
	 @GesUNGPAssessmentFormId uniqueidentifier,
	 @GesUNGPAssessmentFormResourcesId uniqueidentifier,
	 @GesUNGPAssessmentFormAuditColumnsList  GesUNGPAssessmentForm_Audit_Columns_list READONLY,
	 @AuditDMLAction varchar(10),
	 @AuditUserId nvarchar(200)
AS

DECLARE @GesUNGPAssessmentFormResourceAuditId uniqueidentifier;
DECLARE @AuditUser nvarchar(200)

DECLARE @columns_list GesUNGPAssessmentForm_Audit_Columns_list;
DECLARE @ColumnName nvarchar(150)
DECLARE @ColumnNameDescription nvarchar(250)
DECLARE @OldValue nvarchar(max)
DECLARE @NewValue nvarchar(max)
DECLARE @AuditDataState varchar(10)
DECLARE @TransactionId uniqueidentifier


DECLARE ungpResourceCur CURSOR FOR SELECT ColumnName,ColumnNameDescription, OldValue, NewValue, AuditDataState FROM @GesUNGPAssessmentFormAuditColumnsList

BEGIN
	set @GesUNGPAssessmentFormResourceAuditId = NEWID();

	exec GetUserFullName @AuditUserId, @AuditUser output
		
	Insert into GesUNGPAssessmentFormResource_Audit(GesUNGPAssessmentFormResource_Audit_Id, GesUNGPAssessmentFormId, GesUNGPAssessmentFormResourcesId, AuditDMLAction, AuditUser, AuditDatetime)
	Values(@GesUNGPAssessmentFormResourceAuditId, @GesUNGPAssessmentFormId, @GesUNGPAssessmentFormResourcesId,@AuditDMLAction, @AuditUser, GETDATE());

OPEN ungpResourceCur

FETCH NEXT FROM ungpResourceCur INTO @ColumnName,@ColumnNameDescription, @OldValue, @NewValue, @AuditDataState

WHILE @@FETCH_STATUS = 0 BEGIN
    insert into GesUNGPAssessmentFormResource_Audit_Details(GesUNGPAssessmentFormResource_Audit_Details_Id, GesUNGPAssessmentFormResource_Audit_Id, ColumnName,ColumnNameDescription, OldValue ,NewValue, AuditDataState, AuditUser, AuditDatetime)
	Values(NEWID(), @GesUNGPAssessmentFormResourceAuditId, @ColumnName,@ColumnNameDescription, @OldValue, @NewValue, @AuditDataState, @AuditUser, GETDATE())

    FETCH NEXT FROM ungpResourceCur INTO  @ColumnName,@ColumnNameDescription, @OldValue, @NewValue, @AuditDataState
END

CLOSE ungpResourceCur    
DEALLOCATE ungpResourceCur

END

