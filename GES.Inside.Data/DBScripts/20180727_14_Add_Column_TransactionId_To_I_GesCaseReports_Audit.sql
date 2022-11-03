IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports_Audit' 
		  AND COLUMN_NAME = 'TransactionId') 
ALTER TABLE [dbo].[I_GesCaseReports_Audit] ADD [TransactionId] nvarchar(150)

