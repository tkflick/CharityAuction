Imports System.Data

Partial Class reminder
    Inherits System.Web.UI.Page

    Public oUtil As New utilities

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
        Dim sUserName As String = String.Empty
        Dim sUserEmail As String = String.Empty

        sUserName = Session("UserName")
        sUserEmail = oUtil.GetUserEmail(sUserName)

        lblEmail.Text = sUserEmail
    End Sub

    Private Sub btnNotification_Click(sender As Object, e As EventArgs) Handles btnNotification.Click
        Dim sUserName As String = String.Empty
        Dim sUserEmail As String = String.Empty

        Try
            sUserName = Session("UserName")
            sUserEmail = oUtil.GetUserEmail(sUserName)
            If Not oUtil.AddNewAuctionNotification(sUserEmail) Then
                Throw New Exception("Email Notification Flag Not Updated.")
            End If
            Response.Redirect("./thankyou.aspx?i=4", False)
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx")
        End Try
    End Sub
End Class
