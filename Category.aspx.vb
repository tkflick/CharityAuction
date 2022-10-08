Imports System.Data

Partial Class Category
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
        If Not IsPostBack Then
            PopulateCategoryList()
        End If
    End Sub

    Private Sub PopulateCategoryList()
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Try
            If Not oST.GetCategoryList(ds, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            dlCategorys.DataSource = ds
            dlCategorys.DataBind()
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub
End Class
