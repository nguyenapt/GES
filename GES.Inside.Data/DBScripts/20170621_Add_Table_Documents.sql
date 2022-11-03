
/****** Object:  Table [dbo].[G_Organizations_GesDocument]    Script Date: 06/21/2017 4:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[G_Organizations_GesDocument](
	[G_Organizations_GesDocument_Id] [dbo].[IId] IDENTITY(1,1) NOT NULL,
	[G_Organizations_Id] [dbo].[IId] NULL,
	[GesDocumentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_G_Organizations_GesDocument] PRIMARY KEY CLUSTERED 
(
	[G_Organizations_GesDocument_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GesDocument]    Script Date: 06/21/2017 4:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GesDocument](
	[DocumentId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[FileName] [nvarchar](500) NULL,
	[GesDocumentServiceId] [dbo].[IId] NULL,
	[Source] [nvarchar](500) NULL,
	[Metadata01] [nvarchar](255) NULL,
	[Metadata02] [nvarchar](255) NULL,
	[Metadata03] [nvarchar](255) NULL,
	[Comment] [ntext] NULL,
	[Created] [datetime] NULL,
	[CreatedBy] [dbo].[IId] NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [dbo].[IId] NULL,
 CONSTRAINT [PK_GesDocument] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GesDocumentService]    Script Date: 06/21/2017 4:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GesDocumentService](
	[GesDocumentServiceId] [dbo].[IId] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[SortOrder] [int] NULL,
 CONSTRAINT [PK_GesDocumentService] PRIMARY KEY CLUSTERED 
(
	[GesDocumentServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[GesDocument] ADD  CONSTRAINT [DF_GesDocument_DocumentId]  DEFAULT (newid()) FOR [DocumentId]
GO
ALTER TABLE [dbo].[G_Organizations_GesDocument]  WITH CHECK ADD  CONSTRAINT [FK_G_Organizations_GesDocument_G_Organizations] FOREIGN KEY([G_Organizations_Id])
REFERENCES [dbo].[G_Organizations] ([G_Organizations_Id])
GO
ALTER TABLE [dbo].[G_Organizations_GesDocument] CHECK CONSTRAINT [FK_G_Organizations_GesDocument_G_Organizations]
GO
ALTER TABLE [dbo].[G_Organizations_GesDocument]  WITH CHECK ADD  CONSTRAINT [FK_G_Organizations_GesDocument_GesDocument] FOREIGN KEY([GesDocumentId])
REFERENCES [dbo].[GesDocument] ([DocumentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[G_Organizations_GesDocument] CHECK CONSTRAINT [FK_G_Organizations_GesDocument_GesDocument]
GO
ALTER TABLE [dbo].[GesDocument]  WITH CHECK ADD  CONSTRAINT [FK_GesDocument_GesDocumentService] FOREIGN KEY([GesDocumentServiceId])
REFERENCES [dbo].[GesDocumentService] ([GesDocumentServiceId])
GO
ALTER TABLE [dbo].[GesDocument] CHECK CONSTRAINT [FK_GesDocument_GesDocumentService]
GO

GO
INSERT	INTO dbo.GesDocumentService
        ( Name, SortOrder )
VALUES  ( N'Clients > Reports > Annual', 0 )
INSERT	INTO dbo.GesDocumentService
        ( Name, SortOrder )
VALUES  ( N'Clients > Reports > Quarterly', 1 )
INSERT	INTO dbo.GesDocumentService
        ( Name, SortOrder )
VALUES  ( N'Clients > Reports > Position Papers', 2 )
INSERT	INTO dbo.GesDocumentService
        ( Name, SortOrder )
VALUES  ( N'Oekom', 3 )