-- START OF: Apply to be setup for RoATP
UPDATE LoginService.Clients
SET ServiceDetails =  JSON_MODIFY(JSON_MODIFY(ServiceDetails,'$.ServiceName','Register of apprenticeship training providers service'),'$.ServiceTeam','The Apprenticeship Service')
WHERE  IdentityServerClientId = 'apply'

-- SignUpInvitation
UPDATE LoginService.Clients
SET ServiceDetails = REPLACE(ServiceDetails,'a2fc2212-253e-47c1-b847-27c10f83f7f5','c3026bc0-a718-48bf-afc4-41bd390c5ba7')
WHERE  IdentityServerClientId = 'apply'

-- PasswordReset
UPDATE LoginService.Clients
SET ServiceDetails = REPLACE(ServiceDetails,'ecbff8b8-3ad4-48b8-a42c-7d3f602dbbd3','08176748-d991-4fca-bf60-e824aacb34dd')
WHERE  IdentityServerClientId = 'apply'

-- PasswordResetNoAccount
UPDATE LoginService.Clients
SET ServiceDetails = REPLACE(ServiceDetails,'04326941-2067-4956-8dc2-4ccd60c84af5','87c1174c-55bf-45ae-811f-919307e3b2af')
WHERE  IdentityServerClientId = 'apply'

-- LoginPasswordWasReset
UPDATE LoginService.Clients
SET ServiceDetails = REPLACE(ServiceDetails,'fa156448-44d5-4d76-8407-685a609a14ca','888481e3-dba5-439e-b3c3-4e1af4f72bfd')
WHERE  IdentityServerClientId = 'apply'

-- LoginSignupError
UPDATE LoginService.Clients
SET ServiceDetails = REPLACE(ServiceDetails,'2b49c5be-43fc-4998-b40d-6bb5b4c1fcee','31fa9456-f14f-4bd3-802a-1f4c9da7dfbe')
WHERE  IdentityServerClientId = 'apply'

-- END Of: Apply to be setup for RoATP


-- setup Password Blacklist
:r Blacklist.sql




