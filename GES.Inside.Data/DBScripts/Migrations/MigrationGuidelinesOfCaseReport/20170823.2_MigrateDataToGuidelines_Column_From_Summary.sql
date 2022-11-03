
DISABLE TRIGGER dbo.I_GesCaseReports_Insert ON dbo.I_GesCaseReports;
DISABLE TRIGGER dbo.I_GesCaseReports_Update ON dbo.I_GesCaseReports;
DISABLE TRIGGER dbo.I_GesCaseReports_Delete ON dbo.I_GesCaseReports;

UPDATE [dbo].[I_GesCaseReports]  SET [Guidelines] = CAST(CASE 
	WHEN Summary IS NULL OR CHARINDEX('.', REVERSE(Summary), 2) = 0
		THEN Summary
	ELSE
		REVERSE(SUBSTRING(REVERSE(Summary),0, CHARINDEX('.', REVERSE(Summary), 2)))	
	END as Nvarchar(max))
WHERE [Guidelines] IS NULL;

ENABLE TRIGGER dbo.I_GesCaseReports_Insert ON dbo.I_GesCaseReports;
ENABLE TRIGGER dbo.I_GesCaseReports_Update ON dbo.I_GesCaseReports;
ENABLE TRIGGER dbo.I_GesCaseReports_Delete ON dbo.I_GesCaseReports;
