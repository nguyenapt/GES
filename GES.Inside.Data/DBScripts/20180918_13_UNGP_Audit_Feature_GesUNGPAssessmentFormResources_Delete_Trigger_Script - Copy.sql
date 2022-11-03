CREATE TRIGGER [dbo].[GesUNGPAssessmentFormResources_Delete]
ON [dbo].[GesUNGPAssessmentFormResources]
FOR DELETE
AS
	DECLARE @audit_columns_list [GesUNGPAssessmentForm_Audit_Columns_list];
	DECLARE @oldGesUNGPAssessmentFormResourcesId  as NVARCHAR(200); 
	DECLARE @oldGesUNGPAssessmentFormId  as NVARCHAR(200); 
	DECLARE @oldSourcesName  as NVARCHAR(500); 
	DECLARE @oldSourcesLink  as NVARCHAR(500); 
	DECLARE @oldSourceDate  as NVARCHAR(200); 
	DECLARE @oldModified  as NVARCHAR(200); 
	DECLARE @oldCreated  as NVARCHAR(200); 
	DECLARE @oldModifiedBy  as NVARCHAR(200);  

	DECLARE @hasChangedValue AS bit = 'false';

	SELECT
		@oldGesUNGPAssessmentFormId = CAST([GesUNGPAssessmentFormId] AS uniqueidentifier),
		@oldGesUNGPAssessmentFormResourcesId = CAST([GesUNGPAssessmentFormResourcesId] AS uniqueidentifier),
		@oldSourcesName = [SourcesName],
		@oldSourcesLink = [SourcesLink],
		@oldSourceDate = [SourceDate],
		@oldModified = [Modified],
		@oldCreated = [Created],
		@oldModifiedBy = [ModifiedBy]
	FROM DELETED;

	IF (@oldSourcesName IS NOT NULL)
	BEGIN
	  SET @hasChangedValue = 'true'

	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourcesName', @oldSourcesName, NULL, 'Delete')
	END

	IF (@oldSourcesLink IS NOT NULL)
	BEGIN
	  SET @hasChangedValue = 'true'

	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourcesLink', @oldSourcesLink, NULL, 'Delete')
	END

	IF (@oldSourceDate IS NOT NULL)
	BEGIN
	  SET @hasChangedValue = 'true'

	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourceDate', @oldSourceDate, NULL, 'Delete')
	END  

  IF (@hasChangedValue = 'true')
  BEGIN
    EXEC UpdateGesUNGPAssessmentFormResource_Audit @oldGesUNGPAssessmentFormId,
												   @oldGesUNGPAssessmentFormResourcesId,
												   @audit_columns_list,
												   'Delete',
												   @oldModifiedBy
  END