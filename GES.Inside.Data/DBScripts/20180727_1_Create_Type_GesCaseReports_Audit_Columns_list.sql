CREATE TYPE dbo.GesCaseReports_Audit_Columns_list AS TABLE (
    TableName nvarchar(100)  NOT NULL,
    ColumnName nvarchar(150)  NOT NULL,
    OldValue nvarchar(max)  NULL,
    NewValue nvarchar(max)  NULL,
    AuditDataState varchar(10)  NOT NULL
);