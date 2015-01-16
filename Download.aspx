<%@ Page Title="SIS SQL Backup ondemand" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Download.aspx.cs" Inherits="Download" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div>
        <table class="center">
            <tr>
                <td>Please select a backup to download</td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox ID="lstBackupfiles" runat="server"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button CssClass="btn" ID="btnDownload" runat="server" UseSubmitBehavior="true" Text="Download" onclick="btnDownload_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
</asp:Content>