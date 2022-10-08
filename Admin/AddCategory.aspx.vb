Imports System.Data

Partial Class Admin_AddCategory
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
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Try
            If Not IsPostBack Then
                If Not oST.GetAllCategories(ds, sErrMsg) Then
                    Throw New Exception(sErrMsg)
                End If
                dgCategory.DataSource = ds
                dgCategory.DataBind()
            End If
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub

    Protected Sub dgCategory_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dgCategory.ItemDataBound
        If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
            'Now, reference the Button control that the Delete ButtonColumn has been rendered to
            Dim deleteButton As Button = e.Item.Cells(2).Controls(0)

            'We can now add the onclick event handler
            deleteButton.Attributes("onclick") = "javascript:return " &
                       "confirm('Are you sure you want to delete the category - " &
                       DataBinder.Eval(e.Item.DataItem, "cat_name") & "?')"
        End If
    End Sub

    Sub dgCategory_DeleteCategory(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        'First, get the category id to "delete"
        Dim sErrMsg As String = String.Empty
        Dim cat_id As String = String.Empty
        Try
            cat_id = e.Item.Cells(0).Text

            If Not oST.DeleteCategory(cat_id, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            Response.Redirect("AddCategory.aspx", False)
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("../Error/SystemError.aspx", False)
        End Try
    End Sub

    Private Sub bSave_Click(sender As Object, e As EventArgs) Handles bSave.Click
        Dim sErrMsg As String = String.Empty
        Dim sCategoryName As String = String.Empty
        Try
            sCategoryName = tCatName.Text
            If Not oST.AddCategory(sCategoryName, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            Response.Redirect("AddCategory.aspx", False)
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("../Error/SystemError.aspx", False)
        End Try
    End Sub
End Class
