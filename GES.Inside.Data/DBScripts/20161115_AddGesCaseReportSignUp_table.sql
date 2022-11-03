SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GesCaseReportSignUp](
	[GesCaseReportSignUpId] [dbo].[IId] IDENTITY(1,1) NOT NULL,
	[I_GesCaseReports_Id] [dbo].[IId] NULL,
	[G_Individuals_Id] [dbo].[IId] NULL,
 CONSTRAINT [PK_GesCaseReportSignUp] PRIMARY KEY CLUSTERED 
(
	[GesCaseReportSignUpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GesCaseReportSignUp]  WITH CHECK ADD  CONSTRAINT [FK_GesCaseReportSignUp_G_Individuals] FOREIGN KEY([G_Individuals_Id])
REFERENCES [dbo].[G_Individuals] ([G_Individuals_Id])
GO

ALTER TABLE [dbo].[GesCaseReportSignUp] CHECK CONSTRAINT [FK_GesCaseReportSignUp_G_Individuals]
GO

ALTER TABLE [dbo].[GesCaseReportSignUp]  WITH CHECK ADD  CONSTRAINT [FK_GesCaseReportSignUp_I_GesCaseReports] FOREIGN KEY([I_GesCaseReports_Id])
REFERENCES [dbo].[I_GesCaseReports] ([I_GesCaseReports_Id])
GO

ALTER TABLE [dbo].[GesCaseReportSignUp] CHECK CONSTRAINT [FK_GesCaseReportSignUp_I_GesCaseReports]
GO