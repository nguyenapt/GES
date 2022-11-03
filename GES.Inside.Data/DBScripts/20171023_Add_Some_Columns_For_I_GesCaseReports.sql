IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'CompanyPreparedness') 
ALTER TABLE [dbo].I_GesCaseReports ADD [CompanyPreparedness] nvarchar(MAX)


IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'GAPAnalysis') 
ALTER TABLE [dbo].I_GesCaseReports ADD GAPAnalysis nvarchar(MAX)

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'PositiveDevelopment') 
ALTER TABLE [dbo].I_GesCaseReports ADD PositiveDevelopment nvarchar(MAX)

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'MileStone') 
ALTER TABLE [dbo].I_GesCaseReports ADD MileStone INT

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'MileStoneModified') 
ALTER TABLE [dbo].I_GesCaseReports ADD MileStoneModified DATETIME
