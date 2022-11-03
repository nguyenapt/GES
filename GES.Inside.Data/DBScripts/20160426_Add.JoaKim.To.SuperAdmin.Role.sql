declare @userId nvarchar(255)
select @userId = Id from ges.Users
	where UserName = 'joakim.westin'

if @userId is not null and not exists (select * from ges.UserRoles where UserId = @userId)
	insert into ges.UserRoles(UserId, RoleId)
	values(@userId, 2)
