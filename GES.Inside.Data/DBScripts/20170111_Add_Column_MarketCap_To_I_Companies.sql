IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_Companies' 
		  AND COLUMN_NAME = 'MarketCap') 
ALTER TABLE [dbo].[I_Companies] ADD [MarketCap] decimal(10,2)

