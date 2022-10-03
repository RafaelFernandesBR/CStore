//Constrói a URL depois que o DOM estiver pronto
document.addEventListener("DOMContentLoaded", function () {
    var url = encodeURIComponent(window.location.href); //url
    var title = encodeURIComponent(document.title); //título
    var telegramLink = "https://telegram.me/share/url?url=" + url + "&text=" + title;
    document.getElementById("telegram-share-btt").href = telegramLink;
}, false);
