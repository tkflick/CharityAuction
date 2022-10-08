
Partial Class Auction
    Inherits System.Web.UI.MasterPage

    Public sDistrict As String = String.Empty
    Public sLocation As String = String.Empty
    Public sClubName As String = String.Empty
    Public sShortClubName As String = String.Empty
    Public sFacebookURL As String = String.Empty
    Public sAmazonSmileURL As String = String.Empty

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Session("AdminUser") Then AdminLink.Visible = True
    End Sub

    Private Sub Auction_Load(sender As Object, e As EventArgs) Handles Me.Load
        sLocation = System.Configuration.ConfigurationManager.AppSettings("LOCATION")
        sDistrict = System.Configuration.ConfigurationManager.AppSettings("DISTRICT")
        sClubName = System.Configuration.ConfigurationManager.AppSettings("FULL_CLUB_NAME")
        sShortClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
        sFacebookURL = System.Configuration.ConfigurationManager.AppSettings("FacebookLink")
        sAmazonSmileURL = System.Configuration.ConfigurationManager.AppSettings("AmazonSmileURL")
    End Sub

End Class

