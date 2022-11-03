CREATE FUNCTION [dbo].[fnCalcDevelopmentGrade] 
(   
    @progressGrade int,
    @responseGrade int
)

RETURNS NVarchar(50)
AS
Begin
	declare @newAvg int = (@progressGrade + @responseGrade) / 2 ;
	declare @devGrade NVarchar(50) = 'Medium';

	if (@progressGrade <= 2 and @responseGrade <= 2)
	Begin 
		set  @devGrade = 'Low';
	end 
	else if (@newAvg > 3 and  @responseGrade >= 3 and @progressGrade >= 3)
	Begin
		set @devGrade = 'High';
	end 

	if (@newAvg = 0)
	begin 
		set @devGrade = 'N/A';
	end 

	return @devGrade;

End