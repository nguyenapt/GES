USE [GESClients]
GO
/****** Object:  UserDefinedFunction [dbo].[fnCalcDevelopmentGrade]    Script Date: 04/22/2019 2:08:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   FUNCTION [dbo].[fnCalcDevelopmentGrade] 
(   
    @progressGrade float,
    @responseGrade float
)

RETURNS NVarchar(50)
AS
Begin
    if(@progressGrade Is null and  @responseGrade  is null)
	Begin
		return null
	end 

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