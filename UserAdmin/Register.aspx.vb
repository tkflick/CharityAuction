Imports System.Data

Partial Class User_Register
    Inherits System.Web.UI.Page

    Public sClubName As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
        bSubmit.Attributes.Add("OnClick", "javascript:return validate();")
        tFName.Focus()
    End Sub

    Protected Sub bSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bSubmit.Click
        Dim sFNAME As String = tFName.Text
        Dim sLNAME As String = tLName.Text
        Dim sEMAIL As String = tEmail.Text
        Dim sUSERNAME As String = tUserName.Text
        Dim sNICKNAME As String = tNickname.Text
        Dim sPASSWORD As String = tPassword.Text
        Dim sOUTBIDEMAIL As String = chkOutbidEmail.Checked
        Dim bSUCCESS As Boolean = False
        Dim bUSERNAMEFREE As Boolean = False
        Dim bEMAILFREE As Boolean = False
        Dim bNICKNAMEFREE As Boolean = False
        Dim sRETMSG As String = String.Empty
        Dim ds As New DataSet

        Dim oUtil As New utilities
        Dim oMsg As New messaging

        Try
            Session.Add("pw", sPASSWORD)
            bUSERNAMEFREE = oUtil.IsUserNameFree(sUSERNAME)
            bEMAILFREE = oUtil.IsEmailFree(sEMAIL)
            bNICKNAMEFREE = oUtil.IsNickNameFree(sNICKNAME)

            If Not bUSERNAMEFREE Then
                sRETMSG = "usertaken"
            ElseIf Not bNICKNAMEFREE Then
                sRETMSG = "nicktaken"
            ElseIf Not bEMAILFREE Then
                sRETMSG = "emailtaken"
            ElseIf Not oUtil.IsPasswordStrong(sPASSWORD) Then
                sRETMSG = "weakpassword"
            End If
            Select Case sRETMSG
                Case "usertaken"
                    '---[ username not available ]---
                    lblError.Visible = True
                    lblError.Text = "The user name you choose is already in use."
                Case "nicktaken"
                    '---[ nickname not available ]---
                    lblError.Visible = True
                    lblError.Text = "The nickname you choose is already in use."
                Case "emailtaken"
                    '---[ email address not available ]---
                    lblError.Visible = True
                    lblError.Text = "The email address you supplied is already on file."
                Case "weakpassword"
                    '---[ password does not meet minimum strength standard ]---
                    lblError.Visible = True
                    lblError.Text = "You need a stronger password." & vbCrLf & "It must be 6-16 characters long, contain 1 uppercase and 1 lowercase letter, at least 1 number and 1 special character (' @ or # or $ or % ')"
                Case Else
                    bSUCCESS = oUtil.AddNewUser(sFNAME, sLNAME, sEMAIL, sUSERNAME, sNICKNAME, sPASSWORD, sOUTBIDEMAIL)
                    If bSUCCESS Then
                        oMsg.SendRegistrationEmails(sFNAME & " " & sLNAME, sUSERNAME, sEMAIL)
                        Response.Redirect("./Registered.aspx", False)
                    End If
            End Select
        Catch ex As Exception
            Session.Add("ErrMsg", ex.Message)
            Response.Redirect("../Error/SystemError.aspx", False)
        Finally
            If Not (oUtil Is Nothing) Then oUtil.Dispose()
            If Not (oMsg Is Nothing) Then oMsg.Dispose()
        End Try
    End Sub

End Class
