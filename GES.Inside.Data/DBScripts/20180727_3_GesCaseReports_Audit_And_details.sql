-- tables
-- Table: GesCaseReports_Audit
CREATE TABLE GesCaseReports_Audit (
    GesCaseReports_Audit_Id uniqueidentifier  NOT NULL,
    I_GesCaseReports_Id int  NOT NULL,
    AuditUser nvarchar(128)  NOT NULL,
    AuditDMLAction varchar(10)  NOT NULL,
    AuditDatetime datetime  NOT NULL,
    CONSTRAINT GesCaseReports_Audit_pk PRIMARY KEY  (GesCaseReports_Audit_Id)
);

-- Table: GesCaseReports_Audit_Details
CREATE TABLE GesCaseReports_Audit_Details (
    GesCaseReports_Audit_Details uniqueidentifier  NOT NULL,
    GesCaseReports_Audit_GesCaseReports_Audit_Id uniqueidentifier  NOT NULL,
    TableName nvarchar(100)  NOT NULL,
    ColumnName nvarchar(150)  NOT NULL,
    OldValue nvarchar(max)  NULL,
	NewValue nvarchar(max)  NULL,
    AuditDataState varchar(10)  NOT NULL,
    AuditUser nvarchar(128)  NOT NULL,
    AuditDatetime datetime  NOT NULL,
    CONSTRAINT GesCaseReports_Audit_Details_pk PRIMARY KEY  (GesCaseReports_Audit_Details)
);

-- foreign keys
-- Reference: GesCaseReports_Audit_Details_GesCaseReports_Audit (table: GesCaseReports_Audit_Details)
ALTER TABLE GesCaseReports_Audit_Details ADD CONSTRAINT GesCaseReports_Audit_Details_GesCaseReports_Audit
    FOREIGN KEY (GesCaseReports_Audit_GesCaseReports_Audit_Id)
    REFERENCES GesCaseReports_Audit (GesCaseReports_Audit_Id);

-- End of file.