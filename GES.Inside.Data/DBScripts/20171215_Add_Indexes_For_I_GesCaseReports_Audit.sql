IF NOT EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'ix_NewI_GesCaseReportStatuses_Id')   
     CREATE NONCLUSTERED INDEX ix_NewI_GesCaseReportStatuses_Id  
    ON dbo.I_GesCaseReports_Audit (NewI_GesCaseReportStatuses_Id);
GO