<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="FlatFilesConverter.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-xs-12 col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Register</h3>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <label for="TextBoxRegisterUsername">Username *</label>
                        <asp:TextBox ID="TextBoxRegisterUsername" runat="server" CssClass="form-control" autofocus="autofocus" required="required" placeholder="Email" />
                        <asp:RequiredFieldValidator ControlToValidate="TextBoxRegisterUsername" CssClass="text-danger" runat="server" ErrorMessage="Username is required!"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label for="TextBoxRegisterPassword">Password *</label>
                        <asp:TextBox ID="TextBoxRegisterPassword" runat="server" TextMode="Password" CssClass="form-control" required="required" placeholder="Password" />
                        <asp:RequiredFieldValidator ControlToValidate="TextBoxRegisterPassword"  CssClass="text-danger" runat="server" ErrorMessage="Password is required!"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label for="TextBoxRegisterPasswordConfirm">Confirm Password *</label>
                        <asp:TextBox ID="TextBoxRegisterPasswordConfirm" runat="server" TextMode="Password" CssClass="form-control" required="required" placeholder="Password" />
                        <asp:RequiredFieldValidator ControlToValidate="TextBoxRegisterPasswordConfirm"  CssClass="text-danger" runat="server" ErrorMessage="Password Confirmation is required!"></asp:RequiredFieldValidator>
                        <br />
                        <asp:CompareValidator ControlToValidate="TextBoxRegisterPasswordConfirm" ControlToCompare="TextBoxRegisterPassword" CssClass="text-danger"  runat="server" ErrorMessage="Password does not match!"></asp:CompareValidator>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div id="DivRegisterError" runat="server" visible="false">
                                <asp:Label ID="RegisterErrorMessages" CssClass="text-danger" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:Button ID="ButtonRegister" Text="Register" runat="server" CssClass="btn btn-primary" OnClick="ButtonRegister_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
