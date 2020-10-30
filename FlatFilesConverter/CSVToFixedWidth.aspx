<%@ Page Title="CSV To Fixed Width" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CSVToFixedWidth.aspx.cs" Inherits="FlatFilesConverter.CSVToFixedWidth" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Step 1: Select your input </legend>
        <div class="form-group">
            <label for="FileUpload">File Input</label>
            <asp:FileUpload ID="FileUpload" runat="server"/>
            <asp:Label ID="LabelFileUploadError" runat="server" ForeColor="Red" />
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
                <asp:TextBox ID="TextBoxFieldName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="TextBoxColumnPosition">Column position *</label>
                <asp:TextBox ID="TextBoxColumnPosition" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="TextBoxFieldLength">Field length *</label>
                <asp:TextBox ID="TextBoxFieldLength" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <asp:Button ID="ButtonAddRow" runat="server" Text="Add Row" CssClass="btn btn-default" OnClick="ButtonAddRow_Click" />
        </div>
        <br />
        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonAddRow" />
            </Triggers>
            <ContentTemplate>
                <asp:Label ID="LabelColumnsEmptyError" runat="server" ForeColor="Red"></asp:Label>
                <asp:BulletedList ID="BulletedListError" runat="server" ForeColor="Red"></asp:BulletedList>
                <div class="table-responsive-sm">
                    <asp:GridView CssClass="table" ID="GridViewLayout" runat="server" OnRowDeleting="GridViewLayout_RowDeleting">
                        <Columns>
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <br />
    <fieldset>
        <legend>Step 3: Convert</legend>
        <asp:Button ID="ButtonConvert" runat="server" Text="Convert" OnClick="ButtonConvert_Click" CssClass="btn btn-primary" />
        <br />
    </fieldset>
</asp:Content>

