CREATE TRIGGER [dbo].[GesUNGPAssessmentFormResource_Insert]
ON [dbo].[GesUNGPAssessmentFormResources]
FOR INSERT
AS
  DECLARE @audit_columns_list [GesUNGPAssessmentForm_Audit_Columns_list];

	DECLARE @newGesUNGPAssessmentFormResourcesId  as NVARCHAR(200); 
	DECLARE @newGesUNGPAssessmentFormId  as NVARCHAR(200); 

	DECLARE @newSourcesName  as NVARCHAR(500); 
	DECLARE @newSourcesLink  as NVARCHAR(500); 
	DECLARE @newSourceDate  as NVARCHAR(200); 
	DECLARE @newModified  as NVARCHAR(200); 
	DECLARE @newCreated  as NVARCHAR(200); 
	DECLARE @newModifiedBy  as NVARCHAR(200); 

  DECLARE @hasChangedValue AS bit = 'false';

  SELECT
    @newGesUNGPAssessmentFormId = CAST([GesUNGPAssessmentFormId] AS uniqueidentifier),
    @newGesUNGPAssessmentFormResourcesId = CAST([GesUNGPAssessmentFormResourcesId] AS uniqueidentifier),
    @newGesUNGPAssessmentFormId = [GesUNGPAssessmentFormId],
    @newSourcesName = [SourcesName],
    @newSourcesLink = [SourcesLink],
    @newSourceDate = [SourceDate],
    @newModified = [Modified],
    @newCreated = [Created],
    @newModifiedBy = [ModifiedBy]
  FROM INSERTED;

	IF (@newSourcesName IS NOT NULL)
	BEGIN
	  SET @hasChangedValue = 'true'
	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourcesName', NULL, @newSourcesName, 'Insert')
	END
	IF (@newSourcesLink IS NOT NULL)
	BEGIN
	  SET @hasChangedValue = 'true'
	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourcesLink', NULL, @newSourcesLink, 'Insert')
	END
	IF (@newSourceDate IS NOT NULL)
	BEGIN
	  SET @hasChangedValue = 'true'
	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourceDate', NULL, @newSourceDate, 'Insert')
	END  

  IF (@hasChangedValue = 'true')
  BEGIN
    EXEC UpdateGesUNGPAssessmentFormResource_Audit @newGesUNGPAssessmentFormId,
												   @newGesUNGPAssessmentFormResourcesId,
												   @audit_columns_list,
												   'Insert',
												   @newModifiedBy
  END