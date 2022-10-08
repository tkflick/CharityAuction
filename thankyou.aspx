<%@ Page Language="VB" AutoEventWireup="false" CodeFile="thankyou.aspx.vb" MasterPageFile="~/Auction.master" Inherits="thankyou" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
    <!-- BODY -->
    <table width="100%" border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td class="table_font">
                <!-- CONTENT GOES HERE -->
                <table align="center">
                    <tr>
                        <td>
                            <h1><asp:Label runat="server" ID="lblHeader"></asp:Label></h1>
                            <asp:Label runat="server" ID="lblMessage"></asp:Label>
                            <br /><br />
                            <b>~ Webmaster</b><br />&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>