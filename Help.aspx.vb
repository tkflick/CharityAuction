Imports System

Partial Class Help
    Inherits System.Web.UI.Page

    Public sClubEmail As String = String.Empty
    Public sWebURL As String = String.Empty

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        sClubEmail = System.Configuration.ConfigurationManager.AppSettings("CLUB_EMAIL")
        sWebURL = System.Configuration.ConfigurationManager.AppSettings("WebURL")
    End Sub
End Class
