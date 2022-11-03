
CREATE TABLE [dbo].[GesAnnouncement](
	[GesAnnouncementId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](500) NULL,
	[LinkTitle] [nvarchar](500) NULL,
	[Content] [nvarchar](max) NULL,
	[AnnouncementDate] [datetime] NULL,
 CONSTRAINT [PK_GesAnnouncement] PRIMARY KEY CLUSTERED 
(
	[GesAnnouncementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


