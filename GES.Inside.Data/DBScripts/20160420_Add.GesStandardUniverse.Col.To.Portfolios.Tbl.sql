IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_Portfolios' 
		  AND COLUMN_NAME = 'GESStandardUniverse') 
ALTER TABLE [dbo].[I_Portfolios] ADD [GESStandardUniverse] [bit] NOT NULL CONSTRAINT [DF_I_Portfolios_GESStandardUniverse]  DEFAULT ((0))
