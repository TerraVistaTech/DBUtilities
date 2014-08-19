<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Download.aspx.cs" Inherits="Download" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div>
        <div id="download">

            <asp:ListBox ID="lstBackupfiles" runat="server">
            </asp:ListBox>

            <br />

            <asp:Button CssClass="btn" ID="btnDownload" runat="server" UseSubmitBehavior="true" Text="Download" onclick="btnDownload_Click" />
        </div>
    </div>
    <p><asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label></p>
</asp:Content>