<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Home.aspx.vb" MasterPageFile="./Admin.master" Inherits="Admin_Home" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
        <div class="PageLayout">
            <h2>Auction Admin Tools</h2>
            <ul>
                <li><a href="./AddAuction.aspx">Add New Auctions</a></li>
                <li><a href="./AddCategory.aspx">Add New Auction Categories</a></li>
            </ul> 
        </div>
</asp:Content>