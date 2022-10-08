Imports System.Data

Partial Class Auctions
    Inherits System.Web.UI.Page

    Public oUtil As New utilities
    Public oST As New systools
    Public sQS As String = String.Empty
    Public sClubName As String = String.Empty
    Public ReadOnly Property iCategory As Integer
        Get
            If Request.QueryString("c") IsNot Nothing Then
                Response.Cookies("c").Value = Request.QueryString("c").ToString()
                Return Integer.Parse(Request.QueryString("c").ToString())
            ElseIf Request.Cookies("c") IsNot Nothing Then
                Return Integer.Parse(Request.Cookies("c").Value.ToString())
            Else
                Return 0
            End If
        End Get
    End Property

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        oUtil.SessionCheck()
    End Sub

    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        Response.CacheControl = "No-cache"
        Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate") 'HTTP 1.1.
        Response.AppendHeader("Pragma", "no-cache") 'HTTP 1.0.
        Response.AppendHeader("Expires", "0") 'Proxies.
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sClubName = System.Configuration.ConfigurationManager.AppSettings("CLUB_NAME")
        Dim sSortQS As String = String.Empty
        Dim sCatQS As String = String.Empty

        If Request.QueryString("s") IsNot Nothing Then
            sSortQS = Request.QueryString("s")
        End If
        If Request.QueryString("c") IsNot Nothing Then
            sCatQS = Request.QueryString("c")
            If sCatQS = "0" Then sCatQS = String.Empty
        End If

        If Not iCategory.Equals(0) Then
            litHeading.Text = "Displaying items in <b>" & getCategoryName(iCategory) & "</b> (<a href='Category.aspx'>change category</a>)"
        Else
            litHeading.Text = "Displaying all items in auction (<a href='Category.aspx'>change category</a>)"
        End If
        If sSortQS.Equals("") And sCatQS.Equals("") Then
            'i have neither, default query
            getListings("item_date_close")
        ElseIf Not sSortQS.Equals("") And Not sCatQS.Equals("") Then
            'i have both, pull category and sort it
            getListings(sSortQS, sCatQS)
            sQS = "&c=" & sCatQS
        ElseIf Not sSortQS.Equals("") Then
            'i have the sort criteria only, so pull everything and sort it
            getListings(sSortQS)
        Else 'i have the category only, so pull all of that category and apply default sort
            getListingsByCatID(sCatQS)
            sQS = "&c=" & sCatQS
        End If

        litTotal.Text = getTotalRaised()
        If litTotal.Text = "0.00" Then
            pnlMoneyRaised.Visible = False
        Else
            pnlMoneyRaised.Visible = True
        End If

        If existsCurrentAuctions() Then
            pnlNoAuctions.Visible = False
        Else
            pnlNoAuctions.Visible = True
        End If
    End Sub

    Public Function FormatDescription(ByVal x As String) As String
        If x.Length < 130 Then
            Return x
        Else
            Return x.Substring(0, 130) & "<span title='..." & x.Substring(130) & "'>...</span>"
        End If
    End Function

    Public Function FormatAmount(ByVal s As String) As String
        If s.Equals(String.Empty) Then
            Return "No bids"
        Else
            Return "$ " & String.Format("{0:F2}", Decimal.Parse(s))
        End If
    End Function

    Public Function FormatAmount(ByVal d As Decimal) As String
        If d.Equals(0) Then
            Return "No bids"
        Else
            Return "$ " & String.Format("{0:F2}", d)
        End If
    End Function

    Public Function FormatCountdown(ByVal dtIn As String) As String
        Dim returnvalue As String = String.Empty
        Dim dtCount As DateTime = New DateTime()
        dtCount = CType(Convert.ToDateTime(dtIn), DateTime)

        If dtCount.Ticks > DateTime.Now.Ticks Then
            If (dtCount.AddTicks(-DateTime.Now.Ticks).Month - 1) > 0 Then
                returnvalue = dtCount.ToString("dd-MMM-yyyy HH:mm")
            Else
                If (dtCount.AddTicks(-DateTime.Now.Ticks).Day - 1) > 0 Then returnvalue += (dtCount.AddTicks(-DateTime.Now.Ticks).Day - 1) & "d "
                If dtCount.AddTicks(-DateTime.Now.Ticks).Hour > 0 Then returnvalue += dtCount.AddTicks(-DateTime.Now.Ticks).Hour & "h "

                If dtCount.AddTicks(-DateTime.Now.Ticks).Minute > 0 Then
                    returnvalue += dtCount.AddTicks(-DateTime.Now.Ticks).Minute & "m "
                End If

                If Not ((dtCount.AddTicks(-DateTime.Now.Ticks).Day - 1) > 0) And (Not (dtCount.AddTicks(-DateTime.Now.Ticks).Hour > 0)) And (Not (dtCount.AddTicks(-DateTime.Now.Ticks).Minute > 5)) Then returnvalue += dtCount.AddTicks(-DateTime.Now.Ticks).Second & "s"
            End If
        Else
            returnvalue = "<font color=red>Ended</font>"
        End If
        Return returnvalue
    End Function

    Private Function getTotalRaised() As String
        Dim sRetVal As String = String.Empty
        Dim ds As New DataSet
        Dim returnvalue As Decimal
        Dim sErrMsg As String = String.Empty
        Try
            'populate the temp table
            If Not oST.CalculateTotalRaised() Then
                Throw New Exception("Unable to populate the temp holding table.")
            End If
            'get the total raised
            If Not oST.GetTotalRaised(ds, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            'truncate the temp table
            If Not oST.TruncateTempTable() Then
                Throw New Exception("Unable to truncate temp holding table.")
            End If

            If Decimal.TryParse(ds.Tables(0).Rows(0)("total_raised").ToString(), returnvalue) Then
                sRetVal = returnvalue.ToString("0.00")
            Else
                sRetVal = "0.00"
            End If
            Return sRetVal
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx")
        End Try
    End Function

    Private Function getCategoryName(ByVal intCatId As Integer) As String
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Dim sRetVal As String = String.Empty
        Try
            If intCatId.Equals(0) Then
                Return "All"
            Else
                If Not oST.GetCategoryName(ds, intCatId, sErrMsg) Then
                    Throw New Exception(sErrMsg)
                End If
            End If
            sRetVal = ds.Tables(0).Rows(0)("cat_name").ToString()
            Return sRetVal
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx")
        End Try
    End Function


    Private Sub getListings(ByVal sSort As String)
        '---[ i have the sort value, need to implement it ]---
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Try
            If Not oST.GetListings(ds, sSort, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            If ds.Tables(0).Rows.Count = 0 Then
                dlListings.Visible = False
            Else
                dlListings.DataSource = ds
                dlListings.DataBind()
            End If

        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub

    Private Sub getListings(ByVal sSort As String, ByVal iCategory As Integer)
        '---[ i have the sort value, need to implement it ]---
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Try
            If Not oST.GetListings(ds, sSort, iCategory, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            If ds.Tables(0).Rows.Count = 0 Then
                dlListings.Visible = False
            Else
                dlListings.DataSource = ds
                dlListings.DataBind()
            End If
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub

    Private Sub getListingsByCatID(ByVal iCategory As Integer)
        '---[ i have the sort value, need to implement it ]---
        Dim ds As New DataSet
        Dim sErrMsg As String = String.Empty
        Try
            If Not oST.GetListingsByCatID(ds, iCategory, sErrMsg) Then
                Throw New Exception(sErrMsg)
            End If
            If ds.Tables(0).Rows.Count = 0 Then
                dlListings.Visible = False
            Else
                dlListings.DataSource = ds
                dlListings.DataBind()
            End If
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Sub

    Private Function existsCurrentAuctions() As Boolean
        Dim bRetVal As Boolean = False
        Dim sErrMsg As String = String.Empty
        Try
            bRetVal = oST.GetCurrentAuctions()
            Return bRetVal
        Catch ex As Exception
            Session("ErrMsg") = ex.Message()
            Response.Redirect("./Error/SystemError.aspx", False)
        End Try
    End Function
End Class
