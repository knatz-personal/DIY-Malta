<%@ Page Title="" Language="C#" MasterPageFile="~/masters/SiteSkeleton.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="UI._default" %>

<%@ Import Namespace="BLL.Navigation" %>
<%@ Import Namespace="Common.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TITLE" runat="server">
    Home
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HEAD">
    <link href="/js/jpaginator/css/style.css" rel="stylesheet" />
    <style type="text/css">
        .form-box {
            display: inline-block;
            height: 23px;
            margin-left: 0;
            margin-right: 0;
            overflow: hidden;
            padding: 0;
            padding-bottom: 15px;
            padding-top: 10px;
            width: 99.5%;
        }

        .SaleMarker {
            position: absolute;
            top: 0;
            right: 0;
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

        .cat-container {
            background-color: #fff;
            border-bottom: 1px double #227ac8;
            color: #227ac8;
            margin-left: 0;
            margin-right: 0;
            overflow: hidden;
            padding: 0;
            padding-bottom: 10px;
            width: 99.5%;
        }

            .cat-container h2 {
                border-bottom: 1px solid gainsboro;
                line-height: 45px;
                padding: 5px;
                text-align: center;
            }

        .product-bttn {
            background: #77bace;
            border: 1px solid #77BACE;
            display: inline-block;
            float: left;
            font-size: small;
            height: 32px;
            line-height: 32px;
            margin-left: 3px;
            margin-right: 3px;
            padding: 4px;
            text-align: center;
            vertical-align: middle;
            width: 100px;
        }

            .product-bttn:hover {
                -moz-transition: background-color 0.35s ease-in-out;
                -ms-transition: background-color 0.35s ease-in-out;
                -o-transition: background-color 0.35s ease-in-out;
                -webkit-transition: background-color 0.35s ease-in-out;
                background: orangered;
                color: #fff;
                font-weight: 900;
                transition: background-color 0.35s ease-in-out;
            }

        input.productQuantity {
            border: 1px solid #77BACE;
            display: inline-block;
            float: right;
            height: 32px;
            margin-left: 3px;
            margin-right: 3px;
            padding: 4px;
            text-align: center;
            width: 80px;
        }

        .box p {
            height: 32px;
        }

        .box .price {
            font-weight: 900;
            font-size: large;
            width: 60%;
            float: right;
        }

        .stockDisplay {
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            background: #fff url(/img/icn_alert_info.png) no-repeat;
            background-position: 5px 5px;
            border: 1px solid #77BACE;
            border-radius: 5px;
            color: #082B33;
            display: block;
            font-size: 14px;
            margin: 5px;
            min-height: 63px;
            overflow: hidden;
            padding: 5px;
            text-align: right;
        }

            .stockDisplay span {
                display: block;
                float: left;
                margin: 0;
                min-height: 23px;
                padding: 0;
                width: 100%;
            }

        .pager {
            display: inline-block;
            float: left;
            margin-left: 2px;
            overflow: hidden;
            width: 60%;
        }

        .pageSizer {
            display: inline-block;
            float: right;
            margin-right: 2px;
            padding: 5px;
        }

        #counter {
            display: inline-block;
            float: right;
            margin-right: 2px;
            padding: 5px;
        }

        .quick_search {
            display: inline-block;
            float: left;
            margin: 0;
            margin-top: -5px;
            overflow: hidden;
            padding: 0;
            text-align: center;
            width: 320px;
        }


            .quick_search input[type=text] {
                -moz-box-shadow: inset 0 2px 2px #ccc, 0 1px 0 #fff;
                -ms-border-radius: 20px;
                -webkit-box-shadow: inset 0 2px 2px #ccc, 0 1px 0 #fff;
                background: #fff url('/img/icn_search.png') no-repeat;
                background-position: 10px 6px;
                border: 1px solid #bbb;
                border-radius: 20px;
                box-shadow: inset 0 2px 2px #ccc, 0 1px 0 #fff;
                height: 23px;
                padding: 5px;
                text-indent: 30px;
                width: 300px;
            }

                .quick_search input[type=text]:focus {
                    -moz-box-shadow: inset 0 2px 2px #ccc, 0 0 10px #ADDCE6;
                    -webkit-box-shadow: inset 0 2px 2px #ccc, 0 0 10px #ADDCE6;
                    border: 1px solid #77BACE;
                    box-shadow: inset 0 2px 2px #ccc, 0 0 10px #ADDCE6;
                    outline: none;
                }

        .jPag-pages {
            margin-left: 5px;
            margin-right: 5px;
            border: 1px solid #77BACE;
        }
    </style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ASIDECONTENT">
    <div class="cat-container">
        <h3>Filter</h3>
        <ul>
            <li>Current:<br/><span id="currentCategory"></span></li>
        </ul>
        <h2>Category</h2>
        <h3>All Categories</h3>
        <ul>
            <li><a id="0" href="#" title="All categories">All Categories</a></li>
        </ul>
        <%
            foreach (VwCategory root in new BlCategory().GetRootParents())
            {
                Response.Write("<h3>" + root.Name + "</h3>");
                Response.Write("<ul>");
                Response.Write("<li><a id='" + root.ID + "' href='#' title='" + root.Description + "' >" + root.Name + "</a></li>");
                if (new BlCategory().GetChildren(root.ID).Any())
                {
                    foreach (VwCategory sub in new BlCategory().GetChildren(root.ID))
                    {
                        Response.Write("<li><a id='" + sub.ID + "' href='#' title='" + sub.Description + "' >" + sub.Name + "</a></li>");
                    }
                }
                Response.Write("</ul>");
            }
        %>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="RIGHTPANEL">
    <div id="row">
        <div class="form-box">
            <div class="quick_search">
                <input id="quicksearch" type="text" value="Quick Search" placeholder="Quick Search">
            </div>
            <select class="pageSizer">
                <option value="12">10</option>
                <option value="27">25</option>
                <option value="51">50</option>
                <option value="75">75</option>
                <option value="102">100</option>
            </select>
        </div>
        <div id="Catalog"></div>
        <div class="form-box">
            <div class="pager"></div>
            <select class="pageSizer">
                <option value="12">10</option>
                <option value="27">25</option>
                <option value="51">50</option>
                <option value="75">75</option>
                <option value="102">100</option>
            </select>
            <span id="counter"></span>
        </div>
    </div>
    <div id="dialog-message"></div>
    <div id="loading_dialog"></div>
    <script src="/js/jpaginator/jquery.paginate.js"></script>
    <script src="/js/jquery-cookie/jquery.cookie.js"></script>
    <script src="/js/hoverintent.js"></script>
    <script type="text/javascript">
        $(window).load(function () {
            CATALOGUEDISPLAY.Init();
        });

        var CATALOGUEDISPLAY = {
            Pagination: function () {
                var pagerSizer = $(".pageSizer");
                pagerSizer.change(function () {
                    var pageSize = $(this).val();
                    pagerSizer.val(pageSize);
                    $.cookie("pageSize", pageSize, { expires: 1 });
                    $.cookie("startIndex", 1, { expires: 1 });

                    CATALOGUEDISPLAY.LoadCatalogue();
                });
                var pager = $(".pager");
                pager.paginate({
                    count: $.cookie("pageCount"),
                    start: $.cookie("startIndex"),
                    display: $.cookie("pageSize"),
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    background_hover_color: '#CFCFCF',
                    onChange: function (page) {
                        $.cookie("startIndex", page, { expires: 1 });
                        $(".pager").change();
                        CATALOGUEDISPLAY.LoadCatalogue();
                        return false;
                    }
                });
            },
            QuickSearch: function (identifier) {
                $(identifier).val("");
                $(identifier).autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            url: "/default.aspx/FindProduct",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ query: request.term }),
                            success: function (data) {
                                response(data.d);
                            }
                        });
                    },
                    select: function (event, ui) {
                        $.cookie("query", ui.item.value, { expires: 1 });
                        CATALOGUEDISPLAY.LoadCatalogue();
                    },
                    open: function (event, ui) {
                        $(".ui-autocomplete").css("z-index", 1000);
                    }
                });
                $(identifier).on('keyup', function (event) {
                    if (event.which == 13) {
                        $.cookie("query", $(identifier).val(), { expires: 1 });
                        CATALOGUEDISPLAY.LoadCatalogue();
                    }
                });

            },
            Category: function () {
                $("#sidebar").accordion({
                    header: "h3",
                    heightStyle: "content",
                    event: "click hoverintent"
                });
                $("#sidebar ul li a").click(function (event) {
                    var categoryID = $(this).attr("id");
                    $.cookie("category", categoryID, { expires: 1 });
                    $.cookie("categoryName", $(this).text(), { expires: 1 });
                    CATALOGUEDISPLAY.LoadCatalogue();
                    event.preventDefault();
                });
            },
            LoadCatalogue: function () {

                var pageSize = $.cookie("pageSize");
                var itemCount = $.cookie("itemCount");
                var startIndex = $.cookie("startIndex");
                var query = $.cookie("query");
                var category = $.cookie("category");
                $("#currentCategory").text($.cookie("categoryName"));

                $.ajax({
                    type: "POST",
                    url: "/default.aspx/LoadCatalog",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        query: query,
                        category: category,
                        startIndex: startIndex,
                        pageSize: pageSize
                    }),
                    beforeSend: function () {
                        $("#loading_dialog").loading();
                    },
                    complete: function () {
                        $("#loading_dialog").loading("loadStop");
                        CATALOGUEDISPLAY.Cart();
                    },
                    success: function (dataResponse) {
                        var data = dataResponse.d;
                        if (data.Result != 'OK') {
                            var dialog = $("#dialog-message");
                            dialog.text(data.Message);
                            dialog.dialog("open");
                        } else {
                            $.cookie("itemCount", data.TotalRecordCount, { expires: 1 });
                            $.cookie("pageCount", Math.ceil($.cookie("itemCount") / $.cookie("pageSize")), { expires: 1 });
                            $("#counter").text("Pages: " + $.cookie("pageCount") + " Items: " + $.cookie("itemCount"));
                            CATALOGUEDISPLAY.Pagination();
                            $("#Catalog").html(data.Records);
                        }
                    },
                    error: function (data) {
                        var dialog = $("#dialog-message");
                        dialog.text("An error occurred while loading records. Sorry for the inconvenience.");
                        dialog.dialog("open");
                    }
                });
            },
            Cart: function () {
                $('.productQuantity').val("");
                $(".product-bttn").click(function (event) {
                    var id = $(this).attr("id");
                    var idlength = $(this).attr("id").length;

                    var dad = $(this).parent();
                    var qchild = dad.find('input[type="text"]');
                    var quantity = 0;
                    if (qchild.val() !== "") {
                        quantity = parseInt(qchild.val());
                    }
                    var pid = id.slice(8, idlength);
                    var qty = quantity;


                    $.ajax({
                        type: "POST",
                        url: "/default.aspx/AddItemToCart",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            productId: pid,
                            quantity: qty
                        }),
                        success: function (data) {
                            var message = dad.find('.incartmessage');
                            if (data.d === false) {
                                message.text("Failed to add item to cart");
                                message.css("color", "red");
                            } else {

                                message.text("Added To Cart");
                                message.css("color", "orangered");
                                GLOBALSCRIPT.CartSummary();
                            }
                        },
                        error: function (data) {
                            var dialog = $("#dialog-message");
                            dialog.text("An error occurred while adding item to your cart. Sorry for the inconvenience.");
                            dialog.dialog("open");
                        }
                    });

                    $('.productQuantity').val("");
                    event.preventDefault();
                });
            },
            Test: function () {
                console.log("CATALOGUEDISPLAY loaded successfully");
            },
            Init: function () {
                $.cookie("pageSize", 12, { expires: 1 });
                $.cookie("startIndex", 1, { expires: 1 });
                $.cookie("itemCount", 500, { expires: 1 });
                $.cookie("query", "", { expires: 1 });
                $.cookie("category", 0, { expires: 1 });
                $.cookie("pageCount", Math.ceil(500 / 10), { expires: 1 });
                $(".pageSizer").val("12");
                $("#counter").text("Pages: " + $.cookie("pageCount") + " Items: " + $.cookie("itemCount"));
                $("#currentCategory").text("All Categories");
                //cookies end
                this.Test();
                this.Cart();
                this.Category();
                this.Pagination();
                this.QuickSearch("#quicksearch");
                this.LoadCatalogue();
            }
        };
    </script>
</asp:Content>
