<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Restore.aspx.cs" Inherits="Restore" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div>
        <asp:DropDownList ID="ddlConnections" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlConnections_SelectedIndexChanged">
        </asp:DropDownList>
        
        <div id="restore">
            <asp:DropDownList ID="ddlDatabases" runat="server">
            </asp:DropDownList>

            <br />

            <asp:ListBox ID="lstBackupfiles" runat="server">
            </asp:ListBox>

            <br />

            <asp:Button CssClass="btn" ID="btnRestore" runat="server" UseSubmitBehavior="true" Text="Restore" onclick="btnRestore_Click" />
        </div>
    </div>
    <p><asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label></p>
</asp:Content>
