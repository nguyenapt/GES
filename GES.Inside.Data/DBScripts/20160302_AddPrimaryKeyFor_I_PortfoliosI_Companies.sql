
IF NOT EXISTS (SELECT * FROM information_schema.table_constraints  
				WHERE constraint_type = 'PRIMARY KEY'   
				AND table_name = 'I_PortfoliosI_Companies')
BEGIN
	ALTER TABLE dbo.I_PortfoliosI_Companies
	ADD CONSTRAINT PK_I_PortfoliosI_Companies PRIMARY KEY(I_PortfoliosI_Companies_Id)
END