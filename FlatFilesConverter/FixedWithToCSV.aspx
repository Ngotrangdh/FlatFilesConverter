<%@ Page Title="Fixed Width To CSV" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FixedWithToCSV.aspx.cs" Inherits="FlatFilesConverter.FixedWidthToCSV" %>


<asp:Content  ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Step 1: Select your input </legend> 
    </fieldset>
    <br />
        <asp:FileUpload ID="FileUpload" runat="server" />
        <asp:Label ID="UpLoadStatusLabel" runat="server"></asp:Label>
        <br />
        <asp:Label ID="LableIsFirstLineHeader" runat="server" Text="Is First Line Header"></asp:Label>
        <asp:CheckBox ID="CheckBoxIsFirstLineHeader" runat="server" />
        <br />
        <asp:Label ID="LabelFieldName" runat="server" Text="Field Name"></asp:Label>
        <asp:TextBox ID="TextBoxFieldName" runat="server"></asp:TextBox>
        <asp:Label ID="LabelColumnPosition" runat="server" Text="Column Position"></asp:Label>
        <asp:TextBox ID="TextBoxColumnPosition" runat="server"></asp:TextBox>
        <asp:Label ID="LabelFieldLength" runat="server" Text="Field Length"></asp:Label>
        <asp:TextBox ID="TextBoxFieldLength" runat="server"></asp:TextBox>
        <asp:Button ID="ButtonAddRow" runat="server" Text="Add Row" OnClick="ButtonAddRow_Click"/>
        <br />
        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonAddRow" />
            </Triggers>
            <ContentTemplate>
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
        <br />
        <asp:Button ID="ButtonDownload" runat="server" Text="DownLoad" OnClick="ButtonDownload_Click" />
    </fieldset>
    

</asp:Content>   