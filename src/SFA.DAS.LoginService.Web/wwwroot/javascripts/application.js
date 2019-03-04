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

        // document
        //   .querySelectorAll(".govuk-error-message")
        //   .forEach(function(message) {
        //     message.style.display = "none";
        //   });
        // document.querySelector(".govuk-error-summary").style.display = "none";

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
    }
  };

  GOVUK.showHidePassword = {
    init: function() {
      document.addEventListener("click", this.togglePassword);
    },

    togglePassword: function(event) {
      if (event.target.className.indexOf("js-show-hide") === -1) return false;
      event.preventDefault();

      var showHideLink = document.querySelector(".js-show-hide");
      var passwordInput = document.querySelector("#Password");
      var confirmPasswordInput = document.querySelector("#ConfirmPassword");

      if (passwordInput.type === "text") {
        showHideLink.innerText = "Show passwords";
        passwordInput.type = "password";
        confirmPasswordInput.type = "password";
      } else {
        showHideLink.innerText = "Hide passwords";
        passwordInput.type = "text";
        confirmPasswordInput.type = "text";
      }
    }
  };

  global.GOVUK = GOVUK;
})(window);

window.GOVUKFrontend.initAll();
// window.GOVUK.passwordConditions.init();
window.GOVUK.showHidePassword.init();
