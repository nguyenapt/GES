
IF NOT EXISTS (SELECT * FROM dbo.I_EngagementTypeCategories WHERE I_EngagementTypeCategories_Id = 8)
BEGIN
	SET IDENTITY_INSERT [dbo].[I_EngagementTypeCategories] ON

	INSERT INTO dbo.I_EngagementTypeCategories
			( I_EngagementTypeCategories_Id, Name, Created )
			VALUES  ( 8,
					'Corporate Governance',
					GETDATE()
					)

	SET IDENTITY_INSERT [dbo].[I_EngagementTypeCategories] OFF
END

UPDATE dbo.I_EngagementTypes
	SET	I_EngagementTypeCategories_Id = 8,
	Name = 'AGM related Engagement'
	WHERE I_EngagementTypes_Id = 26

	GO
IF NOT EXISTS (SELECT * FROM dbo.I_EngagementTypes WHERE I_EngagementTypes_Id = 36)
BEGIN
	SET IDENTITY_INSERT [dbo].[I_EngagementTypes] ON

	INSERT INTO dbo.I_EngagementTypes
			(I_EngagementTypes_Id, I_EngagementTypeCategories_Id, Name, Created, Description, Goal, NextStep, ContactG_Users_Id, NonSubscriberInformation, ThemeImage, IsShowInClientMenu )
			VALUES  ( 36,
					8,
					'Ongoing Engagement',
					GETDATE(),
					N'The GES Corporate Governance Engagement programme is a proactive engagement service. We engage with companies on corporate governance issues, including risk management, board composition, executive pay, shareholders’ rights and ESG strategy.',
					N'The key objective of the engagement is to improve the company’s corporate governance structures and processes and promote best practice. We identify companies suitable for engagement based off a number of criteria, such as:

•	prioritising the largest holdings in the countries with the largest weighting in clients’ portfolios; 
•	prioritising companies where significant concerns arise from votes cast at shareholder meetings; 
•	focusing on sectors where ESG issues have a material impact;  
•	focusing on markets where minority shareholders’ rights need specific attention; and 
•	paying specific attention to companies where engagement can achieve change over time.',
N'Follow-up on the results of the 2018 AGM season.',
					2019,
					N'You are currently not subscribing to this engagement type. For more information about this subscription, please contact Kate Jalbert at kate.jalbert@gesinternational.com.',
					'engagement-type-26.jpg',
					1
					)

	SET IDENTITY_INSERT [dbo].I_EngagementTypes OFF
END

GO
IF NOT EXISTS (SELECT * FROM dbo.G_Services WHERE G_Services_Id = 63)
BEGIN
	SET IDENTITY_INSERT [dbo].[G_Services] ON

	INSERT INTO dbo.G_Services
			(G_Services_Id, Name, I_EngagementTypes_Id,Url,ShowInNavigation,ShowInClient )
			VALUES  ( 63,
					'Corporate governance - Ongoing Engagement',
					36,
					'/en-US/client/engagement_forum/engagement_type.aspx?I_EngagementTypes_Id=36',
					1,
					1
					)

	SET IDENTITY_INSERT [dbo].G_Services OFF
END

UPDATE dbo.G_Services
SET Name = 'Corporate governance - AGM related Engagement'
WHERE G_Services_Id = 53

GO

DECLARE @OrganizationId IId
DECLARE @ModifiedByG_Users_Id IId
DECLARE @G_ServiceStates_Id IId

DECLARE curR CURSOR FOR SELECT G_Organizations_Id, ModifiedByG_Users_Id, G_ServiceStates_Id  FROM dbo.G_OrganizationsG_Services WHERE G_Services_Id = 53

OPEN curR

FETCH NEXT FROM curR INTO @OrganizationId, @ModifiedByG_Users_Id, @G_ServiceStates_Id

WHILE @@FETCH_STATUS = 0 BEGIN
	INSERT INTO dbo.G_OrganizationsG_Services
	        ( G_Organizations_Id ,
	          G_Services_Id ,
	          Created ,
	          Modified ,
	          ModifiedByG_Users_Id ,
	          G_ServiceStates_Id ,
	          TermsAccepted ,
	          W_SuperFilter
	         
	        )
	VALUES  ( @OrganizationId , -- G_Organizations_Id - IId
	          63 , -- G_Services_Id - IId
	          GETDATE() , -- Created - datetime
	          NULL , -- Modified - datetime
	          @ModifiedByG_Users_Id , -- ModifiedByG_Users_Id - IId
	          @G_ServiceStates_Id , -- G_ServiceStates_Id - IId
	          0 , -- TermsAccepted - bit
	          0 -- W_SuperFilter - bit
	        )

    FETCH NEXT FROM curR INTO @OrganizationId, @ModifiedByG_Users_Id, @G_ServiceStates_Id

END

CLOSE curR    
DEALLOCATE curR


GO
UPDATE dbo.I_GesCaseReportsI_EngagementTypes
SET I_EngagementTypes_Id = 36
WHERE I_GesCaseReportsI_EngagementTypes_Id IN (

SELECT ge.I_GesCaseReportsI_EngagementTypes_Id FROM dbo.I_GesCaseReportsI_EngagementTypes ge
INNER JOIN dbo.I_GesCaseReports gc ON gc.I_GesCaseReports_Id = ge.I_GesCaseReports_Id
WHERE I_EngagementTypes_Id = 26
AND gc.ReportIncident LIKE 'ongoing%')

GO


DECLARE @I_PortfoliosG_Organizations_Id IId

DECLARE curPortfolioOrganizationServices CURSOR FOR SELECT I_PortfoliosG_Organizations_Id  FROM dbo.I_PortfoliosG_OrganizationsG_Services WHERE G_Services_Id = 53

OPEN curPortfolioOrganizationServices

FETCH NEXT FROM curPortfolioOrganizationServices INTO @I_PortfoliosG_Organizations_Id

WHILE @@FETCH_STATUS = 0 BEGIN
	INSERT INTO dbo.I_PortfoliosG_OrganizationsG_Services
	        ( I_PortfoliosG_Organizations_Id ,
	          G_Services_Id ,
	          Created
	        )
	VALUES  ( @I_PortfoliosG_Organizations_Id , -- I_PortfoliosG_Organizations_Id - IId
	          63 , -- G_Services_Id - IId
	          GETDATE()  -- Created - datetime
	        )

    FETCH NEXT FROM curPortfolioOrganizationServices INTO @I_PortfoliosG_Organizations_Id

END

CLOSE curPortfolioOrganizationServices    
DEALLOCATE curPortfolioOrganizationServices


