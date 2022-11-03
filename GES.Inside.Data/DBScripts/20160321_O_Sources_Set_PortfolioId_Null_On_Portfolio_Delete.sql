ALTER TABLE [dbo].[O_Sources] DROP CONSTRAINT [FK_O_Sources_I_Portfolios]
GO

ALTER TABLE [dbo].[O_Sources]  WITH CHECK ADD CONSTRAINT [FK_O_Sources_I_Portfolios] FOREIGN KEY([I_Portfolios_Id])
REFERENCES [dbo].[I_Portfolios] ([I_Portfolios_Id])
ON DELETE SET NULL
GO


