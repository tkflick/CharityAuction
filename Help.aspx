<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Help.aspx.vb" Inherits="Help" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
     <head>
        <title>Additional Information</title>
	    <link href="./css/style.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <div class="PageLayout">
            <img src="./images/banner-logo.png" alt="" />
            <h2>Why are we doing this? </h2>
            <p>
                All the funds raised from the auction are used to help our communtity - to read more about our charity, please visit :- 
                <a href="<%=sWebURL%>" target="_blank"><%=sWebURL%></a> 
            </p>
            <h2>Donating prizes for the Auction</h2>
            <p>
                We would love to have any donated gifts that you feel would be suitable for the auction - bottles of wine/champagne, boxes 
                of chocolates, DVDs, CDs etc. Just email <a href="mailto:<%=sClubEmail%>"><%=sClubEmail%></a> 
                and let us know what the item is you have to donate to the auction. 
            </p>
            <h2>How to use the Auction </h2>
            <p><b>Review the item:</b>
                Make sure you know what you are buying. Once you find a listing that interests you, click on it to view details of the item 
                and find out as much as you can. Once you've found the item you want to bid on, decide the maximum you're willing to pay.
            </p>
            <p>
                <b>Place A Bid:</b> 
                Enter the price you are prepared to pay for the item. Keep watching to track the bidding progress of the item you have bid for.
            </p>
            <p>
                <b>Paying for your item:</b>
                After you have won the item, click on the "Check Out" link next to your name and you can pay with either PayPal or a Credit Card.
            </p>
            <p>
                <b>Picking up your item:</b>
                You must be local and available to pick up the item, as we do not ship or deliver.
            </p>
            <a href="javascript:close();">Close window</a>
        </div>
    </body>
</html>