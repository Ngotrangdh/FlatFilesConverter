<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FlatFilesConverter._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container body-content">
        <div class="jumbotron">
            <h1>Flat Files Converter</h1>
            <p class="lead">A web application to convert CSV to Fixed Width, and vice versa.</p>
            <div>
                <a href="CSVToFixedWidth.aspx" class="btn btn-primary btn-lg">CSV To Fixed Width &raquo;</a>
                <a href="FixedWidthToCSV.aspx" class="btn btn-primary btn-lg">Fixed Width To CSV &raquo;</a>
            </div>
        </div>

    </div>
</asp:Content>
