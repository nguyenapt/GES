
CREATE FUNCTION [dbo].[GetUNGPSCores] 
(   
    @ScoreId varchar(200)
)

RETURNS int
AS
BEGIN
	declare @ScoreValue int;
	SELECT @ScoreValue = Score FROM GesUNGPAssessmentScores WHERE GesUNGPAssessmentScoresId = cast(@ScoreId as uniqueidentifier);
	return @ScoreValue;


END