<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="FlatFilesConverter.ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="ServerError" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <asp:LinkButton ID="LinkButtonToHomePage" runat="server" PostBackUrl="Default.aspx">Return to Home page</asp:LinkButton>
    </form>
</body>
</html>
