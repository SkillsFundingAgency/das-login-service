(function(global) {
    "use strict";

    var DASFrontend = global.DASFrontend || {};

    DASFrontend.showHidePassword = {
        passwordInput: document.querySelector("[data-show-hide='password']"),

        init: function() {
            if (!this.passwordInput) return false;
            document.addEventListener("click", this.togglePassword.bind(this));
        },

        togglePassword: function(event) {
            if (event.target.id !== "show-hide") return false;

            var confirmPasswordInput = document.querySelector(
                "[data-show-hide='confirm-password']"
            );
            var showHideText = document.querySelector(
                ".govuk-label__show-hide-text"
            );

            if (this.passwordInput.type === "text") {
                this.passwordInput.type = "password";
                if (confirmPasswordInput)
                    confirmPasswordInput.type = "password";
                showHideText.innerText = "show";
            } else {
                this.passwordInput.type = "text";
                if (confirmPasswordInput) confirmPasswordInput.type = "text";
                showHideText.innerText = "hide";
            }
        }
    };

    global.DASFrontend = DASFrontend;
})(window);
