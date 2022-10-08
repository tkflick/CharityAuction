Imports System
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Net.Mail

Partial Class thankyou
    Inherits System.Web.UI.Page

    Public lot_num As String = String.Empty

    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        Response.CacheControl = "No-cache"
        Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate") 'HTTP 1.1.
        Response.AppendHeader("Pragma", "no-cache") 'HTTP 1.0.
        Response.AppendHeader("Expires", "0") 'Proxies.

        lot_num = Request.QueryString("o")
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim retval As Boolean = False
        Dim sRecipient As String = Request.QueryString("r")
        Dim sAddress As String = Request.QueryString("a")
        Dim sCity As String = Request.QueryString("c")
        Dim sState As String = Request.QueryString("s")
        Dim sZip As String = Request.QueryString("z")
        Dim sEmail As String = Request.QueryString("e")
        Dim message As String = String.Empty

        Select Case Request.QueryString("i").ToString()
            Case "1" '---[ success from paypal ]---
                '---[ now create the body for the webmaster email to go to our treasurer ]---
                message = CreateBody(sRecipient, sAddress, sCity, sState, sZip, sEmail)

                '---[  send the email ]---
                SendMail("Auction Payment", message)

                '---[ update lot number to "PAID" ]---
                UpdateAuctionToPaid(lot_num)

                '---[  display the success message ]---
                DisplayMessage("Success!", "Your Payment has been submitted!<br />We will contact you shortly to arrange pick up.")

            Case "2" '---[ cancel from paypal ]---
                '---[ display the cancel message ]---
                DisplayMessage("Your payment has been cancelled.", "You may use the menu to continue browsing our website.")

            Case "3" '---[ email sent successfully ]---
                DisplayMessage("Thank you!", "Your email has been sent.<br />We will respond to you shortly.")

            Case "4" '---[ email sent successfully ]---
                DisplayMessage("Thank you!", "You have been subscribed to our New Auction Update email.")
        End Select

    End Sub

    Private Sub DisplayMessage(ByVal vHeader As String, ByVal vMessage As String)
        lblHeader.Text = vHeader
        lblMessage.Text = vMessage
    End Sub

    Private Function CreateBody(ByVal vRecipient As String, ByVal vAddress As String, ByVal vCity As String, ByVal vState As String, ByVal vZipCode As String, ByVal vEmail As String) As String
        Dim sBody As New StringBuilder
        Dim body As String = String.Empty
        Dim sOrderDetails As String = String.Empty
        Dim sItemName As String = String.Empty
        Dim oSys As New systools

        '---[ Get Item_Name for email ]--- 
        sItemName = oSys.GetItemName(lot_num)

        '---[ Create body message ]---
        sBody.AppendLine("Auction Lot Number: " & lot_num & " - " & sItemName & "<br />")
        sBody.AppendLine("Buyer:" & "<br />")
        sBody.AppendLine(vRecipient & "<br />")
        sBody.AppendLine(vEmail & "<br />")
        sBody.AppendLine("<br /><br />")
        sBody.AppendLine("Shipping Address:" & "<br />")
        sBody.AppendLine(vRecipient & "<br />")
        sBody.AppendLine(vAddress & "<br />")
        sBody.AppendLine(vCity & ", " & vState & "  " & vZipCode & "<br />")
        sBody.AppendLine("<br /><br />")

        CreateBody = sBody.ToString()
    End Function

    Public Sub SendMail(ByVal _SUBJECT As String, ByVal _BODY As String)
        Try
            Dim mailMessage As New MailMessage
            Dim sTOADDRESS As String = System.Configuration.ConfigurationManager.AppSettings("CLUB_EMAIL")
            Dim sWebMasterEmail As String = System.Configuration.ConfigurationManager.AppSettings("WEBMASTER_EMAIL")
            Dim sBCCEmail As String = System.Configuration.ConfigurationManager.AppSettings("BCC_EMAIL")
            Dim sClubEmail As String = System.Configuration.ConfigurationManager.AppSettings("CLUB_EMAIL")
            Dim oSB As New StringBuilder()

            Try
                oSB.Append("Auction Payment" & "<br />")
                oSB.Append("<br />")
                oSB.Append(_BODY)

                mailMessage.From = New MailAddress(sWebMasterEmail)
                mailMessage.To.Add(New MailAddress(sTOADDRESS))
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

    Public Sub UpdateAuctionToPaid(ByVal _LOTNUMBER As String)
        Dim oSys As New systools
        Dim sErrMsg As String = String.Empty
        Try
            oSys.UpdateAuctionToPaid(_LOTNUMBER, sErrMsg)
            If sErrMsg <> "" Then
                Throw New Exception(sErrMsg)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

End Class
