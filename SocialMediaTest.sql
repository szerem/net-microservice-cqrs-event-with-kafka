use SocialMedia
go 

IF NOT EXISTS(select * from sys.server_principals where name = 'SMUser')
BEGIN 
    CREATE LOGIN SMUser WITH PASSWORD =N'SmPA$$06500', DEFAULT_DATABASE=SocialMedia
END


IF NOT EXISTS(select * from sys.database_principals where name = 'SMUser')
BEGIN 
    EXEC sp_adduser 'SMUser', 'SMUser', 'db_owner';
END


-- use master 
-- go
-- drop DATABASE SocialMedia;