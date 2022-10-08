<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PasswordRequest.aspx.vb" MasterPageFile="~/UserAdmin/User.master" Inherits="User_PasswordRequest" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
<form id="frm" runat="server">
    <div align="center">
    <table bgcolor="#003D7E" align="center"><tr><td>
    <table id="tRequest" runat="server" visible="true" border="0" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
        <tr><td colspan="2" bgcolor="#003D7E">&nbsp;</td></tr>
        <tr>
            <td><img src="../images/Email.jpg" border="0" alt="Password will be sent to your email address" /></td>
            <td>
                <font size="+2">Enter your email address:</font><br />
                <asp:TextBox ID="tEmail" runat="server" Width="150"></asp:TextBox><br />
                <asp:Button ID="bSend" runat="server" Text="Send Password" />
            </td>
        </tr>
    </table>
    <table id="tSuccess" runat="server" visible="false" border="0" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
        <tr><td colspan="2" bgcolor="#003D7E">&nbsp;</td></tr>
        <tr>
            <td><img src="../images/Email.jpg" border="0" alt="Check your email for your password" /></td>
            <td>
                Your password has been sent!<br />
                <br />
                <asp:Button ID="bContinue" runat="server" Text="Return to log in screen" />
            </td>
        </tr>
    </table>
    </td></tr></table>
    </div>
</form>
</asp:Content>