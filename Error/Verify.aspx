<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Verify.aspx.vb" Inherits="Error_Verify" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <title>Error: Unable to log in...</title>
    <style type="text/css">
		td	{font-family:Arial,Helvetica,sans-serif;font-size:9pt;color:black}
		p	{font-family:Arial,Helvetica,sans-serif;font-size:9pt;color:black}
		body{font-family:Arial,Helvetica,sans-serif;font-size:9pt;margin-left:10px;margin-right:0;margin-top:10px;margin-bottom:0;margin:10px;color:black}
	</style>
</head>
<body>
    <form id="frm" runat="server">
    <div align="center">
        <a href="../Default.aspx"><img src="../images/inactive.gif" border="0" alt="Click to return to log in page" /></a><br />
        We are unable to log you in because the User Validation email has not been acted upon.<br />
        Please check your email and follow the instruction to activate your account.<br />
        <br />
        If you need us to resend the email, click here:<br />
        <asp:Button ID="bUserRequest" runat="server" Text="Request Email" /><br />
        <br />
        ~Webmaster
    </div>
    </form>
</body>
</html>
