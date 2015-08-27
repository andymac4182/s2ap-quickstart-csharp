var successHandler = function(params){
  console.log("Object added successfully" + params);
}

var failureHandler = function(params){
  console.log("Object insertion failed" + params);
}

function init() {
    document.getElementById("loyalty").addEventListener("click", function () {
        $.get("insert?type=loyalty", function (data) {
            console.log(data);
        })
    });

    document.getElementById("offer").addEventListener("click", function () {
        $.get("insert?type=offer", function (data) {
            console.log(data);
        })
    });
}

$.when($.get("jwt?type=loyalty", function(data){
    saveToWallet = document.createElement("g:savetowallet");
    saveToWallet.setAttribute("jwt", data);
    saveToWallet.setAttribute("theme", "light");
    saveToWallet.setAttribute("onsuccess","successHandler");
    saveToWallet.setAttribute("onfailure","failureHandler");
    document.querySelector("#loyaltysave").appendChild(saveToWallet);}),
    $.get("jwt?type=offer", function(data){
    saveToWallet = document.createElement("g:savetowallet");
    saveToWallet.setAttribute("jwt", data);
    saveToWallet.setAttribute("theme", "light");
    saveToWallet.setAttribute("onsuccess","successHandler");
    saveToWallet.setAttribute("onfailure","failureHandler");
    document.querySelector("#offersave").appendChild(saveToWallet);})).done(function(){
        script = document.createElement("script");
        script.src = "https://apis.google.com/js/plusone.js";
        document.head.appendChild(script);
    });


$(window).ready(function(){
  init();
});