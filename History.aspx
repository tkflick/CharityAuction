<%@ Page Language="VB" AutoEventWireup="false" CodeFile="History.aspx.vb" Inherits="History" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
	<title>Bid History</title>
	<link rel="stylesheet" type="text/css" href="./css/style.css">
</head>
<body bgcolor="white">
    <div class="PageLayout">
		<form id="Form1" method="post" runat="server">
            <div class="Heading">
                <a href="./Auctions.aspx"><img alt="Banner Image" src="./images/banner-logo.png"  class="BannerImage" /></a>
            </div>
			<table class="AuctionItem" >
				<tr style="height:35px;">
					<td>
			            <h2>Bid History</h2>
                        <asp:Label runat="server" ID="lblItemName" Font-Bold="true" Font-Size="Medium" Font-Underline="true"></asp:Label>
			            <asp:datalist id="dlListings" runat="server">
				            <HeaderTemplate>
					            <table border="0" cellpadding="4" cellspacing="0">
				            </HeaderTemplate>
				            <FooterTemplate>
					            </TABLE>
				            </FooterTemplate>
				            <ItemTemplate>
					            <tr>
						            <td width="150"><%# DataBinder.Eval(Container.DataItem, "item_date_bid") %></td>
						            <td width="80">
                                        <b>$<%# trimer(DataBinder.Eval(Container.DataItem, "item_amount").ToString()) %></b>
						            </td>
						            <td>
                                        <%# DataBinder.Eval(Container.DataItem, "item_bidder") %>
						            </td>
					            </tr>
				            </ItemTemplate>
			            </asp:datalist>
			            <p>
				            <a href="javascript:window.close()">Close</a>
			            </p>
					</td>
				</tr>
			</table>
		</form>
    </div>
</body>
</html>
