CREATE TRIGGER [dbo].[GesUNGPAssessmentFormResources_Update]
ON [dbo].[GesUNGPAssessmentFormResources]
FOR UPDATE
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

	DECLARE @oldSourcesName  as NVARCHAR(500); 
	DECLARE @oldSourcesLink  as NVARCHAR(500); 
	DECLARE @oldSourceDate  as NVARCHAR(200); 
	DECLARE @oldModified  as NVARCHAR(200); 
	DECLARE @oldCreated  as NVARCHAR(200); 
	DECLARE @oldModifiedBy  as NVARCHAR(200);  

	DECLARE @mode AS nvarchar(20);
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

	SELECT
		@oldSourcesName = [SourcesName],
		@oldSourcesLink = [SourcesLink],
		@oldSourceDate = [SourceDate],
		@oldModified = [Modified],
		@oldCreated = [Created],
		@oldModifiedBy = [ModifiedBy]
	FROM DELETED;

	IF (dbo.isUNGPStringValueChanged(@newSourcesName, @oldSourcesName) = 1)
	BEGIN
	  SET @hasChangedValue = 'true'
	  
	  SET @mode = 'Update';	  
	  IF (@oldSourcesName IS NULL)
      SET @mode = 'Insert';

	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourcesName', @oldSourcesName, @newSourcesName, @mode)
	END

	IF (dbo.isUNGPStringValueChanged(@newSourcesLink, @oldSourcesLink) = 1)
	BEGIN
	  SET @hasChangedValue = 'true'
	  
	  SET @mode = 'Update';
	  IF (@oldSourcesLink IS NULL)
      SET @mode = 'Insert';

	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourcesLink', @oldSourcesLink, @newSourcesLink, @mode)
	END

	IF (dbo.isUNGPStringValueChanged(@newSourceDate, @oldSourceDate) = 1)
	BEGIN
	  SET @hasChangedValue = 'true'

	  SET @mode = 'Update';
	  IF (@oldSourceDate IS NULL)
      SET @mode = 'Insert';

	  INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
		VALUES ('SourceDate', @oldSourceDate, @newSourceDate, @mode)
	END  

  IF (@hasChangedValue = 'true')
  BEGIN
    EXEC UpdateGesUNGPAssessmentFormResource_Audit @newGesUNGPAssessmentFormId,
												   @newGesUNGPAssessmentFormResourcesId,
												   @audit_columns_list,
												   'Update',
												   @newModifiedBy
  END