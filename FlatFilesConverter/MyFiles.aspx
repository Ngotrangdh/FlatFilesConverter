<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyFiles.aspx.cs" Inherits="FlatFilesConverter.MyFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-xs-12 col-md-4">
            <div>
                <asp:GridView CssClass="table" AutoGenerateColumns="False" ID="GridViewFileList" runat="server" GridLines="None" ShowHeader="False">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="FileName" DataNavigateUrlFormatString="MyFiles?id={0}" DataTextField="FileName" HeaderText="File Name" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" ReadOnly="True" ShowHeader="False" />
                        <asp:TemplateField HeaderText="Link" SortExpression="Link">
                            <ItemTemplate>
                                <asp:LinkButton runat="server">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="col-xs-12 col-md-8">
            <div class="row">
                <asp:Button ID="ButtonDownloadCSV" Visible="false" runat="server" Text="Download as CSV" OnClick="ButtonDownloadCSV_Click" />
                <asp:Button ID="ButtonDownloadFixedWidth" Visible="false" runat="server" Text="Download as Fixed Width" OnClick="ButtonDownloadFixedWidth_Click" />
            </div>
            <br />
            <div class="row">
                <asp:GridView CssClass="table" ID="GridViewFileView" runat="server">
                </asp:GridView>
                <asp:Label ID="LabelNoFileUploaded" runat="server"></asp:Label>
                <asp:Label ID="LabelNoTableChoosen" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
