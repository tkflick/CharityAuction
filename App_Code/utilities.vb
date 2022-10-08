Imports System
Imports System.Data
Imports System.Net.Mail
Imports System.Configuration.ConfigurationSettings

Public Class utilities
    Inherits System.Web.UI.Page

    Dim objDO As New dataOperations
    Private sEncryptionKey As String = System.Configuration.ConfigurationManager.AppSettings("EncryptionKey")

#Region "  LOG IN FUNCTIONS  "

    Public Function AuthenticateUser(ByVal _USERNAME As String, ByVal _PASSWORD As String,
                                     ByRef _DATASET As DataSet, ByRef _ERROR As String) As Boolean

        Dim bAUTHENTICATED As Boolean = False
        Dim sSQL As New StringBuilder
        Dim sDecryptedPW As String = String.Empty

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERNAME & "' ")

        Dim DS As New DataSet
        DS = objDO.GetDataSet(sSQL.ToString(), "user_db")
        If DS.Tables("dataset").Rows.Count > 0 Then
            Dim sPassword As String = objDO.GetSingleValue("Password", DS)
            Dim sPassExpire As String = objDO.GetSingleValue("PassExpire", DS)

            sDecryptedPW = DKryptit(sPassword, sEncryptionKey)

            If sPassExpire = "" Then
                sPassExpire = "01/01/1980"
            End If

            Dim sStatus As String = objDO.GetSingleValue("Status", DS)
            Dim sVerified As String = objDO.GetSingleValue("UserVerified", DS)
            Dim sMustChange As String = objDO.GetSingleValue("MustChange", DS)

            Session.Add("reset", "n")

            If _PASSWORD = sDecryptedPW Then
                If CDate(sPassExpire) > Now() AndAlso sStatus = "A" AndAlso sVerified = "Y" Then
                    bAUTHENTICATED = True
                    _DATASET = DS
                    If sMustChange = "Y" Then
                        _ERROR = "reset"
                        Session("reset") = "y"
                    End If
                    Session("UserName") = _USERNAME
                Else
                    If sStatus <> "A" Then
                        _ERROR = "inactive"
                    ElseIf sVerified = "N" Then
                        _ERROR = "unverified"
                    Else
                        _ERROR = "expired"
                    End If
                End If
            Else
                _ERROR = "Password is incorrect"
            End If
        Else
            _ERROR = "User Name not found"
        End If

        Return bAUTHENTICATED

    End Function

    Public Function BypassManualLogin(ByVal _USERNAME As String, ByVal _PASSWORD As String,
                                      ByRef _DATASET As DataSet) As String

        Dim bAUTHENTICATED As Boolean = False
        Dim sSQL As New StringBuilder

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERNAME & "'")

        Dim DS As New DataSet
        DS = objDO.GetDataSet(sSQL.ToString(), "user_db")
        If DS.Tables("dataset").Rows.Count > 0 Then
            bAUTHENTICATED = True
        End If

        Return bAUTHENTICATED

    End Function

#End Region

#Region "  PASSWORD FUNCTIONS  "

    Public Function GetPassword(ByVal _EMAIL As String) As String
        Dim sPASSWORD As String = String.Empty
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE Email = '" & _EMAIL & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        sPASSWORD = objDO.GetSingleValue("Password", ds)
        sPASSWORD = DKryptit(sPASSWORD, sEncryptionKey)

        Return sPASSWORD

    End Function

    Public Function ChangePassword(ByVal _USERNAME As String, ByVal _CURRENTPASS As String,
                                   ByVal _NEWPASS As String, ByRef _ERR As String) As Boolean

        Dim sSQL As New StringBuilder
        Dim bSUCCESS As Boolean = False
        Dim bCURRENTPW As Boolean = False
        Dim dNewExpireDate As Date = Now().AddDays(90).ToShortDateString
        Dim ds As New DataSet
        Dim sEncryptedCurrentPW As String = Kryptit(_CURRENTPASS, sEncryptionKey)
        Dim sEncryptedNewPW As String = Kryptit(_NEWPASS, sEncryptionKey)

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERNAME & "' AND Password = '" & sEncryptedCurrentPW & "' ")
        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")

        If ds.Tables("dataset").Rows.Count = 0 Then
            bCURRENTPW = False
            _ERR = "Password not found."
        Else
            bCURRENTPW = True
        End If

        If bCURRENTPW Then
            sSQL.Length = 0
            sSQL.Append("UPDATE tUserInfo ")
            sSQL.Append("SET [Password] = '" & sEncryptedNewPW & "', [PassExpire] = Date() + 90 ")
            sSQL.Append("WHERE UserName = '" & _USERNAME & "' ")
            bSUCCESS = objDO.AddUpdateRecord(sSQL.ToString(), "user_db")
        End If

        Return bSUCCESS

    End Function

