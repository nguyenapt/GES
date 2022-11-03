
GO

UPDATE dbo.GesCaseReportSignUp
SET G_Organizations_Id = (SELECT G_Organizations_Id FROM dbo.G_Individuals I WHERE I.G_Individuals_Id = GesCaseReportSignUp.G_Individuals_Id)
