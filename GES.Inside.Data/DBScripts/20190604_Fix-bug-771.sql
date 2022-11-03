
/****** Object:  StoredProcedure [dbo].[CompanyNormalSearch]    Script Date: 6/4/2019 4:12:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CompanyNormalSearch]
	@CompanyIssueName NVARCHAR(255),
	@OrgId BIGINT,
	@IndividualId BIGINT,
	@IsHideClosedCases BIT,
	@PortfolioIds VARCHAR(Max),
	@Isin VARCHAR(50),
	@HomeCountryIds VARCHAR(MAX),
	@IndustryIds VARCHAR(max)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @MainQuery TABLE (Id IId NULL, CompanyId IId, CompanyIssueName Name,CompanyName Name, HomeCountry NVARCHAR(255), Isin VARCHAR(50), Sedol VARCHAR(50), MsciIndustry Name, MarketCap DECIMAL, NumCases INT NULL, NumAlerts INT NULL, IsInFocusList INT NULL, CountryId UNIQUEIDENTIFIER NULL, SubPeerGroupId int NULL, SustainalyticsID int NULL)

	DECLARE @tempCompanyIds TABLE(Id IID)
	CREATE TABLE #tempFilteredCompanies (I_Companies_Id BIGINT NULL)
	
	IF @PortfolioIds IS NOT NULL AND LEN(@PortfolioIds) > 0 BEGIN
		DECLARE @tempPortFolioIds TABLE(Id IId PRIMARY KEY)
		INSERT INTO @tempPortFolioIds
		SELECT temp.value AS Id from dbo.splittoint(@PortfolioIds, ',') AS temp

		INSERT INTO #tempFilteredCompanies
		SELECT CASE WHEN c.MasterI_Companies_Id IS NOT NULL THEN c.MasterI_Companies_Id ELSE c.I_Companies_Id END AS I_Companies_Id
		FROM I_PortfoliosG_Organizations po 
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_Companies AS c ON pc.I_Companies_Id = c.I_Companies_Id
		WHERE po.G_Organizations_Id = @orgId 
			AND (@Isin IS NULL OR c.Isin LIKE '%' + @Isin + '%') 
			AND (@CompanyIssueName IS NULL OR c.Name LIKE '%' + @CompanyIssueName + '%' OR c.Isin LIKE '%' + @CompanyIssueName + '%')
			AND po.I_PortfoliosG_Organizations_Id IN (SELECT p.Id FROM @tempPortFolioIds p)
			AND c.ShowInClient  = 1
			AND c.Id >= 1000000000 
	END
	ELSE BEGIN
		INSERT INTO #tempFilteredCompanies
		SELECT CASE WHEN c.MasterI_Companies_Id IS NOT NULL THEN c.MasterI_Companies_Id ELSE c.I_Companies_Id END AS I_Companies_Id
		FROM I_PortfoliosG_Organizations po 
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_Companies AS c ON pc.I_Companies_Id = c.I_Companies_Id
		WHERE po.G_Organizations_Id = @orgId 
			AND (@Isin IS NULL OR c.Isin LIKE '%' + @Isin + '%') 
			AND (@CompanyIssueName IS NULL OR c.Name LIKE '%' + @CompanyIssueName + '%' OR c.Isin LIKE '%' + @CompanyIssueName + '%')
			AND c.ShowInClient  = 1
			AND c.Id >= 1000000000
			AND EXISTS (SELECT * FROM I_PortfoliosG_Organizations x
						INNER JOIN I_PortfoliosG_OrganizationsG_Services y ON y.I_PortfoliosG_Organizations_Id = x.I_PortfoliosG_Organizations_Id
						INNER JOIN G_OrganizationsG_Services z ON z.G_Services_Id = y.G_Services_Id
						INNER	JOIN G_Services s ON s.G_Services_Id = z.G_Services_Id
						WHERE x.I_PortfoliosG_Organizations_Id = po.I_PortfoliosG_Organizations_Id AND (s.I_EngagementTypes_Id IS NOT NULL OR s.G_Services_Id = 20)
			)
	END

	IF @CompanyIssueName IS NOT NULL AND LEN(@CompanyIssueName) > 0
	BEGIN
		INSERT INTO @tempCompanyIds
		SELECT DISTINCT grc.I_GesCompanies_Id
		FROM dbo.I_GesCaseReports AS grc
		INNER JOIN dbo.I_GesCompanies gc ON gc.I_GesCompanies_Id = grc.I_GesCompanies_Id
		INNER JOIN dbo.I_Companies c ON c.I_Companies_Id = gc.I_Companies_Id
		LEFT JOIN dbo.I_GesCaseReportsExtra as grce ON grce.I_GesCaseReports_Id = grc.I_GesCaseReports_Id
		WHERE 
		grc.ReportIncident != 'Temporary Dialogue Case'
		AND grc.ShowInClient = 1
		AND c.Id >= 1000000000
		AND (grc.ReportIncident LIKE '%' + @CompanyIssueName + '%' OR grce.Keywords LIKE '%' + @CompanyIssueName + '%')
	END

	INSERT INTO @MainQuery
	SELECT DISTINCT
			gc.I_GesCompanies_Id,
			c.I_Companies_Id,
			LTRIM(RTRIM(c.Name)),
			LTRIM(RTRIM(c.Name)) CompanyName,
			NULL,
			c.Isin,
			c.Sedol,
			NULL,
			c.MarketCap,
			0, 0, 0, c.CountryOfIncorporationId, c.SubPeerGroupId, c.Id
	FROM dbo.I_Companies AS c
	INNER JOIN dbo.I_GesCompanies AS gc ON gc.I_Companies_Id = c.I_Companies_Id
	--//get master company if company is child
	--INNER JOIN (
	--		SELECT DISTINCT tp.I_Companies_Id
	--		FROM #tempFilteredCompanies AS tp
	--) AS c1 ON c1.I_Companies_Id = c.I_Companies_Id
	WHERE c.ShowInClient  = 1
	AND c.Id >= 1000000000
	AND c.Name IS NOT NULL 
	AND (c.MasterI_Companies_Id IS NULL OR c.MasterI_Companies_Id = c.I_Companies_Id) 
	--AND (@CompanyIssueName IS NULL OR c.Name LIKE '%' + @CompanyIssueName + '%' OR c.Isin = @CompanyIssueName OR gc.I_GesCompanies_Id IN (SELECT * FROM @tempCompanyIds))

	AND	( c.I_Companies_Id IN  (SELECT I_Companies_Id FROM #tempFilteredCompanies) 
	 OR gc.I_GesCompanies_Id IN (SELECT * FROM @tempCompanyIds) )

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

	UPDATE @MainQuery
	SET NumCases = cr.NumberOfCases
	FROM @MainQuery AS m
	JOIN(
		SELECT gcr.I_GesCompanies_Id, COUNT(DISTINCT gcr.I_GesCaseReports_Id) AS NumberOfCases 
			FROM dbo.I_GesCaseReports AS gcr
			WHERE (gcr.[ShowInClient] = 1) 
				AND (@IsHideClosedCases = 0 						
					OR (gcr.NewI_GesCaseReportStatuses_Id <> 3 AND gcr.NewI_GesCaseReportStatuses_Id <> 9 AND gcr.NewI_GesCaseReportStatuses_Id <> 10)) 
				AND (gcr.I_NormAreas_Id IS NULL or gcr.I_NormAreas_Id <> 6)
		GROUP BY gcr.I_GesCompanies_Id
	) AS cr ON m.Id = cr.I_GesCompanies_Id

	UPDATE @MainQuery
	SET HomeCountry = hc.Name
	FROM @MainQuery m
	JOIN dbo.Countries AS hc ON hc.Id = m.CountryId

	UPDATE @MainQuery
	SET MsciIndustry = ms.Name
	FROM @MainQuery m
	JOIN dbo.SubPeerGroups AS ms ON ms.Id= m.SubPeerGroupId

	--Number of alerts
	UPDATE @MainQuery
	SET NumAlerts = na.NumAlerts
	FROM @MainQuery m
	JOIN(
		SELECT I_Companies_Id, COUNT(*) AS NumAlerts FROM dbo.I_NaArticles
		GROUP BY I_Companies_Id
	) AS na ON m.CompanyId = na.I_Companies_Id

	IF (@HomeCountryIds IS NOT NULL AND LEN(@HomeCountryIds) > 0) OR (@IndustryIds IS NOT NULL AND LEN(@IndustryIds) > 0)
	BEGIN
		DECLARE @tempIndustryIds TABLE (Id bigint)
		IF @IndustryIds IS NOT NULL AND LEN(@IndustryIds) > 0
			INSERT INTO @tempIndustryIds (Id)  
			SELECT * from dbo.splittoint(@IndustryIds, ',')
		ELSE
			INSERT INTO @tempIndustryIds VALUES (-1)

		DECLARE @tempHomeCountryIds TABLE (Id UNIQUEIDENTIFIER)
		IF @HomeCountryIds IS NOT NULL AND LEN(@HomeCountryIds) > 0
			INSERT INTO @tempHomeCountryIds (Id)  
			SELECT * from dbo.splitToGuid(@HomeCountryIds, ',')
		ELSE
			INSERT INTO @tempHomeCountryIds VALUES (null)

		SELECT m.* 
		FROM @MainQuery AS m
		JOIN @tempIndustryIds AS ti ON ti.Id = -1 OR m.SubPeerGroupId = ti.Id
		JOIN @tempHomeCountryIds AS hc ON hc.Id IS null OR m.CountryId = hc.Id
	END
	ELSE
		SELECT * FROM @MainQuery

	DROP TABLE #tempFilteredCompanies
END


GO


/****** Object:  StoredProcedure [dbo].[SearchCheckMasterCompany]    Script Date: 6/4/2019 6:12:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SearchCheckMasterCompany]
	@CompanyIssueName NVARCHAR(255),
	@OrgId BIGINT,	
	@PortfolioIds VARCHAR(Max),
	@Isin VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @MainQuery TABLE (MasterCompanyId IId, SubCompanyId IId, SubCompanyName Name)

	DECLARE @tempCompanyIds TABLE(Id IID)
	CREATE TABLE #tempFilteredCompanies (I_Companies_Id BIGINT NULL)
	
	IF @PortfolioIds IS NOT NULL AND LEN(@PortfolioIds) > 0 BEGIN
		DECLARE @tempPortFolioIds TABLE(Id IId PRIMARY KEY)
		INSERT INTO @tempPortFolioIds
		SELECT temp.value AS Id from dbo.splittoint(@PortfolioIds, ',') AS temp

		INSERT INTO @MainQuery
		SELECT DISTINCT c.MasterI_Companies_Id, c.I_Companies_Id, c.Name
		FROM I_PortfoliosG_Organizations po 
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_Companies AS c ON pc.I_Companies_Id = c.I_Companies_Id
		WHERE MasterI_Companies_Id IS NOT NULL AND c.MasterI_Companies_Id <> c.I_Companies_Id
			AND po.G_Organizations_Id = @orgId AND ((@Isin IS NULL OR c.Isin LIKE '' + @Isin + '') OR (@CompanyIssueName IS NULL OR c.Name LIKE '%' + @CompanyIssueName + '%'))
			AND po.I_PortfoliosG_Organizations_Id IN (SELECT p.Id FROM @tempPortFolioIds p)
			AND c.ShowInClient  = 1 AND c.Id >= 1000000000
	END
	ELSE BEGIN
		INSERT INTO @MainQuery
		SELECT DISTINCT c.MasterI_Companies_Id, c.I_Companies_Id, c.Name
		FROM I_PortfoliosG_Organizations po 
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_Companies AS c ON pc.I_Companies_Id = c.I_Companies_Id
		WHERE po.G_Organizations_Id = @orgId AND ((@Isin IS NULL OR c.Isin LIKE '' + @Isin + '') OR (@CompanyIssueName IS NULL OR c.Name LIKE '%' + @CompanyIssueName + '%'))
			AND c.ShowInClient  = 1 AND c.Id >= 1000000000
			AND MasterI_Companies_Id IS NOT NULL AND c.MasterI_Companies_Id <> c.I_Companies_Id
			AND EXISTS (SELECT * FROM I_PortfoliosG_Organizations x
						INNER JOIN I_PortfoliosG_OrganizationsG_Services y ON y.I_PortfoliosG_Organizations_Id = x.I_PortfoliosG_Organizations_Id
						INNER JOIN G_OrganizationsG_Services z ON z.G_Services_Id = y.G_Services_Id
						INNER	JOIN G_Services s ON s.G_Services_Id = z.G_Services_Id
						WHERE x.I_PortfoliosG_Organizations_Id = po.I_PortfoliosG_Organizations_Id AND (s.I_EngagementTypes_Id IS NOT NULL OR s.G_Services_Id = 20)
			)
	END

		SELECT * FROM @MainQuery
END