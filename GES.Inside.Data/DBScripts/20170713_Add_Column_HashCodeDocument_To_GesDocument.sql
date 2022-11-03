IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesDocument' 
		  AND COLUMN_NAME = 'HashCodeDocument') 
ALTER TABLE [dbo].[GesDocument] ADD [HashCodeDocument] varchar(6)

