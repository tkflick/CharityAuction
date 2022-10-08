Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Partial Class History
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

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        getHistory()
    End Sub

    Public Function trimer(ByVal x As String) As String
        Dim d As Decimal = CDec(Convert.ToDecimal(x))
        Dim s As String = String.Format("{0:F2}", d)
        Return s
    End Function

    Private Sub getHistory()
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Dim sItemId As String = Request.QueryString("i").ToString()

        Try
            If Not oST.GetBidHistory(ds, sItemId, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            lblItemName.Text = ds.Tables(0).Rows(0)("item_name").ToString()
            dlListings.DataSource = ds
            dlListings.DataBind()
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub
End Class
