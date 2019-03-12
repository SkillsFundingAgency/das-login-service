(function(global) {
    "use strict";

    var DASFrontend = global.DASFrontend || {};

    DASFrontend.passwordConditions = {
        passwordInput: document.querySelector("[data-password-conditions='password']"),
        confirmPasswordInput: document.querySelector(
            "[data-password-conditions='confirm-password']"
        ),

        init: function() {
            // quit if either password and confirm password don't exist
            if (!this.passwordInput || !this.confirmPasswordInput) return false;

            this.handlePageLoad();
            document.addEventListener("keyup", this.handlePassword.bind(this));
            document.addEventListener("change", this.handlePassword.bind(this));
        },

        passwordsMatch: function(passwordValue, confirmValue) {
            if (
                passwordValue === confirmValue &&
                passwordValue !== "" &&
                confirmValue !== ""
            ) {
                document
                    .querySelector("[data-condition='mustMatch']")
                    .classList.add("passed");
            } else {
                document
                    .querySelector("[data-condition='mustMatch']")
                    .classList.remove("passed");
            }
        },

        passwordConditions: function(passwordValue) {
            var conditions = [
                {
                    name: "minChars",
                    regex: /^.{8,}$/
                },
                {
                    name: "anyLetter",
                    regex: /.*[a-zA-Z].*/
                },
                {
                    name: "anyNumber",
                    regex: /.*[0-9].*/
                }
            ];
            conditions.forEach(condition => {
                var element = document.querySelector(
                    "[data-condition='" + condition.name + "']"
                );
                passwordValue.match(condition.regex)
                    ? element.classList.add("passed")
                    : element.classList.remove("passed");
            });
        },

        handlePageLoad: function() {
            this.passwordConditions(this.passwordInput.value);
            this.passwordsMatch(
                this.passwordInput.value,
                this.confirmPasswordInput.value
            );
        },

        handlePassword: function(event) {
            if (!event.target.dataset.passwordConditions) return false;

            removeErrors();

            // Check for match on keyup of either #Password or #ConfirmPassword
            this.passwordsMatch(
                this.passwordInput.value,
                this.confirmPasswordInput.value
            );

            // Check other conditions only on keyup of #Password
            if (event.target.dataset.passwordConditions === "password") {
                this.passwordConditions(this.passwordInput.value);
            }

            function removeErrors() {
                var errorSummary = document.querySelector(
                    ".govuk-error-summary"
                );
                var errors = document.querySelectorAll(".govuk-error-message");
                var inputErrors = document.querySelectorAll(
                    ".govuk-input--error"
                );
                var formGroupErrors = document.querySelectorAll(
                    ".govuk-form-group--error"
                );

                if (inputErrors) {
                    inputErrors.forEach(function(error) {
                        error.classList.remove("govuk-input--error");
                    });
                }

                if (formGroupErrors) {
                    formGroupErrors.forEach(function(error) {
                        error.classList.remove("govuk-form-group--error");
                    });
                }

                if (errorSummary) {
                    errorSummary.parentNode.removeChild(errorSummary);
                }

                if (errors) {
                    errors.forEach(function(message) {
                        message.parentNode.removeChild(message);
                    });
                }
            }
        }
    };

    global.DASFrontend = DASFrontend;
})(window);
