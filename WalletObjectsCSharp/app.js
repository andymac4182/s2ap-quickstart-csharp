/**
 * Save to Wallet success handler
 */
var successHandler = function(params){
  console.log("Object added successfully", params);
}

/**
 * Save to Wallet failure handler
 */
var failureHandler = function(params){
  console.log("Object insertion failed", params);
  var errorLi = $('<li>').text('Error: ' + JSON.stringify(params));
  $('#errors').append(errorLi);
}

/**
 * Initialization function
 */
function init(){
  document.getElementById("loyalty").addEventListener("click", function(){
    $.get("insert?type=loyalty", function(data){
      console.log("Loyalty", data);
    })
  });
  document.getElementById("offer").addEventListener("click", function(){
    $.get("insert?type=offer", function(data){
      console.log("Offer", data);
    })
  });
  document.getElementById("giftcard").addEventListener("click", function(){
    $.get("insert?type=giftcard", function(data){
      console.log("Gift Card", data);
    })
  });

  $.when(
        $.get("jwt?type=loyalty", function(data) {
          saveToAndroidPay = document.createElement("g:savetoandroidpay");
          saveToAndroidPay.setAttribute("theme", "light");
          saveToAndroidPay.setAttribute("jwt", data);
          saveToAndroidPay.setAttribute("onsuccess","successHandler");
          saveToAndroidPay.setAttribute("onfailure","failureHandler");
          document.querySelector("#loyaltysave").appendChild(saveToAndroidPay);
        }),
        $.get("jwt?type=offer", function(data) {
          saveToAndroidPay = document.createElement("g:savetoandroidpay");
          saveToAndroidPay.setAttribute("theme", "light");
          saveToAndroidPay.setAttribute("jwt", data);
          saveToAndroidPay.setAttribute("onsuccess","successHandler");
          saveToAndroidPay.setAttribute("onfailure","failureHandler");
          document.querySelector("#offersave").appendChild(saveToAndroidPay);
        }),
        $.get("jwt?type=giftcard", function(data) {
          saveToAndroidPay = document.createElement("g:savetoandroidpay");
          saveToAndroidPay.setAttribute("theme", "light");
          saveToAndroidPay.setAttribute("jwt", data);
          saveToAndroidPay.setAttribute("onsuccess","successHandler");
          saveToAndroidPay.setAttribute("onfailure","failureHandler");
          document.querySelector("#giftcardsave").appendChild(saveToAndroidPay);
      })).done(function() {
        script = document.createElement("script");
        script.src = "https://apis.google.com/js/plusone.js";
        document.head.appendChild(script);
      });
}


$(window).ready(function(){
  init();
});
