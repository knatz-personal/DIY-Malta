<%@ Page Title="" Language="C#" MasterPageFile="~/masters/SiteSkeleton.Master" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="UI.detail" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="BLL.Catalogue" %>
<%@ Import Namespace="Common.Views" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TITLE" runat="server">
    Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HEAD" runat="server">
    <style type="text/css">
        .form-box {
            margin: 50px auto;
            width: 650px;
            position: relative;
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

<asp:Content ID="Content6" ContentPlaceHolderID="MAINCONTENTPLACEHOLDER" runat="server">
    <% 
        VwProduct product = null;
        if (Guid.Empty != GetProductID)
        {
            product = new BlProducts().Read(GetProductID, HttpContext.Current.User.Identity.Name);
        }
        if (product != null)
        { 
    %>

    <h4 class="intromessage">Details of <%= product.Name %></h4>
    <div class="form-box">
        <div>
            <h1><%= product.Name %></h1>
        </div>
        <%   if (product.Discount > 0)
             { %>
        <div><span class='SaleMarker'>Sale</span></div>
        <%    } %>
        <br />
        <div style="-webkit-box-shadow: 2px 2px 2px 2px #828282; border: 1px solid #227ac8; box-shadow: 2px 2px 2px 2px #828282; height: 300px; margin-bottom: 20px; width: 300px;">
            <img src="<%= product.Image %>" style="height: 300px; width: 300px;" alt="Image of <%= product.Name %>" />
        </div>
        <table style="width: 100%;">
            <tr>
                <td><b>Description:</b></td>
                <td style="text-align: left; padding: 5px;">
                    <%= product.Description %>
                </td>
            </tr>
            <tr>
                <td><b>Price:</b></td>
                <td style="text-align: left; padding: 5px;">
                    <% if (product.Discount > 0)
                       { %>
                    <span style='text-decoration: line-through; color: red; font-size: medium; text-align: left;'><%= String.Format(new CultureInfo("MT"), "Was {0:C}", product.UnitPrice) %>
                    </span>
                    <br />
                     <%= String.Format(new CultureInfo("MT"), "Discount Price {0:C}", ((product.UnitPrice*(100-product.Discount))/100))  %>
                    <% }
                       else
                       {%>
                    <%= String.Format(new CultureInfo("MT"), "{0:C}", product.UnitPrice)  %>
                    <%  } %>
                </td>
            </tr>
            <tr>
                <td><b>Tax:</b></td>
                <td style="text-align: left; padding: 5px;">
                    <%= String.Format(new CultureInfo("MT"), "{0:C}",((product.VAT * product.UnitPrice)/100)) %>
                </td>
            </tr>
            <tr>
                <td><b>Current Stock:</b></td>
                <td style="text-align: left; padding: 5px;">
                    <%= product.Stock %>
                </td>
            </tr>
            <tr>
                <td><b>Product Code:</b></td>
                <td style="text-align: left; padding: 5px;">
                    <%= product.ID %>
                </td>
            </tr>
        </table>
    </div>
    <% } %>
</asp:Content>
