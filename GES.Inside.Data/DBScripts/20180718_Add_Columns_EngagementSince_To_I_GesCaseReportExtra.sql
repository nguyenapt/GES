IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReportsExtra' 
		  AND COLUMN_NAME = 'EngagementSince') 
ALTER TABLE [dbo].[I_GesCaseReportsExtra] ADD [EngagementSince] datetime null


GO
--Migrate data
 UPDATE dbo.I_GesCaseReportsExtra 
	SET EngagementSince = (                         
	SELECT TOP 1 AuditDateTime FROM dbo.I_GesCaseReports_Audit
	WHERE  I_GesCaseReports_Id = I_GesCaseReportsExtra.I_GesCaseReports_Id AND AuditChangedColumns LIKE '%NewI_GesCaseReportStatuses_Id%' AND NewI_GesCaseReportStatuses_Id = 7
	ORDER BY AuditDateTime
)
