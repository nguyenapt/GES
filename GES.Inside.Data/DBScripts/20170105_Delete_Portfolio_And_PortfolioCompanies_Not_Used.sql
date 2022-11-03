
delete I_Portfolios 
where StandardPortfolio = 0 AND I_Portfolios_Id not in (select I_Portfolios_Id from I_PortfoliosG_Organizations)

delete I_PortfoliosI_Companies
where I_Portfolios_Id not in (select I_Portfolios_Id from I_Portfolios)
