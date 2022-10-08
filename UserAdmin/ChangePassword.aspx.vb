Imports System.Data
Imports dataOperations

Partial Class User_ChangePassword
    Inherits System.Web.UI.Page

    Public sClubName As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
        bSubmit.Attributes.Add("OnClick", "javascript:return validate();")
    End Sub

    Protected Sub bSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bSubmit.Click
        Dim sCURRENTPW As String = tCurrentPW.Text
        Dim sNEWPW As String = tNewPW.Text
        Dim sUSERNAME As String = Session("UserName").ToString()
        Dim sERRMSG As String = ""
        Dim dop As New utilities
        Dim bSUCCESS As Boolean = False
        bSUCCESS = dop.ChangePassword(sUSERNAME, sCURRENTPW, sNEWPW, sERRMSG)
        If bSUCCESS Then
            tChange.Visible = False
            tFailure.Visible = False
            tSuccess.Visible = True
        Else
            tChange.Visible = False
            tFailure.Visible = True
            tSuccess.Visible = False
            If sERRMSG > "" Then
                lErr.Text = "ERROR: " & sERRMSG
                lErr.Visible = True
            End If
        End If
    End Sub

    Protected Sub bContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bContinue.Click
        Response.Redirect("../index.aspx", False)
    End Sub

    Protected Sub bRetry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bRetry.Click
        tChange.Visible = True
        tFailure.Visible = False
        tSuccess.Visible = False
        tCurrentPW.Text = ""
        tNewPW.Text = ""
        tConfirmPW.Text = ""
    End Sub
End Class

