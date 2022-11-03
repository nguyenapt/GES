
ALTER TRIGGER [dbo].[I_GesCaseReports_Insert] 
ON [dbo].[I_GesCaseReports] 
FOR INSERT 
AS 
DECLARE @IGesCaseReportsId IId;
DECLARE @audit_columns_list GesCaseReports_Audit_Columns_list;
DECLARE @newResponseStatusesValue as NVarchar(1024);
DECLARE @newProgressStatusesValue as NVarchar(1024);
DECLARE @newRecommendationValue as NVarchar(1024);
DECLARE @newRecommendationModifiedDate as NVarchar(1024);
DECLARE @newConclusionValue as NVARCHAR(MAX);
DECLARE @hasChangedValue as bit = 'false';
DECLARE @newDedevelopmentValue as NVarchar(1024);


 select @IGesCaseReportsId = [I_GesCaseReports_Id],
		@newResponseStatusesValue = [I_ResponseStatuses_Id], 
		@newProgressStatusesValue = [I_ProgressStatuses_Id],
		@newRecommendationValue = [NewI_GesCaseReportStatuses_Id],
		@newConclusionValue = [I_GesCaseReportStatuses_Id]
 from INSERTED;

 if (@newResponseStatusesValue IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'

	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_ResponseStatuses_Id', NULL, @newResponseStatusesValue, 'Insert')
 end 
 
 if (@newProgressStatusesValue IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_ProgressStatuses_Id', NULL, @newProgressStatusesValue, 'Insert')
 end

 if (@newResponseStatusesValue IS NOT NULL OR @newProgressStatusesValue IS NOT NULL) 
 Begin	
	Select  @newDedevelopmentValue =  dbo.fnCalcDevelopmentGrade (@newProgressStatusesValue, @newResponseStatusesValue);

	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','DevelopmentPerformance', NULL, @newDedevelopmentValue, 'Update')
 end

 if (@newRecommendationValue IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','NewI_GesCaseReportStatuses_Id', NULL, @newRecommendationValue, 'Insert')
	set @newRecommendationModifiedDate = getDate();
 end 

 if (@newRecommendationModifiedDate IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'
	--RecommendationModifiedDate
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','NewI_GesCaseReportStatuses_IdModified', NULL, GETDATE(), 'Insert')
 end

 if (@newConclusionValue IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'
	--Conclusion
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_GesCaseReportStatuses_Id', NULL, @newConclusionValue, 'Insert')
 end

 INSERT INTO dbo.I_GesCaseReports_Audit 
 SELECT [I_GesCaseReports_Id]
      ,[OriginalI_GesCaseReports_Id]
      ,[I_GesCompanies_Id]
      ,[AnalystG_Users_Id]
      ,[ReportIncident]
      ,[ReportIncidentModified]
      ,[I_GesCaseReportStatuses_Id]
      ,[I_GesCaseReportStatuses_IdReviewed]
      ,[NewI_GesCaseReportStatuses_Id]
      ,[NewI_GesCaseReportStatuses_IdReviewed]
      ,[NewI_GesCaseReportStatuses_IdModified]
      ,[LocationG_Countries_Id]
      ,[LocationG_Countries_IdModified]
      ,[Location2G_Countries_Id]
      ,[Location3G_Countries_Id]
      ,[Location4G_Countries_Id]
      ,[Location5G_Countries_Id]
      ,[I_NaArticles_Id]
      ,[Summary]
      ,[SummaryReviewed]
      ,[SummaryModified]
      ,[NewSummary]
      ,[GesComment]
      ,[GesCommentReviewed]
      ,[GesCommentModified]
      ,[EditI_GesCaseReportStatuses_Id]
      ,[EditText]
      ,[Description]
      ,[DescriptionNew]
      ,[Conclusion]
      ,[ConclusionReviewed]
      ,[ConclusionNew]
      ,[ConclusionObs]
      ,[Recommendation]
      ,[RecommendationExludeEf]
      ,[Revision]
      ,[SourceDialogueSummary]
      ,[SourceDialogueNew]
      ,[Confirmed]
      ,[I_GesCaseReportSourceTypes_Id]
      ,[CompanyDialogueSummary]
      ,[CompanyDialogueSummaryReviewed]
      ,[CompanyDialogueNew]
      ,[CompanyDialogueNewReviewed]
      ,[CompanyDialogueSummaryObs]
      ,[EngagementSummaryClientReport]
      ,[ShowInClient]
      ,[FeatureInClient]
      ,[I_GesCaseReportsI_ProcessStatuses_Id]
      ,[ProcessGoal]
      ,[ProcessGoalReviewed]
      ,[ProcessGoalModified]
      ,[ProcessStep]
      ,[ProcessStepReviewed]
      ,[ProcessStepUpdated]
      ,[G_ForumMessages_Id]
      ,[I_ResponseStatuses_Id]
      ,[I_ResponseStatuses_IdReviewed]
      ,[I_ProgressStatuses_Id]
      ,[I_ProgressStatuses_IdReviewed]
      ,[I_NormAreas_Id]
      ,[FullReportG_ManagedDocuments_Id]
      ,[LatestNews]
      ,[LatestNewsModified]
      ,[Comment]
      ,[IsBurmaEngCase]
      ,[I_BurmaRecommendations_Id]
      ,[EntryDate]
      ,[EngagementTypeIncident]
      ,[EngagementTypeCompany]
      ,[EngagementTypeHighRiskZone]
      ,[EngagementTypeI_HighRiskZones_Id]
      ,[EngagementTypeThematic]
      ,[EngagementTypeI_Themes_Id]
      ,[EngagementTypeSector]
      ,[EngagementTypeI_Msci_Id]
      ,[SuccessStory]
      ,[IncidentAnalysisSummary]
      ,[IncidentAnalysisDialogueAndAnalysis]
      ,[IncidentAnalysisConclusion]
      ,[IncidentAnalysisGuidelines]
      ,[IncidentAnalysisSources]
      ,[SendNotifications]
      ,[Modified]
      ,[ModifiedByG_Users_Id]
      ,[Created]
      ,[ClosingIncidentAnalysisSummary]
      ,[ClosingIncidentAnalysisDialogueAndAnalysis]
      ,[ClosingIncidentAnalysisConclusion]
      ,[ConclusionChanged],'New','Insert', null, SUSER_SNAME(),getdate(), NEWID()  FROM INSERTED

	if(@hasChangedValue = 'true')
	Begin
		Exec GesUpdateGesCaseReportsAudit 'I_GesCaseReports', @IGesCaseReportsId, @audit_columns_list, 'Insert'
	End
