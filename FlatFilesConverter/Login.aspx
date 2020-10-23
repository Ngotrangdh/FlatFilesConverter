<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FlatFilesConverter.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-xs-12 col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Login</h3>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <label for="TextBoxLoginUsername">Username *</label>
                        <asp:TextBox ID="TextBoxLoginUsername" runat="server" CssClass="form-control" autofocus="autofocus" required="required" placeholder="Email" />
                        <asp:RequiredFieldValidator ControlToValidate="TextBoxLoginUsername" CssClass="text-danger" runat="server" ErrorMessage="Username is required!"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label for="TextBoxLoginPassword">Password *</label>
                        <asp:TextBox ID="TextBoxLoginPassword" runat="server" TextMode="Password" CssClass="form-control" required="required" placeholder="Password" />
                        <asp:RequiredFieldValidator ControlToValidate="TextBoxLoginPassword"  CssClass="text-danger" runat="server" ErrorMessage="Password is required!"></asp:RequiredFieldValidator>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div id="DivLoginError" runat="server" visible="false">
                                <asp:Label ID="LoginErrorMessages" CssClass="text-danger" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:Button ID="ButtonLogin" Text="Log In" runat="server" CssClass="btn btn-primary" OnClick="ButtonLogin_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
