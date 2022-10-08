<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Checkout.aspx.vb" MasterPageFile="~/Auction.master" Inherits="Checkout" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <script src="https://www.paypalobjects.com/api/checkout.js" type="text/javascript"></script>
    <script type="text/javascript">
        paypal.Button.render({

            env: '<%=Paypal_Env%>',  // sandbox | production

            // Specify the style of the button
            style: {
                layout: 'vertical',  // horizontal | vertical
                size: 'medium',      // medium | large | responsive
                shape: 'rect',       // pill | rect
                color: 'gold'        // gold | blue | silver | black
            },

            funding: {
                allowed: [paypal.FUNDING.CARD],
                disallowed: []
            },

            client: {
                sandbox: '<%=Paypal_Sandbox_ClientID%>',
                production: '<%=Paypal_Production_ClientID%>'
            },

            payment: function (data, actions) {
                return actions.payment.create({
                    payment: {
                        transactions: [
                            {
                                amount: {
                                    total: '<%=sHighBid%>', currency: 'USD'
                                },
                                item_list:
                                {
                                    items: [
                                        <%=sItemList%>
                                    ]
                                }
                            }
                        ]
                    }
                });
            },
            onAuthorize: function (data, actions) {

            // Get the payment details

                return actions.payment.get().then(function (data) {
                    // Display the payment details and a confirmation button
                    var email = data.payer.payer_info.email;
                    var shipping = data.payer.payer_info.shipping_address;
                    var recipient = shipping.recipient_name;
                    var addr = shipping.line1;
                    var city = shipping.city;
                    var state = shipping.state;
                    var zip = shipping.postal_code;
                    var country = shipping.country_code;

                    // Execute the payment
                    return actions.payment.execute().then(function () {
                        location.href = 'thankyou.aspx?o=<%=sId%>&i=1&a=' + addr + '&c=' + city + '&s=' + state + '&z=' + zip + '&e=' + email + '&r=' + recipient;
                    });
                });
            },
            onCancel: function(data, actions) {
                location.href = 'thankyou.aspx?o=<%=sId%>&i=2';
            }
        }, '#paypal-button-container');
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("confirm").style.display = 'none';
            $("thanks").style.display = 'none';
        });
    </script>

    <div class="PageLayout">
		<form id="Form1" method="post" runat="server">
			<table class="AuctionItem">
				<tr class="ThemeColor" style="height:35px;">
					<td>
                        <asp:label id="lblItemName" runat="server" Font-Size="Medium" Font-Bold="true"></asp:label>
                    </td>
					<td style="text-align:right;" nowrap="nowrap">
						Lot number <asp:label id="lblItemId" runat="server"></asp:label>
					</td>
	    		</tr>
                <tr><td /></tr>
				<tr class="ThemeColorAlt2">
					<td style="vertical-align:top; width:50%;">
                        <table width="100%" border="0">
                            <tr>
                                <td colspan="2">
                                    <h2><font color="red">Congratulations <asp:label id="lblHighBidder" runat="server"></asp:label>!</font></h2>
                                </td>
                            </tr>
							<tr>
								<td>Winning bid: <asp:label id="lblWinningBid" runat="server"></asp:label>&nbsp;</td>
								<td>
                                    
                                </td>
							</tr>
                            <tr>
					            <td style="vertical-align:top;" colspan="2">
                                    <asp:Image runat="server" ID="iItem" Height="700" Width="700" />
					            </td>
                            </tr>
						</table>
					</td>
					<td style="vertical-align:top;"></td>
				</tr>
                <tr><td /></tr>
				<tr class="ThemeColor">
					<td>Check Out</td>
                    <td />
				</tr>
                <tr><td /></tr>
				<tr>
					<td colspan="2" style="text-align:center;">
                        <div id="paypal-button-container"></div>
                        Pay with Paypal or Credit/Debit Card
					</td>
				</tr>
				<tr><td /></tr>
				<tr class="ThemeColor" style="height:5px;">
					<td colspan="5"></td>
				</tr>
			</table>
            <div class="ItemFooter">
				<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="Auctions.aspx">Back to Auctions</asp:HyperLink>
		    </div>
        </form>
	</div>
</asp:Content>