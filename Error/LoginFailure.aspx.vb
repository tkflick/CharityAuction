Imports System.Data

Partial Class Error_LoginFailure
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sErr As String = Request.QueryString("err")
        Select Case sErr
            Case "notfound"
                lbl1.Text = "The User Name you supplied was not found."
                lbl2.Text = "Please check that you spelled it correctly."

            Case "incorrectpw"
                lbl1.Text = "The Password you supplied was incorrect."
                lbl2.Text = "Please check that you spelled it correctly."

            Case Else
                lbl1.Text = "An unexpected login failure has occurred."
                lbl2.Text = "The webmaster has been notified. If you feel this error was yours, try logging in again."

        End Select
    End Sub

End Class
