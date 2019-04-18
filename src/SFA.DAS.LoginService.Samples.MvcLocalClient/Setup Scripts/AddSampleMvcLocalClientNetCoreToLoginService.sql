BEGIN TRANSACTION

	DECLARE @samplesmvclocalclientnetcore INT
	
	-------------------------------------------------------------------------------------------------------------------
	-- An Identity Server Client is an application which can be authenticatied by identity server
	-------------------------------------------------------------------------------------------------------------------
	INSERT INTO [IdentityServer].[Clients]
           ([Enabled]
           ,[ClientId]
           ,[ProtocolType]
           ,[RequireClientSecret]
           ,[ClientName]
           ,[Description]
           ,[ClientUri]
           ,[LogoUri]
           ,[RequireConsent]
           ,[AllowRememberConsent]
           ,[AlwaysIncludeUserClaimsInIdToken]
           ,[RequirePkce]
           ,[AllowPlainTextPkce]
           ,[AllowAccessTokensViaBrowser]
           ,[FrontChannelLogoutUri]
           ,[FrontChannelLogoutSessionRequired]
           ,[BackChannelLogoutUri]
           ,[BackChannelLogoutSessionRequired]
           ,[AllowOfflineAccess]
           ,[IdentityTokenLifetime]
           ,[AccessTokenLifetime]
           ,[AuthorizationCodeLifetime]
           ,[ConsentLifetime]
           ,[AbsoluteRefreshTokenLifetime]
           ,[SlidingRefreshTokenLifetime]
           ,[RefreshTokenUsage]
           ,[UpdateAccessTokenClaimsOnRefresh]
           ,[RefreshTokenExpiration]
           ,[AccessTokenType]
           ,[EnableLocalLogin]
           ,[IncludeJwtId]
           ,[AlwaysSendClientClaims]
           ,[ClientClaimsPrefix]
           ,[PairWiseSubjectSalt]
           ,[Created]
           ,[Updated]
           ,[LastAccessed]
           ,[UserSsoLifetime]
           ,[UserCodeType]
           ,[DeviceCodeLifetime]
           ,[NonEditable])
    VALUES
           (1 --<Enabled, bit,>
           ,'samples.mvclocalclient.netcore' --<ClientId, nvarchar(200),>
           ,'oidc' --<ProtocolType, nvarchar(200),>
           ,0 --<RequireClientSecret, bit,>
           ,'Example Implicit Client Application .Net Core' --<ClientName, nvarchar(200),>
           ,NULL --<Description, nvarchar(1000),>
           ,NULL --<ClientUri, nvarchar(2000),>
           ,NULL --<LogoUri, nvarchar(2000),>
           ,0 --<RequireConsent, bit,>
           ,1 --<AllowRememberConsent, bit,>
           ,0 --<AlwaysIncludeUserClaimsInIdToken, bit,>
           ,0 --<RequirePkce, bit,>
           ,0 --<AllowPlainTextPkce, bit,>
           ,0 --<AllowAccessTokensViaBrowser, bit,>
           ,NULL --<FrontChannelLogoutUri, nvarchar(2000),>
           ,1 --<FrontChannelLogoutSessionRequired, bit,>
           ,NULL --<BackChannelLogoutUri, nvarchar(2000),>
           ,1 --<BackChannelLogoutSessionRequired, bit,>
           ,0 --<AllowOfflineAccess, bit,>
           ,300 --<IdentityTokenLifetime, int,>
           ,3600 --<AccessTokenLifetime, int,>
           ,300 --<AuthorizationCodeLifetime, int,>
           ,NULL --<ConsentLifetime, int,>
           ,2592000 --<AbsoluteRefreshTokenLifetime, int,>
           ,1296000 --<SlidingRefreshTokenLifetime, int,>
           ,1 --<RefreshTokenUsage, int,>
           ,0 --<UpdateAccessTokenClaimsOnRefresh, bit,>
           ,1 --<RefreshTokenExpiration, int,>
           ,0 --<AccessTokenType, int,>
           ,1 --<EnableLocalLogin, bit,>
           ,0 --<IncludeJwtId, bit,>
           ,0 --<AlwaysSendClientClaims, bit,>
           ,'client_' --<ClientClaimsPrefix, nvarchar(200),>
           ,NULL --<PairWiseSubjectSalt, nvarchar(200),>
           ,GETDATE() --<Created, datetime2(7),>
           ,NULL --<Updated, datetime2(7),>
           ,NULL --<LastAccessed, datetime2(7),>
           ,NULL --<UserSsoLifetime, int,>
           ,NULL --<UserCodeType, nvarchar(100),>
           ,300 --<DeviceCodeLifetime, int,>
           ,0 --<NonEditable, bit,>
		   )

	SELECT @samplesmvclocalclientnetcore = SCOPE_IDENTITY()

	-------------------------------------------------------------------------------------------------------------------
	INSERT INTO [IdentityServer].[ClientPostLogoutRedirectUris]
           ([PostLogoutRedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:44376/signout-callback-oidc' --<PostLogoutRedirectUri, nvarchar(2000),>
           ,@samplesmvclocalclientnetcore --<ClientId, int,>
		   )
 
	-------------------------------------------------------------------------------------------------------------------
	INSERT INTO [IdentityServer].[ClientGrantTypes]
           ([GrantType]
           ,[ClientId])
     VALUES
           ('implicit' --<GrantType, nvarchar(250),>
           ,@samplesmvclocalclientnetcore --<ClientId, int,>
		   )

	-------------------------------------------------------------------------------------------------------------------
	INSERT INTO [IdentityServer].[ClientScopes]
           ([Scope]
           ,[ClientId])
	VALUES
           ('openid' --<Scope, nvarchar(200),>
           ,@samplesmvclocalclientnetcore --<ClientId, int,>
		   )

	INSERT INTO [IdentityServer].[ClientScopes]
           ([Scope]
           ,[ClientId])
	VALUES
           ('profile' --<Scope, nvarchar(200),>
           ,@samplesmvclocalclientnetcore --<ClientId, int,>
		   )

	-------------------------------------------------------------------------------------------------------------------
	INSERT INTO [IdentityServer].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
	VALUES
           ('https://localhost:44376/signin-oidc' --<RedirectUri, nvarchar(2000),>
           ,@samplesmvclocalclientnetcore --<ClientId, int,>
		   )

	-------------------------------------------------------------------------------------------------------------------
	-- The LoginService Client is additional information about the a client which 
	-------------------------------------------------------------------------------------------------------------------
	INSERT INTO [LoginService].[Clients]
           ([Id]
           ,[ServiceDetails]
           ,[IdentityServerClientId]
           ,[AllowInvitationSignUp]
           ,[AllowLocalSignUp])
	VALUES
           ('47390580-FD62-48D4-BDDF-7C722AADAB5F' --<Id, uniqueidentifier,>
		   ,N'{"ServiceName":"Sample Mvc Local Client .Net Core Service","ServiceTeam":"Sample Mvc Local Client .Net Core Service Team","CreateAccountPurpose":"to demonstrate local sign-up","SupportUrl":"https://acme.com/support","PostPasswordResetReturnUrl":"https://localhost:44376/","EmailTemplates":[{"Name":"SignUpInvitation","TemplateId":"a2fc2212-253e-47c1-b847-27c10f83f7f5"},{"Name":"PasswordReset","TemplateId":"ecbff8b8-3ad4-48b8-a42c-7d3f602dbbd3"},{"Name":"PasswordResetNoAccount","TemplateId":"04326941-2067-4956-8dc2-4ccd60c84af5"},{"Name":"LoginPasswordWasReset", "TemplateId":"fa156448-44d5-4d76-8407-685a609a14ca"},{"Name":"ConfirmEmail", "TemplateId":"e1786517-84de-4583-9bc3-a41ce9769ea3"}]}'  --<ServiceDetails, nvarchar(max),>
           ,'samples.mvclocalclient.netcore' --<IdentityServerClientId, nvarchar(250),>
           ,0 --<AllowInvitationSignUp, bit,>
           ,1 --<AllowLocalSignUp, bit,>
		   )
	     
COMMIT TRANSACTION