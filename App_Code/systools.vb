Imports System
Imports System.Data
Imports System.Configuration.ConfigurationSettings

Public Class systools
    Inherits System.Web.UI.Page

    Dim objDO As New dataOperations

#Region "  AUCTION METHODS  "

    ''' <summary>
    ''' Returns a dataset containing details of the auctio associated with the 
    ''' ItemId passed in.  Called from item.aspx  (spItemDetails)
    ''' </summary>
    ''' <param name="dsDataSet">DataSet</param>
    ''' <param name="sItemID">String</param>
    ''' <param name="_ERRMSG">String</param>
    ''' <returns>Boolean</returns>
    Public Function GetItemDetails(ByRef dsDataSet As DataSet, ByVal sItemID As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT item_name, item_description, item_date_close, item_seller, item_location, donated_by, opening_bid, img, paid,")
            sSQL.Append("   (SELECT TOP 1 item_userid FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_userid, ")
            sSQL.Append("   (SELECT TOP 1 item_amount FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_amount, ")
            sSQL.Append("   (SELECT TOP 1 item_bidder FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_bidder, ")
            sSQL.Append("   (SELECT COUNT(0) FROM tbBids WHERE tbBids.item_id = i.item_id) AS item_bids, ")
            sSQL.Append("   (SELECT cat_name from tbCategories where cat_id = i.cat_id) as cat_name ")
            sSQL.Append("FROM tbItems as i ")
            sSQL.Append("WHERE item_id = " & sItemID & " ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Places a bid for an auction item.  Called from item.aspx (spBid)
    ''' </summary>
    ''' <remarks>
    ''' due to limitations of Access DBs, I need to check the validity of the big first,
    ''' then if it's ok, insert the bid.
    ''' </remarks>
    ''' <param name="sItemID">string</param>
    ''' <param name="sBidAmount">string</param>
    ''' <param name="sBidder">string</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns>Boolean</returns>
    Public Function PlaceBid(ByVal sItemID As String, ByVal sBidAmount As String, ByVal sBidder As String, ByVal sUserId As String, ByRef _ERRMSG As String) As Boolean
        Dim ds As New DataSet
        Dim ds2 As New DataSet
        Dim sSQL As New StringBuilder
        Dim bRetVal As Boolean = False
        Dim sCurrentBid As String = String.Empty
        Dim sClosingDate As String = String.Empty
        Try
            '---[ Check the DATE, is the auction over ]---
            sSQL.Append("SELECT item_date_close FROM tbItems WHERE item_id = " & sItemID & " ")
            ds = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            sClosingDate = ds.Tables(0).Rows(0)("item_date_close").ToString()

            '---[ Check the BID, is it a valid bid ]---
            'sSQL.Clear()
            sSQL = New StringBuilder
            sSQL.Append("SELECT max(item_amount) as item_amount FROM tbBids WHERE item_id = " & sItemID & " ")
            ds2 = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            sCurrentBid = ds2.Tables(0).Rows(0)("item_amount").ToString()
            If sCurrentBid.Trim() = "" Then sCurrentBid = 0

            If Convert.ToDateTime(sClosingDate) > Now() Then
                If (Convert.ToDecimal(sBidAmount) > Convert.ToDecimal(sCurrentBid) + 1) Then
                    '---[ insert the bid ]---
                    'sSQL.Clear()
                    sSQL = New StringBuilder
                    sSQL.Append("INSERT INTO tbBids (item_id, item_amount, item_bidder, item_userid, item_date_bid) ")
                    sSQL.Append("VALUES('" & sItemID & "', '" & sBidAmount & "', '" & sBidder & "', '" & sUserId & "', Date())")
                    If Not objDO.AddUpdateRecord(sSQL.ToString, "auction_db") Then
                        Throw New Exception("Error inserting bid.")
                    End If
                    bRetVal = True
                Else
                    Throw New Exception("BID AMOUNT NOT VALID")
                End If
            Else
                Throw New Exception("AUCTION HAS ENDED")
            End If
            Return bRetVal
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' spBidHistory
    ''' </summary>
    ''' <param name="dsDataSet">dataset</param>
    ''' <param name="sItemID">string</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns></returns>
    Public Function GetBidHistory(ByRef dsDataSet As DataSet, ByVal sItemID As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT tbBids.item_date_bid, tbBids.item_amount, tbBids.item_bidder, tbBids.item_userid, tbItems.item_name ")
            sSQL.Append("FROM tbBids, tbItems ")
            sSQL.Append("WHERE tbBids.item_id = " & sItemID & " AND tbBids.item_id = tbItems.item_id ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    Public Function AddCategory(ByVal sCatName As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("INSERT INTO tbCategories(cat_name) ")
            sSQL.Append("VALUES('" & sCatName & "') ")
            '---[ run the SQL and put the results in the dataset object ]---
            If Not objDO.AddUpdateRecord(sSQL.ToString(), "auction_db") Then
                Throw New Exception("Failed to insert new category.")
            End If
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    Public Function DeleteCategory(ByVal sCatId As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("DELETE FROM tbCategories ")
            sSQL.Append("WHERE cat_id = " & sCatId & " ")
            '---[ run the SQL and put the results in the dataset object ]---
            If Not objDO.AddUpdateRecord(sSQL.ToString(), "auction_db") Then
                Throw New Exception("Failed to delete category.")
            End If
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' spListCategory
    ''' </summary>
    ''' <param name="dsDataSet">dataset</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns>Boolean</returns>
    Public Function GetCategoryList(ByRef dsDataSet As DataSet, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT c.cat_id, c.cat_name, COUNT(i.item_id) AS TOTAL_ITEMS ")
            sSQL.Append("FROM tbCategories c LEFT OUTER JOIN ")
            sSQL.Append("   tbItems i ON i.cat_id = c.cat_id ")
            sSQL.Append("GROUP BY c.cat_id, c.cat_name ")
            sSQL.Append("HAVING COUNT(i.item_id) <> 0 ")
            sSQL.Append("UNION ")
            sSQL.Append("SELECT '0' AS cat_id, 'All' AS cat_name, COUNT(0) AS TOTAL_ITEMS ")
            sSQL.Append("FROM tbItems ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' spListAllCategory
    ''' </summary>
    ''' <returns>dataset</returns>
    Public Function GetAllCategories() As DataSet
        Dim sSQL As New StringBuilder
        Dim dsDataSet As New DataSet
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT c.cat_id, c.cat_name ")
            sSQL.Append("FROM tbCategories c ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return dsDataSet
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            Throw New Exception(ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' spTotalRaised
    ''' </summary>
    ''' <remarks>
    ''' This function will populate the holding table with all the max bids so GetTotalRaised can 
    ''' do a SUM and report the grand total.
    ''' to do: SELECT SUM(SELECT max(item_amount) FROM tbBids GROUP BY item_id)) ??
    ''' </remarks>
    ''' <returns>boolean</returns>
    Public Function CalculateTotalRaised() As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("INSERT INTO tbRaised(max_bid) ")
            sSQL.Append("SELECT max(item_amount) FROM tbBids GROUP BY item_id ")
            '---[ run the SQL and put the results in the dataset object ]---
            If Not objDO.AddUpdateRecord(sSQL.ToString(), "auction_db") Then

            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function TruncateTempTable() As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("DELETE * FROM tbRaised ")
            '---[ run the SQL and put the results in the dataset object ]---
            If Not objDO.AddUpdateRecord(sSQL.ToString(), "auction_db") Then

            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' spTotalRaised
    ''' </summary>
    ''' <remarks>
    ''' This is a holding table to aggregate funds raised.  It will be truncated after each use.
    ''' </remarks>
    ''' <param name="dsDataSet">dataset</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns>boolean</returns>
    Public Function GetTotalRaised(ByRef dsDataSet As DataSet, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT SUM(max_bid) as total_raised ")
            sSQL.Append("FROM tbRaised ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Get all categories for AddCategory.aspx
    ''' </summary>
    ''' <param name="dsDataSet"></param>
    ''' <param name="_ERRMSG"></param>
    ''' <returns>boolean</returns>
    Public Function GetAllCategories(ByRef dsDataSet As DataSet, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT * ")
            sSQL.Append("FROM tbCategories ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' spCatName
    ''' </summary>
    ''' <param name="dsDataSet">dataset</param>
    ''' <param name="sCatID">string</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns>boolean</returns>
    Public Function GetCategoryName(ByRef dsDataSet As DataSet, ByVal sCatID As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT cat_name ")
            sSQL.Append("FROM tbCategories ")
            sSQL.Append("WHERE cat_id = " & sCatID & " ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' spListings
    ''' </summary>
    ''' <param name="dsDataSet">dataset</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns>boolean</returns>
    Public Function GetListings(ByRef dsDataSet As DataSet, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT i.item_id, i.item_name, i.item_description, i.item_date_open, i.item_date_close, i.item_seller, i.item_location, ")
            sSQL.Append("   (SELECT TOP 1 item_amount FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_amount, ")
            sSQL.Append("   (SELECT TOP 1 item_bidder FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_bidder, ")
            sSQL.Append("   (SELECT COUNT(0) FROM tbBids WHERE tbBids.item_id = i.item_id) AS item_bids, ")
            sSQL.Append("   'Everything Else' as cat_name ")
            sSQL.Append("FROM tbItems as i ")
            sSQL.Append("ORDER BY item_date_close ASC ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    Public Function GetListings(ByRef dsDataSet As DataSet, ByVal sSort As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT i.item_id, i.item_name, i.item_description, i.item_date_open, i.item_date_close, i.item_seller, i.item_location, ")
            sSQL.Append("   (SELECT TOP 1 item_amount FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_amount, ")
            sSQL.Append("   (SELECT TOP 1 item_bidder FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_bidder, ")
            sSQL.Append("   (SELECT TOP 1 item_userid FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_userid, ")
            sSQL.Append("   (SELECT COUNT(0) FROM tbBids WHERE tbBids.item_id = i.item_id) AS item_bids, ")
            sSQL.Append("   'Everything Else' as cat_name ")
            sSQL.Append("FROM tbItems as i ")
            sSQL.Append("ORDER BY " & sSort & " DESC ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    Public Function GetListings(ByRef dsDataSet As DataSet, ByVal sSort As String, ByVal sCatID As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Dim lSort As String = String.Empty
        Try
            If sSort = "item_amount" Or sSort = "item_bidder" Or sSort = "item_bids" Then
                lSort = "tbBids."
            Else
                lSort = "i."
            End If
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT i.item_id, i.item_name, i.item_description, i.item_date_open, i.item_date_close, i.item_seller, i.item_location, cat_id ")
            sSQL.Append("   (SELECT TOP 1 item_amount FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_amount, ")
            sSQL.Append("   (SELECT TOP 1 item_bidder FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_bidder, ")
            sSQL.Append("   (SELECT COUNT(0) FROM tbBids WHERE tbBids.item_id = i.item_id) AS item_bids, ")
            sSQL.Append("   'Everything Else' as cat_name ")
            sSQL.Append("FROM tbItems as i ")
            sSQL.Append("WHERE cat_id = " & sCatID & " ")
            sSQL.Append("ORDER BY " & lSort & sSort & " ASC ")

            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' spListings
    ''' </summary>
    ''' <param name="dsDataSet">dataset</param>
    ''' <param name="sCatID">string</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns></returns>
    Public Function GetListingsByCatID(ByRef dsDataSet As DataSet, ByVal sCatID As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT i.item_id, i.item_name, i.item_description, i.item_date_open, i.item_date_close, i.item_seller, i.item_location, ")
            sSQL.Append("   (SELECT TOP 1 item_amount FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_amount, ")
            sSQL.Append("   (SELECT TOP 1 item_bidder FROM tbBids WHERE tbBids.item_id = i.item_id ORDER BY item_amount DESC) AS item_bidder, ")
            sSQL.Append("   (SELECT COUNT(0) FROM tbBids WHERE tbBids.item_id = i.item_id) AS item_bids, ")
            sSQL.Append("   (select cat_name from tbCategories where cat_id = i.cat_id) as cat_name	")
            sSQL.Append("FROM tbItems as i ")
            sSQL.Append("WHERE i.cat_id = " & sCatID & " ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    ''' <summary>
    ''' spAddAuction
    ''' </summary>
    ''' <param name="sName">string</param>
    ''' <param name="sDesc">string</param>
    ''' <param name="sCloseDate">string</param>
    ''' <param name="sSeller">string</param>
    ''' <param name="sLocation">string</param>
    ''' <param name="sCategory">string</param>
    ''' <param name="sFilename">string</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns>boolean</returns>
    Public Function AddAuction(ByVal sName As String, ByVal sDesc As String, ByVal sCloseDate As String, ByVal sSeller As String, ByVal sLocation As String, ByVal sCategory As String, ByVal sFilename As String, ByVal sDonatedBy As String, ByVal sOpeningBid As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("INSERT INTO tbItems (item_name, item_description, item_date_open, item_date_close, item_seller, item_location, donated_by, opening_bid, cat_id, img) ")
            sSQL.Append("VALUES ('" & sName & "', '" & sDesc.Replace("'", "''") & "', DATE(), '" & sCloseDate & "', '" & sSeller & "', '" & sLocation & "', '" & sDonatedBy & "', '" & sOpeningBid & "', '" & sCategory & "', '" & sFilename & "') ")

            '---[ run the SQL and put the results in the dataset object ]---
            If Not objDO.AddUpdateRecord(sSQL.ToString(), "auction_db") Then
                Throw New Exception("Auction Insert Failed")
            End If
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    Public Function UpdateAuctionToPaid(ByVal sLotNumber As String, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to update record in the DATABASE ]---
            sSQL.Append("UPDATE tbItems ")
            sSQL.Append("SET paid = 'Y' ")
            sSQL.Append("WHERE item_id = " & sLotNumber)

            '---[ run the SQL and put the results in the dataset object ]---
            If Not objDO.AddUpdateRecord(sSQL.ToString(), "auction_db") Then
                Throw New Exception("Auction Update Failed")
            End If
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function

    Public Function GetItemName(ByVal sLotNumber As String) As String
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim sRetVal As String = String.Empty
        Try
            '---[ Create the SQL to update record in the DATABASE ]---
            sSQL.Append("SELECT item_name ")
            sSQL.Append("FROM tbItems ")
            sSQL.Append("WHERE item_id = " & sLotNumber)

            '---[ run the SQL and put the results in the dataset object ]---
            ds = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            sRetVal = objDO.GetSingleValue("item_name", ds)

            '---[ no errors so return TRUE ]---
            Return sRetVal
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            Throw New Exception(ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' spTime
    ''' </summary>
    ''' <param name="dsDataSet">dataset</param>
    ''' <param name="_ERRMSG">string</param>
    ''' <returns>booleans</returns>
    Public Function GetTime(ByRef dsDataSet As DataSet, ByRef _ERRMSG As String) As Boolean
        Dim sSQL As New StringBuilder
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT Date() as [date] ")
            '---[ run the SQL and put the results in the dataset object ]---
            dsDataSet = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            '---[ no errors so return TRUE ]---
            Return True
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            _ERRMSG = ex.Message
            Return False
        End Try
    End Function


    Public Function GetCurrentAuctions() As Boolean
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim iCount As Integer = 0
        Dim bRetVal As Boolean = False
        Try
            '---[ Create the SQL to pull records from the DATABASE ]---
            sSQL.Append("SELECT count(*) as iCount ")
            sSQL.Append("FROM tbItems ")
            sSQL.Append("WHERE item_date_close > Date() ")

            '---[ run the SQL and put the results in the dataset object ]---
            ds = objDO.GetDataSet(sSQL.ToString(), "auction_db")
            iCount = objDO.GetSingleValue("iCount", ds)
            If iCount = 0 Then
                bRetVal = False
            Else
                bRetVal = True
            End If
            '---[ no errors so return TRUE ]---
            Return bRetVal
        Catch ex As Exception
            '---[ we had an error, populate the error desc and return false ]---
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

End Class
