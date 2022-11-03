IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_PortfolioCompaniesImport' 
		  AND COLUMN_NAME = 'Screened') 
ALTER TABLE [dbo].[I_PortfolioCompaniesImport] ADD [Screened] [bit] NOT NULL CONSTRAINT [DF_I_PortfolioCompaniesImport_Screened]  DEFAULT ((0))
