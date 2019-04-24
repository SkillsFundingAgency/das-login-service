/*
	This master script is run on each database deployment; each child post deployment script will be run; and each must have been written
	such that it can run repeatably without affecting the integrity of an existing database; the script exists so that an existing database can
	be updated during deployment.

	REMOVE SCRIPTS WHICH ARE NO LONGER NEEDED
*/
:r .\PostDeploymentScripts\UpdateProfileClaims.sql

:r .\PostDeploymentScripts\UpdateEmailTemplates.sql

:r .\PostDeploymentScripts\UpdateEmailConfirmed.sql
