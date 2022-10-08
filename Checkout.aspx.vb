Imports System.Data

Partial Class Checkout
    Inherits System.Web.UI.Page

    Public oUtil As New utilities
    Public oST As New systools
    Public sSort As String = String.Empty
    Public sId As String = String.Empty
    Public sHighBid As String = String.Empty
    Public sItemList As String = String.Empty
    Public Paypal_Env As String = String.Empty
    Public Paypal_Sandbox_ClientID As String = String.Empty
    Public Paypal_Production_ClientID As String = String.Empty

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        oUtil.SessionCheck()
    End Sub

    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        Response.CacheControl = "No-cache"
        Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate") 'HTTP 1.1.
        Response.AppendHeader("Pragma", "no-cache") 'HTTP 1.0.
        Response.AppendHeader("Expires", "0") 'Proxies.

        sId = Request.QueryString("i")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Paypal_Env = System.Configuration.ConfigurationManager.AppSettings("PAYPAL_ENV")
        Paypal_Sandbox_ClientID = System.Configuration.ConfigurationManager.AppSettings("PAYPAL_SANDBOX_CLIENTID")
        Paypal_Production_ClientID = System.Configuration.ConfigurationManager.AppSettings("PAYPAL_PROD_CLIENTID")

        If Not IsPostBack Then
            getItem()
        End If
    End Sub

    Private Sub getItem()
        Dim ds As New DataSet
        Dim sHighBidUserId As String = String.Empty
        Dim sErrMsg As String = String.Empty
        Dim sImageName() As String

        Try
            If Not oST.GetItemDetails(ds, sId, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            lblItemName.Text = ds.Tables(0).Rows(0)("item_name").ToString()
            lblItemId.Text = sId

            sImageName = ds.Tables(0).Rows(0)("img").ToString().Split(",")
            iItem.ImageUrl = "./auction_pictures/" & sImageName(0)

            '---[ a '$' will cause the Paypal code to throw an error, so I strip off the '$' for the variable used in the Paypay javascript ]---
            sHighBid = FormatAmount(ds.Tables(0).Rows(0)("item_amount").ToString()).Replace("$", "").Trim()
            lblWinningBid.Text = "<b>" & sHighBid & "</b>"

            lblHighBidder.Text = "<b>" & ds.Tables(0).Rows(0)("item_bidder").ToString() & "</b>"
            sHighBidUserId = ds.Tables(0).Rows(0)("item_userid").ToString()

            SetPayPalVariables(lblItemName.Text, sHighBid)
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx")
        End Try
    End Sub

    Public Function FormatAmount(ByVal x As String) As String
        If x = "" Then
            Return "No bids"
        Else
            Dim d As Decimal = CDec(Convert.ToDecimal(x))
            Dim s As String = String.Format("{0:F2}", d)
            Return "$ " & s
        End If
    End Function

    Public Sub SetPayPalVariables(ByVal vItemName As String, ByVal vPrice As String)
        '"items": [
        '{
        '    "quantity": "1",
        '    "name": "item 1",
        '    "price": "1",
        '    "currency": "USD",
        '    "description": "item 1 description"
        '},
        '{
        '    "quantity": "1",
        '    "name": "item 2",
        '    "price": "1",
        '    "currency": "USD",
        '    "description": "item 2 description"
        '}]
        Dim obs As New StringBuilder
        obs.Append("{")
        obs.Append("    quantity: '1',")
        obs.Append("    name: '" & vItemName & "',")
        obs.Append("    price: '" & vPrice.Replace("$", "").Trim() & "',") 'strip off $ to avoid paypal errors
        obs.Append("    currency: 'USD',")
        obs.Append("    description: '" & vItemName & "'")
        obs.Append("}")
        sItemList = obs.ToString()
    End Sub
End Class
