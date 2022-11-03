
/****** Object:  Table [dbo].[GesCompanyProfiles]    Script Date: 7/17/2017 10:00:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT 1 FROM sys.Objects WHERE  Object_id = OBJECT_ID(N'dbo.GesCompanyProfiles') AND Type = N'U')
BEGIN  
	CREATE TABLE [dbo].[GesCompanyProfiles](
		[GesCompanyProfileId] [uniqueidentifier] NOT NULL,
		[Isin] [nvarchar](100) NULL,
		[IsPrime] [bit] NULL,
		[Grade] [nvarchar](50) NULL,
		[PrimeThreshold] [nvarchar](50) NULL,
		[KeyIssues] [ntext] NULL,
		[Distribution] [ntext] NULL,
		[DocumentId] [uniqueidentifier] NULL,
		[Created] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
		[Modified] [datetime] NULL,
		[ModifiedBy] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_GesCompanyProfiles] PRIMARY KEY CLUSTERED 
	(
		[GesCompanyProfileId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	
	ALTER TABLE [dbo].[GesCompanyProfiles] ADD  CONSTRAINT [DF_GesCompanyProfiles_GesCompanyProfileId]  DEFAULT (newid()) FOR [GesCompanyProfileId]

	ALTER TABLE [dbo].[GesCompanyProfiles]  WITH CHECK ADD  CONSTRAINT [FK_GesCompanyProfiles_GesDocument] FOREIGN KEY([DocumentId])
	REFERENCES [dbo].[GesDocument] ([DocumentId])

	ALTER TABLE [dbo].[GesCompanyProfiles] CHECK CONSTRAINT [FK_GesCompanyProfiles_GesDocument]
END
