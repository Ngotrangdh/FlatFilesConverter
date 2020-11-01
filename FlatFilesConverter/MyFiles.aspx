<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyFiles.aspx.cs" Inherits="FlatFilesConverter.MyFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid body-content">
        <div class="row">
            <div class="col-xs-12 col-md-4">
                <asp:GridView CssClass="table no-border" AutoGenerateColumns="False" ID="GridViewFileList" runat="server" GridLines="None" ShowHeader="False" OnRowDeleting="GridViewFileList_RowDeleting">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ControlStyle-ForeColor="Red" />
                        <asp:HyperLinkField DataNavigateUrlFields="FileName" DataNavigateUrlFormatString="MyFiles?id={0}" DataTextField="FileName" HeaderText="File Name" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" ReadOnly="True" ShowHeader="False" />
                        <asp:TemplateField HeaderText="Link" SortExpression="Link">
                            <ItemTemplate>
                                <asp:LinkButton runat="server"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col-xs-12 col-md-8">
                <div class="row">
                    <asp:Button ID="ButtonDownloadCSV" Visible="false" runat="server" Text="Download as CSV" CssClass="btn btn-primary" OnClick="ButtonDownloadCSV_Click" />
                    <asp:Button ID="ButtonDownloadFixedWidth" Visible="false" runat="server" Text="Download as Fixed Width" CssClass="btn btn-primary" OnClick="ButtonDownloadFixedWidth_Click" />
                </div>
                <br />
                <div class="row">
                    <asp:GridView CssClass="table" ID="GridViewFileView" runat="server">
                    </asp:GridView>
                    <h3 class="text-center">
                        <asp:Label ID="LabelNoFileUploaded" runat="server" CssClass="label label-primary"></asp:Label>
                        <asp:Label ID="LabelNoTableChoosen" runat="server" CssClass="label label-primary"></asp:Label>
                    </h3>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
