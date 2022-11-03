
IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_Milestones_Audit' 
		  AND COLUMN_NAME = 'GesMilestoneTypesId') 
ALTER TABLE [dbo].[I_Milestones_Audit] ADD [GesMilestoneTypesId] uniqueidentifier NULL




