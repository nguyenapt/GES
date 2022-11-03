CREATE function [dbo].[RePlaceGEStoSustainalytics](@text nvarchar(max)) 
returns nvarchar(max)
as 
BEGIN 
	set @text = ' ' + @text

	set @text = REPLACE(@text,' GES ', ' Sustainalytics ')
	set @text = REPLACE(@text,' GES', ' Sustainalytics')
	set @text = REPLACE(@text,CHAR(10) + 'GES',CHAR(10) + 'Sustainalytics')
	set @text = REPLACE(@text,'(GES ', '(Sustainalytics ')

	set @text = LTRIM(@text)

	return @text
END

GO

DISABLE TRIGGER dbo.I_GesCaseReports_Update ON dbo.I_GesCaseReports; 
GO
UPDATE dbo.I_GesCaseReports 
SET Summary = [dbo].[RePlaceGEStoSustainalytics](Summary) 
WHERE Summary LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET NewSummary = [dbo].[RePlaceGEStoSustainalytics](NewSummary) 
WHERE NewSummary LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET GesComment = [dbo].[RePlaceGEStoSustainalytics](GesComment) 
WHERE GesComment LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET EditText = [dbo].[RePlaceGEStoSustainalytics](EditText) 
WHERE EditText LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET DESCRIPTION = [dbo].[RePlaceGEStoSustainalytics](DESCRIPTION) 
WHERE DESCRIPTION LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET DescriptionNew = [dbo].[RePlaceGEStoSustainalytics](DescriptionNew) 
WHERE DescriptionNew LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET Conclusion = [dbo].[RePlaceGEStoSustainalytics](Conclusion) 
WHERE Conclusion LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET ConclusionNew = [dbo].[RePlaceGEStoSustainalytics](ConclusionNew) 
WHERE ConclusionNew LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET ConclusionObs = [dbo].[RePlaceGEStoSustainalytics](ConclusionObs) 
WHERE ConclusionObs LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET Recommendation = [dbo].[RePlaceGEStoSustainalytics](Recommendation) 
WHERE Recommendation LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET RecommendationExludeEf = [dbo].[RePlaceGEStoSustainalytics](RecommendationExludeEf) 
WHERE RecommendationExludeEf LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET Revision = [dbo].[RePlaceGEStoSustainalytics](Revision) 
WHERE Revision LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET SourceDialogueSummary = [dbo].[RePlaceGEStoSustainalytics](SourceDialogueSummary) 
WHERE SourceDialogueSummary LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET SourceDialogueNew = [dbo].[RePlaceGEStoSustainalytics](SourceDialogueNew) 
WHERE SourceDialogueNew LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET CompanyDialogueSummary = [dbo].[RePlaceGEStoSustainalytics](CompanyDialogueSummary) 
WHERE CompanyDialogueSummary LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET CompanyDialogueNew = [dbo].[RePlaceGEStoSustainalytics](CompanyDialogueNew) 
WHERE CompanyDialogueNew LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET CompanyDialogueSummaryObs = [dbo].[RePlaceGEStoSustainalytics](CompanyDialogueSummaryObs) 
WHERE CompanyDialogueSummaryObs LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET EngagementSummaryClientReport = [dbo].[RePlaceGEStoSustainalytics](EngagementSummaryClientReport) 
WHERE EngagementSummaryClientReport LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET LatestNews = [dbo].[RePlaceGEStoSustainalytics](LatestNews) 
WHERE LatestNews LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET Comment = [dbo].[RePlaceGEStoSustainalytics](Comment) 
WHERE Comment LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET SuccessStory = [dbo].[RePlaceGEStoSustainalytics](SuccessStory) 
WHERE SuccessStory LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET IncidentAnalysisSummary = [dbo].[RePlaceGEStoSustainalytics](IncidentAnalysisSummary) 
WHERE IncidentAnalysisSummary LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET IncidentAnalysisDialogueAndAnalysis = [dbo].[RePlaceGEStoSustainalytics](IncidentAnalysisDialogueAndAnalysis) 
WHERE IncidentAnalysisDialogueAndAnalysis LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET IncidentAnalysisConclusion = [dbo].[RePlaceGEStoSustainalytics](IncidentAnalysisConclusion) 
WHERE IncidentAnalysisConclusion LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET IncidentAnalysisGuidelines = [dbo].[RePlaceGEStoSustainalytics](IncidentAnalysisGuidelines) 
WHERE IncidentAnalysisGuidelines LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET IncidentAnalysisSources = [dbo].[RePlaceGEStoSustainalytics](IncidentAnalysisSources) 
WHERE IncidentAnalysisSources LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET ClosingIncidentAnalysisSummary = [dbo].[RePlaceGEStoSustainalytics](ClosingIncidentAnalysisSummary) 
WHERE ClosingIncidentAnalysisSummary LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET ClosingIncidentAnalysisDialogueAndAnalysis = [dbo].[RePlaceGEStoSustainalytics](ClosingIncidentAnalysisDialogueAndAnalysis) 
WHERE ClosingIncidentAnalysisDialogueAndAnalysis LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET ClosingIncidentAnalysisConclusion = [dbo].[RePlaceGEStoSustainalytics](ClosingIncidentAnalysisConclusion) 
WHERE ClosingIncidentAnalysisConclusion LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET Guidelines = [dbo].[RePlaceGEStoSustainalytics](Guidelines) 
WHERE Guidelines LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET CompanyPreparedness = [dbo].[RePlaceGEStoSustainalytics](CompanyPreparedness) 
WHERE CompanyPreparedness LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET GAPAnalysis = [dbo].[RePlaceGEStoSustainalytics](GAPAnalysis) 
WHERE GAPAnalysis LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET PositiveDevelopment = [dbo].[RePlaceGEStoSustainalytics](PositiveDevelopment) 
WHERE PositiveDevelopment LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO
UPDATE dbo.I_GesCaseReports 
SET ProcessGoal = [dbo].[RePlaceGEStoSustainalytics](ProcessGoal) 
WHERE ProcessGoal LIKE '%GES%' AND NewI_GesCaseReportStatuses_Id IN (6,7,8)
GO

