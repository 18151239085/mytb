var clients = [];
$(function () {
    clients = $.clientsInit();
})
$.clientsInit = function () {
    var dataJson = {
        authorizedButtons: []
    };
    var init = function () {
        $.ajax({
            url: "/Account/GetAuthorizedItems",
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                dataJson.authorizedButtons = data.authorizedButtons;
            }
        });
    }
    init();
    return dataJson;
}