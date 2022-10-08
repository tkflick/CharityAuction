Imports System.Data
Imports System.Net.Mail

Public Class messaging
    Inherits System.Web.UI.Page

#Region " Email Functions "

    Public Sub SendRegistrationEmails(ByVal _FULLNAME As String, ByVal _USERID As String, ByVal _EMAIL As String)
        Dim ds As New DataSet
        Dim sSQL As New StringBuilder
        Dim objDO As New dataOperations
        Dim sUSERID As String = String.Empty
        Dim sACTIVATIONCODE As String = String.Empty

        '---[ Get UserID and Activation Code ]---
        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERID & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        sUSERID = objDO.GetSingleValue("id", ds)
        sACTIVATIONCODE = objDO.GetSingleValue("ActivationCode", ds)

        '---[ Send Registration Validate Email ]---
        BuildRegistrationMail(_EMAIL, _FULLNAME, sUSERID, sACTIVATIONCODE)
    End Sub

    Public Sub SendOutbidEmail(ByVal _FULLNAME As String, ByVal _USERID As String, ByVal _EMAIL As String)
        '---[ Send Outbid Email ]---
        BuildOutbidMail(_EMAIL, _FULLNAME, _USERID)
    End Sub

    Public Sub SendNewAuctionEmail(ByVal _EMAIL() As String)
        Dim oUtil As New utilities
        Dim sFullName As String = String.Empty
        Dim ds As DataSet
        Dim i As Integer = 0

        Do Until i > UBound(_EMAIL)
            '---[ reset dataset object ]---
            ds = New DataSet

            '---[ get full name and user name ]---
            sFullName = oUtil.GetFullNameByEmail(_EMAIL(i))

            '---[ Send Notification Email ]---
            BuildNewAuctionMail(_EMAIL(i), sFullName)

            i = i + 1
        Loop
    End Sub

    Private Sub BuildRegistrationMail(ByVal _TOADDRESS As String, ByVal _FULLNAME As String, ByVal _USERID As String, ByVal _VERIFICATIONCODE As String)
        Dim bodyMessage As New StringBuilder
        Dim sSubject As String = String.Empty
        Dim sWebURL As String = System.Configuration.ConfigurationManager.AppSettings("WebURL")
        Dim sCLUB_NAME As String = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")

        bodyMessage.Append("Hello " & _FULLNAME & ", thank you for registering to use " & sCLUB_NAME & " Online Auction.<br /><br />")
        bodyMessage.Append("To activate your account, please visit this link: <a href='" & sWebURL & "/UserAdmin/VerifyUser.aspx?u=" & _USERID & "&i=" & _VERIFICATIONCODE & "'>Activate Here</a>.<br />")
        bodyMessage.Append("<br />If this link does not work, please copy/paste the following link into your browser's address bar:<br />")
        bodyMessage.Append(sWebURL & "/UserAdmin/VerifyUser.aspx?u=" & _USERID & "&i=" & _VERIFICATIONCODE & ".<br />")
        bodyMessage.Append("Thank you.<br />~Webmaster")
        sSubject = "Account Verification Needed"

        SendMail(_TOADDRESS, sSubject, bodyMessage.ToString())
    End Sub

    Private Sub BuildOutbidMail(ByVal _TOADDRESS As String, ByVal _FULLNAME As String, ByVal _USERID As String)
        Dim bodyMessage As New StringBuilder
        Dim sSubject As String = String.Empty
        Dim sWebURL As String = System.Configuration.ConfigurationManager.AppSettings("WebURL")

        bodyMessage.Append("Hello " & _FULLNAME & ", this is to inform you that you have been outbid on your Lions Club auction item.<br /><br />")
        bodyMessage.Append("To increase your bid and support your local Lions Club, please visit this link: <a href='" & sWebURL & "'>Lions Club Auction</a>.<br />")
        bodyMessage.Append("<br /><br />")
        bodyMessage.Append("Thank you for your support.<br />~Webmaster")
        sSubject = "Lions Club Auction Outbid Notification"

        SendMail(_TOADDRESS, sSubject, bodyMessage.ToString())
    End Sub

    Private Sub BuildNewAuctionMail(ByVal _TOADDRESS As String, ByVal _FULLNAME As String)
        Dim bodyMessage As New StringBuilder
        Dim sSubject As String = String.Empty
        Dim sWebURL As String = System.Configuration.ConfigurationManager.AppSettings("WebURL")
        Dim sCLUB_NAME As String = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")

        bodyMessage.Append("Hello " & _FULLNAME & ",<br />This is to inform you that the " & sCLUB_NAME & " has a new item up on the auction website.<br /><br />")
        bodyMessage.Append("Please visit our site at <a href='" & sWebURL & "'>" & sCLUB_NAME & "</a> to check out this new auction!<br />")
        bodyMessage.Append("<br /><br />")
        bodyMessage.Append("As always, we thank you for your support.<br />~Webmaster")
        sSubject = "Lions Club New Auction Update"

        SendMail(_TOADDRESS, sSubject, bodyMessage.ToString())
    End Sub

    Public Sub SendMail(ByVal _TOADDRESS As String, ByVal _SUBJECT As String, ByVal _BODY As String)
        Try
            Dim mailMessage As New MailMessage
            Dim oSB As New StringBuilder()
            Dim sWebMasterEmail As String = System.Configuration.ConfigurationManager.AppSettings("WEBMASTER_EMAIL")
            Dim sClubEmail As String = System.Configuration.ConfigurationManager.AppSettings("CLUB_EMAIL")
            Dim sBCCEmail As String = System.Configuration.ConfigurationManager.AppSettings("BCC_EMAIL")
            Dim sCLUB_NAME As String = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
            Try
                oSB.Append(sCLUB_NAME & " Update" & "<br />")
                oSB.Append("<br />")
                oSB.Append(_BODY)

                mailMessage.From = New MailAddress(sWebMasterEmail)
                mailMessage.To.Add(New MailAddress(_TOADDRESS))
                mailMessage.CC.Add(New MailAddress(sClubEmail))
                mailMessage.Bcc.Add(New MailAddress(sBCCEmail))
                mailMessage.IsBodyHtml = True
                mailMessage.Subject = _SUBJECT
                mailMessage.Body = oSB.ToString()
                Dim smtpClient As New SmtpClient

                smtpClient.Send(mailMessage)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

#End Region

End Class
