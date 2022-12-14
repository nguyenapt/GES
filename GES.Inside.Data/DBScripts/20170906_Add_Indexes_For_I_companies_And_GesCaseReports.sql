
IF NOT EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_I_Companies_NameCoalesce')   
     CREATE NONCLUSTERED INDEX IX_I_Companies_NameCoalesce   
    ON dbo.I_Companies (NameCoalesce);
GO  

IF NOT EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_I_Companies_Isin')   
     CREATE NONCLUSTERED INDEX IX_I_Companies_Isin   
    ON dbo.I_Companies (Isin);
GO  

IF NOT EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_I_GesCaseReports_ReportIncident')   
     CREATE NONCLUSTERED INDEX IX_I_GesCaseReports_ReportIncident  
    ON dbo.I_GesCaseReports (ReportIncident);
GO
