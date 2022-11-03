
IF NOT EXISTS (SELECT Column_Name
               FROM INFORMATION_SCHEMA.COLUMNS
               WHERE Table_Name = 'I_GesCaseReports'
               AND Column_Name = 'Guidelines')
BEGIN
	
	ALTER TABLE [dbo].[I_GesCaseReports] ADD [Guidelines] NVARCHAR(MAX);
END
