<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Dashboard.master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="UI.members.cart" %>

<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TITLE2" runat="server">
    Shopping Cart
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HEAD2" runat="server">
    <link href="/css/listviewStyle.css" rel="stylesheet" />
    <script src="/js/jquery.validate.min.js"></script>
    <script src="/js/additional-methods.min.js"></script>
    <style type="text/css">
        .ui-dialog .ui-state-error {
            padding: .3em;
        }

        fieldset {
            border: 0;
            margin-top: 25px;
            padding: 0;
        }

        .form-box {
            margin: 0 auto;
        }
        .oldPriceContainer {
            position: relative;
        }
        .oldPrice {
            position: absolute;
            top: -40px;
            left: 0;
            margin: 0;
            padding: 0;
            color: red;
            text-decoration: line-through;
            font-size: smaller;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MAINCONTENTPLACEHOLDER2" runat="server">

    <div id="edit-dialog" title="Update Item Quantity">
        <div id="hirow" class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
            <p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span></p>
            <p class="validateTips"></p>
        </div>
        <fieldset>
            <label>Quantity</label>
            <br />
            <input id="quantityedit" type="text" name="quantityedit" value="" title="Enter item quantity" style="padding-left: 10px;" />
        </fieldset>
    </div>
    <div id="loading_dialog"></div>
    <div id="dialog-message"></div>
    <div class="form-box">
        <asp:ListView ID="ListViewCart" runat="server">
            <LayoutTemplate>
                <article class="module">
                    <header>
                        <h3>Shopping Cart</h3>
                    </header>
                    <table class="tablesorter" cellspacing="0">
                        <thead>
                            <tr>
                                <th style="text-align: center; width: 20px;">
                                    <input id="globalCheckbox" type="checkbox" name="globalCheckbox" />
                                </th>
                                <th style="text-align: center; width: 100px;">Image</th>
                                <th>Name</th>
                                <th>Category</th>
                                <th style="width: 100px;">Unit Price</th>
                                <th style="width: 100px;">Quantity</th>
                                <th style="width: 100px;">Total Price</th>
                                <th style="text-align: center; width: 80px;">Actions</th>
                            </tr>
                            <tr></tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                        </tbody>
                        <tfoot>
                            <tr></tr>
                            <tr>
                                <td colspan="8" style="font-size: x-large; font-weight: 900; padding-right: 10px; text-align: right;">
                                    <a id="globalRemove" href="#" title="Remove Selected Items" class="linkButton" style="text-align: left;">Remove Selected</a>
                                    <span>Sub Total: € <span id="grandTotal">0.00</span></span>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                    <footer>
                        <a href="/default.aspx" title="Continue shopping" class="linkButton">Continue shopping</a>
                        <a href="checkout.aspx" title="Continue to checkout page" class="linkButton" style="float: right;">Go to Checkout</a>
                    </footer>
                </article>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <style>
                    .form-box {
                        width: 70%;
                    }
                </style>
                <div style="background: #fff; padding: 20px; text-align: center;">Your shopping cart is empty</div>
            </EmptyDataTemplate>
            <ItemTemplate>
                <tr>
                    <td style="text-align: center; width: 20px;">
                        <input id="item-id-<%# Eval("ID") %>" type="checkbox" class="chItem" name="item-id-<%# Eval("ID") %>">
                    </td>
                    <td style="text-align: center; width: 100px;"><a href="/detail.aspx?productId=<%# Eval("ProductID") %>">
                        <img src="<%# Eval("Image") %>" alt="Image of <%# Eval("Name") %>" style="width: 75px;" /></a></td>
                    <td><span><%# Eval("Name") %></span></td>
                    <td><span><%# Eval("Category") %></span></td>
                    <td>
                        <%#  (decimal)Eval("DiscountPrice") == 0 ?  "<span id='price'>"+ String.Format(new CultureInfo("MT"), "{0:C}",Eval("UnitPrice"))+"</span>"  :"<div class='oldPriceContainer'><span class='oldPrice'>"+ String.Format(new CultureInfo("MT"), "{0:C}",Eval("UnitPrice"))+"</span></div><span id='price'>"+ String.Format(new CultureInfo("MT"), "{0:C}",Eval("DiscountPrice"))+"</span>"  %>
                    </td>
                    <td><span id="quantity"><%# Eval("Quantity") %></span></td>
                    <td><span id="total"><%#  String.Format(new CultureInfo("MT"), "{0:C}",Eval("TotalPrice")) %></span></td>
                    <td style="text-align: center;">
                        <a href="/detail.aspx?productId=<%# Eval("ProductID") %>">
                            <img src="/img/icn_alert_info.png" title="Detail" /></a>
                        <a id="item-edit-<%# Eval("ID") %>" href="#" onclick=" event.preventDefault();CARTMANAGEMENT.Edit(this, '<%# Eval("ID") %>'); ">
                            <img src="/js/jtable/themes/lightcolor/edit.png" title="Edit" /></a>
                        <a id="item-delete-<%# Eval("ID") %>" href="#" onclick=" event.preventDefault();CARTMANAGEMENT.Remove(this, '<%# Eval("ID") %>'); ">
                            <img src="/img/delete.png" title="Remove" /></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <script type="text/javascript">
        function ApplyUpdate(itemId, price, row) {
            newqty = $("#quantityedit").val();

            $.ajax({
                type: "POST",
                url: "cart.aspx/UpdateItem",
                data: JSON.stringify({ item: itemId, quantity: newqty }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $("#loading_dialog").loading();
                },
                complete: function () {
                    dialog.dialog("close");
                    $("#loading_dialog").loading("loadStop");
                },
                success: function (data) {
                    if (data.d === false) {
                        $("#dialog-message").html("<hr/><p>Failed to update item.</p><hr/>");
                        $("#dialog-message").dialog("open");
                    } else {
                        CARTMANAGEMENT.GetCartTotal();
                        $("#quantity", row).text(newqty);
                        var newtotal = newqty * price;
                        $("#total", row).text("€" + newtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                        GLOBALSCRIPT.CartSummary();
                    }
                },
                error: function (data) {
                    var r = jQuery.parseJSON(data.responseText);
                    $("#dialog-message").html("<hr/><p>" + r.Message + "</p><hr/>");
                    $("#dialog-message").dialog("open");
                }
            });

        };

        function updateTips(t) {
            tipcontainer.show();
            tips.text(t);
            setTimeout(function () {
                tipcontainer.hide();
            }, 3000);
        }

        function checkLength(o, n, min, max) {
            if (o.val().length > max || o.val().length < min) {
                o.addClass("ui-state-error");
                if (max === 'undefined') {
                    updateTips("Length of " + n + " must be at least " +
                        min + " characters long.");
                } else {
                    updateTips("Length of " + n + " must be between " +
                        min + " and " + max + ".");
                }
                return false;
            } else {
                return true;
            }
        }

        function checkRegexp(o, regexp, n) {
            if (!(regexp.test(o.val()))) {
                o.addClass("ui-state-error");
                updateTips(n);
                return false;
            } else {
                return true;
            }
        }

        var CARTMANAGEMENT = {
            Remove: function (deleteButton, itemId) {
                var returnVal = confirm("Remove this item from cart?");
                if (returnVal) {
                    var row = $(deleteButton).parent().parent();
                    CARTMANAGEMENT.RemoveProcess(row, itemId);
                }
            },
            RemoveProcess: function (row, itemId) {
                $.ajax({
                    type: "POST",
                    url: "cart.aspx/RemoveItem",
                    data: JSON.stringify({ item: itemId }),
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
                            $("#dialog-message").html("<hr/><p>Failed to remove item.</p><hr/>");
                            $("#dialog-message").dialog("open");
                        } else {
                            row.remove();
                            GLOBALSCRIPT.CartSummary();
                            CARTMANAGEMENT.GetCartTotal();
                        }
                    },
                    error: function (data) {
                        var r = jQuery.parseJSON(data.responseText);
                        $("#dialog-message").html("<hr/><p>" + r.Message + "</p><hr/>");
                        $("#dialog-message").dialog("open");
                    }
                });
            },
            RemoveSelected: function () {
                $("#globalRemove").click(function (event) {
                    var numberOfChecked = $('.chItem:checked').length;
                    if (numberOfChecked > 0) {

                        var returnVal = confirm("Remove selected items from cart?");
                        if (returnVal) {
                            $('.chItem').each(function () {
                                if ($(this).is(":checked")) {
                                    var row = $(this).parent().parent();
                                    var id = $(this).attr("id");
                                    var idlength = $(this).attr("id").length;
                                    var precursor = "item-id-".length;
                                    var itemId = id.slice(precursor, idlength);
                                    CARTMANAGEMENT.RemoveProcess(row, itemId);
                                }
                            });
                        }
                    }
                    event.preventDefault();
                });
            },
            Edit: function (editButton, itemId) {
                row = $(editButton).parent().parent();
                quantity = $("#quantity", row).text();
                var price = $("#price", row).text();
                    price = price.slice(1, price.length);

                dialog.dialog(
                    'option',
                    'buttons', {
                        Save: function () {
                            var valid = true;
                            tipcontainer.hide();
                            valid = valid && checkLength(quantityedit, "quantity", 1, 'undefined');
                            valid = valid && checkRegexp(quantityedit, /^[0-9]/i, "The quantity must be a numeric value");
                            if (valid) {
                                ApplyUpdate(itemId, price, row);
                                dialog.dialog("close");
                            }
                        },
                        Cancel: function () {
                            dialog.dialog("close");
                        }
                    });

                dialog.dialog('open');

            },
            Dialogs: function () {

                quantityedit = $("#quantityedit"),
                    allFields = $([]).add(quantityedit),
                    tips = $(".validateTips");
                tipcontainer = $("#hirow");
                tipcontainer.hide();
                dialog = $("#edit-dialog").dialog({
                    autoOpen: false,
                    modal: true,
                    open: function () {
                        $("#quantityedit").val(quantity);
                    },
                    close: function () {
                        dialog.dialog("close");
                    }
                });
            },
            GetCartTotal: function () {
                $.ajax({
                    type: "POST",
                    url: "/default.aspx/GetCartSummary",
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    dataType: "json",
                    data: JSON.stringify({}),
                    success: function (data) {
                        $("#grandTotal").text(JSON.parse(data.d).CartTotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                    }
                });
            },
            Test: function () {
                console.log("CARTMANAGEMENT loaded successfully");
            },
            Init: function () {
                this.Test();
                this.GetCartTotal();
                this.RemoveSelected();
                this.Dialogs();
                $("#globalCheckbox").change(function () {
                    if ($(this).is(":checked")) {
                        $('.chItem').each(function () { this.checked = true; });
                    } else {
                        $('.chItem').each(function () { this.checked = false; });
                    }
                });
            }
        };
        $(document).ready(CARTMANAGEMENT.Init());
    </script>
</asp:Content>
