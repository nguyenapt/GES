IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_Companies' 
		  AND COLUMN_NAME = 'UnGlobalCompact') 
ALTER TABLE [dbo].[I_Companies] ADD [UnGlobalCompact] nvarchar(4000) NULL

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_Companies' 
		  AND COLUMN_NAME = 'GriAlignedDisclosure') 
ALTER TABLE [dbo].[I_Companies] ADD [GriAlignedDisclosure] nvarchar(4000) NULL
