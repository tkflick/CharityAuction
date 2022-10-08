Imports System.Data

Partial Class Admin_AddAuction
    Inherits System.Web.UI.Page

    Public oUtil As New utilities
    Public oST As New systools

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        oUtil.SessionCheck()
    End Sub

    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        Response.CacheControl = "No-cache"
        Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate") 'HTTP 1.1.
        Response.AppendHeader("Pragma", "no-cache") 'HTTP 1.0.
        Response.AppendHeader("Expires", "0") 'Proxies.
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Calendar1.SelectedDate = DateTime.Now
                Calendar1.VisibleDate = DateTime.Now

                FillDDL(DropDownList1, "cat_name", "cat_id")
            End If
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub

    Protected Sub bSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bSave.Click
        Dim filename As String = String.Empty
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Try
            filename = UploadPhotos()
            If Not oST.AddAuction(txtName.Text, txtDescription.Text, Calendar1.SelectedDate.ToShortDateString() & " " + txtTime.Text, txtSeller.Text, txtLocation.Text, DropDownList1.SelectedValue.ToString(), filename, tDonatedBy.Text, txtOpeningBid.Text, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If

            litHeader.Text = "<h1>Auction added</h1><p><a href='AddAuction.aspx'>Click here to add another item</a></p>"
            txtDescription.Enabled = False
            txtName.Enabled = False
            txtLocation.Enabled = False
            txtSeller.Enabled = False
            txtTime.Enabled = False
            Calendar1.Enabled = False
            FileUpload1.Enabled = False
            bSave.Enabled = False

            '---[ Send update email to those who want to know about new auctions ]---
            SendNewAuctionUpdateEmail()

        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("../Error/SystemError.aspx", False)
        End Try
    End Sub

    Public Sub FillDDL(ByRef ddl As DropDownList, ByVal vTextField As String, ByVal vValueField As String)
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty

        ds = oST.GetAllCategories()

        ddl.DataSource = ds
        ddl.DataTextField = vTextField
        ddl.DataValueField = vValueField
        ddl.DataBind()
        ddl.Items.Insert(0, "--Select--")
        ddl.Items(0).Value = "-1"

        For j As Integer = 1 To ddl.Items.Count - 1
            ddl.Items(j).Attributes.Add("title", ddl.Items(j).Text)
        Next
    End Sub

    Public Function UploadPhotos() As String
        Dim lsFileName As String = String.Empty
        Dim sRetVal As String = String.Empty

        If FileUpload1.HasFile Then
            lsFileName = Guid.NewGuid().ToString() & FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf("."))
            FileUpload1.SaveAs(Server.MapPath("~/auction_pictures/" & lsFileName))
            sRetVal = lsFileName
        End If
        If FileUpload2.HasFile Then
            lsFileName = Guid.NewGuid().ToString() & FileUpload2.FileName.Substring(FileUpload2.FileName.LastIndexOf("."))
            FileUpload2.SaveAs(Server.MapPath("~/auction_pictures/" & lsFileName))
            sRetVal = sRetVal & "," & lsFileName
        End If
        If FileUpload3.HasFile Then
            lsFileName = Guid.NewGuid().ToString() & FileUpload3.FileName.Substring(FileUpload3.FileName.LastIndexOf("."))
            FileUpload3.SaveAs(Server.MapPath("~/auction_pictures/" & lsFileName))
            sRetVal = sRetVal & "," & lsFileName
        End If
        If FileUpload4.HasFile Then
            lsFileName = Guid.NewGuid().ToString() & FileUpload4.FileName.Substring(FileUpload4.FileName.LastIndexOf("."))
            FileUpload4.SaveAs(Server.MapPath("~/auction_pictures/" & lsFileName))
            sRetVal = sRetVal & "," & lsFileName
        End If
        Return sRetVal
    End Function

    Public Sub SendNewAuctionUpdateEmail()
        Dim sAddressList As String = String.Empty
        Dim sEmailAddress() As String
        Dim oMSG As New messaging
        Try
            'get email list of users to be notified they've been outbid
            sAddressList = oUtil.GetNotificationList()
            sEmailAddress = sAddressList.Split(",")

            'send email notification
            oMSG.SendNewAuctionEmail(sEmailAddress)

        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub
End Class
