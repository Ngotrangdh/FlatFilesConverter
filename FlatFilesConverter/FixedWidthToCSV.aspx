<%@ Page Title="Fixed Width To CSV" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FixedWithToCSV.aspx.cs" Inherits="FlatFilesConverter.FixedWidthToCSV" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container body-content">
        <fieldset>
            <legend>Step 1: Select your input </legend>
            <div class="form-group">
                <label for="FileUpload">File Input</label>
                <asp:FileUpload ID="FileUpload" runat="server" />
                <asp:Label ID="LabelFileUploadError" runat="server" ForeColor="Red"></asp:Label>
            </div>
            <div class="checkbox">
                <label>
                    <asp:CheckBox ID="CheckBoxIsFirstLineHeader" runat="server" />
                    Is first line header
                </label>
            </div>

            <asp:UpdatePanel runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonAddRow" />
                </Triggers>
                <ContentTemplate>
                    <div class="form-inline">
                        <div class="form-group">
                            <label for="TextBoxFieldName">Field name</label>
                            <asp:TextBox ID="TextBoxFieldName" runat="server" CssClass="form-control"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxFieldName" ErrorMessage="Field name required" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="form-group">
                            <label for="TextBoxColumnPosition">Column Position</label>
                            <asp:TextBox ID="TextBoxColumnPosition" runat="server" CssClass="form-control"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxColumnPosition" ErrorMessage="Column position is required" Text="**" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <%--<asp:RegularExpressionValidator runat="server" ValidationExpression="^\d+$" ControlToValidate="TextBoxColumnPosition" ErrorMessage="Invalid column position input" Text="*" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                        </div>
                        <div class="form-group">
                            <label for="TextBoxFieldLength">Field length</label>
                            <asp:TextBox ID="TextBoxFieldLength" runat="server" CssClass="form-control"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxFieldLength" ErrorMessage="Field name is required" Text="**" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <%--<asp:RegularExpressionValidator runat="server" ValidationExpression="^\d+$" ControlToValidate="TextBoxFieldLength" ErrorMessage="Invalid field length input" Text="*" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                        </div>
                        <asp:Button ID="ButtonAddRow" runat="server" Text="Add Row" CssClass="btn btn-default" OnClick="ButtonAddRow_Click" />
                    </div>
                    <br />
                    <%--<asp:ValidationSummary ForeColor="Red" ID="ErrorSummary" runat="server" />--%>
                </ContentTemplate>
            </asp:UpdatePanel>


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
            <legend>Step 2: Define Output Delimiter</legend>
            <div class="form-group">
                <label for="TextBoxDelimiter">Delimiter</label>
                <asp:TextBox ID="TextBoxDelimiter" runat="server" CssClass="form-control" Text=","></asp:TextBox>
                <asp:Label ID="LabelDelimiterError" runat="server" ForeColor="Red"></asp:Label>

            </div>
        </fieldset>
        <fieldset>
            <legend>Step 3: Convert</legend>
            <asp:Button ID="ButtonConvert" runat="server" Text="Convert" OnClick="ButtonConvert_Click" CssClass="btn btn-primary" />
            <br />
        </fieldset>
    </div>
</asp:Content>
