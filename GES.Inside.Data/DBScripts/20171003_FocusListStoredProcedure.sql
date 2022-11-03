
/****** Object:  StoredProcedure [dbo].[CompanyFocusList]    Script Date: 10/5/2017 11:54:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CompanyFocusList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CompanyFocusList]
GO
CREATE PROCEDURE [dbo].[CompanyFocusList]
	@OrgId BIGINT,
	@IndividualId BIGINT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @MainQuery TABLE (Id IId NULL, CompanyId IId, CompanyIssueName Name,CompanyName Name, HomeCountry NVARCHAR(255), Isin VARCHAR(50), Sedol VARCHAR(50), MsciIndustry Name, MarketCap DECIMAL, NumCases INT NULL, NumAlerts INT NULL, IsInFocusList INT NULL, CountryIncG_Countries_Id IId NULL, I_Msci_Id IId NULL)

	CREATE TABLE #tempFilteredCompanies (I_Companies_Id BIGINT NULL, G_Services_Id BIGINT NULL)
	
		INSERT INTO #tempFilteredCompanies
		SELECT CASE WHEN c.MasterI_Companies_Id IS NOT NULL THEN c.MasterI_Companies_Id ELSE c.I_Companies_Id END AS I_Companies_Id, pos.G_Services_Id
		FROM I_PortfoliosG_Organizations po 
		INNER JOIN dbo.I_PortfoliosG_OrganizationsG_Services AS pos ON pos.I_PortfoliosG_Organizations_Id = po.I_PortfoliosG_Organizations_Id
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_Companies AS c ON pc.I_Companies_Id = c.I_Companies_Id
		INNER JOIN dbo.I_GesCompanies gc ON gc.I_Companies_Id = CASE WHEN c.MasterI_Companies_Id IS NOT NULL THEN c.MasterI_Companies_Id ELSE c.I_Companies_Id END
		INNER JOIN (
			SELECT cw.I_GesCompanies_Id FROM dbo.I_GesCompanyWatcher cw
			WHERE cw.G_Individuals_Id = @IndividualId
			UNION ALL	
			SELECT gr.I_GesCompanies_Id FROM dbo.I_GesCaseReportsG_Individuals gi
			INNER JOIN dbo.I_GesCaseReports gr ON gr.I_GesCaseReports_Id = gi.I_GesCaseReports_Id
			WHERE gi.G_Individuals_Id = @IndividualId
		) AS tbl ON tbl.I_GesCompanies_Id = gc.I_GesCompanies_Id
		WHERE po.G_Organizations_Id = @orgId 
			AND c.ShowInClient  = 1

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

	UPDATE @MainQuery
	SET HomeCountry = hc.Name
	FROM @MainQuery m
	JOIN dbo.G_Countries AS hc ON hc.G_Countries_Id = m.CountryIncG_Countries_Id

	UPDATE @MainQuery
	SET MsciIndustry = ms.Name
	FROM @MainQuery m
	JOIN dbo.I_Msci AS ms ON ms.I_Msci_Id= m.I_Msci_Id

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
			INNER JOIN dbo.I_GesCaseReportsI_EngagementTypes AS gce ON gce.I_GesCaseReports_Id = gcr.I_GesCaseReports_Id
			INNER JOIN dbo.G_Services AS b ON b.I_EngagementTypes_Id = gce.I_EngagementTypes_Id
			INNER JOIN dbo.G_OrganizationsG_Services AS os ON os.G_Services_Id = b.G_Services_Id
			LEFT JOIN dbo.I_EngagementTypes AS et ON et.I_EngagementTypes_Id = b.I_EngagementTypes_Id
			INNER JOIN (
				SELECT rp.I_GesCaseReports_Id, et.I_EngagementTypes_Id
				FROM #tempFilteredCompanies AS tp
				INNER JOIN dbo.G_Services AS g ON g.G_Services_Id = tp.G_Services_Id
				INNER JOIN dbo.I_EngagementTypes AS et ON et.I_EngagementTypes_Id = g.I_EngagementTypes_Id
				INNER JOIN dbo.I_GesCompanies AS gc ON gc.I_Companies_Id = tp.I_Companies_Id
				INNER JOIN dbo.I_GesCaseReports AS rp ON rp.I_GesCompanies_Id = gc.I_GesCompanies_Id
				WHERE rp.ShowInClient = 1
			) AS ft ON ft.I_GesCaseReports_Id = gcr.I_GesCaseReports_Id AND ft.I_EngagementTypes_Id = et.I_EngagementTypes_Id
			
			LEFT JOIN dbo.I_GesCompanyWatcher cw ON cw.I_GesCompanies_Id = gcr.I_GesCompanies_Id AND cw.G_Individuals_Id = @IndividualId
			LEFT JOIN dbo.I_GesCaseReportsG_Individuals gi ON gi.I_GesCaseReports_Id = gcr.I_GesCaseReports_Id AND gi.G_Individuals_Id = @IndividualId
			WHERE (gcr.[ShowInClient] = 1) 
				AND (os.[G_Organizations_Id] = @OrgId)
				AND	(cw.I_GesCompanies_Id IS NOT NULL OR gi.I_GesCaseReports_Id IS NOT NULL)

		GROUP BY gcr.I_GesCompanies_Id

	) AS cr ON m.Id = cr.I_GesCompanies_Id

	--Number of alerts
	UPDATE @MainQuery
	SET NumAlerts = na.NumAlerts
	FROM @MainQuery m
	JOIN(
		SELECT I_Companies_Id, COUNT(*) AS NumAlerts FROM dbo.I_NaArticles
		GROUP BY I_Companies_Id
	) AS na ON m.CompanyId = na.I_Companies_Id
	
	SELECT * FROM @MainQuery
END
