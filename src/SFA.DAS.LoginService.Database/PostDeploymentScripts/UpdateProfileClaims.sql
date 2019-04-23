/*
	The table [AspNetUserClaims] contains the claims which are automaically populated for the users identity context when they
	match a scope for the client which is connected.

	The claims in [AspNetUserClaims] are synchronized with the values in [AspNetUsers]
*/
BEGIN TRY

    BEGIN TRANSACTION

		-- insert scopes for assessor and apply which the profile identity resources
		IF NOT EXISTS(SELECT Id FROM [IdentityServer].[ClientScopes] WHERE Scope = 'profile' AND ClientId = 2)
		BEGIN
			INSERT INTO [IdentityServer].[ClientScopes] ([Scope], [ClientId]) VALUES (N'profile', 2)
		END

		IF NOT EXISTS(SELECT Id FROM [IdentityServer].[ClientScopes] WHERE Scope = 'profile' AND ClientId = 3)
		BEGIN
			INSERT [IdentityServer].[ClientScopes] ([Scope], [ClientId]) VALUES (N'profile', 3)
		END

		-- for existing users add claims for given_name and family_name
		INSERT INTO IdentityServer.AspNetUserClaims (ClaimType, ClaimValue, UserId)
		SELECT 'family_name' AS ClaimType, users.FamilyName, users.Id 
		FROM IdentityServer.AspNetUsers AS users
		WHERE NOT EXISTS (SELECT UserId FROM IdentityServer.AspNetUserClaims claims WHERE claims.ClaimType = 'family_name' AND claims.UserId = users.Id)

		INSERT INTO IdentityServer.AspNetUserClaims (ClaimType, ClaimValue, UserId)
		SELECT 'given_name' AS ClaimType, users.GivenName, users.Id 
		FROM IdentityServer.AspNetUsers AS users
		WHERE NOT EXISTS (SELECT UserId FROM IdentityServer.AspNetUserClaims claims WHERE claims.ClaimType = 'given_name' AND claims.UserId = users.Id)

    COMMIT

END TRY
BEGIN CATCH
    IF XACT_STATE()!=0
    BEGIN
		ROLLBACK TRANSACTION
    END
END CATCH
GO