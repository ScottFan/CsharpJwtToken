$(function () {
    var token = $.cookie("Authorization");
    var td = $("#token tbody tr:eq(0) td:eq(1)").html();
    $("#token tbody tr:eq(1) td:eq(1)").html(token);
    $.ajax({
        url: "/Home/Token",
        type: "get",
        contentType: "application/text",
        dataType: "text",
        headers: {
            Authorization: token
        },
        success: function (data, textStatus, jqXHR) {
            //alert(jqXHR.getResponseHeader("Authorization"));
            $("#token tbody tr:eq(0) td:eq(1)").html(jqXHR.getResponseHeader("Authorization"));
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });
    $("#VerifyToken").click(function () {
        $.ajax({
            url: "/api/Login/VerifyToken",
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            headers: {
                Authorization: token
            },
            success: function (data, textStatus, jqXHR) {
                alert(data.Status);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(textStatus);
                alert(XMLHttpRequest.responseJSON.Message);
                window.location = "/Home/Index";
            }
        });
    })
})