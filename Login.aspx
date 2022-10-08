<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" MasterPageFile="~/Auction.master"Inherits="Login" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <form id="frm" runat="server">
    <br /><br />
    <table width="400px" align="center" cellpadding="0px" cellspacing="0px">
	    <tr>
		    <td width="15px">&nbsp;</td>
		    <td>
			    <table width="100%" cellpadding="0px" cellspacing="0px" border="0" align="left">
				    <tr><td height="11px" colspan="3"></td></tr>
				    <tr>
					    <td colspan="3">
					        <table cellpadding="0" cellspacing="0" border="0" width="100%" bgcolor="#FFFFFF">
                                <tr><td colspan="4" bgcolor="#003D7E" style="height:2px"></td></tr>
							    <tr valign="middle" style="height:25px">
                                    <td width="3px" bgcolor="#003D7E">&nbsp;</td>
                                    <td style="background-color:#003D7E; background-repeat:repeat-x;" valign="middle"><img src="./images/Secrecy.png" alt="" title="" /></td>
								    <td style="background-color:#003D7E; background-repeat:repeat-x; width:100%" valign="middle"><font size="2" color="white"><b>Charity Auction Login</b></font></td>
                                    <td width="3px" bgcolor="#003D7E">&nbsp;</td>
							    </tr>
                            </table>
                            <table cellpadding="0px" cellspacing="0px" border="0" width="100%" bgcolor="#FFFFFF">
                                <tr><td colspan="3" bgcolor="#003D7E" style="height:3px"></td></tr>
							    <tr>
								    <td width="3px" bgcolor="#003D7E">&nbsp;</td>
								    <td>
								        <asp:Label ID="lErrorMsg" runat="server" Visible="false" ForeColor="red"></asp:Label>
									    <table border="0" cellspacing="0px" cellpadding="1px">
										    <tr><td rowspan="8" width="20px" valign="top">&nbsp;</td><td colspan="2">&nbsp;</td><td rowspan="7"><img src="./images/main.png" alt="" /></td></tr>
										    <tr>
											    <td colspan="2" align="center">&nbsp;</td>
										    </tr>
										    <tr>
											    <td align="right" nowrap="nowrap">
												    <b>Website URL:</b>
											    </td>
                                                <td>
                                                    &nbsp;<b><i><u><font color="maroon"><%=sWebURL%></font></u></i></b>
											    </td>
										    </tr>
										    <tr>
											    <td align="right">
												    <b>Username</b>
											    </td>
											    <td>
												    &nbsp;<asp:TextBox ID="tUserName" runat="server"></asp:TextBox>
											    </td>
										    </tr>
										    <tr>
											    <td align="right">
												    <b>Password</b>
											    </td>
											    <td>
												    &nbsp;<asp:TextBox ID="tPassword" runat="server" TextMode="password"></asp:TextBox>
											    </td>
										    </tr>
										    <tr>
											    <td>&nbsp;</td>
											    <td>
												    <br />
												    <asp:ImageButton ID="bSubmit" runat="server" ImageUrl="./images/login.png" BorderWidth="0px" />
											    </td>
										    </tr>
										    <tr>
											    <td colspan="3">
												    <br />
                                                    <span style="font-size:x-small">
                                                    <b>Register:</b> <a href="./UserAdmin/Register.aspx">Sign up to use the Auction Site</a><br />
												    <b>Forgot Your Password?</b> <a href="./UserAdmin/PasswordRequest.aspx">Password Reminder</a><br />
                                                    </span>
											    </td>
										    </tr>
									    </table>
 								    </td>
								    <td width="3px" bgcolor="#003D7E">&nbsp;</td>
							    </tr>
                                <tr>
                                    <td width="3px" bgcolor="#003D7E">&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td width="3px" bgcolor="#003D7E">&nbsp;</td>
                                </tr>
							    <tr>
								    <td colspan="3" bgcolor="#003D7E" height="4px" align="center">
								        <asp:Label ID="lError" runat="server" ForeColor="cornsilk" Font-Size="small" Font-Bold="true" Visible="false"></asp:Label>
								    </td>
							    </tr>
						    </table>
					    </td>
				    </tr>
				    <tr><td colspan="3" height="11px"></td></tr>
			    </table>
		    </td>
	    </tr>
    </table>
    </form>

<script language="javascript" type="text/javascript">
    function validate() { 
        var ctlUserID = document.getElementById("<%=tUserName.clientID%>")
        var ctlPassword = document.getElementById("<%=tPassword.clientID%>")

        if (ctlUserID.value.length == 0) {
            alert("You must supply a user name.");
            ctlUserID.focus();
            return false;
        }
        if (ctlPassword.value.length == 0) {
            alert("You must supply a password.");
            ctlPassword.focus();
            return false;
        }
        return true;
    }
</script>
</asp:Content>