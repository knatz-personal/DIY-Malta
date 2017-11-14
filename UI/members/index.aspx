<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Dashboard.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="UI.members.index" %>
<%@ Import Namespace="System.Globalization" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TITLE2" runat="server">
    Profile
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HEAD2" runat="server">

    <%--STYLES--%>
    <link href="/js/jtable/themes/lightcolor/gray/jtable.min.css" rel="stylesheet" />
    <link href="/js/jtable/validation/css/validationEngine.jquery.css" rel="stylesheet" />

    <%--SCRIPTS--%>
    <script src="/js/jtable/validation/js/jquery.validationEngine.js"></script>
    <script src="/js/jtable/validation/js/languages/jquery.validationEngine-en.js"></script>
    <script src="/js/jtable/external/json2.min.js"></script>
    <script src="/js/jtable/jquery.jtable.js"></script>
    <script src="/js/jtable/extensions/jquery.jtable.aspnetpagemethods.min.js"></script>

    <%--CUSTOM STYLES--%>
    <style type="text/css">
        .form-box input[type="text"], .form-box input[type="date"] {
            -ms-box-shadow: 2px 2px 2px 2px #545454;
            -webkit-box-shadow: 2px 2px 2px 2px #545454;
            box-shadow: 2px 2px 2px 2px #545454;
            display: block;
            float: left;
            margin-right: 15px;
            padding: 5px;
            width: 300px;
        }

        .form-box input[type="submit"] {
            float: left;
            font-size: small;
            height: 38px;
            width: auto;
        }

        .form-box fieldset { margin-bottom: 10px; }

        .ui-datepicker { width: 305px; }

        .ui-autocomplete-loading {
            -ms-background-size: 25px 16px;
            background: white url("/img/loader.gif ") right center no-repeat;
            background-size: 25px 16px;
        }

        #tabs-1 {
            background: #FFF;
            color: #000;
            font-family: Arial, sans-serif;
            font-size: 12px;
            margin: 0 auto;
            position: relative;
            width: 21cm;
        }


        #tabs-1 .clearfix:after {
            clear: both;
            content: "";
            display: table;
        }

        #tabs-1 a {
            color: #000;
            text-decoration: underline;
        }



        #tabs-1 header {
            background-color: #fff;
            clear: none;
            color: #000;
            height: auto;
            margin-bottom: 30px;
            padding: 10px 0;
            text-align: center;
        }



        #tabs-1 h1 {
            border-bottom: 1px solid #5D6975;
            border-top: 1px solid #5D6975;
            color: #000;
            font-size: 2.4em;
            font-weight: normal;
            line-height: 1.4em;
            margin: 0 0 20px 0;
            text-align: center;
        }


        #tabs-1 h2 {
            border: 0;
            color: #000;
            font-size: small;
            font-weight: 900;
            line-height: 1.4em;
            margin: 0 0 20px 0;
            text-align: left;
        }

        #client {
            float: left;
            text-align: left;
        }

        #client span {
            color: #000;
            display: inline-block;
            font-size: 0.8em;
            margin-right: 10px;
            text-align: right;
            width: 58px;
        }

        #company {
            float: right;
            text-align: right;
        }

        #client div,
        #company div { white-space: nowrap; }

        #tabs-1 table {
            border-collapse: collapse;
            border-spacing: 0;
            margin-bottom: 20px;
            width: 100%;
        }

        #tabs-1 table tr:nth-child(2n - 1) td { background: #F5F5F5; }

        #tabs-1 table th,
        #tabs-1 table td { text-align: center; }

        #tabs-1 table th {
            border-bottom: 1px solid #C1CED9;
            color: #000;
            font-weight: normal;
            padding: 5px 20px;
            white-space: nowrap;
        }

        #tabs-1 table .service,
        #tabs-1 table .desc { text-align: left; }

        #tabs-1 table td {
            padding: 20px;
            text-align: right;
        }

        #tabs-1 table td.service,
        #tabs-1 table td.desc { vertical-align: top; }

        #tabs-1 table td.unit,
        #tabs-1 table td.qty,
        #tabs-1 table td.total { font-size: 1.2em; }

        #tabs-1 table td.grand,
        #tabs-1 .topborder { border-top: 1px solid #5D6975; }

        .status {
            display: inline-block;
            float: left;
            height: 25px;
            text-align: left;
        }

        .toolbar {
            display: inline-block;
            float: right;
            height: 25px;
            padding: 5px;
            text-align: right;
        }

        .toolbar img {
            height: 16px;
            padding-left: 5px;
            width: 16px;
        }

        #infocontainer {
            clear: both;
            margin-bottom: 5px;
            overflow: hidden;
            width: auto;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MAINCONTENTPLACEHOLDER2" runat="server">
    <div id="tabs">

        <ul>
            <li><a href="#tabs-1">My Order</a></li>
            <li><a href="#tabs-2">Order History</a></li>
        </ul>
        <div id="tabs-1">
            <%
                if (GetOrder != null)
                {
            %>
                <div class="form-box">

                    <header class="clearfix">
                        <div id="infocontainer">
                            <div class="status">
                                <h2>Status: <span id="orderStatus"><%= GetOrder.Status %></span></h2>
                            </div>
                            <div class="toolbar">
                                <%--  <a href="#" id="<%= GetOrder.ID %>" class="download">
                                <img src="/img/pdf.png" alt="Dowload Invoice" title="Dowload Invoice" /></a>--%>
                                <a href="/members/invoice.aspx?orderId=<%= GetOrder.ID %>">
                                    <img src="/img/print.png" alt="Print Invoice" title="Print Invoice" />
                                </a>

                            </div>
                        </div>
                        <h1>INVOICE </h1>
                        <div id="company" class="clearfix">
                            <div>DIY-Malta</div>
                            <div>
                                4 Paola Hill,
                                <br>
                                PLA1890, MT
                            </div>
                            <div>(356) 27661576</div>
                            <div>
                                <a href="mailto:001knatz@gmail.com">info@diymalta.com</a>
                            </div>
                        </div>
                        <div id="client">
                            <div><span>CLIENT</span> <%= GetUser.FirstName + " " + GetUser.MiddleInitial + " " + GetUser.LastName %></div>
                            <div><span>ADDRESS</span> <%= GetUser.Address + ", " + GetUser.Street + ", " + GetUser.Town %></div>
                            <div><span>POSTCODE</span> <%= GetUser.PostCode %></div>
                            <div><span>COUNTRY</span> Malta</div>
                            <div>
                                <span>EMAIL</span>  <a href="mailto:john@example.com"><%= GetUser.Email %></a>
                            </div>
                            <div><span>DATE</span> <%= GetOrder.DatePlaced.ToLongDateString() %></div>
                        </div>
                        <div style="border-bottom: 1px solid #5D6975; clear: both; margin: 0; padding-top: 10px;">
                            <h2>ORDER CODE:  <%= GetOrder.ID %></h2>
                        </div>
                    </header>

                    <asp:ListView ID="ListViewOrder" runat="server">
                        <LayoutTemplate>
                            <table>
                                <thead>
                                    <tr>
                                        <th class="service">CATEGORY</th>
                                        <th class="desc">DESCRIPTION</th>
                                        <th>PRICE</th>
                                        <th>QUANTITY</th>
                                        <th>TOTAL</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                                </tbody>
                                <tfoot>
                                    <tr class="topborder">
                                        <td colspan="4">SUB TOTAL (<em>excluding VAT</em>)</td>
                                        <asp:PlaceHolder runat="server">
                                            <td class="total"><span id="subTotal"><%= String.Format(new CultureInfo("MT"), "{0:C}", GetOrder.SubTotal) %></span></td>
                                        </asp:PlaceHolder>
                                    </tr>
                                    <tr>
                                        <td colspan="4">VAT</td>
                                        <asp:PlaceHolder runat="server">
                                            <td class="total"><span id="tax"><%= String.Format(new CultureInfo("MT"), "{0:C}", GetOrder.Tax) %></span></td>
                                        </asp:PlaceHolder>
                                    </tr>
                                    <tr>
                                        <td colspan="4">DELIVERY</td>
                                        <td class="total">€0.00</td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="grand total">GRAND TOTAL (<em>including VAT</em>)</td>
                                        <asp:PlaceHolder runat="server">
                                            <td class="grand total"><span id="grandTotal"><%= String.Format(new CultureInfo("MT"), "{0:C}", GetOrder.GrandTotal) %></span></td>
                                        </asp:PlaceHolder>
                                    </tr>
                                </tfoot>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="service"><%# Eval("Category") %></td>
                                <td class="desc"><%# Eval("Item") %></td>
                                <td class="unit">€ <%# Eval("UnitPrice") %></td>
                                <td class="qty"><%# Eval("Quantity") %></td>
                                <td class="total">€  <%# Eval("TotalPrice") %></td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div style="background: #fff; padding: 20px; text-align: center;">No items found</div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            <%
                }
                else
                {
                    Response.Write("<div style=\"background: #fff; padding: 20px; text-align: center;\">No orders yet</div>");
                }
            %>
        </div>
        <div id="tabs-2">
            <div class="form-box">
                <fieldset>
                    <div>
                        <input type="date" title="Start Date" id="osDate" class="datepicker" placeholder="Start Date" />
                        <input type="date" title="End Date" id="oeDate" class="datepicker" placeholder="End Date" />
                    </div>
                    <input type="submit" id="LoadOrderRecords" value="Load records">
                </fieldset>
                <div id="ordersgrid"></div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function JTAdvancedOptions(identifier, title, defaultSorting, operations, fields, events) {
            $(identifier).jtable({
                title: title,
                paging: true,
                sorting: true,
                defaultSorting: defaultSorting,
                actions: operations,
                fields: fields,
                //Initialize validation logic when a form is created
                formCreated: function(event, data) {
                    data.form.validationEngine();
                },
                //Validate form when it is being submitted
                formSubmitting: function(event, data) {
                    return data.form.validationEngine('validate');
                },
                //Dispose validation logic when form is closed
                formClosed: function(event, data) {
                    data.form.validationEngine('hide');
                    data.form.validationEngine('detach');
                }
            });
            events();
        }

        var PROFILE = {
            Tabs: function() {
                $("#tabs").tabs();
            },
            LoadOrderTables: function() {

                var orderf = {
                    ID: {
                        title: 'ID',
                        key: true,
                        list: false,
                        create: false,
                        edit: false
                    },
                    Username: {
                        title: 'Username',
                        edit: false,
                        create: false,
                        list: false
                    },
                    DatePlaced: {
                        title: 'Date Placed',
                        type: 'date',
                        edit: false
                    },
                    State: {
                        title: 'Status',
                        options: '/members/index.aspx/GetOrderStateOptions'
                    },
                    Print: {
                        title: 'Print Invoice',
                        sorting: false,
                        edit: false,
                        create: false,
                        width: '2%',
                        list: true,
                        listClass: "jtable-command-column",
                        display: function(data) {

                            var $img = $('<a href="/members/invoice.aspx?orderId=' + data.record.ID + '">' +
                                '<img src="/img/print.png" title="Print Invoice" alt="Print Invoice" /></a>');
                            return $img;
                        }
                    }

                };

                var actions = {
                    listAction: '/members/index.aspx/ListMyOrders'
                };

                JTAdvancedOptions('#ordersgrid', 'The Orders List', 'DatePlaced DESC',
                    actions,
                    orderf,
                    function() {

                        //Re-load records when user click 'load records' button.
                        $('#LoadOrderRecords').click(function(e) {
                            e.preventDefault();
                            $('#ordersgrid').jtable('load', {
                                start: $('#osDate').val(),
                                end: $('#oeDate').val()
                            });
                        });

                        //Load all records when page is first shown
                        $('#LoadOrderRecords').click();
                    }
                );
            },
            Test: function() {
                console.log("PROFILE script loaded successfully.");
            },
            Init: function() {
                this.Test();
                this.Tabs();
                this.LoadOrderTables();
            }
        };

        $(window).load(function() {
            PROFILE.Init();
        });
    </script>
</asp:Content>