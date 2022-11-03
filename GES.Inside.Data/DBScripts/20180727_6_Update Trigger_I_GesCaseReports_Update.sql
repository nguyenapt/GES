
ALTER TRIGGER [dbo].[I_GesCaseReports_Update] 
ON [dbo].[I_GesCaseReports] 
FOR UPDATE 
AS 
 
DECLARE @cols as NVARCHAR(1024);
DECLARE @IGesCaseReportsId IId;
DECLARE @audit_columns_list GesCaseReports_Audit_Columns_list;
DECLARE @newResponseStatusesValue as NVarchar(1024);
DECLARE @oldResponseStatusesValue as NVarchar(1024);
DECLARE @newProgressStatusesValue as NVarchar(1024);
DECLARE @oldProgressStatusesValue as NVarchar(1024);
DECLARE @newRecommendationValue as NVarchar(1024);
DECLARE @oldRecommendationValue as NVarchar(1024);
DECLARE @oldRecommendationModifiedDate as NVarchar(1024);
DECLARE @newRecommendationModifiedDate as NVarchar(1024);
DECLARE @newConclusionValue as NVARCHAR(MAX);
DECLARE @oldConclusionValue as NVARCHAR(MAX);
DECLARE @transactionNumber  NVARCHAR(MAX) = NEWID();
DECLARE @hasChangedValue as bit = 'false';
DECLARE @oldDedevelopmentValue as NVarchar(1024);
DECLARE @newDedevelopmentValue as NVarchar(1024);

 
select @cols = coalesce(@cols + ',' + quotename(column_name), quotename(column_name))
from INFORMATION_SCHEMA.COLUMNS
where substring(columns_updated(), columnproperty(object_id(table_schema + '.' + table_name, 'U'), column_name, 'columnId') / 8 + 1, 1) & power(2, -1 + columnproperty(object_id(table_schema + '.' + table_name, 'U'), column_name, 'columnId') % 8 ) > 0
    and table_name = 'I_GesCaseReports'

    -- and column_name in ('c1', 'c2') -- limit to specific columns
    -- and column_name not in ('c3', 'c4') -- or exclude specific columns
 
 select @IGesCaseReportsId = [I_GesCaseReports_Id],
		@newResponseStatusesValue = [I_ResponseStatuses_Id], 
		@newProgressStatusesValue = [I_ProgressStatuses_Id],
		@newRecommendationValue = [NewI_GesCaseReportStatuses_Id],
		@newConclusionValue = [I_GesCaseReportStatuses_Id]
 from INSERTED;
 select @oldResponseStatusesValue = [I_ResponseStatuses_Id],
        @oldProgressStatusesValue = [I_ProgressStatuses_Id],
		@oldRecommendationValue = [NewI_GesCaseReportStatuses_Id],
		@oldRecommendationModifiedDate =  [NewI_GesCaseReportStatuses_IdModified],
		@oldConclusionValue = [I_GesCaseReportStatuses_Id] 
 from DELETED;

 if (@newResponseStatusesValue <>  @oldResponseStatusesValue) 
 begin 
	set @hasChangedValue = 'true'

	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_ResponseStatuses_Id', @oldResponseStatusesValue, @newResponseStatusesValue, 'Update')
 end 
 
 if (@newProgressStatusesValue <>  @oldProgressStatusesValue) 
 begin 
	set @hasChangedValue = 'true'
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_ProgressStatuses_Id', @oldProgressStatusesValue, @newProgressStatusesValue, 'Update')
 end

 if (@newResponseStatusesValue <>  @oldResponseStatusesValue OR @newProgressStatusesValue <>  @oldProgressStatusesValue) 
 Begin	

	Select  @oldDedevelopmentValue =  dbo.fnCalcDevelopmentGrade (@oldProgressStatusesValue, @oldResponseStatusesValue);
	Select  @newDedevelopmentValue =  dbo.fnCalcDevelopmentGrade (@newProgressStatusesValue, @newResponseStatusesValue);

	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','DevelopmentPerformance', @oldDedevelopmentValue, @newDedevelopmentValue, 'Update')
 end

 if (@newRecommendationValue <> @oldRecommendationValue) 
 begin 
	set @hasChangedValue = 'true'
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','NewI_GesCaseReportStatuses_Id', @oldRecommendationValue, @newRecommendationValue, 'Update')
	set @newRecommendationModifiedDate = getDate();
 end

 if (@newRecommendationModifiedDate <> @oldRecommendationModifiedDate) 
 begin 
	set @hasChangedValue = 'true'
	--RecommendationModifiedDate
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','NewI_GesCaseReportStatuses_IdModified', @oldRecommendationModifiedDate, @newRecommendationModifiedDate, 'Update')
 end

 if (@newConclusionValue <> @oldConclusionValue) 
 begin 
	set @hasChangedValue = 'true'
	--Conclusion
	INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
	VALUES ('I_GesCaseReports','I_GesCaseReportStatuses_Id', @oldConclusionValue, @newConclusionValue, 'Update')
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
      ,[ConclusionChanged],'New','Update', @cols, SUSER_SNAME(),getdate(), @transactionNumber  FROM INSERTED  
 
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
      ,[ConclusionChanged],'Old','Update', @cols, SUSER_SNAME(),getdate(), @transactionNumber  FROM DELETED

	  if(@hasChangedValue = 'true')
	  Begin
			Exec GesUpdateGesCaseReportsAudit 'I_GesCaseReports', @IGesCaseReportsId, @audit_columns_list, 'Update'
	  End

update I_GesCaseReports
set ConclusionChanged = dbo.GetConclusionChangedNonCached(I_GesCaseReports_Id),
NewI_GesCaseReportStatuses_IdModified = dbo.GetRecommendationChanged(I_GesCaseReports_Id),
SendNotifications=0
where I_GesCaseReports.I_GesCaseReports_Id in (select I_GesCaseReports_Id from INSERTED)

declare @I_GesCaseReports_Id IId
set @I_GesCaseReports_Id = (select top 1 I_GesCaseReports_Id from INSERTED)

exec G_UpdateReviewDate 'I_GesCaseReports', 'I_GesCaseReportStatuses_Id', @I_GesCaseReports_Id
exec G_UpdateReviewDate 'I_GesCaseReports', 'Summary', @I_GesCaseReports_Id
exec G_UpdateReviewDate 'I_GesCaseReports', 'GesComment', @I_GesCaseReports_Id
exec G_UpdateReviewDate 'I_GesCaseReports', 'CompanyDialogueSummary', @I_GesCaseReports_Id
exec G_UpdateReviewDate 'I_GesCaseReports', 'CompanyDialogueNew', @I_GesCaseReports_Id
exec G_UpdateReviewDate 'I_GesCaseReports', 'ProcessGoal', @I_GesCaseReports_Id
exec G_UpdateReviewDate 'I_GesCaseReports', 'ProcessStep', @I_GesCaseReports_Id
exec G_UpdateReviewDate 'I_GesCaseReports', 'I_ResponseStatuses_Id', @I_GesCaseReports_Id
exec G_UpdateReviewDate 'I_GesCaseReports', 'I_ProgressStatuses_Id', @I_GesCaseReports_Id



-- print dbo.GetConclusionChanged(2007)


