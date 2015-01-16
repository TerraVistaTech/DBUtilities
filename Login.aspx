<%@ Page Title="SIS SQL Backup ondemand" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div>
        <table class="center">
            <tr>
                <td width="80">Username : </td>
                <td width="10"></td>
                <td>
                    <asp:TextBox ID="txtUser" Width="150" runat="server" /></td>
            </tr>
            <tr>
                <td>Password : </td>
                <td width="10"></td>
                <td>
                    <asp:TextBox ID="txtPassword" Width="150" TextMode="Password" runat="server" /></td>
            </tr>
            <tr>
            <tr>
                <td></td>
                <td width="10"></td>
                <td>
                    <asp:CheckBox ID="chkPersistLogin" runat="server" />Remember my credentials<br>
                </td>
            </tr>
            <tr>
                <td></td>
                <td width="10"></td>
                <td>
                    <asp:Button ID="cmdLogin" OnClick="ProcessLogin" Text="Login" runat="server" /></td>
            </tr>
        </table>
        <div id="ErrorMessage" runat="server" />
    </form>
</asp:Content>
