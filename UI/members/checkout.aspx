<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Dashboard.master" AutoEventWireup="true" CodeBehind="checkout.aspx.cs" Inherits="UI.members.checkout" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TITLE2" runat="server">
    Checkout Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HEAD2" runat="server">
    <link href="/css/listviewStyle.css" rel="stylesheet" />
    <script src="/js/jquery.validate.min.js"></script>
    <script src="/js/additional-methods.min.js"></script>
    <style type="text/css">
        .form-box {
            margin: 0 auto;
            width: 70%;
        }

        .center {
            margin: 0 auto;
            margin: 0;
            padding: 0;
            text-align: center;
        }

        .left {
            float: left;
            height: 20px;
            line-height: 20px;
        }

        #radioFieldset {
            height: 25px;
            margin: 0;
            overflow: hidden;
            padding: 10px;
        }

            #radioFieldset input[type="radio"] {
                height: 20px;
            }

            #radioFieldset label {
                margin: 0;
                padding: 0;
            }

        fieldset {
            margin: 10px auto;
            width: 90%;
        }

        .module header, .module footer {
            padding: 0;
            width: 100%;
        }

        input[type="text"] {
            -ms-box-shadow: 2px 2px 2px 2px #545454;
            -webkit-box-shadow: 2px 2px 2px 2px #545454;
            box-shadow: 2px 2px 2px 2px #545454;
            float: left;
            margin-right: 10px;
            padding: 10px;
            width: 300px;
        }

        #checkoutFooter {
            -moz-border-radius-bottomleft: 5px;
            -moz-border-radius-bottomright: 5px;
            -webkit-border-bottom-left-radius: 5px;
            -webkit-border-bottom-left-radius: 5px;
            -webkit-border-bottom-right-radius: 5px;
            -webkit-border-bottom-right-radius: 5px;
            background: #F1F1F4 url('/img/module_footer_bg.png') repeat-x;
            border-top: 1px solid #9CA1B0;
            height: 40px;
            padding: 5px 0;
            width: 100%;
        }

            #checkoutFooter input[type='submit'] {
                -moz-border-radius: 4px;
                -webkit-border-radius: 4px;
                background: #fca746;
                background-image: -webkit-linear-gradient(top, #fca746, #ff8c00);
                background-image: -moz-linear-gradient(top, #fca746, #ff8c00);
                background-image: -ms-linear-gradient(top, #fca746, #ff8c00);
                background-image: -o-linear-gradient(top, #fca746, #ff8c00);
                background-image: linear-gradient(to bottom, #fca746, #ff8c00);
                border-radius: 4px;
                float: right;
                font-family: Arial;
                font-size: 14px;
                margin-right: 5px;
                padding: 10px 20px 10px 20px;
                text-decoration: none;
            }

                #checkoutFooter input[type='submit']:hover {
                    background: #3cb0fd;
                    background-image: -webkit-linear-gradient(top, #3cb0fd, #3498db);
                    background-image: -moz-linear-gradient(top, #3cb0fd, #3498db);
                    background-image: -ms-linear-gradient(top, #3cb0fd, #3498db);
                    background-image: -o-linear-gradient(top, #3cb0fd, #3498db);
                    background-image: linear-gradient(to bottom, #3cb0fd, #3498db);
                    text-decoration: none;
                }

                 .SaleMarker {
            position: absolute;
            top: 10px;
            right: 10px;
            font-size: 1.2em;
            margin: 0;
            padding: 0;
        }

            .SaleMarker,
            .SaleMarker:active,
            .SaleMarker:hover {
                color: rgba(255, 255, 255, 1);
                -ms-border-radius: 30px;
                border-radius: 30px;
                display: inline-block;
                height: 60px;
                line-height: 60px;
                text-align: center;
                text-transform: capitalize;
                width: 60px;
                background: #016dba;
                -moz-transition: background .4s ease-out;
                -o-transition: background .4s ease-out;
                -webkit-transition: background .4s ease-out;
                -ms-transition: background .4s ease-out;
                transition: background .4s ease-out;
            }

                .SaleMarker:hover {
                    background: #373785;
                    text-transform: capitalize;
                }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MAINCONTENTPLACEHOLDER2" runat="server">
    <div class="form-box">
        <asp:ListView ID="ListViewCheckout" runat="server">
            <LayoutTemplate>
                <article class="module">
                    <header>
                        <h3>Order Summary</h3>
                    </header>
                    <table class="tablesorter" cellspacing="0">
                        <thead>
                            <tr>
                                <th style="text-align: center; width: 175px;">Image</th>
                                <th style="text-align: center;">Name</th>
                                <th style="border-left: 1px dashed gainsboro; text-align: center; width: 100px;">Total Price</th>
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 175px;"></td>
                                <td style="text-align: center;"></td>
                                <td style="border-left: 1px dashed gainsboro; text-align: center; width: 100px;"></td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                        </tbody>
                        <tfoot>
                            <tr style="background: #fff;">
                                <td colspan="5">
                                    <div>
                                        <div class="center">
                                            <span>We accept</span>&nbsp;
                                            <img src="/img/skeleton/yes.png" alt="Yes Money Card" style="width: 24px;" />&nbsp;
                                            <img src="/img/skeleton/bov.gif" alt="BOV" />&nbsp;
                                            <img src="/img/skeleton/hsbc.gif" alt="HSBC" />&nbsp;
                                            <img src="/img/skeleton/mastercard.gif" alt="Mastercard" />&nbsp;
                                            <img src="/img/skeleton/visa.gif" alt="VISA" />&nbsp;
                                        </div>
                                        <fieldset>
                                            <fieldset>
                                                <div id="summary" style="color: orangered; font-size: large; font-weight: 900; margin: 10px 0;"></div>
                                            </fieldset>
                                            <legend>Payment</legend>
                                            <fieldset>
                                                <label for="payment_type">Payment Options</label>
                                                <select id="payment_type" name="payment_type">
                                                    <option value="-1">Select</option>
                                                    <option value="1">Cash</option>
                                                    <option value="2">Credit Card</option>
                                                </select>
                                            </fieldset>

                                            <fieldset id="cash">
                                                <label>Cash Options</label>
                                                <fieldset id="radioFieldset">
                                                    <input id="pickup" type="radio" name="cash" value="pickup" class="left"><label for="pickup" class="left">Cash On Pickup</label>
                                                    <input id="delivery" type="radio" name="cash" value="delivery" class="left"><label for="delivery" class="left">Cash On Delivery</label>
                                                </fieldset>
                                            </fieldset>

                                            <fieldset id="cards">
                                                <div id="card_detail">

                                                    <legend style="margin-top: 20px;">Card Details</legend>
                                                    <fieldset>
                                                        <label for="card_type">Card Options</label>
                                                        <select id="card_type" name="card_type">
                                                            <option value="">Select</option>
                                                            <option>Visa</option>
                                                            <option>BOV Cashlink</option>
                                                            <option>Mastercard</option>
                                                            <option>Yes card</option>
                                                        </select>
                                                    </fieldset>
                                                    <fieldset>
                                                        <label for="cardholder_name">Cardholder Name</label>
                                                        <p class="intromessage" style="clear: both; margin: 0; margin-bottom: 5px; width: 322px;">
                                                            As spelled on your card
                                                        </p>
                                                        <input type="text" id="cardholder_name" name="cardholder_name" />
                                                        <br />
                                                    </fieldset>
                                                    <fieldset>
                                                        <label for="card_number">Card Number</label>
                                                        <input type="text" id="card_number" name="card_number" />
                                                    </fieldset>
                                                    <fieldset>
                                                        <label for="iban_number">IBAN Number</label>
                                                        <input type="text" id="iban_number" name="iban_number" />
                                                    </fieldset>
                                                    <fieldset>
                                                        <label for="expiry_date">Expiry Date</label>
                                                        <input type="text" id="expiry_date" class="datepicker" name="expiry_date" />
                                                    </fieldset>
                                                    <fieldset>
                                                        <label for="security_code">Security Code</label>
                                                        <input type="text" id="security_code" name="security_code" />
                                                    </fieldset>
                                                </div>
                                            </fieldset>
                                        </fieldset>
                                    </div>
                                </td>
                            </tr>
                            <tr style="background: cornsilk; height: 100px; overflow: hidden;">
                                <td colspan="5" style="font-size: large; font-weight: 600; padding-right: 10px; text-align: right;">
                                    <div>Sub-Total: € <span id="subTotal">0.00</span></div>
                                    <br />
                                    <div>Delivery:  € <span id="deliveryfee">0.00</span> [Free]</div>
                                    <br />
                                    <div>Tax:  € <span id="tax">0.00</span></div>
                                    <br />
                                    <div>Grand Total: € <span id="grandTotal">0.00</span> incl. Tax</div>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                    <footer id="checkoutFooter">
                        <a href="/default.aspx" title="Continue shopping" class="linkButton">Continue shopping</a>
                        <input type="submit" value="Place order" title="Place your order" />
                    </footer>
                </article>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <div style="background: #fff; padding: 20px; text-align: center;">Your shopping cart is empty</div>
            </EmptyDataTemplate>
            <ItemTemplate>
                <tr>
                    <td style="text-align: center; width: 175px;">
                          <%# (decimal)Eval("DiscountPrice") != 0 ?  "<div style='position:relative;'><span class='SaleMarker'>Sale</span></div>" : "" %>
                        <a href="/detail.aspx?productId=<%# Eval("ProductID") %>">
                        <img src="<%# Eval("Image") %>" alt="Image of <%# Eval("Name") %>" style="width: 175px;" /></a>
                    </td>
                    <td style="text-align: center;"><span><%# Eval("Name") %></span></td>
                    <td style="border-left: 1px dashed gainsboro; text-align: center;"><span id="total"><%#  String.Format(new CultureInfo("MT"), "{0:C}",Eval("TotalPrice")) %></span><br />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
      <div id="loading_dialog"></div>
    <script type="text/javascript">
        var CHECKOUT = {
            Process: function () { },
            PlaceOrder: function () {
                $.ajax({
                    type: "POST",
                    url: "checkout.aspx/PlaceOrder",
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    dataType: "json",
                    data: JSON.stringify({}),
                    beforeSend: function () {
                        $("#loading_dialog").loading();
                    },
                    complete: function () {
                        $("#loading_dialog").loading("loadStop");
                    },
                    success: function (data) {
                        var url = '/members/index.aspx';
                        $(location).attr('href', url);
                    },
                    error: function (data) {
                        $("#loading_dialog").loading("loadStop");
                        var r = jQuery.parseJSON(data.responseText);
                        $("#dialog-message").html("<p>" + r.Message + "</p>");
                        $("#dialog-message").dialog("open");
                    }
                });
            },
            CardInputCheck: function () {

                validator = form.validate({
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
                        $("#summary").show();
                        $("#summary").html("<p>Form contains "
                            + this.numberOfInvalids()
                            + " errors, see details below.</p>");
                        this.defaultShowErrors();
                    },
                    rules: {
                        card_type: { required: true },
                        cardholder_name: { required: true },
                        card_number: { required: true },
                        iban_number: { required: true },
                        expiry_date: { required: true },
                        security_code: { required: true }
                    },
                    submitHandler: function (form) {

                        CHECKOUT.PlaceOrder();
                           
                       
                        return false;
                    }
                });
            },
            CheckoutSummary: function () {
                $.ajax({
                    type: "POST",
                    url: "checkout.aspx/GetCheckoutSummary",
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    dataType: "json",
                    data: JSON.stringify({}),
                    success: function (data) {
                        $("#subTotal").text(JSON.parse(data.d).SubTotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                        $("#tax").text(JSON.parse(data.d).Tax.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                        $("#grandTotal").text(JSON.parse(data.d).GrandTotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                    }
                });
            },
            UX: function () {
                $("select").val('-1');
                $("#cards").hide();
                $("#cash").hide();

                $("#payment_type").selectmenu({
                    style: 'dropdown',
                    transferClasses: true,
                    width: 355,
                    change: function () {
                        var optionSelected = $(this).find("option:selected");
                        var valueSelected = optionSelected.val();

                        if (valueSelected === '-1') {
                            $("#cards").hide();
                            $("#cash").hide();
                            $("#summary").hide();
                            form[0].reset();
                        } else if (valueSelected === '1') {
                            $("#cash").show();
                            $("#cards").hide();
                            $("#summary").hide();
                            $("#pickup").attr("checked", "checked");
                            form.submit(function(event) {
                                CHECKOUT.PlaceOrder();
                                event.preventDefault();
                            });
                        } else if (valueSelected === '2') {
                            $("#cards").show();
                            $("#cash").hide();
                            CHECKOUT.CardInputCheck();
                        }
                    }
                }).selectmenu("menuWidget").addClass("overflow");

                var card_type = $("#card_type").selectmenu({
                    style: 'dropdown',
                    transferClasses: true,
                    width: 324,
                    change: function () {
                        var optionSelected = $(this).find("option:selected");
                        form.validate().element(this);
                        return optionSelected.val();
                    }
                }).selectmenu("menuWidget").addClass("overflow");
            },
            Test: function () {
                console.log("CHECKOUT loaded successfully");
            },
            Init: function () {
                this.Test();
                this.UX();
                this.CheckoutSummary();
                jQuery.validator.setDefaults({
                    debug: true,
                    success: "valid"
                });
                form = $("#aspForm");

            }
        };
        $(function () {
            CHECKOUT.Init();
        });
    </script>
</asp:Content>
