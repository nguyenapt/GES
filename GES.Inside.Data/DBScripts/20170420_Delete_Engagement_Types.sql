
DELETE dbo.G_Services
WHERE I_EngagementTypes_Id IN (SELECT I_EngagementTypes_Id FROM dbo.I_EngagementTypes WHERE Name = 'Bespoke Engagement')

DELETE dbo.I_EngagementTypes
WHERE Name = 'Bespoke Engagement'

DELETE dbo.G_Services
WHERE I_EngagementTypes_Id IN (SELECT I_EngagementTypes_Id FROM dbo.I_EngagementTypes WHERE Name = 'Business Ethics & Culture Engagement')

DELETE dbo.I_EngagementTypes
WHERE Name = 'Business Ethics & Culture Engagement'

DELETE dbo.G_Services
WHERE I_EngagementTypes_Id IN (SELECT I_EngagementTypes_Id FROM dbo.I_EngagementTypes WHERE Name = 'Children''s Rights Engagement')

DELETE dbo.I_EngagementTypes
WHERE Name = 'Children''s Rights Engagement'

DELETE dbo.G_Services
WHERE I_EngagementTypes_Id IN (SELECT I_EngagementTypes_Id FROM dbo.I_EngagementTypes WHERE Name = 'Cybersecurity Engagement')

DELETE dbo.I_EngagementTypes
WHERE Name = 'Cybersecurity Engagement'

DELETE dbo.G_Services
WHERE I_EngagementTypes_Id IN (SELECT I_EngagementTypes_Id FROM dbo.I_EngagementTypes WHERE Name = 'Pharma Engagement')

DELETE dbo.I_EngagementTypes
WHERE Name = 'Pharma Engagement'

