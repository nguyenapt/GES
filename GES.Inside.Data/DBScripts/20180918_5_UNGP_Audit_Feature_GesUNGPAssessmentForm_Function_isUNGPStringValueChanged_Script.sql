
CREATE FUNCTION [dbo].[isUNGPStringValueChanged] 
(   
    @oldString nvarchar(max),
	@newString nvarchar(max)
)

RETURNS bit
AS
BEGIN
	
	IF (COALESCE(@newString, 'empty field') <> COALESCE(@oldString, 'empty field'))
	BEGIN
		RETURN 1;
	END
		RETURN 0;

END