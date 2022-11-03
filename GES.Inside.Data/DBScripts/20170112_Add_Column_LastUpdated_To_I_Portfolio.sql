IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_Portfolios' 
		  AND COLUMN_NAME = 'LastUpdated') 
ALTER TABLE [dbo].[I_Portfolios] ADD [LastUpdated] datetime

