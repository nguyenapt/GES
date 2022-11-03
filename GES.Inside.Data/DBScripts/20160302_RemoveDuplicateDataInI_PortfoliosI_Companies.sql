
DELETE FROM dbo.I_PortfoliosI_Companies 
WHERE I_PortfoliosI_Companies_Id NOT IN
 (SELECT MIN(I_PortfoliosI_Companies_Id) 
	FROM dbo.I_PortfoliosI_Companies 
	GROUP BY I_Portfolios_Id, I_Companies_Id) 
