Imports System.Data

Partial Class User_PasswordRequest
    Inherits System.Web.UI.Page

    Public sClubName As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
    End Sub

    Protected Sub bSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bSend.Click
        Dim oMsg As New messaging
        Dim oUtil As New utilities
        Dim sBODY As New StringBuilder

        Dim sEMAIL As String = tEmail.Text
        Dim sSUBJECT As String = String.Empty
        Dim sPASSWORD As String = String.Empty

        sPASSWORD = oUtil.GetPassword(sEMAIL)
        sBODY.Append("This email is in response to your request for your password.<br /><br />")
        sBODY.Append("Your password is: " & sPASSWORD & ".<br /><br />")
        sBODY.Append("~ Webmaster")
        sSUBJECT = "Forgotten Password Request"
        oMsg.SendMail(sEMAIL, sSUBJECT, sBODY.ToString())
        tRequest.Visible = False
        tSuccess.Visible = True
    End Sub

    Protected Sub bContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bContinue.Click
        Response.Redirect("../index.aspx", False)
    End Sub

End Class
