<%@ Page Title="Login" Language="C#" MasterPageFile="~/masters/SiteSkeleton.Master" AutoEventWireup="true" CodeBehind="alt_login.aspx.cs" Inherits="UI.accounts.alt_login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TITLE" runat="server">
    Login
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HEAD" runat="server">
    <style>
        .form-box {
            margin: 0 auto;
            overflow: hidden;
            width: 50%;
        }
        #MAINCONTENTPLACEHOLDER_ChkBxRemember {
            display: inline-block;
            float: left;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MAINCONTENTPLACEHOLDER" runat="server">
    <div class="form-box">


        <table>

            <tr>
                <td>
                    <asp:Label ID="lblUsername" runat="server" Text="Username " Width="120px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtBxUsername" runat="server" Width="300px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" ErrorMessage="The username is required" ControlToValidate="TxtBxUsername" Font-Bold="True" Font-Size="XX-Large" ForeColor="Red">*</asp:RequiredFieldValidator>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPassword" runat="server" Text="Password " Width="120px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtBxPassword" runat="server" Width="300px" MaxLength="50" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server" ErrorMessage="The password is required" ControlToValidate="TxtBxPassword" Font-Bold="True" Font-Size="XX-Large" ForeColor="Red">*</asp:RequiredFieldValidator>

                </td>
            </tr>
             <tr>
                 <td>  <asp:Label ID="lblCheck" runat="server" Text="Remember me " Width="120px"></asp:Label></td>
                <td colspan="1" style="padding-top: 5px;padding-left: 0;overflow: hidden;height: 23px;">
                    <asp:CheckBox ID="ChkBxRemember" runat="server" Text=" " />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right;padding-top: 5px;">
                    <asp:Button ID="BttnLogin" runat="server" Text="Login" OnClick="BttnLogin_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red" HeaderText="Error Summary" />
                </td>
            </tr>
        </table>

    </div>
</asp:Content>
