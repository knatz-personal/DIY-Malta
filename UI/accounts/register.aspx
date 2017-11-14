<%@ Page Title="" Language="C#" MasterPageFile="~/masters/SiteSkeleton.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="UI.accounts.register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TITLE" runat="server">
    Register
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HEAD" runat="server">
    <script src="/js/jquery.validate.min.js"></script>
    <script src="/js/additional-methods.min.js"></script>
    <style>
        .form-box {
            margin: 50px auto;
            width: 350px;
        }

        .ui-state-highlight {
            clear: both;
            min-height: 60px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MAINCONTENTPLACEHOLDER" runat="server">
    <div class="form-box">
        <fieldset>
            <legend>Create an Account</legend>
            <fieldset>
                <ol>
                    <li>
                        <div id="summary" style="color: orangered; font-size: large; font-weight: 900; margin: 10px 0;"></div>
                    </li>
                    <li>
                        <label>Name</label>
                        <input name="name" class="input" id="name" type="text"
                            title="Enter your first name">
                    </li>
                    <li>
                        <label>Middle Initials(optional) </label>
                        <input name="middle" class="input" id="middle" type="text"
                            title="Enter your middle name initial(s)">
                    </li>
                    <li>
                        <label>Surname</label>
                        <input name="surname" class="input" id="surname" type="text" title="Enter your last name">
                    </li>
                    <li>
                        <label>Gender</label>

                        <select id="gender" name="gender" title="Select a gender option">
                            <option value="">Select</option>
                        </select>
                    </li>
                    <li>
                        <label>Date Of Birth e.g 1980-10-25</label>
                        <input name="dob" class="input datepicker" id="dob" type="text" title="Please provide your date of birth">
                    </li>
                </ol>
            </fieldset>
            <fieldset>
                <legend>Address Information</legend>
                <ol>
                    <li>
                        <label>Address Type</label>

                        <select id="addressType" name="addressType" title="Select a address type option">
                            <option value="">Select</option>
                        </select>
                    </li>
                    <li>
                        <label>Address</label>
                        <input name="address" class="input" id="address" type="text" title="Please provide your address">
                    </li>
                    <li>
                        <label>Street</label>
                        <input name="street" class="input" id="street" type="text" title="Please provide your street">
                    </li>
                    <li>
                        <label>Post Code e.g PLA1818</label>
                        <input name="postcode" class="input" id="postcode" type="text" title="Please provide your post code">
                    </li>
                    <li>
                        <label>Town</label>

                        <select id="town" name="town" title="Select a town option">
                            <option value="">Select</option>
                        </select>
                    </li>
                    <li>&nbsp;</li>
                </ol>
            </fieldset>
            <fieldset>
                <legend>Contact Information</legend>
                <ol>
                    <li>
                        <label>Contact Type</label>

                        <select id="contactType" name="contactType" title="Select a contact type option">
                            <option value="">Select</option>
                        </select>
                    </li>
                    <li>
                        <label>Mobile</label>
                        <input name="mobile" class="input" id="mobile" type="text" title="Enter your mobile number">
                    </li>
                    <li>
                        <label>Telephone</label>
                        <input name="phone" class="input" id="phone" type="text" title="Enter your telephone number">
                    </li>
                    <li>
                        <label>Email</label>
                        <input name="email" class="input" id="email" type="text" title="Enter your email address">
                    </li>
                    <li>&nbsp;</li>
                </ol>
            </fieldset>
            <fieldset>
                <legend>Membership Information</legend>
                <ol>
                    <li>
                        <label>Username</label>
                        <input name="username" class="input" id="username" type="text" title="Enter your username">
                    </li>
                    <li>
                        <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                                <br />

                                Passwords are required to be a minimum of 6 characters in length.
                            </p>
                        </div>
                        <label>Password</label>
                        <input name="password" class="input" id="password" type="password" title="Enter a password">
                    </li>
                    <li>
                        <label>Password Confirmation</label>
                        <input name="confirmpassword" class="input" id="confirmpassword" type="password" title="Confirm the password above">
                    </li>
                    <li>&nbsp;</li>
                    <li>
                        <input type="submit" value="Create Account" id="registerbttn" />
                    </li>
                </ol>
                <div id="dialog-message"></div>
                <div id="loading_dialog"></div>
            </fieldset>
        </fieldset>
    </div>
    <script type="text/javascript">
        var REGISTERVALIDATION = {
            Test: function () {
                console.log("REGISTERVALIDATION loaded successfully!");
            },
            Dropdowns: function () {

                function loadList(method, identifier) {
                    $.ajax({
                        type: "POST",
                        url: "register.aspx/" + method,
                        data: JSON.stringify({}),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        cache: false,
                        success: function (data) {
                            var list = data.d;
                            $.each(list, function (key, value) {
                                $(identifier).append($("<option></option>").val
                                (value.ID).html(value.Name));
                            });
                        }
                    });
                }

                loadList("LoadGenderList", "#gender");
                loadList("LoadTownList", "#town");
                loadList("LoadContactTypeList", "#contactType");
                loadList("LoadAddressTypeList", "#addressType");
            },
            DataCheck: function () {
                $.validator.setDefaults({
                    debug: true,
                    success: "valid"
                });
                $.validator.addMethod("emailCheck", function (value, element) {
                    var result = "undefined";

                    var foo = $.ajax(
                    {
                        url: "register.aspx/DoesEmailExist",
                        type: "post",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        data: JSON.stringify({
                            email: value
                        }),
                        dataFilter: function (data) {
                            var msg = JSON.parse(data);
                            if (msg.hasOwnProperty('d')) {
                                return msg.d;
                            } else {
                                return msg;
                            }
                        }
                    });
                    result = foo.responseJSON;
                    return result;
                }, 'This email is already in use. Please try again.');

                $.validator.addMethod("usernameCheck", function (value, element) {
                    var result = "undefined";
                    var bar = $.ajax(
                    {
                        type: "POST",
                        url: "register.aspx/IsUsernameTaken",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        data: JSON.stringify({
                            username: value
                        }),
                        dataFilter: function (data) {
                            var msg = JSON.parse(data);
                            if (msg.hasOwnProperty('d'))
                                return msg.d;
                            else
                                return msg;
                        }
                    });
                    result = bar.responseJSON;
                    return result;
                }, "This username is already in use. Please try again.");

                $("#aspForm").validate({
                    ignore: [],
                    ignoreTitle: true,
                    errorPlacement: function (error, element) {
                        var target = element.attr("name");
                        if (element.is("select")) {
                            error.insertAfter("#" + target + "-button");
                        } else {
                            error.insertAfter(element);
                        }
                    },
                    showErrors: function (errorMap, errorList) {
                        $("#summary").html("<p>Your form contains "
                            + this.numberOfInvalids()
                            + " errors,<br /> see details below.</p>");
                        this.defaultShowErrors();
                    },
                    rules: {
                        name: {
                            required: true
                        },
                        surname: {
                            required: true
                        },
                        dob: {
                            required: true,
                            date: true
                        },
                        address: {
                            required: true
                        },
                        street: {
                            required: true
                        },
                        postcode: {
                            required: true,
                            minlength: 7,
                            maxlength: 7,
                            pattern: /[A-Z]{3}[0-9]{4}/i
                        },
                        phone: {
                            required: true,
                            digits: true,
                            rangelength: [8, 13],
                            pattern: /(00356)?(21|27|22|25)[0-9]{6}/i
                        },
                        mobile: {
                            required: true,
                            digits: true,
                            rangelength: [8, 13],
                            pattern: /(00356)?(99|79|77)[0-9]{6}/i
                        },
                        username: {
                            required: true,
                            minlength: 6,
                            usernameCheck: true
                        },
                        email: {
                            required: true,
                            pattern: /^[a-zA-Z0-9._-]+@[a-zA-Z0-9-]+\.[a-zA-Z.]{2,5}$/i,
                            emailCheck: true
                        },

                        password: {
                            required: true,
                            minlength: 6
                        },
                        confirmpassword: {
                            required: true,
                            minlength: 6,
                            equalTo: "#password"
                        },
                        gender: {
                            required: true,
                            digits: true
                        },
                        addressType: {
                            required: true,
                            digits: true
                        },
                        town: {
                            required: true,
                            digits: true
                        },
                        contactType: {
                            required: true,
                            digits: true
                        }
                    },
                    messages: {
                        name: {
                            required: "Please provide your first name"
                        },
                        surname: {
                            required: "Please provide your last name"
                        },
                        dob: {
                            required: "Please provide your date of birth",
                            date: "Please provide a valid date of birth (year-month-day e.g 2010-01-20)"
                        },
                        address: {
                            required: "Please provide the name or number of your house/flat/apartment"
                        },
                        street: "Please provide the name of your street",
                        postcode: {
                            required: "Please provide your postal code",
                            pattern: "Please provide a valid Malta postal code"
                        },
                        phone: {
                            required: "Please provide a telephone number",
                            digits: "Only numbers are allowed",
                            pattern: "Please provide a valid Malta telephone number"
                        },
                        mobile: {
                            required: "Please provide a mobile phone number",
                            digits: "Only numbers are allowed",
                            pattern: "Please provide a valid Malta mobile phone "
                        },
                        username: {
                            required: "Please enter your username",
                            minlength: "Username must be at least 6 characters long"
                        },
                        email: {
                            required: "Please provide an email address",
                            pattern: "Please provide a valid email"
                        },
                        password: {
                            required: "Please provide a password",
                            minlength: "Password must be at least 6 characters long"
                        },
                        confirmpassword: {
                            required: "Please provide a password",
                            minlength: "Password must be at least 6 characters long",
                            equalTo: "Please enter the same password as above"
                        },
                        gender: {
                            required: "Please select a gender"
                        },
                        addressType: {
                            required: "Please select a address type"
                        },
                        town: {
                            required: "Please select a town"
                        },
                        contactType: {
                            required: "Please select a contact type"
                        }
                    },
                    submitHandler: function (form) {
                        REGISTERVALIDATION.Process();
                        return false;
                    }
                }
                );
            },
            Process: function () {
                var jsonObject = JSON.stringify({
                    name: $("#name").val(),
                    middle: $("#middle").val(),
                    surname: $("#surname").val(),
                    street: $("#street").val(),
                    email: $("#email").val(),
                    dob: $("#dob").val(),
                    phone: $("#phone").val(),
                    mobile: $("#mobile").val(),
                    address: $("#address").val(),
                    postCode: $("#postcode").val(),
                    addressType: $("#addressType").val(),
                    contactType: $("#contactType").val(),
                    genderId: $("#gender").val(),
                    townId: $("#town").val(),
                    username: $("#username").val(),
                    password: $("#password").val()
                });

                $.ajax({
                    type: "POST",
                    url: "register.aspx/Register",
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
                        if (data.d.result === false) {
                            $("#summary").empty().show();
                            $("#summary").html("<p>Failed to create an account. Verify that the details you entered are accurate</p>" + data.d.message);
                        } else {
                            $("#summary").empty().hide();
                            var url = '/default.aspx';
                            $(location).attr('href', url);
                        }
                    },
                    error: function (data) {
                        var r = jQuery.parseJSON(data.responseText);
                        $("#dialog-message").html("<hr /><p>" + r.Message + "</p><hr />");
                        $("#dialog-message").dialog("open");
                    }
                });

            },
            Init: function () {

                this.Test();
                this.Dropdowns();
                this.DataCheck();
                $("select").selectmenu({
                    style: 'dropdown',
                    transferClasses: true,
                    width: 300,
                    change: function () {
                        $("#aspForm").validate().element(this);
                    }
                }).selectmenu("menuWidget").addClass("overflow");

            }
        };
        $.ready(REGISTERVALIDATION.Init());
    </script>
</asp:Content>