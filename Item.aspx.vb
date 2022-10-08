Imports System.Data
Imports System.Net.Mail

Partial Class Item
    Inherits System.Web.UI.Page

    Public oUtil As New utilities
    Public oST As New systools
    Public sSort As String = String.Empty
    Public sId As String = String.Empty
    Public sOpeningBid As String = String.Empty

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        oUtil.SessionCheck()
    End Sub

    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        Response.CacheControl = "No-cache"
        Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate") 'HTTP 1.1.
        Response.AppendHeader("Pragma", "no-cache") 'HTTP 1.0.
        Response.AppendHeader("Expires", "0") 'Proxies.

        sSort = Request.QueryString("s")
        sId = Request.QueryString("i")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AdminUser") Then AdminLink.Visible = True
        If Not IsPostBack Then
            getItem()
            Dim dtStartDate As DateTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings("LockBidDate"))

            If dtStartDate.Ticks > DateTime.Now.Ticks Then
                ClientScript.RegisterStartupScript(GetType(Page), "alert", "alert('Bidding on this item will start at " & dtStartDate.ToString() & ".');", True)
                btnBid.Enabled = False
                txtBid.Enabled = False
            End If

            If lblEndTime.Text.Contains("Ended") Then
                litUpdate.Text = "<p>Bidding on this item has finished.  I'm sorry you were too late!</p>"
            Else
                If lblCurrentBid.Text.Equals("<b>No bids</b>") Then
                    litUpdate.Text = "<p>Bidding on this item starts at $" & sOpeningBid & ", <b>Good luck!</b></p>"
                    txtBid.Text = sOpeningBid
                    txtBid.Enabled = False
                Else
                    Dim newvalue As Double = Double.Parse(lblCurrentBid.Text.Replace("<b>$ ", "").Replace("</b>", "")) + 1.0  '<< Replace with bid increment from web.config
                    litUpdate.Text = "<p>Minimum bid for this item is $ " & newvalue.ToString("0.00") & ", <b>Bid now!<b/></p>"
                End If
            End If
        End If
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

    Public Function FormatCountdown(ByVal dtIn As String) As String
        Dim returnvalue As String = String.Empty
        Dim dtCount As DateTime = New DateTime()
        dtCount = CType(Convert.ToDateTime(dtIn), DateTime)

        If dtCount.Ticks > DateTime.Now.Ticks Then
            If (dtCount.AddTicks(-DateTime.Now.Ticks).Month - 1) > 0 Then returnvalue += (dtCount.AddTicks(-DateTime.Now.Ticks).Month - 1) & "month(s) "
            If (dtCount.AddTicks(-DateTime.Now.Ticks).Day - 1) > 0 Then returnvalue += (dtCount.AddTicks(-DateTime.Now.Ticks).Day - 1) & "d "
            If dtCount.AddTicks(-DateTime.Now.Ticks).Hour > 0 Then returnvalue += dtCount.AddTicks(-DateTime.Now.Ticks).Hour & "h "

            If dtCount.AddTicks(-DateTime.Now.Ticks).Minute > 0 Then
                returnvalue += (dtCount.AddTicks(-DateTime.Now.Ticks).Minute) & "m "
            End If

            If Not ((dtCount.AddTicks(-DateTime.Now.Ticks).Day - 1) > 0) And (Not (dtCount.AddTicks(-DateTime.Now.Ticks).Hour > 0)) And (Not (dtCount.AddTicks(-DateTime.Now.Ticks).Minute > 5)) Then returnvalue += dtCount.AddTicks(-DateTime.Now.Ticks).Second & "s"
            lBidder.Text = "High Bidder: "
        Else
            returnvalue = "<font color=red>Ended</font>"
        End If
        Return returnvalue
    End Function

    Private Sub getItem()
        Dim ds As New DataSet
        Dim sPhotoList As String = String.Empty
        Dim aPhotoList() As String
        Dim sHighBidUserId As String = String.Empty
        Dim sNickname As String = String.Empty
        Dim sErrMsg As String = String.Empty
        Dim sEndTime As String = String.Empty
        Dim sAuctionPaid As String = String.Empty
        Try
            If Not oST.GetItemDetails(ds, sId, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            lblCurrentBid.Text = "<b>" & FormatAmount(ds.Tables(0).Rows(0)("item_amount").ToString()) & "</b>"
            lblItemName.Text = ds.Tables(0).Rows(0)("item_name").ToString()
            lblItemId.Text = sId
            lblDescription.Text = ds.Tables(0).Rows(0)("item_description").ToString().Replace(vbCrLf, "<br>")
            sEndTime = FormatCountdown(ds.Tables(0).Rows(0)("item_date_close").ToString()) & " (" + (Convert.ToDateTime(ds.Tables(0).Rows(0)("item_date_close").ToString())).ToString("r") & ")"
            lblEndTime.Text = "<b>" & sEndTime & "</b>"
            lblSeller.Text = "<b>" & ds.Tables(0).Rows(0)("item_seller").ToString() & "</b>"
            lblDonatedBy.Text = "<b>" & ds.Tables(0).Rows(0)("donated_by").ToString() & "</b>"
            sHighBidUserId = ds.Tables(0).Rows(0)("item_userid").ToString()
            sAuctionPaid = ds.Tables(0).Rows(0)("paid").ToString()
            sOpeningBid = ds.Tables(0).Rows(0)("opening_bid").ToString()

            '---[ save current high bidder for later if he gets outbid ]---
            hidHighBidder.Value = sHighBidUserId
            '---[ mask high bidders name with nickname ]---
            sNickname = oUtil.GetNickName(sHighBidUserId)
            If sNickname.Trim() = "" Then
                'use name
                lblHighBidder.Text = "<b>" & ds.Tables(0).Rows(0)("item_bidder").ToString() & "</b>"
            Else
                'use nickname
                lblHighBidder.Text = "<b>" & sNickname & "</b>"
            End If

            '---[ should the checkout button display? ]---
            If sEndTime.Contains("Ended") Then
                btnBid.Enabled = False
                txtBid.Enabled = False
                lBidder.Text = "Winning Bidder: "
                If ShowCheckoutLink(sHighBidUserId) Then
                    If sAuctionPaid = "Y" Then
                        lblWinner.Visible = True
                        lblWinner.Text = "(PAID - Thank you!)"
                    Else
                        lblWinner.Visible = True
                        lblWinner.Text = "(<a href='Checkout.aspx?i=" & sId & "'>Check Out</a>)"
                    End If
                End If
            End If

            lnkBids.Text = "<b>" & ds.Tables(0).Rows(0)("item_bids").ToString() & " bid(s)</b>"
            If lnkBids.Text <> "<b>0 bid(s)</b>" Then lnkBids.NavigateUrl = "History.aspx?i=" & sId
            If lblHighBidder.Text = "<b></b>" Then lblHighBidder.Text = "<b>None</b>"
            lblLocation.Text = "<b>" & ds.Tables(0).Rows(0)("item_location").ToString() & "</b>"
            If lblLocation.Text = "<b></b>" Then lblLocation.Text = "<b>Not specified</b>"
            If lblDescription.Text = "" Then lblDescription.Text = "No description"
            '---[ set up photos list ]---
            sPhotoList = ds.Tables(0).Rows(0)("img").ToString()
            aPhotoList = sPhotoList.Split(",")
            Repeater1.DataSource = aPhotoList
            Repeater1.DataBind()

            If aPhotoList.Length = 1 Then
                images.Visible = False
            Else
                images.Visible = True
            End If

        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub

    Protected Sub btnBid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBid.Click
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Dim fullname As String = Session("FullName").ToString()
        Dim Nickname As String = Session("Nickname").ToString()
        Dim UserId As String = Session("UserName").ToString()
        Dim bidamount As Decimal = Convert.ToDecimal(txtBid.Text.ToString())
        Try
            If bidamount > 214500 Then
                litUpdate.Text = "<p><b>Your bid was rejected,</b><br />Did you mean to bid $" & bidamount.ToString("0.00") & " ?<br />If so you have too much money!"
            Else
                If oST.PlaceBid(sId, bidamount, Nickname, UserId, sErrMsg) Then
                    litUpdate.Text = "<p>You are currently the <b>highest bidder</b>, Good luck!</p>"
                Else
                    If sErrMsg = "AUCTION HAS ENDED" Then
                        litUpdate.Text = "<p>Bidding on this item has finished.  I'm sorry you were too late!</p>"
                    ElseIf sErrMsg = "BID AMOUNT NOT VALID" Then
                        Dim newvalue As Double = Double.Parse(lblCurrentBid.Text.Replace("<b>$ ", "").Replace("</b>", "")) + 1.0
                        litUpdate.Text = "<p><b>Your bid was rejected,</b> <br />Bid amount was too low or someone else has outbid you.<br /><br />The minimum bid for this item is $" & newvalue.ToString("0.00") & ", Good luck!</p>"
                    Else
                        Throw New Exception(sErrMsg)
                    End If
                End If
            End If
            notifyOutbidUser(sId)
            getItem()
            txtBid.Text = String.Empty
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        getItem()

        If lblEndTime.Text.Contains("Ended") Then
            litUpdate.Text = "<p>Bidding on this item has finished. I'm sorry you were too late!</p>"
        Else
            If lblCurrentBid.Text.Equals("<b>No bids</b>") Then
                litUpdate.Text = "<p>Bidding on this item start at $1, <b>Good luck!</b></p>"
            Else
                Dim newvalue As Double = Double.Parse(lblCurrentBid.Text.Replace("<b>$ ", "").Replace("</b>", "")) + 1.0
                litUpdate.Text = "<p>Minimum bid for this item is $ " & newvalue.ToString("0.00") & ", <b>Bid now!<b/></p>"
            End If
        End If
        txtBid.Text = String.Empty
    End Sub

    Public Function ShowCheckoutLink(ByVal vUserID As String) As Boolean
        Dim bRetVal As Boolean = False
        If vUserID = Session("UserName").ToString() Then
            bRetVal = True
        End If
        Return bRetVal
    End Function

    Public Sub notifyOutbidUser(ByVal vId As String)
        Dim sEmailAddress As String = String.Empty
        Dim sFullName As String = String.Empty
        Dim sCurrentHighBidder As String = String.Empty
        Dim oMSG As New messaging
        Try
            sCurrentHighBidder = hidHighBidder.Value
            If sCurrentHighBidder.Trim() <> "" Then
                'get email of user to be notified they've been outbid
                sEmailAddress = oUtil.GetUserEmail(sCurrentHighBidder)
                'get full name value
                sFullName = oUtil.GetFullName(sCurrentHighBidder)
                'send email notification
                oMSG.SendOutbidEmail(sFullName, sCurrentHighBidder, sEmailAddress)
            End If
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub

End Class
