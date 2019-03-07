(function(global) {
  "use strict";

  var GOVUK = global.GOVUK || {};

  GOVUK.passwordConditions = {
    init: function() {
      document.addEventListener("keyup", this.handlePassword);
      document.addEventListener("change", this.handlePassword);
    },

    handlePassword: function(event) {
      if (
        event.target.id !== "Password" &&
        event.target.id !== "ConfirmPassword"
      ) {
        return false;
      }

      var passwordInput = document.querySelector("#Password");
      var confirmPasswordInput = document.querySelector("#ConfirmPassword");

      // quit if either password and confirm password don't exist
      if (!passwordInput || !confirmPasswordInput) return false;

      removeErrors();

      // Check for match on keyup of either #Password or #ConfirmPassword
      if (
        event.target.id === "Password" ||
        event.target.id === "ConfirmPassword"
      ) {
        if (
          passwordInput.value === confirmPasswordInput.value &&
          passwordInput.value !== "" &&
          confirmPasswordInput.value !== ""
        ) {
          document
            .querySelector("[data-condition='mustMatch']")
            .classList.add("passed");
        } else {
          document
            .querySelector("[data-condition='mustMatch']")
            .classList.remove("passed");
        }
      }

      // Check other conditions only on keyup of #Password
      if (event.target.id === "Password") {
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
          event.target.value.match(condition.regex)
            ? element.classList.add("passed")
            : element.classList.remove("passed");
        });
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
    passwordInput: document.querySelector("[data-enhance='js-show-hide']"),

    init: function() {
      if (!this.passwordInput) return false;
      document.addEventListener("click", this.togglePassword.bind(this));
    },

    togglePassword: function(event) {
      if (event.target.id !== "show-hide") return false;

      var confirmPasswordInput = document.querySelector("#ConfirmPassword");
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
