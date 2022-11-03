
DECLARE @GesCaseReportsId IId
DECLARE @ConclusionId IId, @AuditDatetime DATETIME
DECLARE @AuditId UNIQUEIDENTIFIER


DECLARE curR CURSOR FOR SELECT  I_GesCaseReports_Audit.I_GesCaseReports_Id ,
        I_GesCaseReports_Audit.I_GesCaseReportStatuses_Id ,
        MIN(I_GesCaseReports_Audit.AuditDateTime) AS AuditDateTime
FROM    I_GesCaseReports_Audit
        INNER JOIN I_GesCaseReports ON I_GesCaseReports.I_GesCaseReports_Id = I_GesCaseReports_Audit.I_GesCaseReports_Id
		WHERE
		I_GesCaseReports_Audit.I_GesCaseReportStatuses_Id IS NOT NULL
GROUP BY I_GesCaseReports_Audit.I_GesCaseReports_Id ,
        I_GesCaseReports_Audit.I_GesCaseReportStatuses_Id

OPEN curR

FETCH NEXT FROM curR INTO @GesCaseReportsId, @ConclusionId, @AuditDatetime

WHILE @@FETCH_STATUS = 0 
BEGIN
	
		SET @AuditId = NEWID()

		INSERT INTO dbo.GesCaseReports_Audit
		        ( GesCaseReports_Audit_Id ,
		          I_GesCaseReports_Id ,
		          AuditUser ,
		          AuditDMLAction ,
		          AuditDatetime
		        )
		VALUES  ( @AuditId , -- GesCaseReports_Audit_Id - uniqueidentifier
		          @GesCaseReportsId , -- I_GesCaseReports_Id - int
		          N'' , -- AuditUser - nvarchar(128)
		          '' , -- AuditDMLAction - varchar(10)
		          @AuditDatetime  -- AuditDatetime - datetime
		        )

		INSERT INTO dbo.GesCaseReports_Audit_Details
		        ( GesCaseReports_Audit_Details ,
		          GesCaseReports_Audit_GesCaseReports_Audit_Id ,
		          TableName ,
		          ColumnName ,
		          OldValue ,
		          NewValue ,
		          AuditDataState ,
		          AuditUser ,
		          AuditDatetime
		        )
		VALUES  ( NEWID() , -- GesCaseReports_Audit_Details - uniqueidentifier
		          @AuditId , -- GesCaseReports_Audit_GesCaseReports_Audit_Id - uniqueidentifier
		          N'I_GesCaseReports' , -- TableName - nvarchar(100)
		          N'I_GesCaseReportStatuses_Id' , -- ColumnName - nvarchar(150)
		          N'' , -- OldValue - nvarchar(max)
		          CAST(@ConclusionId AS NVARCHAR(max)) , -- NewValue - nvarchar(max)
		          '' , -- AuditDataState - varchar(10)
		          N'' , -- AuditUser - nvarchar(128)
		          @AuditDatetime  -- AuditDatetime - datetime
		        )

    FETCH NEXT FROM curR INTO @GesCaseReportsId, @ConclusionId, @AuditDatetime
END

CLOSE curR
DEALLOCATE curR
