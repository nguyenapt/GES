
ALTER TRIGGER [dbo].[I_GesCaseReports_Delete] 
ON [dbo].[I_GesCaseReports] 
FOR DELETE 
AS 
 
DECLARE @IGesCaseReportsId IId;
DECLARE @audit_columns_list GesCaseReports_Audit_Columns_list;
DECLARE @oldResponseStatusesValue as NVarchar(1024);
DECLARE @oldProgressStatusesValue as NVarchar(1024);
DECLARE @oldRecommendationValue as NVarchar(1024);
DECLARE @oldRecommendationModifiedDate as NVarchar(1024);
DECLARE @oldConclusionValue as NVARCHAR(MAX);
DECLARE @hasChangedValue as bit = 'false';
DECLARE @oldDedevelopmentValue as NVarchar(1024);

 select @oldResponseStatusesValue = [I_ResponseStatuses_Id],
        @oldProgressStatusesValue = [I_ProgressStatuses_Id],
		@oldRecommendationValue = [NewI_GesCaseReportStatuses_Id],
		@oldRecommendationModifiedDate =  [NewI_GesCaseReportStatuses_IdModified],
		@oldConclusionValue = [I_GesCaseReportStatuses_Id] 
 from DELETED;

 if (@oldResponseStatusesValue IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'

	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_ResponseStatuses_Id', @oldResponseStatusesValue, NULL, 'Delete')
 end 
 
 if (@oldProgressStatusesValue IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_ProgressStatuses_Id', @oldProgressStatusesValue, NULL, 'Delete')
 end

 if (@oldProgressStatusesValue IS NOT NULL OR @oldResponseStatusesValue IS NOT NULL) 
 Begin	
	Select  @oldDedevelopmentValue =  dbo.fnCalcDevelopmentGrade (@oldProgressStatusesValue, @oldResponseStatusesValue);

	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','DevelopmentPerformance', @oldDedevelopmentValue, NULL, 'Delete')
 end

 if (@oldRecommendationValue IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','NewI_GesCaseReportStatuses_Id', @oldRecommendationValue, NULL, 'Delete')
 end
 if ( @oldRecommendationModifiedDate IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'
	--RecommendationModifiedDate
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','NewI_GesCaseReportStatuses_IdModified', @oldRecommendationModifiedDate, NULL, 'Delete')
 end

 if (@oldConclusionValue IS NOT NULL) 
 begin 
	set @hasChangedValue = 'true'
	--Conclusion
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_GesCaseReportStatuses_Id', @oldConclusionValue, NULL, 'Delete')
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
      ,[ConclusionChanged],'Old','Delete', null, SUSER_SNAME(),getdate(), NEWID() FROM DELETED

	  if(@hasChangedValue = 'true')
	  Begin
			Exec GesUpdateGesCaseReportsAudit 'I_GesCaseReports', @IGesCaseReportsId, @audit_columns_list, 'Delete'
	  End
