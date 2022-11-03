
CREATE PROCEDURE [dbo].[GetUserFullName]
 @UserId nvarchar(128),
 @AuditUser nvarchar(128) output
AS
DECLARE @OldUserId IId;
DECLARE @Result nvarchar(200);

select @OldUserId = [OldUserId] from [ges].[Users] where [Id] = @UserId;


SELECT @AuditUser = FirstName + ' ' + LastName
FROM G_Users
join G_Individuals on G_Individuals.G_Individuals_Id = G_Users.G_Individuals_Id
join G_Organizations ON G_Individuals.G_Organizations_Id = G_Organizations.G_Organizations_Id
where G_Users.G_Users_Id = @OldUserId
ORDER BY Name
