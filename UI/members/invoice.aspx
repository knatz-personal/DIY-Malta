<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="invoice.aspx.cs" Inherits="UI.members.invoice" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Runtime.InteropServices" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <title>Invoice</title>

    <script src="/js/jquery-1.11.0.min.js"></script>

    <!--FAVICON-->
    <link rel="icon" href="/img/skeleton/favicon.ico" type="image/x-icon">
    <link rel="icon" href="/img/skeleton/favicon.png" type="image/x-icon">

    <%--STYLE--%>
    <style type="text/css">
        .clearfix:after {
            clear: both;
            content: "";
            display: table;
        }

        a {
            color: #000;
            text-decoration: underline;
        }

        body {
            background: #FFF;
            color: #000;
            font-family: Arial, sans-serif;
            font-size: 12px;
            height: 29.7cm;
            margin: 0 auto;
            position: relative;
            width: 21cm;
        }

        header {
            margin-bottom: 30px;
            padding: 10px 0;
        }

        #logo {
            margin-bottom: 10px;
            text-align: center;
        }

            #logo img {
                width: 300px;
            }

        h1 {
            border-bottom: 1px solid #5D6975;
            border-top: 1px solid #5D6975;
            color: #000;
            font-size: 2.4em;
            font-weight: normal;
            line-height: 1.4em;
            margin: 0 0 20px 0;
            text-align: center;
        }

        #client {
            float: left;
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
            #company div {
                white-space: nowrap;
            }

        table {
            border-collapse: collapse;
            border-spacing: 0;
            margin-bottom: 20px;
            width: 100%;
        }

            table tr:nth-child(2n - 1) td {
                background: #F5F5F5;
            }

            table th,
            table td {
                text-align: center;
            }

            table th {
                border-bottom: 1px solid #C1CED9;
                color: #000;
                font-weight: normal;
                padding: 5px 20px;
                white-space: nowrap;
            }

            table .service,
            table .desc {
                text-align: left;
            }

            table td {
                padding: 20px;
                text-align: right;
            }

                table td.service,
                table td.desc {
                    vertical-align: top;
                }

                table td.unit,
                table td.qty,
                table td.total {
                    font-size: 1.2em;
                }

                table td.grand,
                .topborder {
                    border-top: 1px solid #5D6975;
                }

        #notices .notice {
            border-top: 1px solid #C1CED9;
            color: red;
            font-size: 1.2em;
            text-align: center;
        }

        footer {
            border-top: 1px solid #C1CED9;
            bottom: 0;
            color: #000;
            height: 30px;
            margin-top: 10px;
            padding: 8px 0;
            text-align: center;
            width: 100%;
        }
    </style>
</head>

<body>

    <%
        if (GetOrder != null)
        {
    %>

    <header class="clearfix">
        <div id="logo">
            <img src="/img/skeleton/logo.png">
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
            <h4>ORDER CODE:  <%= GetOrder.ID %></h4>
        </div>
    </header>

    <asp:ListView ID="ItemList" runat="server">
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
                            <td class="total"><span id="subTotal"><%= String.Format(new CultureInfo("MT"),"{0:C}", GetOrder.SubTotal) %></span></td>
                        </asp:PlaceHolder>
                    </tr>
                    <tr>
                        <td colspan="4">VAT</td>
                        <asp:PlaceHolder runat="server">
                            <td class="total"><span id="tax"><%= String.Format(new CultureInfo("MT"),"{0:C}",GetOrder.Tax) %></span></td>
                        </asp:PlaceHolder>
                    </tr>
                    <tr>
                        <td colspan="4">DELIVERY</td>
                        <td class="total">€0.00</td>
                    </tr>
                    <tr>
                        <td colspan="4" class="grand total">GRAND TOTAL (<em>including VAT</em>)</td>
                        <asp:PlaceHolder runat="server">
                            <td class="grand total"><span id="grandTotal"><%= String.Format(new CultureInfo("MT"),"{0:C}",GetOrder.GrandTotal) %></span></td>
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


    <div id="notices">
        <div class="notice">NOTICE: You can return products within 30 days at no extra cost.</div>
    </div>
    <footer>
        **** This is a valid computer generated invoice. ****
    </footer>
    <% } %>
</body>
</html>
