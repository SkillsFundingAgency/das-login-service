-- Apply to be setup for RoATP
UPDATE LoginService.Clients
SET ServiceDetails =  JSON_MODIFY(JSON_MODIFY(ServiceDetails,'$.ServiceName','Register of apprenticeship training providers service'),'$.ServiceTeam','The Apprenticeship Service')
WHERE  IdentityServerClientId = 'apply'

-- setup Password Blacklist
:r Blacklist.sql




