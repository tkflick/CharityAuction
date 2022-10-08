<%@ Page Language="VB" AutoEventWireup="false" CodeFile="reminder.aspx.vb" MasterPageFile="~/Auction.master" Inherits="reminder" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <div class="PageLayout">
		<form id="Form1" method="post" runat="server">
            <table width="100%" border="0">
                <tr>
                    <td colspan="2">
                        <h2><font color="black">Thank you!</font></h2>
                         Your email (<asp:label id="lblEmail" runat="server"></asp:label>) will be signed up to receive a notification when 
                        we add a new fundraiser auction item!<br />
                        <br />
                        <asp:Button runat="server" ID="btnNotification" Text="Get Notified" />
                    </td>
                </tr>
	        </table>
            <div class="ItemFooter">
				<br /><br /><asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="Auctions.aspx">Back to Auctions</asp:HyperLink>
		    </div>
        </form>
	</div>
</asp:Content>