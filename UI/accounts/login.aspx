<%@ Page Title="" Language="C#" MasterPageFile="~/masters/SiteSkeleton.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="UI.accounts.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TITLE" runat="server">
    Login
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HEAD" runat="server">
    <script src="/js/jquery.validate.min.js"></script>
    <script src="/js/additional-methods.min.js"></script>
    <style>
        .form-box {
            margin: 50px auto;
            width: 350px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="MAINCONTENTPLACEHOLDER" runat="server">
    <div class="form-box" id="loginbox">
        <legend>Please Login</legend>
        <br />
        <fieldset>
            <label>Username</label>
            <input name="username" class="input" autocomplete="off" id="username" type="text" title="Enter your username" />
        </fieldset>
        <fieldset>
            <label>Password</label>
            <input name="password" class="input" autocomplete="off" id="password" type="password" title="Enter your password" />
        </fieldset>
        <br>
        <div id="checker">
            <input id="remember" type="checkbox" name="remember" value="remember">remember me
        </div>
        <br>
        <input class="button" value="Log In" id="loginbttn" type="submit" />
        <div id="dialog-message"></div>
        <div id="loading_dialog"></div>
    </div>

    <script type="text/javascript">
        var LOGINVALIDATION = {
            Test: function () {
                console.log("LOGINVALIDATION loaded successfully!");
            },
            DataCheck: function () {
                jQuery.validator.setDefaults({
                    debug: true,
                    success: "valid"
                });
                $("#aspForm").validate({
                    rules: {
                        username: {
                            required: true,
                            minlength: 6
                        },
                        password: {
                            required: true,
                            minlength: 6
                        }
                    },
                    messages: {
                        username: {
                            required: "Please enter your username",
                            minlength: "Username must be at least 6 characters long"
                        },
                        password: {
                            required: "Please provide a password",
                            minlength: "Password must be at least 6 characters long"
                        }
                    },
                    submitHandler: function (form) {
                        LOGINVALIDATION.Process();
                        return false;
                    }
                });
            },
            Process: function () {

                var usernamev = $("#username").val();
                var passwordv = $("#password").val();
                var remember = $("#remember").checked;
                var isRememberedv = false;
                if (remember === true) {
                    isRememberedv = true;
                } else {
                    isRememberedv = false;
                }
                var jsonObject = JSON.stringify({
                    username: usernamev,
                    password: passwordv,
                    isRemembered: isRememberedv
                });

                $.ajax({
                    type: "POST",
                    url: "login.aspx/Authenticate",
                    data: jsonObject,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        $("#loading_dialog").loading();
                    },
                    complete: function () {
                        $("#loading_dialog").loading("loadStop");
                    },
                    success: function (data) {
                        if (data.d === false) {
                            $("#dialog-message").html("<hr/><p>Failed to login. Verify that the details you entered are accurate</p><hr/>");
                            $("#dialog-message").dialog("open");
                        } else {
                            var url = '/default.aspx';
                            $(location).attr('href', url);
                        }
                    },
                    error: function (data) {
                        $("#loading_dialog").loading("loadStop");
                        var r = jQuery.parseJSON(data.responseText);
                        $("#dialog-message").html("<p>" + r.Message + "</p>");
                        $("#dialog-message").dialog("open");
                    }
                });
            },
            Init: function () {
                this.Test();
                this.DataCheck();
            }
        };
        $.ready(LOGINVALIDATION.Init());
    </script>

</asp:Content>
