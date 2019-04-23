/*
	A new mandatory email template has been added; this will be added for each client which does not currently declare it.
*/
BEGIN TRY

    BEGIN TRANSACTION

	-- insert LoginSignupError email template for each client which does not current have this template
	UPDATE LoginService.Clients SET ServiceDetails = JSON_MODIFY(ServiceDetails, 'append $.EmailTemplates', JSON_QUERY('{"Name":"LoginSignupError", "TemplateId":"2b49c5be-43fc-4998-b40d-6bb5b4c1fcee"}'))
	WHERE JSON_QUERY(ServiceDetails, '$.EmailTemplates[4]') IS NULL	

    COMMIT

END TRY
BEGIN CATCH
    IF XACT_STATE()!=0
    BEGIN
		ROLLBACK TRANSACTION
    END
END CATCH
GO