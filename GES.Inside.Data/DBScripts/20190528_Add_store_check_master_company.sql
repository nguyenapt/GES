CREATE PROCEDURE [dbo].[SearchCheckMasterCompany]
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
			AND po.G_Organizations_Id = @orgId AND ((@Isin IS NULL OR c.Isin LIKE '' + @Isin + '') OR (@CompanyIssueName IS NULL OR c.Name = @CompanyIssueName))
			AND po.I_PortfoliosG_Organizations_Id IN (SELECT p.Id FROM @tempPortFolioIds p)
			AND c.ShowInClient  = 1
	END
	ELSE BEGIN
		INSERT INTO @MainQuery
		SELECT DISTINCT c.MasterI_Companies_Id, c.I_Companies_Id, c.Name
		FROM I_PortfoliosG_Organizations po 
		INNER JOIN dbo.I_PortfoliosI_Companies AS pc ON po.I_Portfolios_Id = pc.I_Portfolios_Id
		INNER JOIN dbo.I_Companies AS c ON pc.I_Companies_Id = c.I_Companies_Id
		WHERE po.G_Organizations_Id = @orgId AND ((@Isin IS NULL OR c.Isin LIKE '' + @Isin + '') OR (@CompanyIssueName IS NULL OR c.Name = @CompanyIssueName))
			AND c.ShowInClient  = 1
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
