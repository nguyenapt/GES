Create function [dbo].[GetRecommendationNameBefore](@I_GesCaseReports_Id IId) 
returns nvarchar(50)
as 
BEGIN 

declare @CurrentRecommendation IId
set @CurrentRecommendation =
	(select NewI_GesCaseReportStatuses_Id
	from I_GesCaseReports
	where I_GesCaseReports.I_GesCaseReports_Id = @I_GesCaseReports_Id
	)

declare @RecommendationBefore IId
set @RecommendationBefore =
	(select top 1 I_GesCaseReports_Audit.NewI_GesCaseReportStatuses_Id
	from I_GesCaseReports_Audit
	where
		I_GesCaseReports_Audit.I_GesCaseReports_Id = @I_GesCaseReports_Id
		and NewI_GesCaseReportStatuses_Id <> @CurrentRecommendation
	order by I_GesCaseReports_Audit.AuditDateTime desc
	)

declare @RecommendationNameBefore nvarchar(50)

set @RecommendationNameBefore = (select top 1 I_GesCaseReportStatuses.Name from I_GesCaseReportStatuses
where I_GesCaseReportStatuses.I_GesCaseReportStatuses_Id = @RecommendationBefore)

return @RecommendationNameBefore

END


