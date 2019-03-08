(function(global) {
  "use strict";

  var GOVUK = global.GOVUK || {};

  GOVUK.passwordConditions = {
    passwordInput: document.querySelector("[data-input='password']"),
    confirmPasswordInput: document.querySelector("[data-input='confirm-password']"),

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
      this.passwordConditions(this.passwordInput.value)
      this.passwordsMatch(this.passwordInput.value, this.confirmPasswordInput.value)
    },

    handlePassword: function(event) {
      if (!event.target.dataset.input) return false;

      removeErrors();

      // Check for match on keyup of either #Password or #ConfirmPassword
      this.passwordsMatch(this.passwordInput.value, this.confirmPasswordInput.value)

      // Check other conditions only on keyup of #Password
      if (event.target.dataset.input === "password") {
        this.passwordConditions(this.passwordInput.value)
      }

      function removeErrors() {
        var errorSummary = document.querySelector(".govuk-error-summary");
        var errors = document.querySelectorAll(".govuk-error-message");
        var inputErrors = document.querySelectorAll(".govuk-input--error");
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

  GOVUK.showHidePassword = {
    passwordInput: document.querySelector("[data-show-hide='password']"),

    init: function() {
      if (!this.passwordInput) return false;
      document.addEventListener("click", this.togglePassword.bind(this));
    },

    togglePassword: function(event) {
      if (event.target.id !== "show-hide") return false;

      var confirmPasswordInput = document.querySelector("[data-show-hide='confirm-password']");
      var showHideLink = document.querySelector(".govuk-label--show-hide");

      if (this.passwordInput.type === "text") {
        this.passwordInput.type = "password";
        if (confirmPasswordInput) confirmPasswordInput.type = "password";
        showHideLink.innerText = "SHOW";
      } else {
        this.passwordInput.type = "text";
        if (confirmPasswordInput) confirmPasswordInput.type = "text";
        showHideLink.innerText = "HIDE";
      }
    }
  };

  global.GOVUK = GOVUK;
})(window);

window.GOVUKFrontend.initAll();
window.GOVUK.passwordConditions.init();
window.GOVUK.showHidePassword.init();
