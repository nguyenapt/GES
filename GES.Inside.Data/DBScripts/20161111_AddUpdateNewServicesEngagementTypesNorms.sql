/* Add new Norms */
set identity_insert [dbo].[I_NormAreas] ON
INSERT INTO [dbo].[I_NormAreas]
           ([I_NormAreas_Id],[Name],[Created]) VALUES(9, 'Competition', GETDATE())
INSERT INTO [dbo].[I_NormAreas]
           ([I_NormAreas_Id],[Name],[Created]) VALUES(10, 'Consumer Rights', GETDATE())
set identity_insert [dbo].[I_NormAreas] OFF

/* Add new EngagementTypeCategories */
set identity_insert [dbo].[I_EngagementTypeCategories] ON
INSERT INTO [dbo].[I_EngagementTypeCategories]
           ([I_EngagementTypeCategories_Id]
           ,[Name]
           ,[Created])
     VALUES
           (6, 'Bespoke Engagement', GETDATE())
set identity_insert [dbo].[I_EngagementTypeCategories] OFF

/* Add new Engagement Theme/Norm */

--UPDATE [dbo].[I_EngagementTypes]
--SET I_EngagementTypeCategories_Id = 6, SortOrder = 710
--WHERE Name like '%Low Performers%'

UPDATE [dbo].[I_EngagementTypes]
SET SortOrder = 495
WHERE Name like '%Carbon%'

UPDATE [dbo].[I_EngagementTypes]
SET SortOrder = 515
WHERE Name like '%Emerging%'

UPDATE [dbo].[I_EngagementTypes]
SET SortOrder = 520
WHERE Name like '%Palm%'

UPDATE [dbo].[I_EngagementTypes]
set [Name] = 'Business Conduct - Extended',
	[Description] = 'Business Conduct - Extended',
	[SortOrder] = 505
	where I_EngagementTypes_Id = 18

set identity_insert [dbo].[I_EngagementTypes] ON

INSERT INTO [dbo].[I_EngagementTypes]
           ([I_EngagementTypes_Id]
		   ,[I_EngagementTypeCategories_Id]
           ,[Name]
           ,[Description]
           ,[ContactG_Users_Id]
           ,[SortOrder]
           ,[Created])
     VALUES
           (21, 6, 'Bespoke Engagement', 'Bespoke Engagement', 1164, 700, GETDATE()),
           (22, 2, 'Business Ethics & Culture Engagement', 'Business Ethics & Culture Engagement', 1164, 490, GETDATE()),
           (23, 2, 'Children''s Rights Engagement', 'Children''s Rights Engagement', 1164, 505, GETDATE()),
           (24, 2, 'Cybersecurity Engagement', 'Cybersecurity Engagement', 1164, 510, GETDATE()),
           (25, 2, 'Pharma Engagement', 'Pharma Engagement', 1164, 525, GETDATE())

set identity_insert [dbo].[I_EngagementTypes] OFF

/* Update Services Name */
UPDATE [dbo].[G_Services]
SET Name = 'Alert Service - Extended'
WHERE Name = 'Extended Alert Service'

/* Add new Services */
set identity_insert [dbo].[G_Services] ON
INSERT INTO [dbo].[G_Services]
           ([G_Services_Id]
		   ,[Name]
           ,[ShowInNavigation]
           ,[ShowInClient]
           ,[I_EngagementTypes_Id])
     VALUES 
			(47, 'Business Conduct - Extended', 0, 0, 18),
			(48, 'Business Ethics & Culture Engagement', 0, 0, 22),
			(49, 'Children''s Rights Engagement', 0, 0, 23),
			(50, 'Cybersecurity Engagement', 0, 0, 24),
			(51, 'Pharma Engagement', 0, 0, 25),
			(52, 'Bespoke Engagement', 0, 0, 21)
set identity_insert [dbo].[G_Services] OFF



