<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb" MasterPageFile="~/UserAdmin/User.master" Inherits="User_ChangePassword" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <form id="frm" runat="server">
    <div align="center">
    <table bgcolor="#003D7E" align="center" cellpadding="1">
    <tr><td colspan="3">&nbsp;</td></tr>
    <tr><td>
    <table id="tChange" runat="server" visible="true" border="0" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
        <tr><td colspan="3"><b><font size="+2" color="red">You must change your password</font></b>&nbsp;</td></tr>
        <tr>
            <td><img src="../images/changePassword.png" border="0" alt="Enter your new password" /></td>
            <td>
                Enter your current password:<br />
                <asp:TextBox ID="tCurrentPW" runat="server" Width="150px" TextMode="password"></asp:TextBox><br />
                <br />
                Enter your new password:<br />
                <asp:TextBox ID="tNewPW" runat="server" Width="150px" TextMode="password"></asp:TextBox><br />
                <br />
                Re-Enter new password:<br />
                <asp:TextBox ID="tConfirmPW" runat="server" Width="150px" TextMode="password"></asp:TextBox><br />
                <br />
                <asp:Button ID="bSubmit" runat="server" Text="Change Password" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr><td colspan="3">&nbsp;</td></tr>
    </table>
    <table id="tSuccess" runat="server" visible="false" border="0" bgcolor="#ffffff" cellpadding="0" cellspacing="0">
        <tr><td colspan="2" bgcolor="#003D7E">&nbsp;</td></tr>
        <tr>
            <td><img src="../images/Success.png" border="0" alt="Your password has been changed" /></td>
            <td>
                Your password has been changed!<br />
                <br />
                <asp:Button ID="bContinue" runat="server" Text="Continue" />
            </td>
        </tr>
    </table>
    <table id="tFailure" runat="server" visible="false" border="0" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
        <tr><td colspan="2" bgcolor="#003D7E">&nbsp;</td></tr>
        <tr>
            <td>&nbsp;</td>
            <td><img src="../images/Failure.png" border="0" alt="Your password has been changed" /></td>
            <td>
                Your password has been not been changed!<br />
                <asp:Label ID="lErr" runat="server" ForeColor="red" Font-Bold="true" Font-Size="Medium"></asp:Label><br />
                <br />
                <asp:Button ID="bRetry" runat="server" Text="Retry" />
            </td>
        </tr>
    </table>
    </td></tr>
    </table>
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        function validate() {
            var ctlPassword0 = document.getElementById("<%=tCurrentPW.ClientID%>")
            var ctlPassword1 = document.getElementById("<%=tNewPW.ClientID%>")
            var ctlPassword2 = document.getElementById("<%=tConfirmPW.ClientID%>")

            if (ctlPassword0.value.length == 0) {
                alert("You must supply your current password.");
                ctlPassword0.focus();
                return false;
            }
            if (ctlPassword1.value.length == 0) {
                alert("You must supply a new password.");
                ctlPassword1.focus();
                return false;
            }

            if (ctlPassword0.value == ctlPassword1.value) {
                alert("New password cannot be the same as old password.");
                ctlPassword1.focus();
                return false;
            }

            if (ctlPassword1.value != ctlPassword2.value) {
                alert("Passwords do not match.\nCheck your spelling.");
                ctlPassword1.focus();
                return false;
            }
            return true;
        }
    </script>
</asp:Content>