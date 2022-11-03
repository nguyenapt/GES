/****** Object:  Table [dbo].[I_GesCaseReportsExtra]    Script Date: 2016-08-23 2:14:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[I_GesCaseReportsExtra](
	[I_GesCaseReportsExtra_Id] [dbo].[IId] IDENTITY(1,1) NOT NULL,
	[I_GesCaseReports_Id] [dbo].[IId] NOT NULL,
	[Keywords] [nvarchar](max) NULL,
 CONSTRAINT [PK_I_GesCaseReportsExtra] PRIMARY KEY CLUSTERED 
(
	[I_GesCaseReportsExtra_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[I_GesCaseReportsExtra]  WITH CHECK ADD  CONSTRAINT [FK_I_GesCaseReportsExtra_I_GesCaseReports] FOREIGN KEY([I_GesCaseReports_Id])
REFERENCES [dbo].[I_GesCaseReports] ([I_GesCaseReports_Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[I_GesCaseReportsExtra] CHECK CONSTRAINT [FK_I_GesCaseReportsExtra_I_GesCaseReports]
GO


