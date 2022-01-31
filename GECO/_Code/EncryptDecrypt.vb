Imports System.Convert
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.Encoding

<DebuggerStepThrough()>
Public Module EncryptDecrypt

    Private Const _key As String = "&%#@?,:*"
    Private ReadOnly IV As Byte() = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

    ' Encrypt the text
    Public Function EncryptText(ByVal strText As String) As String
        Return Encrypt(strText, _key)
    End Function

    'Decrypt the text
    Public Function DecryptText(ByVal strText As String) As String
        Return Decrypt(strText, _key)
    End Function

    'The function used to encrypt the text
    <CodeAnalysis.SuppressMessage("Critical Vulnerability", "S5547:Cipher algorithms should be robust",
                                  Justification:="Algorithm is only used to obscure cookie data; no sensitive info is involved.")>
    Private Function Encrypt(strText As String, strEncrKey As String) As String
        Dim byKey = UTF8.GetBytes(Left(strEncrKey, 8)),
            inputByteArray = UTF8.GetBytes(strText),
            ms As New MemoryStream(),
            des As New DESCryptoServiceProvider(),
            cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)

        Try
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()

            Return ToBase64String(ms.ToArray())
        Finally
            If cs IsNot Nothing Then cs.Dispose()
            If des IsNot Nothing Then des.Dispose()
        End Try

    End Function

    'The function used to decrypt the text
    <CodeAnalysis.SuppressMessage("Critical Vulnerability", "S5547:Cipher algorithms should be robust",
                                  Justification:="Algorithm is only used to obscure cookie data; no sensitive info is involved.")>
    Private Function Decrypt(strText As String, sDecrKey As String) As String

        Dim byKey = UTF8.GetBytes(Left(sDecrKey, 8)),
            inputByteArray = FromBase64String(strText),
            ms As New MemoryStream(),
            DES As New DESCryptoServiceProvider(),
            cs As New CryptoStream(ms, DES.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)

        Try
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()

            Return UTF8.GetString(ms.ToArray())
        Finally
            If cs IsNot Nothing Then cs.Dispose()
            If DES IsNot Nothing Then DES.Dispose()
        End Try

    End Function

End Module
