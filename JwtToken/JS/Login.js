$(function () {
    $("#btnLogin").click(function () {
        if($("#username").val() == "")
        {
            alert("请输入用户名");
            return;
        }
        if ($("#password").val() == "") {
            alert("请输入密码");
            return;
        }
        var objPara = {
            'UserCode': $("#username").val(),
            'Password': $("#password").val()
        }
        $.ajax({
            url: "/api/Login/CheckLogin",
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(objPara),
            success: function (data, textStatus, jqXHR) {
                $.cookie("Authorization", jqXHR.getResponseHeader("Authorization"), { expires: 1, path: '/' });
                //alert($.cookie("Authorization"));
                //alert(jqXHR.getResponseHeader("Authorization"));
                window.location = "/Home/Token";
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
    });
})