<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SystemError.aspx.vb" Inherits="Error_SystemError" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
    <title>Help</title>
	<link href="../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="PageLayout">
        <img src="../images/banner-logo.png" alt="" />
	    <table id="tblErrorDesc" style="height:100%" cellspacing="0" cellpadding="0" width="100%" bgcolor="gainsboro" border="0">
		    <tr style="height:25px">
			    <td width="25"></td>
			    <td></td>
		    </tr>
		    <tr style="height:25px">
			    <td></td>
			    <td valign="top">
				    The following error has occurred and we are unable to process your request at 
				    this time.
				    <br />
				    This error has been recorded and will be investigated by the system 
				    administrator.
			    </td>
		    </tr>
		    <tr style="height:25px">
			    <td></td>
			    <td valign="top">
				    We apologize for this inconvenience.
			    </td>
		    </tr>
		    <tr style="height:25px">
			    <td></td>
			    <td valign="top">
				    <table id="tblError" cellspacing="0" cellpadding="10" width="700" bgcolor="white" border="1" bordercolor="black">
					    <tr>
						    <td class="ErrorText">
							    <asp:Label id="ErrorDescription" runat="server" Width="700px" Font-Bold="True" Font-Names="Arial" ForeColor="Red" BackColor="white"></asp:Label>
						    </td>
					    </tr>
				    </table>
			    </td>
		    </tr>
		    <tr style="height:25px">
			    <td></td>
			    <td valign="top">
				    Click
				    <asp:HyperLink id="prevLink" runat="server" BackColor="gainsboro">here</asp:HyperLink>
				    to return to previous page.
				    <br />
				    <br />
				    Click
				    <asp:HyperLink id="defaultLink" runat="server" BackColor="gainsboro">here</asp:HyperLink>
				    to restart.
			    </td>
		    </tr>
	    </table>
    </div>
</body>
</html>
