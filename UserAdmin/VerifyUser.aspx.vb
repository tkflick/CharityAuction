Imports System.Data

Partial Class User_VerifyUser
    Inherits System.Web.UI.Page

    Public sClubName As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
        Dim iUSERID As Integer = Request.QueryString("u")
        Dim iVALIDATIONID As Integer = Request.QueryString("i")
        Dim bACTIVATED As Boolean = False
        Dim rsErrDesc As String = String.Empty

        Dim dop As New utilities
        bACTIVATED = dop.ValidateUser(iUSERID, iVALIDATIONID, rsErrDesc)
        If bACTIVATED Then
            tSuccess.Visible = True
            tFailure.Visible = False
        Else
            tSuccess.Visible = False
            tFailure.Visible = True
            If Not rsErrDesc.Equals("") Then
                err.Text = rsErrDesc
                err.Visible = True
            End If
        End If
    End Sub
End Class