#End Region

#Region "  USER / ACCOUNT ACTIVATION  "

    Public Function GetUserInfo(ByVal _USERNAME As String) As String
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim sUSEREMAIL As String = ""

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERNAME & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        sUSEREMAIL = objDO.GetSingleValue("Email", ds)

        Return sUSEREMAIL

    End Function

    Public Function ValidateUser(ByVal _USERID As Integer, ByVal _VALIDATIONID As Integer, ByRef _ERR As String) As Boolean
        Dim bSUCCESS As Boolean = False
        Dim bUSEREXISTS As Boolean = False
        Dim sSQL As New StringBuilder
        Dim obj As New dataOperations
        Dim ds As New DataSet

        '---[ Verify that the userID and ActivationCode exist in the DB ]--- 
        sSQL.Append("SELECT * from tUserInfo ")
        sSQL.Append("WHERE ActivationCode = " & _VALIDATIONID & " AND id = " & _USERID & " ")
        ds = obj.GetDataSet(sSQL.ToString(), "user_db")
        If ds.Tables("dataset").Rows.Count > 0 Then
            bUSEREXISTS = True
        End If

        '---[ Update the Verification field ]---
        If bUSEREXISTS Then
            sSQL.Length = 0
            sSQL.Append("UPDATE tUserInfo ")
            sSQL.Append("SET UserVerified = 'Y' ")
            sSQL.Append("WHERE ActivationCode = " & _VALIDATIONID & " AND id = " & _USERID & " ")

            bSUCCESS = obj.AddUpdateRecord(sSQL.ToString(), "user_db")

            '---[ If Update was successful, we need to check to see if it's time to activate the user ]---
            If bSUCCESS Then
                Dim sUSER As String = ""
                '---[ Check the status field. If both Client and Tech have verified, set to "A" for Active ]---
                sSQL.Length = 0
                sSQL.Append("SELECT * FROM tUserInfo ")
                sSQL.Append("WHERE id = " & _USERID & " ")
                ds = obj.GetDataSet(sSQL.ToString(), "user_db")
                sUSER = obj.GetSingleValue("UserVerified", ds)
                If sUSER = "Y" Then
                    '---[ Set the user from the default Inactive (I) to Active (A) ]---
                    sSQL.Length = 0
                    sSQL.Append("UPDATE tUserInfo ")
                    sSQL.Append("SET Status = 'A' ")
                    sSQL.Append("WHERE id = " & _USERID & " ")
                    obj.AddUpdateRecord(sSQL.ToString(), "user_db")
                End If
            End If
        Else
            _ERR = "Your information is not available."
        End If

        Return bSUCCESS

    End Function

#End Region

