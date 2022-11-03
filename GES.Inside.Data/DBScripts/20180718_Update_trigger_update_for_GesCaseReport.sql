/****** Object:  Trigger [dbo].[I_GesCaseReports_Update]    Script Date: 7/18/2018 3:19:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[I_GesCaseReports_Update] 
ON [dbo].[I_GesCaseReports] 
FOR UPDATE 
AS 
 
 DECLARE @cols as NVARCHAR(1024);

 
select @cols = coalesce(@cols + ',' + quotename(column_name), quotename(column_name))
from INFORMATION_SCHEMA.COLUMNS
where substring(columns_updated(), columnproperty(object_id(table_schema + '.' + table_name, 'U'), column_name, 'columnId') / 8 + 1, 1) & power(2, -1 + columnproperty(object_id(table_schema + '.' + table_name, 'U'), column_name, 'columnId') % 8 ) > 0
    and table_name = 'I_GesCaseReports'

    -- and column_name in ('c1', 'c2') -- limit to specific columns
    -- and column_name not in ('c3', 'c4') -- or exclude specific columns


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
      ,[ConclusionChanged],'New','Update', @cols, SUSER_SNAME(),getdate()  FROM INSERTED  
 
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
      ,[ConclusionChanged],'Old','Update', @cols, SUSER_SNAME(),getdate()  FROM DELETED



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


--update engagement since
IF EXISTS	(SELECT * FROM Inserted WHERE Inserted.NewI_GesCaseReportStatuses_Id = 7 AND @cols LIKE '%NewI_GesCaseReportStatuses_Id%' )
begin
IF EXISTS (SELECT * FROM dbo.I_GesCaseReportsExtra WHERE I_GesCaseReports_Id = @I_GesCaseReports_Id)
UPDATE dbo.I_GesCaseReportsExtra
SET EngagementSince = GETDATE()
WHERE I_GesCaseReports_Id = @I_GesCaseReports_Id AND EngagementSince IS NULL
ELSE
INSERT dbo.I_GesCaseReportsExtra
        ( I_GesCaseReports_Id ,
          Keywords ,
          EngagementSince
        )
VALUES  ( @I_GesCaseReports_Id , -- I_GesCaseReports_Id - IId
          N'' , -- Keywords - nvarchar(500)
          GETDATE()  -- EngagementSince - datetime
        )

END

