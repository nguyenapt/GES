
CREATE TABLE [dbo].[IssueHeadingGroup](
	[Id] [dbo].[IId] NOT NULL,
	[Name] [NVARCHAR](255) NULL,
	[SortOrder] [INT] NULL,
 CONSTRAINT [PK_IssueHeadingGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[IssueHeading](
	[Id] [dbo].[IId] IDENTITY(1,1) NOT NULL,
	[Name] [NVARCHAR](MAX) NULL,
	[IssueHeadingGroupId] [dbo].[IId] NULL,
 CONSTRAINT [PK_IssueHeading] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[IssueHeading]  WITH CHECK ADD  CONSTRAINT [FK_IssueHeading_IssueHeadingGroup] FOREIGN KEY([IssueHeadingGroupId])
REFERENCES [dbo].[IssueHeadingGroup] ([Id])
GO

ALTER TABLE [dbo].[IssueHeading] CHECK CONSTRAINT [FK_IssueHeading_IssueHeadingGroup]
GO

INSERT INTO dbo.IssueHeadingGroup ( Id, Name, SortOrder ) VALUES  ( 1, N'LABOUR RIGHTS', 5 )
INSERT INTO dbo.IssueHeadingGroup ( Id, Name, SortOrder ) VALUES  ( 2, N'HUMAN RIGHTS', 10 )
INSERT INTO dbo.IssueHeadingGroup ( Id, Name, SortOrder ) VALUES  ( 3, N'CONSUMER RIGHTS', 15 )
INSERT INTO dbo.IssueHeadingGroup ( Id, Name, SortOrder ) VALUES  ( 4, N'ENVIRONMENT', 20 )
INSERT INTO dbo.IssueHeadingGroup ( Id, Name, SortOrder ) VALUES  ( 5, N'COMBINED ISSUES', 25 )
INSERT INTO dbo.IssueHeadingGroup ( Id, Name, SortOrder ) VALUES  ( 6, N'INHUMANE WEAPONS', 30 )
INSERT INTO dbo.IssueHeadingGroup ( Id, Name, SortOrder ) VALUES  ( 7, N'BUSINESS ETHICS', 35 )

GO

INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Anti-union practices', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Child labour', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Discrimination', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Exploitation of migrant workers', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Fatal workplace accident', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Forced labour', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Hazardous working conditions', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Labour rights violations at company operations', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Labour rights violations at contractors', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Labour rights violations at plantations', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Labour rights violations in supply chain', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Poor working conditions', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Recurring labour rights violations', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Recurring workplace accidents', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Retaliation', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Systemic labour rights violations', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Wage issues', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Workplace accident', 1 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Workplace harassment', 1 )

INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Activities resulting in adverse human rights impacts', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Activities resulting in negative health impacts', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Community protests', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Fatal accident', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Fatal accidents', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Financing of controversial activities', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Financing of controversial project', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Financing of illegal settlements in occupied territories', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Forced eviction', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Human rights impacts of surveillance systems', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Illegal exploitation of natural resources', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Land grabbing', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Operations in occupied territories', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Operations in territories with elevated human rights risks ', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Project resulting in adverse human rights impacts', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Project resulting in negative health impacts', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Restrictions of freedom of opinion and expression', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Violations of indigenous people’s rights', 2 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Worker protests', 2 )

INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Improper marketing practices', 3 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Marketing of alcohol to children', 3 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Marketing of tobacco to children', 3 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Product-related fatalities', 3 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Product-related incidents', 3 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Quality and safety violations', 3 )

INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Activities resulting in negative environmental impacts', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Air and land pollution', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Air and water pollution', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Air pollution', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Deforestation', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Ecosystem damage', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Environmental pollution', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Incident resulting in negative environmental impacts', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Land pollution', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Leak resulting in environmental impacts', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Leaks resulting in environmental impacts', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Oil spill', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Oil spills', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Project resulting in negative environmental impacts', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Project with environmental risks', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Rainforest destruction', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Spill resulting in environmental impacts', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Spills resulting in environmental impacts', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Water and land pollution', 4 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Water pollution', 4 )

INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Activities resulting in negative environmental and human rights impacts', 5 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Incident resulting in negative environmental and human rights impacts', 5 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Leak resulting in environmental and human rights impacts', 5 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Leaks resulting in environmental and human rights impacts', 5 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Project resulting in negative environmental and human rights impacts', 5 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Project with environmental and human rights risks', 5 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Spill resulting in environmental and human rights impacts', 5 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Spills resulting in environmental and human rights impacts', 5 )

INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Inhumane weapons', 6 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Involvement in cluster munitions', 6 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Involvement in cluster munitions and land mines', 6 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Involvement in land mines', 6 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Nuclear weapons development', 6 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Nuclear weapons programmes', 6 )

INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Accounting fraud', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Accounting irregularities', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Antitrust violations', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Asset misappropriation', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Collusion', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Competition violations', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Consumer fraud', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Corrupt practices', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Embezzlement', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Fraud', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Money laundering', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Price discrimination', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Price-fixing violations', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Tax evasion', 7 )
INSERT INTO dbo.IssueHeading ( Name, IssueHeadingGroupId ) VALUES  ( N'Taxation irregularities', 7 )
