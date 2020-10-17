<%@ Page Title="Fixed Width To CSV" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FixedWithToCSV.aspx.cs" Inherits="FlatFilesConverter.FixedWidthToCSV" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Step 1: Select your input </legend>
    </fieldset>
    <br />
    <asp:FileUpload ID="FileUpload" runat="server" />
    <asp:Label ID="LabelFileUploadError" runat="server" ForeColor="Red"></asp:Label>
    <br />
    <asp:Label ID="LableIsFirstLineHeader" runat="server" Text="Is First Line Header"></asp:Label>
    <asp:CheckBox ID="CheckBoxIsFirstLineHeader" runat="server" />
    <br />
    <asp:Label ID="LabelFieldName" runat="server" Text="Field Name"></asp:Label>
    <asp:TextBox ID="TextBoxFieldName" runat="server"></asp:TextBox>
    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxFieldName" ErrorMessage="Field name required" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>

    <asp:Label ID="LabelColumnPosition" runat="server" Text="Column Position"></asp:Label>
    <asp:TextBox ID="TextBoxColumnPosition" runat="server"></asp:TextBox>
    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxColumnPosition" ErrorMessage="Column position is required" Text="**" ForeColor="Red"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator runat="server" ValidationExpression="^\d+$" ControlToValidate="TextBoxColumnPosition" ErrorMessage="Invalid column position input" Text="*" ForeColor="Red"></asp:RegularExpressionValidator>--%>

    <asp:Label ID="LabelFieldLength" runat="server" Text="Field Length"></asp:Label>
    <asp:TextBox ID="TextBoxFieldLength" runat="server"></asp:TextBox>
    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxFieldLength" ErrorMessage="Field name is required" Text="**" ForeColor="Red"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator runat="server" ValidationExpression="^\d+$" ControlToValidate="TextBoxFieldLength" ErrorMessage="Invalid field length input" Text="*" ForeColor="Red"></asp:RegularExpressionValidator>--%>
    <asp:Button ID="ButtonAddRow" runat="server" Text="Add Row" OnClick="ButtonAddRow_Click" />
    <hr />
    <%--<asp:ValidationSummary ForeColor="Red" ID="ErrorSummary" runat="server" />--%>

    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonAddRow" />
        </Triggers>
        <ContentTemplate>
            <asp:Label ID="LabelColumnsEmptyError" runat="server" ForeColor="Red"></asp:Label>
            <asp:BulletedList ID="BulletedListError" runat="server" ForeColor="Red"></asp:BulletedList>

            <asp:GridView ID="GridViewLayout" runat="server" OnRowDeleting="GridViewLayout_RowDeleting">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    </fieldset>
    <br />
    <br />
    <fieldset>
        <legend>Step 2: Define Output Delimiter</legend>
        <br />
        <asp:Label ID="LabelCSVDelimiter" runat="server" Text="Delimiter"></asp:Label>
        <asp:TextBox ID="TextBoxDelimiter" runat="server"></asp:TextBox>
        <br />
    </fieldset>
    <br />
    <fieldset>
        <legend>Step 3: Convert</legend>
        <asp:Button ID="ButtonConvert" runat="server" Text="Convert" OnClick="ButtonConvert_Click" />
        <br />
    </fieldset>


</asp:Content>
