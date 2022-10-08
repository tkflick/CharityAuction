<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Registered.aspx.vb" MasterPageFile="~/UserAdmin/User.master" Inherits="User_Registered" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <table bgcolor="#003D7E" align="center"><tr><td>
        <table align="center" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
            <tr><td colspan="2" bgcolor="#003D7E">&nbsp;</td></tr>
            <tr>
                <td><a href="#" onclick="javascript:window.close();"><img src="../images/Email.jpg" border="0" alt="Click to close this window." /></a></td>
                <td style="font-family:Arial;font-size:medium;">
                    <font face="arial" size="+2">Success!</font><br /><br />
                    You have been registered, however your account still needs to be activated.<br />
                    <br />
                    An email has been sent to the email address that you supplied.  
                    You must validate your account before you will be allowed to log in 
                    and begin using the system.<br />
                    <br />
                    Please check your email and follow the instructions there to activate your account.
                    <br /><br />
                    <b>~ Webmaster</b><br />&nbsp;
                </td>
            </tr>
        </table>
    </td></tr></table>
</asp:Content>