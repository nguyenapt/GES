IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_EngagementTypes' 
		  AND COLUMN_NAME = 'IsShowInCaseProfileTemplate') 
ALTER TABLE [dbo].[I_EngagementTypes] ADD [IsShowInCaseProfileTemplate] bit DEFAULT 1 WITH VALUES

Update [I_EngagementTypes] set [IsShowInCaseProfileTemplate] = 1 where I_EngagementTypes_Id <> 2
Update [I_EngagementTypes] set [IsShowInCaseProfileTemplate] = 0 where I_EngagementTypes_Id = 2

