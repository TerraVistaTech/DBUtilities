<%@ Page Title="SIS SQL Backup ondemand" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Backup.aspx.cs" Inherits="Backup" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div>
        <table class="center">
            <tr>
                <td colspan="2">
                    Please select a server and a database.
                </td>
            </tr>
            <tr>
                <td class="textright">Server connection:</td>
                <td class="textleft">
                    <asp:DropDownList ID="ddlConnections" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlConnections_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="right">Database:</td>
                <td class="textleft"><asp:DropDownList Enabled="false" CssClass="fullWidth" ID="ddlDatabases" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td class="center" colspan="2">
                    <asp:Button CssClass="btn" ID="btnBackup" runat="server" UseSubmitBehavior="true" Text="Backup" OnClick="btnBackup_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:Label CssClass="center" ID="lblMessage" ForeColor="Red" runat="server" Text="" /></td>
            </tr>
        </table>
    </div>
</asp:Content>
