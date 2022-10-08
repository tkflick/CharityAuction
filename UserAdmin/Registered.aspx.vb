Imports System.Text

Partial Class User_Registered
    Inherits System.Web.UI.Page

    Public sClubName As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
    End Sub

End Class
