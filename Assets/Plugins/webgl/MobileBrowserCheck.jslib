var MyPlugin = {
   IsMobile: function() {
      var userAgent = navigator.userAgent;
      var isMobile = (
         /\b(BlackBerry|webOS|iPhone|IEMobile)\b/i.test(userAgent) ||
         /\b(Android|Windows Phone|iPad|iPod)\b/i.test(userAgent) ||
         // iPad on iOS 13 detection
         (userAgent.includes("Mac") && "ontouchend" in document)
      );
      return isMobile;
   },
   SetAccessibilityText: function(newText) {
      var button = document.querySelector("#accessibility-button");
      if (button) {
         button.ariaLabel = UTF8ToString(newText);
         button.style.display = "flex";
      }else{
         console.log("Missing accessibility button");
      }
   }
};  
mergeInto(LibraryManager.library, MyPlugin);