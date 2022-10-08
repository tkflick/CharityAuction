<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Register.aspx.vb" MasterPageFile="~/UserAdmin/User.master" Inherits="User_Register" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <form id="frm" runat="server">
        <br />
        <table bgcolor="#003D7E" align="center"><tr><td>
        <table id="tRegister" runat="server" border="0" bgcolor="#FFFFFF" cellpadding="0" cellspacing="0">
            <tr><td colspan="7" bgcolor="#003D7E">&nbsp;</td></tr>
            <tr>
                <td rowspan="10" align="center" width="200"><img src="../images/Register.jpg" alt="" /></td>
                <td colspan="5" align="center">
                    <font face="arial" size="+2">
                        Charity Auction User Registration
                    </font>
                </td>
            </tr>
            <tr>
                <td align="right" nowrap="nowrap" style="font-family:Arial;font-size:small;">First Name:</td>
                <td colspan="5" nowrap="nowrap">
                    <asp:TextBox ID="tFName" runat="server" BackColor="Cornsilk"></asp:TextBox>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" nowrap="nowrap" style="font-family:Arial;font-size:small;">Last Name:</td>
                <td colspan="5" nowrap="nowrap">
                    <asp:TextBox ID="tLName" runat="server" BackColor="Cornsilk"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-family:Arial;font-size:small;">Email Address:</td>
                <td>
                    <asp:TextBox ID="tEmail" runat="server" BackColor="Cornsilk"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="right" style="font-family:Arial;font-size:small;">Confirm Email:</td>
                <td>
                    <asp:TextBox ID="tConfirmEmail" runat="server" BackColor="Cornsilk"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="right" style="font-family:Arial;font-size:small;" nowrap="nowrap">Receive email when outbid?</td>
                <td colspan="4">
                    <asp:CheckBox runat="server" ID="chkOutbidEmail" />
                </td>
                <td>&nbsp;</td>
            </tr>

            <tr>
                <td align="right" style="font-family:Arial;font-size:small;">Choose user name:</td>
                <td colspan="4">
                    <asp:TextBox ID="tUserName" runat="server" BackColor="Cornsilk"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="right" style="font-family:Arial;font-size:small;">Choose display nickname:</td>
                <td colspan="4">
                    <asp:TextBox ID="tNickname" runat="server" BackColor="Cornsilk"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="right" style="font-family:Arial;font-size:small;" nowrap="nowrap">Choose a Password:</td>
                <td>
                    <asp:TextBox ID="tPassword" runat="server" TextMode="password" BackColor="Cornsilk"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="right" style="font-family:Arial;font-size:small;" nowrap="nowrap">Confirm Password:</td>
                <td>
                    <asp:TextBox ID="tConfirmPW" runat="server" TextMode="password" BackColor="Cornsilk"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="7" align="center">
                    <br /><asp:Button ID="bSubmit" runat="server" Text="Submit" />
                </td>
            </tr>
        </table>
        </td></tr></table>
        <br />
        <div align="center">
            <asp:Label ID="lblError" runat="server" Font-Size="medium" Forecolor="red" Visible="false" ></asp:Label>
        </div>
    </form>
    <script language="javascript" type="text/javascript">
        function validate() {
            var ctlFName = document.getElementById("<%=tFName.ClientID%>")
            var ctlLName = document.getElementById("<%=tLName.ClientID%>")
            var ctlEmail1 = document.getElementById("<%=tEmail.ClientID%>")
            var ctlEmail2 = document.getElementById("<%=tConfirmEmail.ClientID%>")
            var ctlUserName = document.getElementById("<%=tUserName.ClientID%>")
            var ctlPassword1 = document.getElementById("<%=tPassword.ClientID%>")
            var ctlPassword2 = document.getElementById("<%=tConfirmPW.ClientID%>")

            if (ctlFName.value.length == 0) {
                alert("You must supply your first and last name.");
                ctlFName.focus();
                return false;
            }
            if (ctlLName.value.length == 0) {
                alert("You must supply your first and last name.");
                ctlLName.focus();
                return false;
            }
            if (ctlEmail1.value.length == 0) {
                alert("You must supply an email address.");
                ctlEmail1.focus();
                return false;
            }
            if (!ctlEmail2.value == ctlEmail1.value) {
                alert("Your email addresses do not match.");
                ctlEmail1.focus();
                return false;
            }
            if (ctlUserName.value.length == 0) {
                alert("You must choose a user name.");
                ctlUserName.focus();
                return false;
            }
            if (ctlPassword1.value.length == 0) {
                alert("You must choose a password.");
                ctlPassword1.focus();
                return false;
            }
            if (ctlPassword2.value != ctlPassword2.value) {
                alert("Your passwords do not match.");
                ctlPassword1.focus();
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
