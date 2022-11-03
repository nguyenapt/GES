CREATE TABLE [dbo].[GesMilestoneTypes](
	[Id] [uniqueidentifier] NOT NULL,
	[MilestoneCode] [varchar](6) NULL,
	[Name] [nvarchar](255) NULL,
	[Level] [int] NOT NULL,
	[Description] [varchar](255) NULL,
	[Created] [datetime] NULL,
	[CreatedBy] [dbo].[IId] NULL,
	[Order] [int] NULL,
 CONSTRAINT [PK_GesMilestoneTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[GesMilestoneTypes] ADD  CONSTRAINT [DF_GesMilestoneTypes_Id]  DEFAULT (NEWID()) FOR [Id]

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesMilestoneTypes' 
		  AND COLUMN_NAME = 'Level') 
ALTER TABLE [dbo].[GesMilestoneTypes] ADD [Level] int NOT NULL

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesMilestoneTypes' 
		  AND COLUMN_NAME = 'Order') 
ALTER TABLE [dbo].[GesMilestoneTypes] ADD [Order] int NULL

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_Milestones' 
		  AND COLUMN_NAME = 'GesMilestoneTypesId') 
ALTER TABLE [dbo].[I_Milestones] ADD [GesMilestoneTypesId] uniqueidentifier NULL

IF NOT EXISTS (
	SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
    WHERE CONSTRAINT_NAME ='FK_I_Milestones_GesMilestoneType')
BEGIN
	ALTER TABLE [dbo].[I_Milestones]  WITH NOCHECK ADD  CONSTRAINT [FK_I_Milestones_GesMilestoneType] FOREIGN KEY([GesMilestoneTypesId])
	REFERENCES [dbo].[GesMilestoneTypes] ([Id])
	ON DELETE CASCADE
END


INSERT INTO [dbo].[GesMilestoneTypes] ([Id],[MilestoneCode],[Name],[Description],[Created],[CreatedBy],[Level],[Order]) VALUES(NEWID(),'Mst1','Milestone 1','Initial communication sent to the engagement company', GETDATE(),'',1,1)
INSERT INTO [dbo].[GesMilestoneTypes] ([Id],[MilestoneCode],[Name],[Description],[Created],[CreatedBy],[Level],[Order]) VALUES(NEWID(),'Mst2','Milestone 2','Dialogue established', GETDATE(),'',2,2)
INSERT INTO [dbo].[GesMilestoneTypes] ([Id],[MilestoneCode],[Name],[Description],[Created],[CreatedBy],[Level],[Order]) VALUES(NEWID(),'Mst3','Milestone 3','Company commits to address issue', GETDATE(),'',3,3)
INSERT INTO [dbo].[GesMilestoneTypes] ([Id],[MilestoneCode],[Name],[Description],[Created],[CreatedBy],[Level],[Order]) VALUES(NEWID(),'Mst4','Milestone 4','Company develops a strategy to address issue(s)', GETDATE(),'',4,4)
INSERT INTO [dbo].[GesMilestoneTypes] ([Id],[MilestoneCode],[Name],[Description],[Created],[CreatedBy],[Level],[Order]) VALUES(NEWID(),'Mst5','Milestone 5','Issue(s) resolved/strategy effective', GETDATE(),'',5,5)


