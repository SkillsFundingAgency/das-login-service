window.GOVUKFrontend.initAll();
window.DASFrontend.passwordConditions.init();
window.DASFrontend.showHidePassword.init();

(function() {
  "use strict";

  var $cookieBanner = document.querySelector('[data-module="cookie-banner"]');
  if ($cookieBanner != null) {
    new CookieBanner($cookieBanner);
  }

  var $cookieSettings = document.querySelector(
    '[data-module="cookie-settings"]'
  );
  if ($cookieSettings != null) {
    var $cookieSettingsOptions = $cookieSettings.dataset.options;
    new CookieSettings($cookieSettings, $cookieSettingsOptions);
  }
})(window);
