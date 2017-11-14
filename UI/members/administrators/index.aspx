<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Dashboard.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="UI.members.administrators.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TITLE2" runat="server">
    Administration 
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

    <%--CUSTOM PAGE STYLES--%>
    <style type="text/css">
        .ui-tabs-panel {
            width: 100%;
        }

        .tableContainer {
            -ms-box-shadow: 2px 2px 2px 2px #828282;
            -webkit-box-shadow: 2px 2px 2px 2px #828282;
            background-color: #fff;
            border: 1px solid #227ac8;
            box-shadow: 2px 2px 2px 2px #828282;
            color: #000;
            margin: 0 auto;
            margin-right: 50px;
            min-width: 350px;
            padding: 20px;
            text-align: left;
        }

            .tableContainer label {
              display: block;
                margin-bottom: 10px;
                padding: 0;
                width: auto;
            }

            .tableContainer input[type="text"], .tableContainer input[type="date"] {
                -ms-box-shadow: 2px 2px 2px 2px #545454;
                -webkit-box-shadow: 2px 2px 2px 2px #545454;
                box-shadow: 2px 2px 2px 2px #545454;
                display: block;
                margin-bottom: 10px;
                padding: 5px;
                width: 300px;
            }

            .tableContainer input[type="submit"] {
                float: left;
                font-size: small;
                height: 38px;
                width: auto;
            }

            .tableContainer fieldset {
                margin-bottom: 10px;
            }

        .jtable-input-field-container input[type="text"], .jtable-input-field-container input[type="password"] {
            -ms-box-shadow: 2px 2px 2px 2px #545454;
            -webkit-box-shadow: 2px 2px 2px 2px #545454;
            background: #fbfbfb;
            border: 1px solid #999999;
            box-shadow: 2px 2px 2px 2px #545454;
            font-size: 14px;
            height: 20px;
            margin-bottom: 10px;
            padding: 10px;
            width: 300px;
        }

        .jtable-input-field-container textarea {
            -ms-box-shadow: 2px 2px 2px 2px #545454;
            -webkit-box-shadow: 2px 2px 2px 2px #545454;
            box-shadow: 2px 2px 2px 2px #545454;
            font-size: 14px;
            height: 300px;
            max-height: 300px;
            overflow-y: scroll;
            padding: 10px;
            resize: vertical;
        }

        .jtable-input-field-container label {
            color: #000;
            display: block;
            margin: 10px 0;
            width: 300px;
        }

        .jtable-input-field-container .ui-selectmenu-button, .jtable-input-field-container select {
            -ms-box-shadow: 2px 2px 2px 2px #545454;
            -webkit-box-shadow: 2px 2px 2px 2px #545454;
            border: 1px solid #1299d3;
            box-shadow: 2px 2px 2px 2px #545454;
            font-size: 14px;
            height: 20px;
            padding: 10px;
            text-transform: capitalize;
        }

        .jtable-input-field-container select {
            height: 30px;
            line-height: 30px;
            padding: 5px;
            width: 322px;
        }

        .ui-datepicker {
            width: 318px;
        }

        .ui-autocomplete-loading {
            background: white url("/img/loader.gif ") right center no-repeat;
            -ms-background-size: 25px 16px;
            background-size: 25px 16px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MAINCONTENTPLACEHOLDER2" runat="server">
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Catalogue</a></li>
            <li><a href="#tabs-2">Orders</a></li>
            <li><a href="#tabs-4">Sales</a></li>
            <li><a href="#tabs-3">Accounts</a></li>
            <li><a href="#tabs-5">Navigation</a></li>
            <li><a href="#tabs-6">Settings</a></li>
        </ul>
        <div id="tabs-1">
            <div class="tableContainer">
                <fieldset>
                    <label>Search:</label>
                    <input type="text" name="query1" id="query1" placeholder="Name"/>
                    <input type="submit" id="LoadProductRecords" value="Load records">
                </fieldset>
                <div id="productsgrid"></div>
            </div>
        </div>
        <div id="tabs-2">
            <div class="tableContainer">
                <fieldset>
                    <div class="ui-widget">
                        <label>Search:</label>
                        <input type="text" name="query2" id="osearch" title="Username" placeholder="Username" />
                    </div>
                    <div>
                        <input type="date" title="Start Date" id="osDate" class="datepicker" placeholder="Start Date" />
                        <input type="date" title="End Date" id="oeDate" class="datepicker" placeholder="End Date" />
                    </div>
                    <div>
                        <select id="osoption" name="osoption" title="Order state">
                            <option selected="selected" value="0">Order States</option>
                        </select>
                    </div>
                    <br />
                    <input type="submit" id="LoadOrderRecords" value="Load records">
                </fieldset>
                <div id="ordersgrid"></div>
            </div>
        </div>
        <div id="tabs-3">
            <div class="tableContainer">
                <fieldset>
                    <label>Search:</label>
                    <input type="text" name="username" id="username1" />
                    <input type="submit" id="LoadUserRecords" value="Load records">
                </fieldset>
                <div id="usersgrid"></div>
            </div>
        </div>
        <div id="tabs-4">
            <div class="tableContainer">
                <div id="selectedTable4">
                    <fieldset>
                        <label>Search:</label>
                        <input type="text" name="query2" id="query2" title="Name of the sale period" placeholder="Name" />
                        <input type="date" title="Start Date" id="sDate" class="datepicker" placeholder="Start Date" />
                        <input type="date" title="End Date" id="eDate" class="datepicker" placeholder="End Date" />
                        <input type="submit" id="LoadProductSalesRecords" value="Load records">
                    </fieldset>
                </div>
                <div id="salesgrid"></div>
            </div>
        </div>
        <div id="tabs-5">
            <div class="form-box">
                <select id="options5" title="Select a navigation table to manage">
                    <option value="-1" selected>Select</option>
                    <option value="1">Menu</option>
                    <option value="2">Category</option>
                </select>
                <div id="selectedTable5"></div>
            </div>
        </div>
        <div id="tabs-6">
            <div class="form-box">
                <select id="options6" title="Select a setting to manage">
                    <option value="-1" selected>Select</option>
                    <option value="1">Role</option>
                    <option value="2">Town</option>
                    <option value="3">Gender</option>
                    <option value="4">User Type</option>
                    <option value="5">Order State</option>
                    <option value="6">Contact Type</option>
                    <option value="7">Address Type</option>
                </select>
                <div id="selectedTable6"></div>
            </div>
        </div>
    </div>
    <script src="admin.js"></script>
</asp:Content>
