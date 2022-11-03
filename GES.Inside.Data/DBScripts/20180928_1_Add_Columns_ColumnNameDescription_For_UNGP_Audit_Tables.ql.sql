
IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesUNGPAssessmentFormResource_Audit_Details' 
		  AND COLUMN_NAME = 'ColumnNameDescription')
ALTER TABLE GesUNGPAssessmentFormResource_Audit_Details ADD [ColumnNameDescription] nvarchar(500) null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesUNGPAssessmentForm_Audit_Details' 
		  AND COLUMN_NAME = 'ColumnNameDescription')
ALTER TABLE GesUNGPAssessmentForm_Audit_Details ADD [ColumnNameDescription] nvarchar(500) null;


DROP PROCEDURE [dbo].[UpdateGesUNGPAssessmentFormResource_Audit]
DROP PROCEDURE [dbo].[UpdateGesUNGPAssessmentForm_Audit]

DROP TYPE [dbo].[GesUNGPAssessmentForm_Audit_Columns_list]

CREATE TYPE [dbo].[GesUNGPAssessmentForm_Audit_Columns_list] AS TABLE(
	[ColumnName] [nvarchar](150) NOT NULL,
	[ColumnNameDescription] nvarchar(500) NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
	[AuditDataState] [varchar](10) NOT NULL
);