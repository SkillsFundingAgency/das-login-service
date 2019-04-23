/*
	Script to to create a SQL script of insert statements which will migrate [EmployerUsersDB].[dbo].[User] into [LoginServiceDB].[IdentityService].[AspNetUsers] 
	whilst transforming the seperate salt and password to a single hash which is marked as a migrated hash and can be verfifed in the LoginService by a custom hash
	verifier.
*/
				
DECLARE EmployeeUser_User_AspNetUsers_Cursor CURSOR FOR  
SELECT 
	Id,
	FirstName GivenName,
	LastName FamilyName,
	Email,
	Email UserName,
	IsActive EmailConfirmed,
	FailedLoginAttempts AccessFailedCount,
	CASE WHEN IsLocked = 1 THEN DATEADD(day, 14, GETDATE()) ELSE NULL END LockoutEnd -- default 14 day lockout in Login Service
FROM 
	[User]
--------- TEST ----------
WHERE
	[User].LastName LIKE '%Woodcock%'
--------- TEST ----------
  
OPEN EmployeeUser_User_AspNetUsers_Cursor;  

DECLARE @id UNIQUEIDENTIFIER
DECLARE @givenName NVARCHAR(50)
DECLARE @familyName NVARCHAR(50)
DECLARE @email VARCHAR(255)
DECLARE @userName VARCHAR(255)
DECLARE @emailConfirmed BIT
DECLARE @accessFailedCount INT
DECLARE @lockoutEnd DATETIME

DECLARE @newLine VARCHAR(2) = char(13)

PRINT '/*'
PRINT '  This script contains insert statements which were generated from the [User] table of EmployerUsers service database; the insert statements create the same'
PRINT '  users with their current passwords hashed such that a custom password hasher in the Login service can verify the existing password after they'
PRINT '  have been migrated.' + @newLine
PRINT '  If the user has not confirmed their password in EmployerUsers then they will be invited to confirm their password when they first attempt to sign-in via'
PRINT '  the Login service after they have been migrated.'
PRINT '*/'
PRINT @newLine + 'BEGIN TRANSACTION'
PRINT @newLine + 'SET IDENTITY_INSERT [IdentityServer].[AspNetUsers] ON'

-- decode into byte the encode base64 format marker
DECLARE @encodedFormatMarker varchar(max) = '/w==' -- 0xFF (marker to identity hash as migrated from EmployerUsers)
DECLARE @decodedFormatMarker varbinary(max) = cast('' as xml).value('xs:base64Binary(sql:variable(''@encodedFormatMarker''))', 'varbinary(max)')
  
FETCH NEXT FROM EmployeeUser_User_AspNetUsers_Cursor INTO @id, @givenName, @familyName, @email, @userName, @emailConfirmed, @accessFailedCount, @lockoutEnd;  
WHILE @@FETCH_STATUS = 0  
BEGIN  
	-- get the currently encode base64 version of the salt and password
	DECLARE @encodedSalt VARCHAR(MAX) = (SELECT Salt FROM [User] WHERE Id = @id)
	DECLARE @encodedPassword VARCHAR(MAX) = (SELECT [Password] FROM [User] WHERE Id = @id)

	DECLARE @decodedSalt VARBINARY(max) = CAST('' as xml).value('xs:base64Binary(sql:variable(''@encodedSalt''))', 'varbinary(max)')
	DECLARE @decodedPassword VARBINARY(max) = CAST('' as xml).value('xs:base64Binary(sql:variable(''@encodedPassword''))', 'varbinary(max)')

	-- concatenate the decoded byte values and re-encode to base64 so store in a single column
	DECLARE @decodedCombined VARBINARY(max) = @decodedFormatMarker + @decodedSalt + @decodedPassword
	DECLARE @endcodedCombined VARCHAR(MAX) = CAST('' as xml).value('xs:base64Binary(sql:variable(''@decodedCombined''))', 'varchar(max)')

	PRINT @newLine + 'INSERT INTO [IdentityServer].[AspNetUsers]'
           + '([Id]'
           + ',[AccessFailedCount]'
           + ',[ConcurrencyStamp]'
           + ',[Email]'
           + ',[EmailConfirmed]'
           + ',[LockoutEnabled]'
           + ',[LockoutEnd]'
           + ',[NormalizedEmail]'
           + ',[NormalizedUserName]'
           + ',[PasswordHash]'
           + ',[PhoneNumber]'
           + ',[PhoneNumberConfirmed]'
           + ',[SecurityStamp]'
           + ',[TwoFactorEnabled]'
           + ',[UserName]'
           + ',[GivenName]'
           + ',[FamilyName])' + @newLine
     + 'VALUES'
           + '(''' + LOWER(CONVERT(VARCHAR(36), @id)) + '''' 
           + ',' + CONVERT(VARCHAR, @accessFailedCount)
           + ',''' + LOWER(CONVERT(VARCHAR(36), NEWID())) + '''' -- ConcurrencyStamp
           + ',''' + @email + ''''
           + ',' + CONVERT(VARCHAR, @emailConfirmed) + 
           + ',' + CONVERT(VARCHAR, 1) + -- LockoutEnabled
		   + ',' + CASE WHEN @lockoutEnd IS NULL THEN 'NULL' ELSE '''' + CONVERT(VARCHAR, @lockoutEnd, 121) + '''' END
           + ',''' + UPPER(@email) + '''' -- NormalizedEmail
           + ',''' + UPPER(@userName) + '''' -- NormalizedUserName
           + ',''' + @endcodedCombined + '''' -- PasswordHash
           + ',' + 'NULL' +  -- PhoneNumber
           + ',' + CONVERT(VARCHAR, 0) -- PhoneNumberConfirmed
           + ',''' + LOWER(REPLACE(CONVERT(VARCHAR(36), NEWID()), '-', '')) + '''' -- SecurityStamp must be created without dashes
           + ',' + CONVERT(VARCHAR, 0) -- TwoFactorEnabled
           + ',''' + @userName + ''''
           + ',''' + @givenName + '''' 
           + ',''' + @familyName + ''''
		   + ')'
	   
	FETCH NEXT FROM EmployeeUser_User_AspNetUsers_Cursor INTO @id, @givenName, @familyName, @email, @userName, @emailConfirmed, @accessFailedCount, @lockoutEnd	   
END;  
  
CLOSE EmployeeUser_User_AspNetUsers_Cursor;  
DEALLOCATE EmployeeUser_User_AspNetUsers_Cursor;

PRINT @newLine + 'SET IDENTITY_INSERT [IdentityServer].[AspNetUsers] OFF'
PRINT @newLine + 'COMMIT TRANSACTION'
