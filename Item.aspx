<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Item.aspx.vb" MasterPageFile="~/Auction.master" Inherits="Item" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <div class="PageLayout">
		<form id="Form1" method="post" runat="server">
			<table class="AuctionItem" >
				<tr class="ThemeColor" style="height:35px;">
					<td>
                        <asp:label id="lblItemName" runat="server" Font-Size="Medium" Font-Bold="true"></asp:label>
                    </td>
					<td style="text-align:right;">
						Lot number <asp:label id="lblItemId" runat="server"></asp:label>
					</td>
	    		</tr>
                <tr><td /></tr>
				<tr class="ThemeColorAlt2">
					<td style="vertical-align:top; width:50%;">

                        <table width="100%" border="0">
							<tr>
								<td>Current bid:</td>
								<td>
                                    <asp:label id="lblCurrentBid" runat="server"></asp:label>&nbsp;
									<asp:linkbutton id="LinkButton1" runat="server" Font-Size="X-Small" CausesValidation="False" onclick="LinkButton1_Click">Refresh</asp:linkbutton>
                                </td>
							</tr>
							<tr>
								<td>End Time:</td>
								<td>
                                    <asp:label id="lblEndTime" runat="server"></asp:label>
                                </td>
							</tr>
							<tr>
								<td>History:</td>
								<td>
                                    <asp:hyperlink id="lnkBids" runat="server" Target="_blank"></asp:hyperlink>
                                </td>
							</tr>
							<tr>
								<td><asp:Label runat="server" ID="lBidder"></asp:Label></td>
								<td>
									<asp:label id="lblHighBidder" runat="server"></asp:label>&nbsp;&nbsp;
                                    <asp:label id="lblWinner" runat="server" Visible="true" Text="" Font-Bold="true" ForeColor="Red" ToolTip="Click here to check out"></asp:label>
                                    <asp:HiddenField ID="hidHighBidder" runat="server" />
                                </td>
							</tr>
							<tr>
								<td>Location:</td>
								<td>
                                    <asp:label id="lblLocation" runat="server"></asp:label>
                                </td>
							</tr>
							<tr>
								<td colspan="2">This item generously donated by: <asp:label id="lblDonatedBy" runat="server"></asp:label>
                                </td>
							</tr>
						</table>
					</td>
					<td style="vertical-align:top;width:50%;">
						<table width="100%" align="right" border="0">
							<tr>
								<td style="vertical-align:top;text-align:right;" colspan="2">
                                    <asp:Literal id="litUpdate" runat="server"></asp:Literal>
                                </td>
							</tr>
							<tr>
								<td style="text-align:right;" colspan="2">
                                    Place a bid&nbsp;
									<asp:textbox id="txtBid" onkeypress="return noenter()" runat="server" Width="80px" MaxLength="11"></asp:textbox>
									<asp:button id="btnBid" runat="server" Width="80px" Text="Bid Now" CssClass="ThemeColor"></asp:button>
									<br />
									<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="<br>No bid amount" ControlToValidate="txtBid" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtBid" ErrorMessage="Bid format incorrect" ValidationExpression="^\d+(?:\.\d{0,2})?$"></asp:RegularExpressionValidator>
								</td>
							</tr>
						</table>
					</td>
				</tr>
                <tr><td /></tr>
				<tr class="ThemeColor">
					<td>Description</td>
                    <td />
				</tr>
            </table>
        </form>
        <table class="AuctionItem" >
			<tr>
				<td class="ThemeColorAlt2" colspan="2" align="center">
					<div style="padding:5px;">
                        <asp:label id="lblDescription" runat="server"></asp:label>
                    </div>
                    <center><asp:Panel runat="server" ID="images" Visible="true">
                        <input type="image" src="./images/btn-lt.png" onclick="javascript:plusDivs(-1)" name="left" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="image" src="./images/btn-rt.png" onclick="javascript:plusDivs(1)" name="right" />
                    </asp:Panel></center>
                    <div style="text-align:left">
                        <asp:Repeater id="Repeater1" runat="server">
                            <ItemTemplate>
                            <div class="mySlides" style="position:center;">
                                <center><img src="./auction_pictures/<%# Container.DataItem %>" alt="" /></center>
                            </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
					<div style="text-align:right">
                        Seller:
						<asp:label id="lblSeller" runat="server"></asp:label>
                    </div>
				</td>
			</tr>
			<tr><td /></tr>
			<tr class="ThemeColor" style="height:5px;">
				<td colspan="5"></td>
			</tr>
		</table>
        <div class="ItemFooter">
			<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="Auctions.aspx">Back to Auctions</asp:HyperLink>&nbsp;&nbsp;
            <asp:HyperLink ID="AdminLink" runat="server" NavigateUrl="./Admin/Home.aspx" Visible="false">Admin Application</asp:HyperLink>
		</div>
	</div>
	<script type="text/javascript" >
    function noenter() {
        return !(window.event && window.event.keyCode == 13); }
    </script>
    <script type="text/javascript">
        var slideIndex = 1;
        showDivs(slideIndex);

        function plusDivs(n) {
            showDivs(slideIndex += n);
        }

        function showDivs(n) {
            var i;
            var x = document.getElementsByClassName("mySlides");
            if (n > x.length) { slideIndex = 1 }
            if (n < 1) { slideIndex = x.length }
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            x[slideIndex - 1].style.display = "block";
        }
    </script>
</asp:Content>