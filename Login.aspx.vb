Imports System.Data

Partial Class Login
    Inherits System.Web.UI.Page

    Protected oCommon As New utilities
    Public sWebURL As String = String.Empty

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        '---[ set the page title on the Master Page for this page ]---
        'Page.Header.Title = System.Configuration.ConfigurationManager.AppSettings("APP_NAME") & " - Log In"
        sWebURL = System.Configuration.ConfigurationManager.AppSettings("WebURL")
        If Session("loggedin") = "y" Then
            Response.Redirect("~/Home.aspx", True)
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        bSubmit.Attributes.Add("OnClick", "javascript:return validate();")
        tUserName.Focus()
        Dim sErrorCode As String = Request.QueryString("error")
        If Not Page.IsPostBack Then
            Select Case sErrorCode
                Case "sessionexpired"
                    lErrorMsg.Text = "Your session has expired, please log in again."
                    lErrorMsg.Visible = True
                Case "loggedout"
                    lErrorMsg.Text = "Thank you for visiting our <b><i>Charity Auction!</i></b>"
                    lErrorMsg.Visible = True
            End Select
        End If
    End Sub

    Protected Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload

    End Sub

    Protected Sub bSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles bSubmit.Click
        '---[ Authenticate User ]---
        Dim sUSERNAME As String = tUserName.Text
        Dim sPASSWORD As String = tPassword.Text
        Dim sPageRedirect As String = String.Empty
        Dim sError As String = String.Empty
        Dim bAUTHENTICATED As Boolean = False
        Dim util As New utilities
        Dim ds As New DataSet

        bAUTHENTICATED = util.AuthenticateUser(sUSERNAME, sPASSWORD, ds, sError)

        Dim bTEST As Boolean = util.IsPasswordStrong(sPASSWORD)

        If bAUTHENTICATED Then
            '---[ Set User Sessions for CheckSession() function ]---
            util.SetSessions(ds)

            '---[ TODO: Implement Logging ]---
            'util.LogVisit(ds, Session.SessionID)

            Select Case sError
                Case "reset"
                    sPageRedirect = "./UserAdmin/ChangePassword.aspx?reason=reset"
                Case Else
                    sPageRedirect = "./Home.aspx"
            End Select
        Else
            '---[ Display Log In Failure Message ]---
            Session("UserName") = sUSERNAME
            Select Case sError
                Case "expired"
                    sPageRedirect = "./UserAdmin/ChangePassword.aspx?reason=expired"
                Case "inactive"
                    sPageRedirect = "./Error/Inactive.aspx"
                Case "unverified"
                    sPageRedirect = "./Error/Verify.aspx?user=" & sUSERNAME
                Case "notfound"
                    sPageRedirect = "./Error/LoginFailure.aspx?err=notfound"
                Case "Password is incorrect"
                    sPageRedirect = "./Error/LoginFailure.aspx?err=incorrectpw"
                Case Else
                    sPageRedirect = "./Error/LoginFailure.aspx"
            End Select
        End If
        Response.Redirect(sPageRedirect, False)
    End Sub

End Class
