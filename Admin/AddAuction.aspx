<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddAuction.aspx.vb" MasterPageFile="~/Admin/Admin.master" Inherits="Admin_AddAuction" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
        <div class="PageLayout">
		    <form id="Form1" method="post" runat="server">
                <asp:Literal runat="server" ID="litHeader">
                    <h2>Add auction item</h2>
                </asp:Literal>
			    <table id="Table1" cellSpacing="5" cellPadding="5" border="0">
				    <tr>
					    <td style="HEIGHT: 27px">
						    <p align="right">Item Name<br />&nbsp;</p>
					    </td>
					    <td style="HEIGHT: 27px">
						    <asp:TextBox id="txtName" runat="server" Width="408px"></asp:TextBox><br />
                            <font size="1">(try to stay away from special characters, especially quotes (") and apostrophes ('). These break the paypal code.)</font>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="Missing name"></asp:RequiredFieldValidator>
                        </td>
				    </tr>
				    <tr>
					    <td valign="top">
						    <p align="right">Item Description</p>
					    </td>
					    <td>
						    <asp:TextBox id="txtDescription" runat="server" Width="408px" Height="136px" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescription" ErrorMessage="Missing description"></asp:RequiredFieldValidator>
                        </td>
				    </tr>
				    <tr>
					    <td valign="top">
						    <p align="right">Opening Bid</p>
					    </td>
					    <td>
						    <asp:TextBox id="txtOpeningBid" runat="server" Width="240px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtOpeningBid" ErrorMessage="Missing opening bid"></asp:RequiredFieldValidator>
                        </td>
				    </tr>
				    <tr>
					    <td>
						    <p align="right">Item Location</p>
					    </td>
					    <td>
						    <asp:TextBox id="txtLocation" runat="server" Width="240px" Text="Windcrest, Tx"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLocation" ErrorMessage="Missing location"></asp:RequiredFieldValidator>
                        </td>
				    </tr>
				    <tr>
					    <td style="text-align: right">Category</td>
					    <td>
                            <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
					    </td>
				    </tr>
                    <tr>
                        <td style="text-align: right">
                            Seller Name</td>
                        <td>
						    <asp:TextBox id="txtSeller" runat="server" Width="240px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSeller" ErrorMessage="Missing seller name"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
				    <tr>
					    <td vAlign="top">
						    <p align="right">Auction close date&nbsp;and time</p>
					    </td>
					    <td>
						    <asp:Calendar id="Calendar1" runat="server" Width="240px" ShowGridLines="True"></asp:Calendar>
						    <p>Time (24h)
						    <asp:TextBox id="txtTime" runat="server" Width="128px">14:00</asp:TextBox></p>
					    </td>
				    </tr>
                    <tr>
					    <td vAlign="top" style="text-align: right">Upload up to 4 Images</td>
					    <td>
						    <asp:FileUpload ID="FileUpload1" runat="server" Width="405px" /><asp:RegularExpressionValidator ID="rexp1" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Only .jpg and .png" ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])$)"></asp:RegularExpressionValidator>
                            <br />
                            <asp:FileUpload ID="FileUpload2" runat="server" Width="405px" /><asp:RegularExpressionValidator ID="rexp2" runat="server" ControlToValidate="FileUpload2" ErrorMessage="Only .jpg and .png" ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])$)"></asp:RegularExpressionValidator>
                            <br />
                            <asp:FileUpload ID="FileUpload3" runat="server" Width="405px" /><asp:RegularExpressionValidator ID="rexp3" runat="server" ControlToValidate="FileUpload3" ErrorMessage="Only .jpg and .png" ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])$)"></asp:RegularExpressionValidator>
                            <br />
                            <asp:FileUpload ID="FileUpload4" runat="server" Width="405px" /><asp:RegularExpressionValidator ID="rexp4" runat="server" ControlToValidate="FileUpload4" ErrorMessage="Only .jpg and .png" ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])$)"></asp:RegularExpressionValidator>
                            <br />
                            Leave blank if no image 
					    </td>
				    </tr>
				    <tr>
					    <td vAlign="top"><p align="right">&nbsp;<br />Donated by text</p></td>
					    <td align="right">
                            <b>This item generously donated by</b><br />
                            <asp:TextBox runat="server" ID="tDonatedBy" Width="240"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tDonatedBy" ErrorMessage="Missing donater name"></asp:RequiredFieldValidator>
					    </td>
				    </tr>
				    <tr>
					    <td vAlign="top"></td>
					    <td align="right">&nbsp;</td>
				    </tr>
				    <tr>
					    <td vAlign="top">&nbsp;</td>
					    <td align="right">
						    <asp:Button id="bSave" runat="server" Text="Save Auction"></asp:Button>
					    </td>
				    </tr>
			    </table>
		    </form>
        </div>
</asp:Content>