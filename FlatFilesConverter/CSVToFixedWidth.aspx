<%@ Page Title="CSV To Fixed Width" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CSVToFixedWidth.aspx.cs" Inherits="FlatFilesConverter.CSVToFixedWidth" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Step 1: Select your input </legend>
        <div class="form-group">
            <label for="FileUpload">File Input</label>
            <asp:FileUpload ID="FileUpload" runat="server" />
            <asp:Label ID="UpLoadStatusLabel" runat="server" />
        </div>
        <div class="form-group">
            <label for="TextBoxDelimiter">Delimiter</label>
            <asp:TextBox ID="TextBoxDelimiter" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="checkbox">
            <label>
                <asp:CheckBox ID="CheckBoxIsFirstLineHeader" runat="server" />
                Is First Line Header
            </label>
        </div>
    </fieldset>
    <fieldset>
        <legend>Step 2: Define Field Layout</legend>
        <div class="form-inline">
            <div class="form-group">
                <label for="TextBoxFieldName">Field name *</label>
                <asp:TextBox ID="TextBoxFieldName" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="TextBoxColumnPosition">Column position *</label>
                <asp:TextBox ID="TextBoxColumnPosition" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="TextBoxFieldLength">Field length *</label>
                <asp:TextBox ID="TextBoxFieldLength" runat="server"></asp:TextBox>
            </div>
            <asp:Button ID="ButtonAddRow" runat="server" Text="Add Row" CssClass="btn btn-default" OnClick="ButtonAddRow_Click" />
        </div>
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
    <fieldset>
        <legend>Step 3: Convert</legend>
        <asp:Button ID="ButtonConvert" runat="server" Text="Convert" OnClick="ButtonConvert_Click" />
        <br />
        <asp:Button ID="ButtonDownload" runat="server" Text="DownLoad" OnClick="ButtonDownload_Click" />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <br />
        <asp:BulletedList ID="BulletedListTable" runat="server"></asp:BulletedList>
    </fieldset>
</asp:Content>

