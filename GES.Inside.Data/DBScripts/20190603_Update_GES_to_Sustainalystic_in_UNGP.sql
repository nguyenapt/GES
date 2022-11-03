
UPDATE dbo.GesUNGPAssessmentForm
SET TheExtentOfHarmesScoreComment = [dbo].[RePlaceGEStoSustainalytics](TheExtentOfHarmesScoreComment) 
WHERE TheExtentOfHarmesScoreComment LIKE '%GES%'


UPDATE dbo.GesUNGPAssessmentForm
SET TheNumberOfPeopleAffectedScoreComment = [dbo].[RePlaceGEStoSustainalytics](TheNumberOfPeopleAffectedScoreComment) 
WHERE TheNumberOfPeopleAffectedScoreComment LIKE '%GES%'


UPDATE dbo.GesUNGPAssessmentForm
SET IsViolationScoreComment = [dbo].[RePlaceGEStoSustainalytics](IsViolationScoreComment) 
WHERE IsViolationScoreComment LIKE '%GES%'

UPDATE dbo.GesUNGPAssessmentForm
SET OverSeveralYearsScoreComment = [dbo].[RePlaceGEStoSustainalytics](OverSeveralYearsScoreComment) 
WHERE OverSeveralYearsScoreComment LIKE '%GES%'


UPDATE dbo.GesUNGPAssessmentForm
SET SeveralLocationsScoreComment = [dbo].[RePlaceGEStoSustainalytics](SeveralLocationsScoreComment) 
WHERE SeveralLocationsScoreComment LIKE '%GES%'


UPDATE dbo.GesUNGPAssessmentForm
SET GesConfirmedViolationScoreComment = [dbo].[RePlaceGEStoSustainalytics](GesConfirmedViolationScoreComment) 
WHERE GesConfirmedViolationScoreComment LIKE '%GES%'

UPDATE dbo.GesUNGPAssessmentForm
SET GesCommentSalientHumanRight = [dbo].[RePlaceGEStoSustainalytics](GesCommentSalientHumanRight) 
WHERE GesCommentSalientHumanRight LIKE '%GES%'

UPDATE dbo.GesUNGPAssessmentForm
SET GesCommentCompanyPreparedness = [dbo].[RePlaceGEStoSustainalytics](GesCommentCompanyPreparedness) 
WHERE GesCommentCompanyPreparedness LIKE '%GES%'

UPDATE dbo.GesUNGPAssessmentForm
SET GesCommentCompanyPreparedness = [dbo].[RePlaceGEStoSustainalytics](GesCommentCompanyPreparedness) 
WHERE GesCommentCompanyPreparedness LIKE '%GES%'


UPDATE dbo.GesUNGPAssessmentForm_Audit_Details
SET ColumnNameDescription = [dbo].[RePlaceGEStoSustainalytics](ColumnNameDescription) 
WHERE ColumnNameDescription LIKE '%GES%'


UPDATE dbo.I_EngagementTypes
SET Description = [dbo].[RePlaceGEStoSustainalytics](Description) 
WHERE Description LIKE '%GES%'


UPDATE dbo.I_EngagementTypes
SET Goal = [dbo].[RePlaceGEStoSustainalytics](Goal) 
WHERE Goal LIKE '%GES%'

UPDATE dbo.I_EngagementTypes
SET NextStep = [dbo].[RePlaceGEStoSustainalytics](NextStep) 
WHERE NextStep LIKE '%GES%'

UPDATE dbo.I_EngagementTypes
SET LatestNews = [dbo].[RePlaceGEStoSustainalytics](LatestNews) 
WHERE LatestNews LIKE '%GES%'

UPDATE dbo.I_EngagementTypes
SET OtherInitiatives = [dbo].[RePlaceGEStoSustainalytics](OtherInitiatives) 
WHERE OtherInitiatives LIKE '%GES%'
