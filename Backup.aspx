<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Backup.aspx.cs" Inherits="Backup" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div>
        <asp:DropDownList ID="ddlConnections" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlConnections_SelectedIndexChanged">
        </asp:DropDownList>

        <div id="backup">
            <asp:DropDownList ID="ddlDatabases" runat="server">
            </asp:DropDownList>

            <br />

            <asp:Button CssClass="btn" ID="btnBackup" runat="server" UseSubmitBehavior="true" Text="Backup" OnClick="btnBackup_Click" />
        </div>
    </div>
    <p><asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label></p>
</asp:Content>
