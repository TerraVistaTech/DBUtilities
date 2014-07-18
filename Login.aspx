<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<html>
<head>
<title>Standard Forms Authentication Login Form</title>
</head>

<body bgcolor="#FFFFFF" text="#000000">
<form runat="server">
<table width="400" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="80">Username : </td>
    <td width="10"> </td>
    <td><asp:TextBox Id="txtUser" width="150" runat="server"/></td>
  </tr>
  <tr>
    <td>Password : </td>
    <td width="10"> </td>
    <td><asp:TextBox Id="txtPassword" width="150" TextMode="Password" runat="server"/></td>
  </tr>
  <tr>
  <tr>
    <td></td>
    <td width="10"> </td>
    <td><asp:CheckBox id="chkPersistLogin" runat="server" />Remember my credentials<br>
    </td>
  </tr>
  <tr>
    <td> </td>
    <td width="10"> </td>
    <td><asp:Button Id="cmdLogin" OnClick="ProcessLogin" Text="Login" runat="server" /></td>
  </tr>
</table>
<br>
<br>
<div id="ErrorMessage" runat="server" />
</form>
</body>
</html>
