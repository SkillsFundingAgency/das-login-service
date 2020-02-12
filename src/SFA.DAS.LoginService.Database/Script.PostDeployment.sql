
-- Insert new scopes for assessor and apply
IF NOT EXISTS(SELECT Id FROM [IdentityServer].[ClientScopes] WHERE Scope = 'profile' AND ClientId = 2)
BEGIN
  INSERT INTO [IdentityServer].[ClientScopes] ([Scope], [ClientId]) VALUES (N'profile', 2)
END

IF NOT EXISTS(SELECT Id FROM [IdentityServer].[ClientScopes] WHERE Scope = 'profile' AND ClientId = 3)
BEGIN
  INSERT [IdentityServer].[ClientScopes] ([Scope], [ClientId]) VALUES (N'profile', 3)
END

-- Backfill user givenname and familyname claims
INSERT INTO IdentityServer.AspNetUserClaims (ClaimType, ClaimValue, UserId)
SELECT 'family_name' AS ClaimType, users.FamilyName, users.Id 
FROM IdentityServer.AspNetUsers AS users
WHERE NOT EXISTS (SELECT UserId FROM IdentityServer.AspNetUserClaims claims WHERE claims.ClaimType = 'family_name' AND claims.UserId = users.Id)

INSERT INTO IdentityServer.AspNetUserClaims (ClaimType, ClaimValue, UserId)
SELECT 'given_name' AS ClaimType, users.GivenName, users.Id 
FROM IdentityServer.AspNetUsers AS users
WHERE NOT EXISTS (SELECT UserId FROM IdentityServer.AspNetUserClaims claims WHERE claims.ClaimType = 'given_name' AND claims.UserId = users.Id)

-- Insert LoginSignupError email template
UPDATE LoginService.Clients SET ServiceDetails = JSON_MODIFY(ServiceDetails, 'append $.EmailTemplates', JSON_QUERY('{"Name":"LoginSignupError", "TemplateId":"2b49c5be-43fc-4998-b40d-6bb5b4c1fcee"}'))
WHERE JSON_QUERY(ServiceDetails, '$.EmailTemplates[4]') IS NULL

UPDATE LoginService.Clients SET ServiceDetails = JSON_MODIFY(ServiceDetails, 'append $.EmailTemplates', JSON_QUERY('{"Name":"LoginSignupInvite", "TemplateId":"5805343e-8bfd-47e6-9a33-7e5e9b95c531"}'))
WHERE JSON_QUERY(ServiceDetails, '$.EmailTemplates[5]') IS NULL

-- Apply to be setup for RoATP
UPDATE LoginService.Clients
SET ServiceDetails =  JSON_MODIFY(JSON_MODIFY(ServiceDetails,'$.ServiceName','Register of apprenticeship training providers service'),'$.ServiceTeam','The Apprenticeship Service')
WHERE  IdentityServerClientId = 'apply'

-- setup Password Blacklist
:r Blacklist.sql




