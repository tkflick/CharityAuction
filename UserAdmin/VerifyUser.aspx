<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VerifyUser.aspx.vb" MasterPageFile="~/UserAdmin/User.master" Inherits="User_VerifyUser" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <table id="tSuccess" runat="server" bgcolor="#003D7E" align="center" visible="false"><tr><td>
        <table align="center" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
            <tr><td colspan="2" bgcolor="#003D7E">&nbsp;</td></tr>
            <tr>
                <td><a href="#" onclick="javascript:window.close();"><img src="../images/Success.png" border="0" alt="Click to close this window." /></a></td>
                <td style="font-family:Arial;font-size:medium;">
                    <font size="+2">Thank you for activating your account!</font><br /><br />
                    You may now begin using the application.<br />
                    <br /><br />
                    <b>~ Webmaster</b><br />&nbsp;
                </td>
            </tr>
        </table>
        </td></tr></table>
        <table id="tFailure" runat="server" bgcolor="#003D7E" align="center" visible="false"><tr><td>
        <table align="center" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
            <tr><td colspan="2" bgcolor="#003D7E">&nbsp;</td></tr>
            <tr>
                <td><a href="#" onclick="javascript:window.close();"><img src="../images/Failure.png" border="0" alt="Click to close this window." /></a></td>
                <td style="font-family:Arial;font-size:medium;">
                    <font size="+2">An error occurred!</font><br /><br />
                    An email has been sent to our development team.<br />
                    Please wait a few minutes and try again. <br />
                    <br /><br />
                    <b>~ Webmaster</b><br />&nbsp;<br />
                    <br />
                    <asp:Label ID="err" runat="server" Font-Bold="true" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </td></tr></table>
</asp:Content>