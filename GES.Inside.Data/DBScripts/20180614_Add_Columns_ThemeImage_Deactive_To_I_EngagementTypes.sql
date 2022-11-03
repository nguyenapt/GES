IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_EngagementTypes' 
		  AND COLUMN_NAME = 'ThemeImage') 
ALTER TABLE [dbo].[I_EngagementTypes] ADD [ThemeImage] nvarchar(500)

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_EngagementTypes' 
		  AND COLUMN_NAME = 'Deactive') 
ALTER TABLE [dbo].[I_EngagementTypes] ADD [Deactive] bit

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_EngagementTypes' 
		  AND COLUMN_NAME = 'IsShowInClientMenu') 
ALTER TABLE [dbo].[I_EngagementTypes] ADD [IsShowInClientMenu] bit


UPDATE dbo.I_EngagementTypes
SET IsShowInClientMenu = 1
WHERE I_EngagementTypes_Id IN (2,5,9,14,17,18,19,20,26,27,28,29)

UPDATE dbo.I_EngagementTypes
SET IsShowInClientMenu = 0
WHERE I_EngagementTypes_Id IN (4,16)

UPDATE dbo.I_EngagementTypes
SET Deactive = 1
WHERE I_EngagementTypes_Id IN (5,16)


UPDATE dbo.I_EngagementTypes
SET ThemeImage = 'engagement-type-5.jpg'
WHERE I_EngagementTypes_Id = 5

UPDATE dbo.I_EngagementTypes
SET ThemeImage = 'engagement-type-9.jpg'
WHERE I_EngagementTypes_Id = 9

UPDATE dbo.I_EngagementTypes
SET ThemeImage = 'engagement-type-14.jpg'
WHERE I_EngagementTypes_Id = 14

UPDATE dbo.I_EngagementTypes
SET ThemeImage = 'engagement-type-17.jpg'
WHERE I_EngagementTypes_Id = 17

UPDATE dbo.I_EngagementTypes
SET ThemeImage = 'engagement-type-19.jpg'
WHERE I_EngagementTypes_Id = 19

UPDATE dbo.I_EngagementTypes
SET ThemeImage = 'engagement-type-20.jpg'
WHERE I_EngagementTypes_Id = 20

UPDATE dbo.I_EngagementTypes
SET ThemeImage = 'engagement-type-26.jpg'
WHERE I_EngagementTypes_Id = 26

UPDATE dbo.I_EngagementTypes
SET ThemeImage = 'engagement-type-20.jpg'
WHERE I_EngagementTypes_Id = 18
