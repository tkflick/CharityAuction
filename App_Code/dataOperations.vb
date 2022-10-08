Imports System
Imports System.Data
Imports System.Net.Mail
Imports System.Configuration.ConfigurationSettings

Public Class dataOperations

#Region " Private Data / Dataset Functions "

    Public Function AddUpdateRecord(ByVal sSQL As String, ByVal sConnType As String) As Boolean
        Dim bSUCCESS As Boolean = False
        Dim sCONSTRING As String = String.Empty
        Dim oCNX As New OleDb.OleDbConnection

        GetConnectionString(sCONSTRING, sConnType)
        oCNX.ConnectionString = sCONSTRING
        oCNX.Open()

        Dim cmd As New OleDb.OleDbCommand(sSQL, oCNX)

        If cmd.ExecuteNonQuery() Then
            bSUCCESS = True
        Else
            bSUCCESS = False
        End If

        oCNX.Close()
        Return bSUCCESS
    End Function

    Public Function GetDataSet(ByVal sSQL As String, ByVal sConnType As String) As DataSet
        Dim oCNX As New OleDb.OleDbConnection
        Dim sCONSTRING As String = String.Empty
        Dim ds As New DataSet()

        GetConnectionString(sCONSTRING, sConnType)
        oCNX.ConnectionString = sCONSTRING
        oCNX.Open()

        Dim da As Object = New OleDb.OleDbDataAdapter(sSQL, oCNX)
        da.Fill(ds, "dataset")
        oCNX.Close()

        Return ds
    End Function

    Public Function GetSingleValue(ByVal sField As String, ByRef dsDataSet As DataSet) As String
        Dim dvDataView As DataView
        Dim drDataRowView As DataRowView
        Dim sReturn As String = String.Empty

        dvDataView = New DataView(dsDataSet.Tables("dataset"))
        For Each drDataRowView In dvDataView
            sReturn = Convert.ToString(drDataRowView(sField))
        Next

        Return (sReturn)
    End Function

    Private Sub GetConnectionString(ByRef _CONNECTIONSTRING As String, ByVal _CONNECTION_TYPE As String)
        Dim oCNX As String = System.Configuration.ConfigurationManager.AppSettings("oCNX")
        Dim db As String = System.Configuration.ConfigurationManager.AppSettings(_CONNECTION_TYPE)

        _CONNECTIONSTRING = oCNX.Replace("XXREPLACEXX", System.Web.HttpContext.Current.Server.MapPath(db))

    End Sub

#End Region

End Class
