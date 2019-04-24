/*
	This script will create initial setup data for a new login service database which has no clients; it uses the first two client of the service
	as the intial clients - these will only be added if the database currently has no clients to avoid adding duplicates.
*/
IF NOT EXISTS(SELECT * FROM [IdentityServer].[Clients])
BEGIN
  
  SET IDENTITY_INSERT [IdentityServer].[IdentityResources] ON 
  
  INSERT [IdentityServer].[IdentityResources] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [ShowInDiscoveryDocument], [Created], [Updated], [NonEditable]) 
  VALUES (1, 1, N'openid', N'Your user identifier', NULL, 1, 0, 1, CAST(N'2019-03-18T11:32:45.4856135' AS DateTime2), NULL, 0)
  
  INSERT [IdentityServer].[IdentityResources] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [ShowInDiscoveryDocument], [Created], [Updated], [NonEditable]) 
  VALUES (2, 1, N'profile', N'User profile', N'Your user profile information (first name, last name, etc.)', 0, 1, 1, CAST(N'2019-03-18T11:32:45.5529611' AS DateTime2), NULL, 0)
 
  INSERT [IdentityServer].[IdentityResources] ([Id], [Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [ShowInDiscoveryDocument], [Created], [Updated], [NonEditable]) 
  VALUES (3, 1, N'email', N'Your email address', NULL, 0, 1, 1, CAST(N'2019-03-18T11:32:45.5744577' AS DateTime2), NULL, 0)
 
  SET IDENTITY_INSERT [IdentityServer].[IdentityResources] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[IdentityClaims] ON 
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (1, N'sub', 1)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (2, N'updated_at', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (3, N'locale', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (4, N'zoneinfo', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (5, N'birthdate', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (6, N'gender', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (7, N'website', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (8, N'email', 3)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (9, N'picture', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (10, N'preferred_username', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (11, N'nickname', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (12, N'middle_name', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (13, N'given_name', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (14, N'family_name', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (15, N'name', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (16, N'profile', 2)
 
  INSERT [IdentityServer].[IdentityClaims] ([Id], [Type], [IdentityResourceId]) VALUES (17, N'email_verified', 3)
 
  SET IDENTITY_INSERT [IdentityServer].[IdentityClaims] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[ApiResources] ON 
 
  INSERT [IdentityServer].[ApiResources] ([Id], [Enabled], [Name], [DisplayName], [Description], [Created], [Updated], [LastAccessed], [NonEditable]) VALUES (1, 1, N'api1', N'My API 1', NULL, CAST(N'2019-03-18T11:32:45.9490670' AS DateTime2), NULL, NULL, 0)
 
  SET IDENTITY_INSERT [IdentityServer].[ApiResources] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[ApiScopes] ON 
 
  INSERT [IdentityServer].[ApiScopes] ([Id], [Name], [DisplayName], [Description], [Required], [Emphasize], [ShowInDiscoveryDocument], [ApiResourceId]) VALUES (1, N'api1', N'My API 1', NULL, 0, 0, 1, 1)
 
  SET IDENTITY_INSERT [IdentityServer].[ApiScopes] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[Clients] ON 
 
  INSERT [IdentityServer].[Clients] ([Id], [Enabled], [ClientId], [ProtocolType], [RequireClientSecret], [ClientName], [Description], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AlwaysIncludeUserClaimsInIdToken], [RequirePkce], [AllowPlainTextPkce], [AllowAccessTokensViaBrowser], [FrontChannelLogoutUri], [FrontChannelLogoutSessionRequired], [BackChannelLogoutUri], [BackChannelLogoutSessionRequired], [AllowOfflineAccess], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [ConsentLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [UpdateAccessTokenClaimsOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [ClientClaimsPrefix], [PairWiseSubjectSalt], [Created], [Updated], [LastAccessed], [UserSsoLifetime], [UserCodeType], [DeviceCodeLifetime], [NonEditable])
  VALUES (1, 1, N'client', N'oidc', 1, N'Client Credentials Client', NULL, NULL, NULL, 1, 1, 0, 0, 0, 0, NULL, 1, NULL, 1, 0, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, N'client_', NULL, CAST(N'2019-03-18T11:32:44.0555601' AS DateTime2), NULL, NULL, NULL, NULL, 300, 0)
 
  INSERT [IdentityServer].[Clients] ([Id], [Enabled], [ClientId], [ProtocolType], [RequireClientSecret], [ClientName], [Description], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AlwaysIncludeUserClaimsInIdToken], [RequirePkce], [AllowPlainTextPkce], [AllowAccessTokensViaBrowser], [FrontChannelLogoutUri], [FrontChannelLogoutSessionRequired], [BackChannelLogoutUri], [BackChannelLogoutSessionRequired], [AllowOfflineAccess], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [ConsentLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [UpdateAccessTokenClaimsOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [ClientClaimsPrefix], [PairWiseSubjectSalt], [Created], [Updated], [LastAccessed], [UserSsoLifetime], [UserCodeType], [DeviceCodeLifetime], [NonEditable]) 
  VALUES (2, 1, N'apply', N'oidc', 1, N'Apply Client', NULL, NULL, NULL, 0, 1, 0, 0, 0, 0, NULL, 1, NULL, 1, 0, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, N'client_', NULL, CAST(N'2019-03-18T11:32:44.3415692' AS DateTime2), NULL, NULL, NULL, NULL, 300, 0)
 
  INSERT [IdentityServer].[Clients] ([Id], [Enabled], [ClientId], [ProtocolType], [RequireClientSecret], [ClientName], [Description], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AlwaysIncludeUserClaimsInIdToken], [RequirePkce], [AllowPlainTextPkce], [AllowAccessTokensViaBrowser], [FrontChannelLogoutUri], [FrontChannelLogoutSessionRequired], [BackChannelLogoutUri], [BackChannelLogoutSessionRequired], [AllowOfflineAccess], [IdentityTokenLifetime], [AccessTokenLifetime], [AuthorizationCodeLifetime], [ConsentLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [UpdateAccessTokenClaimsOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [ClientClaimsPrefix], [PairWiseSubjectSalt], [Created], [Updated], [LastAccessed], [UserSsoLifetime], [UserCodeType], [DeviceCodeLifetime], [NonEditable]) 
  VALUES (3, 1, N'assessor', N'oidc', 1, N'Assessor Client', NULL, NULL, NULL, 0, 1, 0, 0, 0, 0, NULL, 1, NULL, 1, 0, 300, 3600, 300, NULL, 2592000, 1296000, 1, 0, 1, 0, 1, 0, 0, N'client_', NULL, CAST(N'2019-03-18T11:32:44.3804435' AS DateTime2), NULL, NULL, NULL, NULL, 300, 0)
 
  SET IDENTITY_INSERT [IdentityServer].[Clients] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[ClientPostLogoutRedirectUris] ON 
 
  INSERT [IdentityServer].[ClientPostLogoutRedirectUris] ([Id], [PostLogoutRedirectUri], [ClientId]) 
  VALUES (1, N'https://apply.apprenticeships.education.gov.uk/signout-callback-oidc', 2)
 
  INSERT [IdentityServer].[ClientPostLogoutRedirectUris] ([Id], [PostLogoutRedirectUri], [ClientId]) 
  VALUES (2, N'https://assessors.apprenticeships.education.gov.uk/signout-callback-oidc', 3)
 
  SET IDENTITY_INSERT [IdentityServer].[ClientPostLogoutRedirectUris] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[ClientGrantTypes] ON 
 
  INSERT [IdentityServer].[ClientGrantTypes] ([Id], [GrantType], [ClientId]) VALUES (1, N'client_credentials', 1)
 
  INSERT [IdentityServer].[ClientGrantTypes] ([Id], [GrantType], [ClientId]) VALUES (2, N'implicit', 2)
 
  INSERT [IdentityServer].[ClientGrantTypes] ([Id], [GrantType], [ClientId]) VALUES (3, N'implicit', 3)
 
  SET IDENTITY_INSERT [IdentityServer].[ClientGrantTypes] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[ClientScopes] ON 
 
  INSERT [IdentityServer].[ClientScopes] ([Id], [Scope], [ClientId]) VALUES (1, N'api1', 1)
 
  INSERT [IdentityServer].[ClientScopes] ([Id], [Scope], [ClientId]) VALUES (2, N'openid', 2)
  INSERT [IdentityServer].[ClientScopes] ([Id], [Scope], [ClientId]) VALUES (4, N'profile', 2)
 
  INSERT [IdentityServer].[ClientScopes] ([Id], [Scope], [ClientId]) VALUES (3, N'openid', 3)
  INSERT [IdentityServer].[ClientScopes] ([Id], [Scope], [ClientId]) VALUES (5, N'profile', 3)
 
  SET IDENTITY_INSERT [IdentityServer].[ClientScopes] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[ClientRedirectUris] ON 
 
  INSERT [IdentityServer].[ClientRedirectUris] ([Id], [RedirectUri], [ClientId]) VALUES (1, N'https://apply.apprenticeships.education.gov.uk/signin-oidc', 2)
 
  INSERT [IdentityServer].[ClientRedirectUris] ([Id], [RedirectUri], [ClientId]) VALUES (2, N'https://assessors.apprenticeships.education.gov.uk/signin-oidc', 3)
 
  SET IDENTITY_INSERT [IdentityServer].[ClientRedirectUris] OFF
 
  SET IDENTITY_INSERT [IdentityServer].[ClientSecrets] ON 
 
  INSERT [IdentityServer].[ClientSecrets] ([Id], [Description], [Value], [Expiration], [Type], [Created], [ClientId]) VALUES (1, NULL, N'fU7fRb+g6YdlniuSqviOLWNkda1M/MuPtH6zNI9inF8=', NULL, N'SharedSecret', CAST(N'2019-03-18T11:32:44.0560440' AS DateTime2), 1)
 
  SET IDENTITY_INSERT [IdentityServer].[ClientSecrets] OFF
 
  -- Additional Client Details for apply
  INSERT [LoginService].[Clients] ([Id], [ServiceDetails], [IdentityServerClientId], [AllowInvitationSignUp], [AllowLocalSignUp]) 
  VALUES (N'2350df68-e325-4ccc-9027-e1051e48d4a7', N'{"ServiceName":"Apprenticeship assessment service","ServiceTeam":"Apprenticeship assessment service team","SupportUrl":"https://apply.apprenticeships.education.gov.uk/","CreateAccountUrl": "https://apply.apprenticeships.education.gov.uk/Users/CreateAccount","PostPasswordResetReturnUrl":"https://apply.apprenticeships.education.gov.uk/","EmailTemplates":[{"Name":"SignUpInvitation","TemplateId":"a2fc2212-253e-47c1-b847-27c10f83f7f5"},{"Name":"PasswordReset","TemplateId":"ecbff8b8-3ad4-48b8-a42c-7d3f602dbbd3"},{"Name":"PasswordResetNoAccount","TemplateId":"04326941-2067-4956-8dc2-4ccd60c84af5"},{"Name":"LoginPasswordWasReset", "TemplateId":"fa156448-44d5-4d76-8407-685a609a14ca"},{"Name":"LoginSignupError", "TemplateId":"2b49c5be-43fc-4998-b40d-6bb5b4c1fcee"}]}', N'apply', 1, 0)
 
  -- Additional Client Details for assessor
  INSERT [LoginService].[Clients] ([Id], [ServiceDetails], [IdentityServerClientId], [AllowInvitationSignUp], [AllowLocalSignUp]) 
  VALUES (N'08372e20-becd-415c-9925-4d33ddf67faf', N'{"ServiceName":"Apprenticeship assessment service","ServiceTeam":"Apprenticeship assessment service team","SupportUrl":"https://assessors.apprenticeships.education.gov.uk/","CreateAccountUrl": "https://assessors.apprenticeships.education.gov.uk/Account/CreateAnAccount","PostPasswordResetReturnUrl":"https://assessors.apprenticeships.education.gov.uk/","EmailTemplates":[{"Name":"SignUpInvitation","TemplateId":"a2fc2212-253e-47c1-b847-27c10f83f7f5"},{"Name":"PasswordReset","TemplateId":"ecbff8b8-3ad4-48b8-a42c-7d3f602dbbd3"},{"Name":"PasswordResetNoAccount","TemplateId":"04326941-2067-4956-8dc2-4ccd60c84af5"},{"Name":"LoginPasswordWasReset", "TemplateId":"fa156448-44d5-4d76-8407-685a609a14ca"},{"Name":"LoginSignupError", "TemplateId":"2b49c5be-43fc-4998-b40d-6bb5b4c1fcee"}]}', N'assessor', 1, 0)
 
END
GO