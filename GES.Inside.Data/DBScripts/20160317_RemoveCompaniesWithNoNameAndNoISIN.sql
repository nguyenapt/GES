-- GES-73
-- remove companies with no name and no isin
delete I_Companies
where Isin is null and NameCoalesce is null


-- GES-79
-- delete records from I_PortfoliosI_Companies where companies have no name and duplicated Isin (~38 companies - 28.08.2015)
delete I_PortfoliosI_Companies
where I_Companies_Id in(
select I_Companies_Id from I_Companies
where Isin is not null 
and NameCoalesce is null 
and Sedol is null)

GO
-- delete Companies with duplicate Isin and no Name (~38 companies - 28.08.2015)
delete I_Companies
where Isin is not null 
and NameCoalesce is null 
and Sedol is null


