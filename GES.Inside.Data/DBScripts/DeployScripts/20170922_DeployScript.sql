
-- Advanced Seach Store procedure

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CompanyAdvancedSearch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CompanyAdvancedSearch]
GO
CREATE PROCEDURE [dbo].[CompanyAdvancedSearch]
	@CompanyIssueName NVARCHAR(255),
	@Isin VARCHAR(50),
	@IndividualId BIGINT,
	@OrgId BIGINT,
	@NormId BIGINT,
	@EngagementTypeId BIGINT,
	@RecommendationIds VARCHAR(Max),
	@ConclusionIds VARCHAR(Max),
	@ProgressIds VARCHAR(Max),
	@ResponseIds VARCHAR(Max),
	@ServiceIds VARCHAR(Max),
	@LocationIds VARCHAR(Max),
	@HomeCountryIds VARCHAR(Max),
	@PortfoliosOrganizationIds VARCHAR(Max),
	@IndustryIds VARCHAR(Max),
	@IsHideClosedCases bit
AS
BEGIN

	DECLARE @tempRecommendationIds TABLE (Id bigint)

	IF @RecommendationIds IS NOT NULL AND LEN(@RecommendationIds) > 0 BEGIN
		INSERT INTO @tempRecommendationIds (Id)  
		SELECT * from dbo.splittoint(@RecommendationIds, ',')
	END
	ELSE BEGIN
		INSERT INTO @tempRecommendationIds VALUES (-1)
	END

	DECLARE @tempConclusionIds TABLE (Id bigint)
	IF @ConclusionIds IS NOT NULL AND LEN(@ConclusionIds) > 0 BEGIN
		INSERT INTO @tempConclusionIds (Id)  
		SELECT * from dbo.splittoint(@ConclusionIds, ',')
	END
	ELSE BEGIN
		INSERT INTO @tempConclusionIds VALUES (-1)
	END

	DECLARE @tempProgressIds TABLE (Id bigint)

	IF @ProgressIds IS NOT NULL AND LEN(@ProgressIds) > 0
		INSERT INTO @tempProgressIds (Id)  
		SELECT * from dbo.splittoint(@ProgressIds, ',')
	ELSE
		INSERT INTO @tempProgressIds VALUES (-1)

	DECLARE @tempResponseIds TABLE (Id bigint)

	IF @ResponseIds IS NOT NULL AND LEN(@ResponseIds) > 0
		INSERT INTO @tempResponseIds (Id)  
		SELECT * from dbo.splittoint(@ResponseIds, ',')
	ELSE
		INSERT INTO @tempResponseIds VALUES (-1)

	DECLARE @tempHomeCountryIds TABLE (Id bigint)
	IF @HomeCountryIds IS NOT NULL AND LEN(@HomeCountryIds) > 0
		INSERT INTO @tempHomeCountryIds (Id)  
		SELECT * from dbo.splittoint(@HomeCountryIds, ',')
	ELSE
		INSERT INTO @tempHomeCountryIds VALUES (-1)

	DECLARE @tempServiceIds TABLE (Id bigint)

	IF @ServiceIds IS NOT NULL AND LEN(@ServiceIds) > 0
		INSERT INTO @tempServiceIds (Id)  
		SELECT * from dbo.splittoint(@ServiceIds, ',')
	ELSE
		INSERT INTO @tempServiceIds VALUES (-1)

	DECLARE @tempLocation TABLE(Id INT)
	IF @LocationIds IS NOT NULL AND LEN(@LocationIds) > 0
		INSERT INTO @tempLocation (Id)  
		SELECT * from dbo.splittoint(@LocationIds, ',')
	ELSE
		INSERT INTO @tempLocation VALUES (-1)

	DECLARE @tempIndustryIds TABLE (Id bigint)
	IF @IndustryIds IS NOT NULL AND LEN(@IndustryIds) > 0
		INSERT INTO @tempIndustryIds (Id)  
		SELECT * from dbo.splittoint(@IndustryIds, ',')
	ELSE
		INSERT INTO @tempIndustryIds VALUES (-1)

	CREATE TABLE #tempFilteredCompanies (I_Companies_Id BIGINT NULL, I_EngagementTypes_Id BIGINT NULL, G_Services_Id BIGINT NULL, G_Organizations_Id BIGINT NULL, I_PortfoliosG_Organizations_Id BIGINT NULL, Isin VARCHAR(50) NULL)
	CREATE TABLE #tempFilteredCaseReport (I_GesCaseReports_Id BIGINT NULL, I_GesCompanies_Id BIGINT NULL, I_NormAreas_Id BIGINT NULL, ReportIncident NVARCHAR(255) NULL, ShowInClient BIT NULL, NewI_GesCaseReportStatuses_Id BIGINT NULL)

	INSERT INTO #tempFilteredCaseReport
	SELECT DISTINCT gcr.I_GesCaseReports_Id, gcr.I_GesCompanies_Id, gcr.I_NormAreas_Id, gcr.ReportIncident, gcr.ShowInClient, gcr.NewI_GesCaseReportStatuses_Id
	FROM dbo.I_GesCaseReports AS gcr
	INNER JOIN @tempRecommendationIds AS xr ON xr.Id = -1 OR gcr.NewI_GesCaseReportStatuses_Id = xr.Id
	INNER JOIN @tempConclusionIds AS xc ON xc.Id = -1 OR gcr.I_GesCaseReportStatuses_Id = xc.Id
	INNER JOIN @tempProgressIds AS xp ON xp.Id = -1 OR gcr.I_ProgressStatuses_Id = xp.Id 
	INNER JOIN @tempResponseIds AS xrp ON xrp.Id = -1 OR gcr.I_ResponseStatuses_Id = xrp.Id
	INNER JOIN @tempLocation AS xl ON xl.Id = -1 OR gcr.LocationG_Countries_Id = xl.Id
	WHERE gcr.ShowInClient = 1 AND (@IsHideClosedCases = 0 
			OR (gcr.NewI_GesCaseReportStatuses_Id <> 3 AND gcr.NewI_GesCaseReportStatuses_Id <> 9 AND gcr.NewI_GesCaseReportStatuses_Id <> 10))

	IF @PortfoliosOrganizationIds IS NOT NULL AND LEN(@PortfoliosOrganizationIds) > 0
	BEGIN
		INSERT INTO #tempFilteredCompanies
		SELECT CASE WHEN c.MasterI_Companies_Id IS NOT NULL THEN c.MasterI_Companies_Id ELSE c.I_Companies_Id END AS  I_Companies_Id,
				et.I_EngagementTypes_Id, pos.G_Services_Id, po.G_Organizations_Id, po.I_PortfoliosG_Organizations_Id, c.Isin		
		FROM dbo.I_Companies AS c
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON pc.I_Companies_Id = c.I_Companies_Id
		INNER JOIN dbo.I_PortfoliosG_Organizations AS po ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_PortfoliosG_OrganizationsG_Services AS pos ON pos.I_PortfoliosG_Organizations_Id = po.I_PortfoliosG_Organizations_Id
		INNER JOIN dbo.G_OrganizationsG_Services AS gs ON gs.G_Services_Id = pos.G_Services_Id
		INNER JOIN dbo.G_Services AS g ON g.G_Services_Id = pos.G_Services_Id
		INNER JOIN dbo.I_EngagementTypes AS et ON et.I_EngagementTypes_Id = g.I_EngagementTypes_Id
		INNER JOIN (SELECT temp.value AS Id from dbo.splittoint(@PortfoliosOrganizationIds, ',') AS temp) AS xp ON po.I_PortfoliosG_Organizations_Id = xp.Id
		
		WHERE po.G_Organizations_Id = @orgId AND gs.G_Organizations_Id = @orgId
		AND (@Isin IS NULL OR c.Isin LIKE '%' + @Isin + '%')
	END
	ELSE
		INSERT INTO #tempFilteredCompanies
		SELECT CASE WHEN c.MasterI_Companies_Id IS NOT NULL THEN c.MasterI_Companies_Id ELSE c.I_Companies_Id END AS  I_Companies_Id,
				et.I_EngagementTypes_Id, pos.G_Services_Id, po.G_Organizations_Id, po.I_PortfoliosG_Organizations_Id, c.Isin		
		FROM dbo.I_Companies AS c
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON pc.I_Companies_Id = c.I_Companies_Id
		INNER JOIN dbo.I_PortfoliosG_Organizations AS po ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_PortfoliosG_OrganizationsG_Services AS pos ON pos.I_PortfoliosG_Organizations_Id = po.I_PortfoliosG_Organizations_Id
		INNER JOIN dbo.G_OrganizationsG_Services AS gs ON gs.G_Services_Id = pos.G_Services_Id
		INNER JOIN dbo.G_Services AS g ON g.G_Services_Id = pos.G_Services_Id
		INNER JOIN dbo.I_EngagementTypes AS et ON et.I_EngagementTypes_Id = g.I_EngagementTypes_Id
		
		WHERE po.G_Organizations_Id = @orgId AND gs.G_Organizations_Id = @orgId
		AND (@Isin IS NULL OR c.Isin LIKE '%' + @Isin + '%')

	CREATE TABLE #tempMainQuery (Id BIGINT, CompanyId BIGINT, CompanyIssueName NVARCHAR(255),CompanyName NVARCHAR(255), HomeCountry NVARCHAR(255), Isin VARCHAR(50), Sedol VARCHAR(50), MarketCap DECIMAL, NumCases INT NULL, NumAlerts INT NULL, IsInFocusList INT NULL, CountryIncG_Countries_Id BIGINT NULL, I_Msci_Id BIGINT NULL)

	INSERT INTO #tempMainQuery
	SELECT DISTINCT
			rp.I_GesCompanies_Id,
			c.I_Companies_Id,
			c.NameCoalesce,
			c.NameCoalesce CompanyName,
			NULL,
			c.Isin,
			c.Sedol,
			c.MarketCap,
			0, 0, 0, c.CountryIncG_Countries_Id, c.I_Msci_Id
	FROM dbo.I_Companies AS c
	INNER JOIN dbo.I_GesCompanies AS gc ON gc.I_Companies_Id = c.I_Companies_Id
	INNER JOIN (
		SELECT DISTINCT tc.I_GesCompanies_Id,
						cret.I_EngagementTypes_Id 
		FROM dbo.G_OrganizationsG_Services AS os
		INNER JOIN dbo.G_Services AS s ON s.G_Services_Id = os.G_Services_Id
		INNER JOIN dbo.I_GesCaseReportsI_EngagementTypes AS cret ON cret.I_EngagementTypes_Id = s.I_EngagementTypes_Id
		INNER JOIN #tempFilteredCaseReport tc ON tc.I_GesCaseReports_Id = cret.I_GesCaseReports_Id
		INNER JOIN dbo.I_GesCompanies gc ON gc.I_GesCompanies_Id = tc.I_GesCompanies_Id
		INNER JOIN dbo.I_Companies c ON c.I_Companies_Id = gc.I_Companies_Id
		LEFT JOIN dbo.I_GesCaseReportsExtra as grce ON grce.I_GesCaseReports_Id = tc.I_GesCaseReports_Id
		INNER JOIN @tempServiceIds AS xs ON xs.Id = -1 OR s.G_Services_Id = xs.Id
		WHERE os.G_Organizations_Id = @orgId
		AND tc.ReportIncident != 'Temporary Dialogue Case'
		AND (@normId IS NULL OR tc.I_NormAreas_Id = @normId)
		AND (@engagementTypeId IS NULL OR cret.I_EngagementTypes_Id = @engagementTypeId)
		AND tc.ShowInClient = 1
		AND (@CompanyIssueName IS NULL OR c.NameCoalesce LIKE '%' + @CompanyIssueName + '%' OR tc.ReportIncident LIKE '%' + @CompanyIssueName + '%' OR grce.Keywords LIKE '%' + @CompanyIssueName + '%' )
		) AS rp ON gc.I_GesCompanies_Id = rp.I_GesCompanies_Id
	INNER JOIN (SELECT tp.I_Companies_Id, tp.I_EngagementTypes_Id FROM #tempFilteredCompanies AS tp) AS c1 ON c1.I_Companies_Id = c.I_Companies_Id AND c1.I_EngagementTypes_Id = rp.I_EngagementTypes_Id
	WHERE c.ShowInClient  = 1
	AND c.NameCoalesce IS NOT NULL 
	AND (c.MasterI_Companies_Id IS NULL OR c.MasterI_Companies_Id = c.I_Companies_Id)

	DECLARE @MainQuery TABLE (Id IId, CompanyId IId, CompanyIssueName Name,CompanyName Name, HomeCountry NVARCHAR(255), Isin VARCHAR(50), Sedol VARCHAR(50), MarketCap DECIMAL, NumCases INT NULL, NumAlerts INT NULL, IsInFocusList INT NULL, CountryIncG_Countries_Id IId NULL, I_Msci_Id IId NULL)

	INSERT INTO @MainQuery
	SELECT m.*
	FROM #tempMainQuery m
	LEFT JOIN dbo.I_Msci AS ms ON ms.I_Msci_Id= m.I_Msci_Id
	JOIN @tempIndustryIds AS ti ON ti.Id = -1 OR ms.ParentI_Msci_Id = ti.Id
	JOIN @tempHomeCountryIds AS xh ON xh.Id = -1 OR m.CountryIncG_Countries_Id = xh.Id

	UPDATE @MainQuery
	SET HomeCountry = hc.Name
	FROM @MainQuery m
	JOIN dbo.G_Countries AS hc ON hc.G_Countries_Id = m.CountryIncG_Countries_Id

	UPDATE @MainQuery
	SET NumCases = cr.NumberOfCases
	FROM @MainQuery AS m
	JOIN(
		SELECT tc.I_GesCompanies_Id, COUNT(DISTINCT tc.I_GesCaseReports_Id) AS NumberOfCases 
		FROM #tempFilteredCaseReport tc 
		INNER JOIN dbo.I_GesCaseReportsI_EngagementTypes AS get ON get.I_GesCaseReports_Id = tc.I_GesCaseReports_Id
		INNER JOIN dbo.G_Services AS b ON b.I_EngagementTypes_Id = get.I_EngagementTypes_Id
		INNER JOIN dbo.G_OrganizationsG_Services AS os ON os.G_Services_Id = b.G_Services_Id
		LEFT JOIN dbo.I_EngagementTypes AS et ON et.I_EngagementTypes_Id = b.I_EngagementTypes_Id
		INNER JOIN	(
			SELECT rp.I_GesCaseReports_Id, tp.I_EngagementTypes_Id 
			FROM #tempFilteredCompanies AS tp
			INNER JOIN dbo.I_GesCompanies AS gc ON gc.I_Companies_Id = tp.I_Companies_Id	
			INNER JOIN dbo.I_GesCaseReports AS rp ON rp.I_GesCompanies_Id = gc.I_GesCompanies_Id
			WHERE rp.ShowInClient = 1 
		) AS ft ON ft.I_GesCaseReports_Id = tc.I_GesCaseReports_Id AND ft.I_EngagementTypes_Id = et.I_EngagementTypes_Id
	
		INNER JOIN @tempServiceIds AS xs ON xs.Id = -1 OR b.G_Services_Id = xs.Id

		WHERE os.G_Organizations_Id = @orgId AND tc.ShowInClient = 1
		AND (@normId IS NULL OR tc.I_NormAreas_Id = @normId)
		AND (@engagementTypeId IS NULL OR get.I_EngagementTypes_Id = @engagementTypeId)
		AND (@IsHideClosedCases = 0 
			OR (tc.NewI_GesCaseReportStatuses_Id <> 3 AND tc.NewI_GesCaseReportStatuses_Id <> 9 AND tc.NewI_GesCaseReportStatuses_Id <> 10))
		 GROUP BY tc.I_GesCompanies_Id
	) AS cr ON m.Id = cr.I_GesCompanies_Id
 
	UPDATE @MainQuery
	SET IsInFocusList = ci.IsInFocus
	FROM @MainQuery m
	JOIN(
		SELECT CASE WHEN cw.I_GesCompanies_Id IS NOT NULL THEN cw.I_GesCompanies_Id ELSE cr.I_GesCompanies_Id END AS Id,
		CASE WHEN cw.IsInFocusList IS NOT NULL THEN cw.IsInFocusList ELSE cr.IsInFocusList END AS IsInFocus
		FROM (
			SELECT I_GesCompanies_Id, 1 AS IsInFocusList FROM dbo.I_GesCompanyWatcher
		WHERE G_Individuals_Id = @individualId
		) AS cw
		FULL JOIN (
			SELECT DISTINCT r.I_GesCompanies_Id, -1 AS IsInFocusList FROM dbo.I_GesCaseReports AS r
			INNER JOIN dbo.I_GesCaseReportsG_Individuals AS ri ON ri.I_GesCaseReports_Id = r.I_GesCaseReports_Id AND ri.G_Individuals_Id = @individualId
			INNER JOIN #tempFilteredCaseReport tc ON tc.I_GesCaseReports_Id = r.I_GesCaseReports_Id
			WHERE ri.G_Individuals_Id = @individualId and (@normId IS NULL OR r.I_NormAreas_Id = @normId)
		)AS cr ON cw.I_GesCompanies_Id = cr.I_GesCompanies_Id
	) AS ci ON m.Id = ci.Id

	UPDATE @MainQuery
	SET NumAlerts = na.NumAlerts
	FROM @MainQuery m
	JOIN(
		SELECT I_Companies_Id, COUNT(*) AS NumAlerts FROM dbo.I_NaArticles
		GROUP BY I_Companies_Id
	) AS na ON m.CompanyId = na.I_Companies_Id

	DROP TABLE #tempFilteredCompanies
	DROP TABLE #tempFilteredCaseReport
	DROP TABLE #tempMainQuery

	SELECT * FROM @MainQuery

END


GO
-- Add indexes

IF NOT EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_I_Companies_NameCoalesce')   
     CREATE NONCLUSTERED INDEX IX_I_Companies_NameCoalesce   
    ON dbo.I_Companies (NameCoalesce);
GO  

IF NOT EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_I_Companies_Isin')   
     CREATE NONCLUSTERED INDEX IX_I_Companies_Isin   
    ON dbo.I_Companies (Isin);
GO  

IF NOT EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_I_GesCaseReports_ReportIncident')   
     CREATE NONCLUSTERED INDEX IX_I_GesCaseReports_ReportIncident  
    ON dbo.I_GesCaseReports (ReportIncident);
GO


--Normal Search Store procedure

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CompanyNormalSearch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CompanyNormalSearch]
GO
CREATE PROCEDURE [dbo].[CompanyNormalSearch]
	@CompanyIssueName NVARCHAR(255),
	@OrgId BIGINT,
	@IndividualId BIGINT,
	@IsHideClosedCases BIT,
	@PortfolioIds NVARCHAR(Max),
	@Isin NVARCHAR(50),
	@HomeCountryIds NVARCHAR(MAX)
