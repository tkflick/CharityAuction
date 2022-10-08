<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Resent.aspx.vb" MasterPageFile="~/UserAdmin/User.master" Inherits="User_Resent" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <table bgcolor="#003D7E" align="center"><tr><td>
        <table align="center" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
            <tr><td colspan="2" bgcolor="#003D7E">&nbsp;</td></tr>
            <tr>
                <td><a href="#" onclick="javascript:window.close();"><img src="../images/Email.jpg" border="0" alt="Click to close this window." /></a></td>
                <td style="font-family:Arial;font-size:medium;">
                    <font size="+2">Success!</font><br /><br />
                    Your email has been resent to the email address that you supplied during registration. 
                    You must validate your account before you will be allowed to log in and begin using the system.<br />
                    <br />
                    Please check your email and follow the instructions there, to activate your account.
                    <br /><br />
                    <b>~ Webmaster</b><br />&nbsp;
                </td>
            </tr>
        </table>
    </td></tr></table>
</asp:Content>