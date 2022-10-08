Imports System.Data

Partial Class Error_Verify
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub bUserRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bUserRequest.Click
        Dim sUSER As String = Request.QueryString("user")
        EmailRequest(sUSER)
    End Sub

    Private Sub EmailRequest(ByVal _USER As String)
        Response.Redirect("../UserAdmin/RequestEmail.aspx?u=" & _USER, False)
    End Sub

End Class
