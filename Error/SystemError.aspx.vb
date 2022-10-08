Imports System.Text

Partial Class Error_SystemError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim lstrErrMsg As String = Session("ErrMsg") & ""
            Dim lstrTemp As String = String.Empty
            Dim lstrReferrer As String = String.Empty

            If Not (Request.UrlReferrer Is Nothing) Then
                lstrReferrer = Request.UrlReferrer.AbsoluteUri
            End If

            If lstrErrMsg.Length > 0 Then lstrTemp = lstrErrMsg

            If lstrTemp.Length > 0 Then
                Me.ErrorDescription.Text = lstrTemp
            Else
                Me.ErrorDescription.Text = "Unknown Error or Error Description Not Available."
            End If

            Me.prevLink.Attributes.Add("onmouseover", "window.status='Returns you to the previous page that you were working on.';return true;")
            Me.prevLink.Attributes.Add("onmouseout", "window.status='';return true;")

            Me.defaultLink.Attributes.Add("onmouseover", "window.status='Returns you to the start page.';return true;")
            Me.defaultLink.Attributes.Add("onmouseout", "window.status='';return true;")

            If lstrReferrer Is Nothing OrElse lstrReferrer.Length = 0 Then
                Me.prevLink.NavigateUrl = "javascript:history.go(-1);"
            Else
                Me.prevLink.NavigateUrl = lstrReferrer
            End If

            Me.defaultLink.NavigateUrl = "~/Auctions.aspx"

        Catch Ex As Exception
        Finally
        End Try
    End Sub

End Class