ENABLE TRIGGER dbo.I_GesCaseReports_Update ON dbo.I_GesCaseReports; 

GO
--MileStones
DISABLE TRIGGER dbo.I_Milestones_Update ON dbo.I_Milestones; 

UPDATE dbo.I_Milestones 
SET Description = [dbo].[RePlaceGEStoSustainalytics](Description) 
WHERE Description LIKE '%GES%' AND  I_Milestones_Id IN (SELECT I_Milestones_Id FROM I_Milestones
INNER JOIN dbo.I_GesCaseReports ON I_GesCaseReports.I_GesCaseReports_Id = I_Milestones.I_GesCaseReports_Id
WHERE NewI_GesCaseReportStatuses_Id IN (6,7,8))
GO
ENABLE TRIGGER dbo.I_Milestones_Update ON dbo.I_Milestones;

--Latest news
GO
DISABLE TRIGGER dbo.I_GesLatestNews_Update ON dbo.I_GesLatestNews; 

UPDATE dbo.I_GesLatestNews 
SET Description = [dbo].[RePlaceGEStoSustainalytics](Description) 
WHERE Description LIKE '%GES%' AND I_GesLatestNews_Id IN (
	SELECT I_GesLatestNews_Id FROM I_GesLatestNews
INNER JOIN dbo.I_GesCaseReports ON I_GesCaseReports.I_GesCaseReports_Id = I_GesLatestNews.I_GesCaseReports_Id
WHERE NewI_GesCaseReportStatuses_Id IN (6,7,8))
GO
ENABLE TRIGGER dbo.I_GesLatestNews_Update ON dbo.I_GesLatestNews; 

GO
--I_GesCommentary
UPDATE dbo.I_GesCommentary
SET Description = [dbo].[RePlaceGEStoSustainalytics](Description) 
WHERE Description LIKE '%GES%' AND i_gescommentary_Id NOT IN (1515,1516) AND I_GesCommentary_Id IN (
	SELECT I_GesCommentary_Id FROM I_GesCommentary
INNER JOIN dbo.I_GesCaseReports ON I_GesCaseReports.I_GesCaseReports_Id = I_GesCommentary.I_GesCaseReports_Id
WHERE NewI_GesCaseReportStatuses_Id IN (6,7,8))


