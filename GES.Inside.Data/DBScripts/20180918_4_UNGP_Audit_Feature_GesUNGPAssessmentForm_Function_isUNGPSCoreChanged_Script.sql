
CREATE FUNCTION [dbo].[isUNGPSCoreChanged] 
(   
    @oldScoreId varchar(200),
	@newScoreId varchar(200)
)

RETURNS bit
AS
BEGIN
	declare @ScoreValue int;
	IF (COALESCE(@newScoreId, '00000000-0000-0000-0000-000000000000') <> COALESCE(@oldScoreId, '00000000-0000-0000-0000-000000000000'))
	BEGIN
		RETURN 1;
	END
		RETURN 0;

END