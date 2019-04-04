/*
	This script should only update the EmailConfirmed flag for already created users if there is no existing ConfirmEmailRequest; such as any [AspNetUser] which
	has been invited before the requirement to set the EmailConfirmed flag to true on invitation.

	Hash Prefix ConfirmRequest	Type of User
	0x01|0x02	Present			Created by Login service or confirmed after migration
	0x01|0x02	Missing         Invited by Login service
	0xFF		Present         Created by migration and not confirmed or not yet loged on after confirmation
	0xFF		Missing         Created by migration and not confirmed
*/
BEGIN TRY

    BEGIN TRANSACTION

	-- create a tempoarary helper function
    EXEC
		('CREATE FUNCTION [dbo].[DecodeBase64_CB12C69D-FDA9-4391-AFD0-C965402B3C10]
			(
				@base64String VARCHAR(MAX)
			)
			RETURNS VARBINARY(MAX)
			AS
			BEGIN
				RETURN CAST('''' as xml).value(''xs:base64Binary(sql:variable(''''@base64String''''))'', ''varbinary(max)'')
			END'
		)

    UPDATE 
		anu SET EmailConfirmed = 1 
	FROM
		[IdentityServer].[AspNetUsers] anu 
		LEFT JOIN [LoginService].[ConfirmEmailRequests] cer
			ON anu.Email = cer.Email
	WHERE 
		cer.Email IS NULL
		AND EmailConfirmed = 0
		AND SUBSTRING([dbo].[DecodeBase64_CB12C69D-FDA9-4391-AFD0-C965402B3C10](PasswordHash),1,1) <> 0xFF

	-- drop the temporary helper function
	EXEC('DROP FUNCTION [DecodeBase64_CB12C69D-FDA9-4391-AFD0-C965402B3C10]')

    COMMIT

END TRY
BEGIN CATCH
    IF XACT_STATE()!=0
    BEGIN
        -- if an error occurs the temporary helper function will be dropped by the transaction
		ROLLBACK TRANSACTION
    END
END CATCH
GO