<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Category.aspx.vb" Inherits="Category" %>

<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
		<title>Charity Auction</title>
        <link href="./css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div class="PageLayout">
    <form id="form1" runat="server">
            <div class="Heading">
                <a href="~/Help.aspx" target="_blank" class="HelpLink"><img alt="Help Image" class="HelpLinkImage" src="./images/help.jpg" /></a>
                <a href="~/Auctions.aspx"><img alt="Banner Image" src="./images/banner-logo.png"  class="BannerImage" /></a>
            </div>
    		<asp:datalist id="dlCategorys" runat="server" BorderStyle="None" BorderWidth="0px" EnableViewState="False">
            <HeaderTemplate>
				<table class="AuctionItem">
					<tr class="ThemeColor" style="height:35px;">
						<td nowrap="nowrap"><b>Categories</b></td>
					</tr>
			</HeaderTemplate>
			<FooterTemplate>
				<tr class="ThemeColor" style="height:5px;">
					<td colspan="6"></td>
				</tr>
				</table>
			</FooterTemplate>
			<ItemTemplate>
				<tr class="ThemeColorAlt">
					<td width="100%">
						<%# DataBinder.Eval(Container.DataItem, "cat_name") %>&nbsp;
						<a href='Auctions.aspx?c=<%# DataBinder.Eval(Container.DataItem, "cat_id")%>'>(<%# DataBinder.Eval(Container.DataItem, "TOTAL_ITEMS")%>)</a>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="ThemeColorAlt2">
					<td width="100%">
						<%# DataBinder.Eval(Container.DataItem, "cat_name") %>&nbsp;
						<a href='Auctions.aspx?c=<%# DataBinder.Eval(Container.DataItem, "cat_id")%>'>(<%# DataBinder.Eval(Container.DataItem, "TOTAL_ITEMS")%>)</a>
					</td>
				</tr>
			</ItemTemplate>
			</AlternatingItemTemplate>
		</asp:datalist>
    </form>
</div>
</body>
</html>