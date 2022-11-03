IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'CompanyPreparedness') 
ALTER TABLE [dbo].I_GesCaseReports ADD [CompanyPreparedness] nvarchar(MAX)


IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'GAPAnalysis') 
ALTER TABLE [dbo].I_GesCaseReports ADD GAPAnalysis nvarchar(MAX)

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'PositiveDevelopment') 
ALTER TABLE [dbo].I_GesCaseReports ADD PositiveDevelopment nvarchar(MAX)

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'MileStone') 
ALTER TABLE [dbo].I_GesCaseReports ADD MileStone INT

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_GesCaseReports' 
		  AND COLUMN_NAME = 'MileStoneModified') 
ALTER TABLE [dbo].I_GesCaseReports ADD MileStoneModified DATETIME


GO
IF NOT EXISTS (SELECT * FROM GesDocumentService WHERE Name = 'Oekom')
INSERT	INTO dbo.GesDocumentService
        ( Name, SortOrder )
VALUES  ( N'Oekom', 3 )

GO

IF EXISTS( SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'GesDocument'
		AND COLUMN_NAME = 'Comment'
		AND DATA_TYPE = 'ntext')
ALTER TABLE dbo.[GesDocument]
   ALTER COLUMN Comment NVARCHAR(MAX)

GO

IF NOT EXISTS (SELECT * FROM I_EngagementTypes WHERE I_EngagementTypes_Id = 26)
BEGIN
SET IDENTITY_INSERT dbo.I_EngagementTypes ON
INSERT	INTO dbo.I_EngagementTypes
		( I_EngagementTypes_Id, 
		I_EngagementTypeCategories_Id ,
          Name ,
          Description ,
          Goal ,
          NextStep ,
          LatestNews ,
          OtherInitiatives ,
          Sources ,
          GesReports ,
          ContactG_Users_Id ,
          Participants ,
          NonSubscriberInformation ,
          SortOrder ,
          Created
        )
VALUES  ( 26,
			2 ,
          'Governance Engagement' ,
          N'The GES Corporate Governance Engagement programme is a proactive engagement service. We engage with companies on corporate governance issues, including risk management, board composition, executive pay, shareholders’ rights and ESG strategy.' ,
          N'The key objective of the engagement is to improve the company’s corporate governance structures and processes and promote best practice.

We identify companies suitable for engagement based off a number of criteria, such as:

•	prioritising the largest holdings in the countries with the largest weighting in clients’ portfolios; 
•	prioritising companies where significant concerns arise from votes cast at shareholder meetings; 
•	focusing on sectors where ESG issues have a material impact;  
•	focusing on markets where minority shareholders’ rights need specific attention; and 
•	paying specific attention to companies where engagement can achieve change over time.' ,
          N'Follow-up on the results of the 2017 AGM season and begin to set-up meetings with companies ahead of the 2018 season.' ,
          NULL , 
          NULL , 
          NULL , 
          NULL , 
          2019 , 
          N'Nomura Asset Management and ACTIAM' , 
          N'You are currently not subscribing to this engagement type. For more information about this subscription, please contact Kate Jalbert at kate.jalbert@ges-invest.com.' ,
          340 , 
          GETDATE()
        )

SET IDENTITY_INSERT dbo.I_EngagementTypes OFF
END

GO
UPDATE dbo.I_EngagementTypes
SET	Name = 'Extended – Taxation'
WHERE I_EngagementTypes_Id = 18

GO
UPDATE dbo.G_Services
SET	Name = 'Extended – Taxation'
WHERE G_Services_Id = 47

GO

IF NOT EXISTS (SELECT * FROM G_Services WHERE G_Services_Id = 53)
BEGIN
SET IDENTITY_INSERT dbo.G_Services ON

INSERT INTO dbo.G_Services
        ( G_Services_Id ,
          Name ,
          Url ,
          ShowInNavigation ,
          ShowInClient ,
          ReportLetter ,
          I_EngagementTypes_Id
        )
VALUES  ( 53 ,
          'Governance Engagement' ,
          '/en-US/client/engagement_forum/engagement_type.aspx?I_EngagementTypes_Id=26' , -- Url - Url
          1 , 
          1 , 
          NULL ,
          26
        )

SET IDENTITY_INSERT dbo.G_Services OFF
END

