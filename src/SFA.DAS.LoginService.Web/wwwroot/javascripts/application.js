(function(global) {
  "use strict";

  var GOVUK = global.GOVUK || {};

  GOVUK.passwordConditions = {
    init: function() {
      document.addEventListener("keyup", this.handlePassword);
    },

    handlePassword: function(event) {
      if (
        event.target.id !== "Password" &&
        event.target.id !== "ConfirmPassword"
      )
        return false;

      if (
        event.target.id === "Password" ||
        event.target.id === "ConfirmPassword"
      ) {
        var passwordInput = document.querySelector("#Password").value;
        var confirmPasswordInput = document.querySelector("#ConfirmPassword")
          .value;

        removeErrors();

        if (
          passwordInput === confirmPasswordInput &&
          passwordInput !== "" &&
          confirmPasswordInput !== ""
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
    // passwordInput: document.querySelector("[data-enhance='js-show-hide']"),
    init: function() {
      // if (this.passwordInput) {
      document.addEventListener("click", this.togglePassword);
      // }
    },

    togglePassword: function(event) {
      // var passwordInput = document.querySelector(
      //   "[data-enhance='js-show-hide']"
      // );
      // console.log(passwordInput);

      if (event.target.className.indexOf("js-show-hide") === -1) return false;
      event.preventDefault();

      var showHideLink = document.querySelector(".js-show-hide");
      var passwordInput = document.querySelector("#Password");
      var confirmPasswordInput = document.querySelector("#ConfirmPassword");

      if (passwordInput.type === "text") {
        passwordInput.type = "password";
        if (confirmPasswordInput) confirmPasswordInput.type = "password";

        showHideLink.innerText = confirmPasswordInput
          ? "Show passwords"
          : "Show password";
      } else {
        passwordInput.type = "text";
        if (confirmPasswordInput) confirmPasswordInput.type = "text";

        showHideLink.innerText = confirmPasswordInput
          ? "Hide passwords"
          : "Hide password";
      }
    }
  };

  global.GOVUK = GOVUK;
})(window);

window.GOVUKFrontend.initAll();
window.GOVUK.passwordConditions.init();
window.GOVUK.showHidePassword.init();
