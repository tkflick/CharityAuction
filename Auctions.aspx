<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Auctions.aspx.vb" MasterPageFile="~/Auction.master" Inherits="Auctions" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <div class="PageLayout">
		<form id="Form1" method="post" runat="server">
			<div class="SubHeading">
                <asp:Literal ID="litHeading" runat="server" EnableViewState="false" Visible="false"/>
			</div>
			<asp:datalist id="dlListings" runat="server" BorderStyle="None" BorderWidth="0px" EnableViewState="False" RepeatLayout="Table" >
				<HeaderTemplate>
					<table class="AuctionList">
						<tr class="ThemeColor" style="height:35px;">
							<td nowrap="nowrap"><a href="Auctions.aspx?s=item_name<%=sQS%>"><b>Item</b></a></td>
							<td nowrap="nowrap" width="100" style="text-align:center;"><b># of Bids</b></td>
							<td nowrap="nowrap" width="100" style="text-align:center;"><b>Current Bid</b></td>
							<td nowrap="nowrap" width="100" style="text-align:center;"><a href="Auctions.aspx?s=item_date_close<%=sQS%>"><b>Time Left</b></a></td>
						</tr>
				</HeaderTemplate>
				<FooterTemplate>
					<tr class="ThemeColor" style="height:5px;"> 
						<td colspan="4"></td>
					</tr>
					</table>
				</FooterTemplate>
				<ItemTemplate>
					<tr class="ThemeColorAlt">
						<td width="100%"><a href='Item.aspx?i=<%# DataBinder.Eval(Container.DataItem, "item_id") %>'><%# DataBinder.Eval(Container.DataItem, "item_name") %></a><br>
							<%# FormatDescription(DataBinder.Eval(Container.DataItem, "item_description").ToString()) %>
						</td>
						<td nowrap="nowrap" style="text-align:center;"><%# DataBinder.Eval(Container.DataItem, "item_bids") %></td>
						<td nowrap="nowrap"><%# FormatAmount(DataBinder.Eval(Container.DataItem, "item_amount").ToString()) %></td>
						<td nowrap="nowrap" style="text-align:right;"><%# FormatCountdown(DataBinder.Eval(Container.DataItem, "item_date_close").ToString()) %></td>
					</tr>
				</ItemTemplate>
				<AlternatingItemTemplate>
					<tr class="ThemeColorAlt2">
						<td width="100%"><a href='Item.aspx?i=<%# DataBinder.Eval(Container.DataItem, "item_id") %>'><%# DataBinder.Eval(Container.DataItem, "item_name") %></a><br>
							<%# FormatDescription(DataBinder.Eval(Container.DataItem, "item_description").ToString()) %>
						</td>
						<td nowrap="nowrap" style="text-align:center;"><%# DataBinder.Eval(Container.DataItem, "item_bids") %></td>
						<td nowrap="nowrap"><%# FormatAmount(DataBinder.Eval(Container.DataItem, "item_amount").ToString())%></td>
						<td nowrap="nowrap" style="text-align:right;"><%# FormatCountdown(DataBinder.Eval(Container.DataItem, "item_date_close").ToString())%></td>
					</tr>
				</AlternatingItemTemplate>
			</asp:datalist>
            <asp:Panel runat="server" ID="pnlNoAuctions">
                <h2>No Current Auctions.</h2>
                If you would like to be notified when a new auction is added, <a href="reminder.aspx">click here</a>.
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlMoneyRaised">
			    <div class="MoneyRaised">
                    So far, we have raised a total of $<asp:Literal id="litTotal" runat="server" EnableViewState="false">0</asp:Literal>.<br />
                    <b>Thank you to everyone who has supported the <%=sClubName%>!</b>
			    </div>
            </asp:Panel>
		</form>
    </div>
</asp:Content>