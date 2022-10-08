<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Home.aspx.vb" MasterPageFile="~/Auction.master" Inherits="Home" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <div class="PageLayout">
        <h2>Welcome to the Auction Website for the <%=sClubName%>.</h2>
        <p>
            Like everyone else, COVID has really had an adverse affect on the Lions Club.  We can't get out into the community
            like we are accustomed to doing, so we've been trying to adjust and do our charity work remotely or socially distanced.<br /> 
            <br />
            We also haven't been able to have our normal fundraisers, which has put a dent in our budget.  As a result, we've 
            had to get creative. One of our members is a programmer / developer and put together this auction site for us. Hopefully, 
            this will enable us to raise some money to continue to help our community.<br />
            <br />
            All the funds raised from these auctions are used to help the various Lions Club charity events and activities 
            we do annually...<br />
            <br /> 
            Things like:</p>
            <ul>
                <li>School supplies for area teachers</li>
                <li>Dictionaries for all local elementary school students</li>
                <li>Food bank donations</li>
                <li>Eye Exams/Glasses for the needy</li>
                <li>Contributions to the Texas Lions Camp</li>
                <li>And so much more...</li>
            </ul> 
            <h2>Donating prizes for the Auction</h2>
            <p>
                We would love to have any donated gifts that you feel would be suitable for the auction - bottles of wine/champagne, boxes 
                of chocolates, DVDs, CDs etc. Just email <a href="mailto:<%=sClubEmail%>"><%=sClubEmail%></a> 
                and let us know what the item is you have to donate to the auction. 
            </p>
            <h2>How to use the Auction Site</h2>
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
        <p align="center">
            <a href="Auctions.aspx"><asp:Image ID="view" runat="server" ImageUrl="./images/ViewAuctions.png" AlternateText="View Current Auctions" /></a>
        </p>
    </div>
</asp:Content>