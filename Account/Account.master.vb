﻿
Partial Class Account_Account
    Inherits System.Web.UI.MasterPage

    Public sDistrict As String = String.Empty
    Public sLocation As String = String.Empty
    Public sClubName As String = String.Empty
    Public sShortClubName As String = String.Empty

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Private Sub Auction_Load(sender As Object, e As EventArgs) Handles Me.Load
        sLocation = System.Configuration.ConfigurationManager.AppSettings("LOCATION")
        sDistrict = System.Configuration.ConfigurationManager.AppSettings("DISTRICT")
        sClubName = System.Configuration.ConfigurationManager.AppSettings("FULL_CLUB_NAME")
        sShortClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
    End Sub

End Class