AS
BEGIN
	DECLARE @MainQuery TABLE (Id IId NULL, CompanyId IId, CompanyIssueName Name,CompanyName Name, HomeCountry NVARCHAR(255), Isin VARCHAR(50), Sedol VARCHAR(50), MsciIndustry Name, MarketCap DECIMAL, NumCases INT NULL, NumAlerts INT NULL, IsInFocusList INT NULL, CountryIncG_Countries_Id IId NULL, I_Msci_Id IId NULL)

	DECLARE @tempCompanyIds TABLE(Id IID)

	CREATE TABLE #tempFilteredCompanies (I_Companies_Id BIGINT NULL, G_Services_Id BIGINT NULL, G_Organizations_Id BIGINT NULL, I_PortfoliosG_Organizations_Id BIGINT NULL, Isin VARCHAR(50) NULL)

	IF @PortfolioIds IS NOT NULL AND LEN(@PortfolioIds) > 0
	BEGIN
		INSERT INTO #tempFilteredCompanies
		SELECT CASE WHEN c.MasterI_Companies_Id IS NOT NULL THEN c.MasterI_Companies_Id ELSE c.I_Companies_Id END AS I_Companies_Id, pos.G_Services_Id, po.G_Organizations_Id, po.I_PortfoliosG_Organizations_Id, c.Isin
		FROM I_PortfoliosG_Organizations po 
		INNER JOIN dbo.I_PortfoliosG_OrganizationsG_Services AS pos ON pos.I_PortfoliosG_Organizations_Id = po.I_PortfoliosG_Organizations_Id
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_Companies AS c ON pc.I_Companies_Id = c.I_Companies_Id
		INNER JOIN (SELECT temp.value AS Id from dbo.splittoint(@PortfolioIds, ',') AS temp) AS xp ON po.I_PortfoliosG_Organizations_Id = xp.Id
		WHERE po.G_Organizations_Id = @orgId AND (@Isin IS NULL OR c.Isin LIKE '%' + @Isin + '%')
	END
	ELSE
		INSERT INTO #tempFilteredCompanies
		SELECT CASE WHEN c.MasterI_Companies_Id IS NOT NULL THEN c.MasterI_Companies_Id ELSE c.I_Companies_Id END AS I_Companies_Id, pos.G_Services_Id, po.G_Organizations_Id, po.I_PortfoliosG_Organizations_Id, c.Isin
		FROM I_PortfoliosG_Organizations po 
		INNER JOIN dbo.I_PortfoliosG_OrganizationsG_Services AS pos ON pos.I_PortfoliosG_Organizations_Id = po.I_PortfoliosG_Organizations_Id
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_Companies AS c ON pc.I_Companies_Id = c.I_Companies_Id
		WHERE po.G_Organizations_Id = @orgId AND (@Isin IS NULL OR c.Isin LIKE '%' + @Isin + '%')

	IF @CompanyIssueName IS NOT NULL AND LEN(@CompanyIssueName) > 0
	BEGIN
		INSERT INTO @tempCompanyIds
		SELECT DISTINCT grc.I_GesCompanies_Id
		FROM dbo.G_OrganizationsG_Services AS os
		INNER JOIN dbo.G_Services AS s ON s.G_Services_Id = os.G_Services_Id
		INNER JOIN dbo.I_GesCaseReportsI_EngagementTypes AS cret ON cret.I_EngagementTypes_Id = s.I_EngagementTypes_Id
		INNER JOIN dbo.I_GesCaseReports AS grc ON grc.I_GesCaseReports_Id = cret.I_GesCaseReports_Id
		INNER JOIN dbo.I_GesCompanies gc ON gc.I_GesCompanies_Id = grc.I_GesCompanies_Id
		INNER JOIN dbo.I_Companies c ON c.I_Companies_Id = gc.I_Companies_Id
		LEFT JOIN dbo.I_GesCaseReportsExtra as grce ON grce.I_GesCaseReports_Id = grc.I_GesCaseReports_Id
		WHERE os.G_Organizations_Id = @orgId
		AND grc.ReportIncident != 'Temporary Dialogue Case'
		AND grc.ShowInClient = 1
		AND (@CompanyIssueName IS NULL OR grc.ReportIncident LIKE '%' + @CompanyIssueName + '%' OR grce.Keywords LIKE '%' + @CompanyIssueName + '%')
	END

	INSERT INTO @MainQuery
	SELECT DISTINCT
			gc.I_GesCompanies_Id,
			c.I_Companies_Id,
			LTRIM(RTRIM(c.NameCoalesce)),
			LTRIM(RTRIM(c.NameCoalesce)) CompanyName,
			NULL,
			c.Isin,
			c.Sedol,
			NULL,
			c.MarketCap,
			0, 0, 0, c.CountryIncG_Countries_Id, c.I_Msci_Id
	FROM dbo.I_Companies AS c
	INNER JOIN dbo.I_GesCompanies AS gc ON gc.I_Companies_Id = c.I_Companies_Id

	--//get master company if company is child
	INNER JOIN (
		SELECT DISTINCT tp.I_Companies_Id		
			FROM #tempFilteredCompanies AS tp
	) AS c1 ON c1.I_Companies_Id = c.I_Companies_Id
	WHERE c.ShowInClient  = 1
	AND c.NameCoalesce IS NOT NULL 
	AND (c.MasterI_Companies_Id IS NULL OR c.MasterI_Companies_Id = c.I_Companies_Id)
	AND (@CompanyIssueName IS NULL OR c.NameCoalesce LIKE '%' + @CompanyIssueName + '%' OR gc.I_GesCompanies_Id IN (SELECT * FROM @tempCompanyIds))

	UPDATE @MainQuery
	SET HomeCountry = hc.Name
	FROM @MainQuery m
	JOIN dbo.G_Countries AS hc ON hc.G_Countries_Id = m.CountryIncG_Countries_Id

	UPDATE @MainQuery
	SET MsciIndustry = ms.Name
	FROM @MainQuery m
	JOIN dbo.I_Msci AS ms ON ms.I_Msci_Id= m.I_Msci_Id

	UPDATE @MainQuery
	SET NumCases = cr.NumberOfCases
	FROM @MainQuery AS m
	JOIN(
		SELECT gcr.I_GesCompanies_Id, COUNT(DISTINCT gcr.I_GesCaseReports_Id) AS NumberOfCases 
			FROM dbo.I_GesCaseReports AS gcr
			INNER JOIN dbo.I_GesCaseReportsI_EngagementTypes AS get ON get.I_GesCaseReports_Id = gcr.I_GesCaseReports_Id
			INNER JOIN dbo.G_Services AS b ON b.I_EngagementTypes_Id = get.I_EngagementTypes_Id
			INNER JOIN dbo.G_OrganizationsG_Services AS os ON os.G_Services_Id = b.G_Services_Id
			LEFT JOIN dbo.I_EngagementTypes AS et ON et.I_EngagementTypes_Id = b.I_EngagementTypes_Id
			INNER JOIN (
				SELECT rp.I_GesCaseReports_Id, et.I_EngagementTypes_Id, tp.G_Organizations_Id 
				FROM #tempFilteredCompanies AS tp
				INNER JOIN dbo.G_Services AS g ON g.G_Services_Id = tp.G_Services_Id
				INNER JOIN dbo.I_EngagementTypes AS et ON et.I_EngagementTypes_Id = g.I_EngagementTypes_Id
				INNER JOIN dbo.I_GesCompanies AS gc ON gc.I_Companies_Id = tp.I_Companies_Id
				INNER JOIN dbo.I_GesCaseReports AS rp ON rp.I_GesCompanies_Id = gc.I_GesCompanies_Id
				WHERE rp.ShowInClient = 1
			) AS ft ON ft.I_GesCaseReports_Id = gcr.I_GesCaseReports_Id AND ft.I_EngagementTypes_Id = et.I_EngagementTypes_Id

			WHERE (gcr.[ShowInClient] = 1) 
				AND (@IsHideClosedCases = 0 
					OR (gcr.NewI_GesCaseReportStatuses_Id <> 3 AND gcr.NewI_GesCaseReportStatuses_Id <> 9 AND gcr.NewI_GesCaseReportStatuses_Id <> 10)) 
				AND (ft.[G_Organizations_Id] = @OrgId) AND (os.[G_Organizations_Id] = @OrgId)

		GROUP BY gcr.I_GesCompanies_Id
	) AS cr ON m.Id = cr.I_GesCompanies_Id

	UPDATE @MainQuery
	SET IsInFocusList = ci.IsInFocus 
	FROM @MainQuery m
	JOIN
	(
		SELECT CASE WHEN cw.I_GesCompanies_Id IS NOT NULL THEN cw.I_GesCompanies_Id ELSE cr.I_GesCompanies_Id END AS Id,
		CASE WHEN cw.IsInFocusList IS NOT NULL THEN cw.IsInFocusList ELSE cr.IsInFocusList END AS IsInFocus
		FROM (SELECT I_GesCompanies_Id, 1 AS IsInFocusList FROM dbo.I_GesCompanyWatcher
		WHERE G_Individuals_Id = @individualId) AS cw
		FULL JOIN (SELECT DISTINCT r.I_GesCompanies_Id, -1 AS IsInFocusList FROM dbo.I_GesCaseReports AS r
			INNER JOIN dbo.I_GesCaseReportsG_Individuals AS ri ON ri.I_GesCaseReports_Id = r.I_GesCaseReports_Id AND ri.G_Individuals_Id = @individualId
			WHERE ri.G_Individuals_Id = @individualId
		) AS cr ON cw.I_GesCompanies_Id = cr.I_GesCompanies_Id
	) AS ci ON m.Id = ci.Id

	--Number of alerts
	UPDATE @MainQuery
	SET NumAlerts = na.NumAlerts
	FROM @MainQuery m
	JOIN(
		SELECT I_Companies_Id, COUNT(*) AS NumAlerts FROM dbo.I_NaArticles
		GROUP BY I_Companies_Id
	) AS na ON m.CompanyId = na.I_Companies_Id

	IF @HomeCountryIds IS NOT NULL AND LEN(@HomeCountryIds) > 0
	BEGIN
		SELECT * 
		FROM @MainQuery AS m
		JOIN (SELECT temp.value AS Id from dbo.splittoint(@HomeCountryIds, ',') AS temp) AS xp ON m.CountryIncG_Countries_Id = xp.Id
	END
	ELSE
		SELECT * FROM @MainQuery

	DROP TABLE #tempFilteredCompanies
END