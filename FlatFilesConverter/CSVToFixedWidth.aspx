<%@ Page Title="CSV To Fixed Width" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CSVToFixedWidth.aspx.cs" Inherits="FlatFilesConverter.CSVToFixedWidth" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="FileUpload" runat="server" />
    <asp:Button ID="SubmitFile" runat="server" Text="Upload" OnClick="SubmitFile_Click" />
</asp:Content>
