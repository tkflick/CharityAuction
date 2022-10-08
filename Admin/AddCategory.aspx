<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCategory.aspx.vb" MasterPageFile="~/Admin/Admin.master" Inherits="Admin_AddCategory" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder" ID="MainContent">
        <div class="PageLayout">
		    <form id="Form1" method="post" runat="server">
                <asp:Literal runat="server" ID="litHeader">
                    <h2>Add Category</h2>
                </asp:Literal>
                <table cellSpacing="5" cellPadding="5" border="0">
                    <tr>
                        <td>
                            Category Name:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tCatName" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="bSave" Text="Save " />
                        </td>
                    </tr>
                </table>
                <br /><br />
                <table>
                    <tr>
                        <td>
                            <asp:datagrid id="dgCategory" runat="server" AutoGenerateColumns="False" Width="300px" GridLines="None" BackColor="White" BorderWidth="1px" BorderStyle="Outset" BorderColor="#CCCCCC" cellpadding="3" Font-names="Arial" font-size="12px" AllowSorting="False" OnItemDataBound="dgCategory_ItemDataBound" OnDeleteCommand="dgCategory_DeleteCategory">
	                            <AlternatingItemStyle BackColor="#E6E6E6"></AlternatingItemStyle>
	                            <HeaderStyle Font-Names="Arial" Font-Bold="True" Wrap="false" ForeColor="#FFC20D" BackColor="#003D7E"></HeaderStyle>
	                            <FooterStyle Height="1px" BackColor="#CCCCCC"></FooterStyle>
	                            <Columns>
		                            <asp:BoundColumn DataField="cat_id" HeaderText="Category ID">
			                            <HeaderStyle Font-Underline="True" Wrap="false" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
			                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
		                            </asp:BoundColumn>
		                            <asp:BoundColumn DataField="cat_name" HeaderText="Category Name">
			                            <HeaderStyle Font-Underline="True" Font-Bold="True"></HeaderStyle>
			                            <ItemStyle Wrap="False"></ItemStyle>
		                            </asp:BoundColumn>
		                            <asp:ButtonColumn ButtonType="PushButton" Text="Delete" CommandName="Delete">
		                                <ItemStyle ForeColor="navy" Font-Size="xx-small" />
		                            </asp:ButtonColumn>                
                                </Columns>
                            </asp:datagrid>
                        </td>
                    </tr>
                </table>
		    </form>
        </div>
</asp:Content>