#Region "  NEW USER FUNCTION GROUP  "

    ''' <summary>
    ''' Username must be unique
    ''' </summary>
    ''' <param name="_USERNAME"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsUserNameFree(ByVal _USERNAME As String) As Boolean
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim bUSERNAMEFREE As Boolean

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERNAME & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        If ds.Tables("dataset").Rows.Count > 0 Then
            '---[ USERNAME IS NOT FREE ]---
            bUSERNAMEFREE = False
        Else
            '---[ USERNAME IS FREE ]---
            bUSERNAMEFREE = True
        End If

        Return bUSERNAMEFREE

    End Function

    Public Function IsNickNameFree(ByVal _NICKNAME As String) As Boolean
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim bNICKNAMEFREE As Boolean

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE NickName = '" & _NICKNAME & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        If ds.Tables("dataset").Rows.Count > 0 Then
            '---[ NICKNAME IS NOT FREE ]---
            bNICKNAMEFREE = False
        Else
            '---[ NICKNAME IS FREE ]---
            bNICKNAMEFREE = True
        End If

        Return bNICKNAMEFREE

    End Function

    ''' <summary>
    ''' Email must be unique
    ''' </summary>
    ''' <param name="_EMAIL"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsEmailFree(ByVal _EMAIL As String) As Boolean
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim bEMAILFREE As Boolean

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE Email = '" & _EMAIL & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        If ds.Tables("dataset").Rows.Count > 0 Then
            '---[ EMAIL IS NOT FREE ]---
            bEMAILFREE = False
        Else
            '---[ EMAIL IS FREE ]---
            bEMAILFREE = True
        End If

        Return bEMAILFREE

    End Function

    ''' <summary>
    ''' Update FLAG to send update email when a new item is put up for auction
    ''' </summary>
    ''' <param name="_EMAIL"></param>
    ''' <returns>boolean</returns>
    Public Function AddNewAuctionNotification(ByVal _EMAIL As String) As Boolean
        Dim obj As New dataOperations
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim bSUCCESS As Boolean

        sSQL.Append("UPDATE tUserInfo ")
        sSQL.Append("SET NewAuctionEmail = 'Y' ")
        sSQL.Append("WHERE Email = '" & _EMAIL & "' ")

        bSUCCESS = obj.AddUpdateRecord(sSQL.ToString(), "user_db")

        Return bSUCCESS

    End Function

    ''' <summary>
    ''' Test the strength of the chosen password and verify that it matches the 
    ''' pattern rules.. at least 1 uppercase, 1 lowercase, 1 number, 1 special character
    ''' and at least 6 characters long and no more than 10 characters
    ''' </summary>
    ''' <param name="_PASSWORD"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsPasswordStrong(ByVal _PASSWORD As String) As Boolean
        Dim PASSWORD_PATTERN As String = "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%])(?!.*\s).{6,16}$"
        Dim _CHECK As New Text.RegularExpressions.Regex(PASSWORD_PATTERN, RegexOptions.IgnorePatternWhitespace)
        Dim bIsValid As Boolean = False

        '---[ make sure an email address was provided ]---
        If String.IsNullOrEmpty(_PASSWORD) Then
            bIsValid = False
        Else
            '---[ use IsMatch to validate the password ]---
            bIsValid = _CHECK.IsMatch(_PASSWORD)
        End If
        '---[ return the value to the calling method ]---
        Return bIsValid

        '---[ description of what each part of the pattern is / does ]---
        '---[ ^			# Start of group
        '---[   (?=.*\d)		# must contain at least one digit from 0-9
        '---[   (?=.*[a-z])		# must contain at least one lowercase character
        '---[   (?=.*[A-Z])		# must contain at least one uppercase character
        '---[   (?=.*[_@#$%])	# must contain at least one special symbol in the list "_@#$%"
        '---[   (?!.*\s)        # disallows a space in the string
        '---[     .         	# match anything with previous condition checking
        '---[   {6,10}	        # length at least 4 characters and maximum of 10	
        '---[ $		   	# End of group

    End Function

    ''' <summary>
    ''' Add the new user to the database
    ''' </summary>
    ''' <param name="_FIRSTNAME"></param>
    ''' <param name="_LASTNAME"></param>
    ''' <param name="_EMAIL"></param>
    ''' <param name="_USERNAME"></param>
    ''' <param name="_PASSWORD"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddNewUser(ByVal _FIRSTNAME As String, ByVal _LASTNAME As String, ByVal _EMAIL As String, ByVal _USERNAME As String, ByVal _PASSWORD As String) As Boolean
        Dim bADDED As Boolean = False
        Dim sSQL As New StringBuilder
        Dim oRandom As New Random()

        Dim iActivationCode As Integer = oRandom.Next(1000000, 9999999)
        Dim sEncryptedPW As String = Kryptit(_PASSWORD, sEncryptionKey)
        Dim sPassExpire As String = Now().AddMonths(3).ToString("MM/dd/yyyy")

        sSQL.Append("INSERT INTO tUserInfo([FirstName], [LastName], [Email], [UserName], [Password], [PassExpire], [ActivationCode], [UserVerified], [CustomerID]) ")
        sSQL.Append("VALUES('" & _FIRSTNAME & "', '" & _LASTNAME & "', '" & _EMAIL & "', '" & _USERNAME & "', '" & sEncryptedPW & "', '" & sPassExpire & "', " & iActivationCode & ",'N', '" & _USERNAME & "')")

        bADDED = objDO.AddUpdateRecord(sSQL.ToString(), "user_db")

        Return bADDED

    End Function

    ''' <summary>
    ''' overload that accepts nickname value
    ''' </summary>
    ''' <param name="_FIRSTNAME"></param>
    ''' <param name="_LASTNAME"></param>
    ''' <param name="_EMAIL"></param>
    ''' <param name="_USERNAME"></param>
    ''' <param name="_NICKNAME"></param>
    ''' <param name="_PASSWORD"></param>
    ''' <returns></returns>
    Public Function AddNewUser(ByVal _FIRSTNAME As String, ByVal _LASTNAME As String, ByVal _EMAIL As String, ByVal _USERNAME As String, ByVal _NICKNAME As String, ByVal _PASSWORD As String, ByVal _OUTBIDEMAIL As String) As Boolean
        Dim bADDED As Boolean = False
        Dim sSQL As New StringBuilder
        Dim oRandom As New Random()

        Dim iActivationCode As Integer = oRandom.Next(1000000, 9999999)
        Dim sEncryptedPW As String = Kryptit(_PASSWORD, sEncryptionKey)
        Dim sPassExpire As String = Now().AddMonths(3).ToString("MM/dd/yyyy")

        sSQL.Append("INSERT INTO tUserInfo([FirstName], [LastName], [Email], [UserName], [Nickname], [Password], [PassExpire], [ActivationCode], [UserVerified], [CustomerID], [OutbidEmail]) ")
        sSQL.Append("VALUES('" & _FIRSTNAME & "', '" & _LASTNAME & "', '" & _EMAIL & "', '" & _USERNAME & "', '" & _NICKNAME & "', '" & sEncryptedPW & "', '" & sPassExpire & "', " & iActivationCode & ",'N', '" & _USERNAME & "','" & _OUTBIDEMAIL & "')")

        bADDED = objDO.AddUpdateRecord(sSQL.ToString(), "user_db")

        Return bADDED

    End Function

    ''' <summary>
    ''' Add the new non master user account to the database
    ''' </summary>
    ''' <param name="_FIRSTNAME"></param>
    ''' <param name="_LASTNAME"></param>
    ''' <param name="_EMAIL"></param>
    ''' <param name="_USERNAME"></param>
    ''' <param name="_MASTERUSERNAME"></param>
    ''' <param name="_PASSWORD"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddNonMasterAccount(ByVal _FIRSTNAME As String, ByVal _LASTNAME As String, ByVal _EMAIL As String, ByVal _USERNAME As String, ByVal _MASTERUSERNAME As String, ByVal _PASSWORD As String) As Boolean
        Dim bADDED As Boolean = False
        Dim sSQL As New StringBuilder
        Dim oRandom As New Random()

        Dim iActivationCode As Integer = oRandom.Next(1000000, 9999999)
        Dim sEncryptedPW As String = Kryptit(_PASSWORD, sEncryptionKey)
        Dim sPassExpire As String = Now().AddMonths(3).ToString("MM/dd/yyyy")

        sSQL.Append("INSERT INTO tUserInfo([FirstName], [LastName], [Email], [UserName], [Password], [PassExpire], [ActivationCode], [UserVerified], [CustomerID], [MasterAccount]) ")
        sSQL.Append("VALUES('" & _FIRSTNAME & "', '" & _LASTNAME & "', '" & _EMAIL & "', '" & _USERNAME & "', '" & sEncryptedPW & "', " & sPassExpire & ", " & iActivationCode & ",'N', '" & _MASTERUSERNAME & "','N'")

        bADDED = objDO.AddUpdateRecord(sSQL.ToString(), "user_db")

        Return bADDED

    End Function

    ''' <summary>
    ''' pull the user's email from the database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNotificationList() As String
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim sEmailList As New StringBuilder

        sSQL.Append("SELECT Email FROM tUserInfo ")
        sSQL.Append("WHERE NewAuctionEmail = 'Y' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")

        If ds.Tables(0).Rows.Count > 0 Then
            For Each dr In ds.Tables(0).Rows
                sEmailList.Append(dr("Email").ToString() & ",")
            Next
        End If

        If sEmailList.Length > 1 Then
            sEmailList.Length = sEmailList.Length - 1
        End If

        Return sEmailList.ToString()
    End Function

    Public Function GetUserEmail(ByVal _USERNAME As String) As String
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim sUSEREMAIL As String = String.Empty

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERNAME & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        sUSEREMAIL = objDO.GetSingleValue("Email", ds)

        Return sUSEREMAIL

    End Function

    Public Function GetNickName(ByVal _USERNAME As String) As String
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim sNickName As String = String.Empty

        sSQL.Append("SELECT * FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERNAME & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        sNickName = objDO.GetSingleValue("NickName", ds)

        Return sNickName
    End Function

    Public Function GetFullName(ByVal _USERNAME As String) As String
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim sFullName As String = String.Empty

        sSQL.Append("SELECT FirstName & ' ' & LastName AS FullName FROM tUserInfo ")
        sSQL.Append("WHERE UserName = '" & _USERNAME & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        sFullName = objDO.GetSingleValue("FullName", ds)

        Return sFullName
    End Function

    Public Function GetFullNameByEmail(ByVal _EMAIL As String) As String
        Dim sSQL As New StringBuilder
        Dim ds As New DataSet
        Dim sFullName As String = String.Empty

        sSQL.Append("SELECT FirstName & ' ' & LastName AS FullName FROM tUserInfo ")
        sSQL.Append("WHERE Email = '" & _EMAIL & "' ")

        ds = objDO.GetDataSet(sSQL.ToString(), "user_db")
        sFullName = objDO.GetSingleValue("FullName", ds)

        Return sFullName
    End Function


#End Region

#Region "  ENCRYPTION / DECRYPTION  "

    Public Function Kryptit(ByVal encode As String, ByVal kryptkey As String) As String
        Dim cntcode As Integer = 0
        Dim cntkey As Integer = 0
        Dim asced As String = ""
        Dim addkey As Boolean
        Dim coded As String = ""
        Dim sRetVal As String = ""

        If encode = "" Then
            Kryptit = ""
            Exit Function
        End If

        If kryptkey = "" Then
            cntcode = 1
            Do While cntcode <= 6
                Randomize()
                kryptkey = kryptkey + Chr(Int((255 - 1 + 1) * Rnd() + 1))
                cntcode = cntcode + 1
            Loop
            addkey = True
        End If

        cntcode = 1
        cntkey = 1

        Do While cntcode <= Len(encode)
            coded = coded + Chr(Asc(Mid(encode, cntcode, 1)) Xor Asc(Mid(kryptkey, cntkey, 1)))
            cntcode = cntcode + 1
            cntkey = cntkey + 1
            If cntkey > Len(kryptkey) Then
                cntkey = 1
            End If
        Loop

        cntcode = 1
        Do While cntcode <= Len(coded)
            asced = asced + Right("0" & Trim(Hex(Asc(Mid(coded, cntcode, 1)))), 2)
            cntcode = cntcode + 1
        Loop
        If addkey Then
            cntcode = 1
            Do While cntcode <= Len(kryptkey)
                asced = asced + Right("0" & Trim(Hex(Asc(Mid(kryptkey, cntcode, 1)))), 2)
                cntcode = cntcode + 1
            Loop
        End If

        cntcode = 1
        cntkey = Len(asced)

        Do While cntcode <= cntkey
            sRetVal = sRetVal + Right(asced, 1)
            asced = Mid(asced, 1, Len(asced) - 1)
            sRetVal = sRetVal + Left(asced, 1)
            asced = Mid(asced, 2)
            cntcode = cntcode + 2
        Loop
        sRetVal = Mid(sRetVal, Len(sRetVal) / 2 + 1) + Mid(sRetVal, 1, Len(sRetVal) / 2)
        sRetVal = StrReverse(sRetVal)

        Return Checksum(sRetVal, True)

    End Function

    Public Function DKryptit(ByVal decode As String, ByVal kryptkey As String) As String
        Dim cntcode As Integer = 0
        Dim cntkey As Integer = 0
        Dim asckey As String = ""
        Dim decoded As String = ""
        Dim deasced As String = ""
        Dim demix As String = ""

        If decode = "" Then
            DKryptit = ""
            Exit Function
        End If
        If Checksum(decode, False) = "1" Then
            decode = Left(decode, Len(decode) - 1)
        Else
            DKryptit = ""
            Exit Function
        End If

        decode = StrReverse(decode)
        decode = Mid(decode, Len(decode) / 2 + 1) + Mid(decode, 1, Len(decode) / 2)
        demix = decode
        decode = ""
        cntcode = 1
        cntkey = Len(demix)

        Do While cntcode <= cntkey
            decode = Right(demix, 1) & decode
            demix = Mid(demix, 1, Len(demix) - 1)
            decode = decode & Right(demix, 1)
            demix = Mid(demix, 1, Len(demix) - 1)
            cntcode = cntcode + 2
        Loop
        If kryptkey = "" Then
            asckey = Right(decode, 12)
            cntcode = 1
            Do While cntcode <= Len(asckey)
                kryptkey = kryptkey + Chr(CByte("&H" + Mid(asckey, cntcode, 2)))
                cntcode = cntcode + 2
            Loop
            decode = Left(decode, Len(decode) - 12)
        End If

        cntcode = 1
        Do While cntcode <= Len(decode)
            deasced = deasced + Chr(CByte("&H" + Mid(decode, cntcode, 2)))
            cntcode = cntcode + 2
        Loop

        cntcode = 1
        cntkey = 1

        Do While cntcode <= Len(deasced)
            decoded = decoded + Chr(Asc(Mid(deasced, cntcode, 1)) Xor Asc(Mid(kryptkey, cntkey, 1)))
            cntcode = cntcode + 1
            cntkey = cntkey + 1
            If cntkey > Len(kryptkey) Then
                cntkey = 1
            End If
        Loop
        Return decoded
    End Function

    Private Function Checksum(ByVal cNumber As String, ByVal bReturnType As String) As String
        Dim sRetVal As String = ""
        Dim cTemp As String = ""
        Dim iCounter As Integer = 0
        Dim iCountFrom As Integer = 0
        Dim iTemp As Integer = 0

        cNumber = cNumber.Trim()

        For iCounter = 1 To Len(cNumber) '---[ Pick only numbers ]---
            If InStr("0123456789", Mid(cNumber, iCounter, 1)) Then
                cTemp = cTemp + Mid(cNumber, iCounter, 1)
            Else
                cTemp = cTemp + CStr(Asc(Mid(cNumber, iCounter, 1)))
            End If
        Next

        If bReturnType = False Then '---[ Check last digit ]---
            iCountFrom = Len(cTemp) - 1
        Else
            iCountFrom = Len(cTemp) '---[ Make last digit ]---
        End If

        For iCounter = iCountFrom To 1 Step -1
            If Int(iCounter / 2) = iCounter / 2 Then
                iTemp = iTemp + CInt(Mid(cTemp, iCounter, 1))
            Else
                If CInt(Mid(cTemp, iCounter, 1)) * 2 > 9 Then
                    iTemp = iTemp + CInt(Mid(cTemp, iCounter, 1)) * 2 - 9
                Else
                    iTemp = iTemp + CInt(Mid(cTemp, iCounter, 1)) * 2
                End If
            End If
        Next

        If bReturnType = False Then
            If Right(cTemp, 1) = Right(Trim(CStr(10 - iTemp Mod 10)), 1) Then
                sRetVal = "1" '---[ Return OK, last digit is correct ]---
            Else
                sRetVal = "0" '---[ Return not OK, last digit is incorrect ]---
            End If
        Else '---[ Return the input string with an added last digit ]---
            sRetVal = cNumber & Right(Trim(CStr(10 - iTemp Mod 10)), 1).ToString
        End If
        Checksum = sRetVal
    End Function

#End Region

#Region "  LOGGING / SESSION CHECKING  "

    Public Sub SetSessions(ByVal _DATASET As DataSet)
        '---[ initialize control sessions ]---
        Session.Add("loggedin", "y")

        '---[ Set User Name Session ]---
        Dim sUserName As String = objDO.GetSingleValue("UserName", _DATASET)
        Session("UserName") = sUserName

        '---[ Set CustomerID Session.. which may be different from the UserName]---
        Dim sCustomerID As String = objDO.GetSingleValue("CustomerID", _DATASET)
        Session.Add("CustomerID", sCustomerID)

        '---[ Set Nickname Session.. used to mask a bidder's identity ]---
        Dim sNickname As String = objDO.GetSingleValue("NickName", _DATASET)
        Session.Add("Nickname", sNickname)

        '---[ Set User Name Session ]---
        Dim sFullName As String = String.Empty
        Dim sFName As String = String.Empty
        Dim sLName As String = String.Empty
        sFName = objDO.GetSingleValue("FirstName", _DATASET)
        sLName = objDO.GetSingleValue("LastName", _DATASET)
        sFullName = String.Format("{0} {1}", sFName, sLName)
        Session.Add("FullName", sFullName)

        '---[ Check User Admin Status ]--- 
        Dim sAdminUser As String = System.Configuration.ConfigurationManager.AppSettings("AdminUser")
        If InStr(sAdminUser, sUserName) Then
            Session("AdminUser") = True
        Else
            Session("AdminUser") = False
        End If

    End Sub

    Public Sub SessionCheck()
        '---[ grab the current page so we can redirect it if the session is expired ]---
        pCheckSession(CType(HttpContext.Current.Handler, Page))
    End Sub

    Private Sub pCheckSession(ByVal PAGE_OBJECT As Page)
        Try
            '---[ if session is expired ]---
            If Session("loggedin") = "n" Or Session("loggedin") = "" Or IsNothing(Session("loggedin")) Then
                PAGE_OBJECT.Response.Redirect("~/login.aspx?error=sessionexpired", True)
            ElseIf Session("loggedin") = "y" AndAlso Session("reset") = "y" Then
                PAGE_OBJECT.Response.Redirect("~/User/ChangePassword.aspx?reason=reset", True)
            End If
        Catch Ex As Exception
            'ignore the error
        End Try
    End Sub

#End Region

End Class
