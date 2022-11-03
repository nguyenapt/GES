-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE   PROCEDURE [dbo].[GesUpdateGesCaseReportsAudit] 
	 @TableName varchar(255),
	 @IGesCaseReportsId IId,
	 @GesCaseReportsAuditColumnsList GesCaseReports_Audit_Columns_list READONLY,
	 @AuditDMLAction varchar(10)
AS
declare @SQL nVarChar(max)
declare @CaseReportsAuditId uniqueidentifier
DECLARE @columns_list GesCaseReports_Audit_Columns_list;
DECLARE @UpdateTableName nvarchar(100)
DECLARE @ColumnName nvarchar(150)
DECLARE @OldValue nvarchar(max)
DECLARE @NewValue nvarchar(max)
DECLARE @AuditDataState varchar(10)

DECLARE curR CURSOR FOR SELECT TableName, ColumnName, OldValue, NewValue, AuditDataState FROM @GesCaseReportsAuditColumnsList

BEGIN
	set @CaseReportsAuditId = NEWID();	
	Insert into GesCaseReports_Audit(GesCaseReports_Audit_Id, I_GesCaseReports_Id, AuditUser,AuditDMLAction,AuditDatetime)
	Values(@CaseReportsAuditId, @IGesCaseReportsId, SUSER_SNAME(), @AuditDMLAction, GETDATE());

OPEN curR

FETCH NEXT FROM curR INTO @UpdateTableName, @ColumnName, @OldValue, @NewValue, @AuditDataState

WHILE @@FETCH_STATUS = 0 BEGIN
    insert into GesCaseReports_Audit_Details(GesCaseReports_Audit_Details, GesCaseReports_Audit_GesCaseReports_Audit_Id,TableName, ColumnName, OldValue,NewValue, AuditDataState, AuditUser, AuditDatetime)
	Values(NEWID(), @CaseReportsAuditId, @UpdateTableName, @ColumnName, @OldValue, @NewValue, @AuditDataState, SUSER_SNAME(), GETDATE())

    FETCH NEXT FROM curR INTO @UpdateTableName, @ColumnName, @OldValue, @NewValue, @AuditDataState
END

CLOSE curR    
DEALLOCATE curR

END
go

