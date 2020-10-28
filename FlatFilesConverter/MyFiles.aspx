<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyFiles.aspx.cs" Inherits="FlatFilesConverter.MyFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="list-group" style="width: 471px; height: 28px">
        <asp:GridView ID="GridViewFileList" runat="server"></asp:GridView>
    </div>
</asp:Content>
