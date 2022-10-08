Imports System

Partial Class Home
    Inherits System.Web.UI.Page

    Public sClubName As String = String.Empty
    Public sClubEmail As String = String.Empty
    Public sWebURL As String = String.Empty
    Public oUtil As New utilities

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        sClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
        sClubEmail = System.Configuration.ConfigurationManager.AppSettings("CLUB_EMAIL")
        sWebURL = System.Configuration.ConfigurationManager.AppSettings("WebURL")
        oUtil.SessionCheck()
    End Sub
End Class
