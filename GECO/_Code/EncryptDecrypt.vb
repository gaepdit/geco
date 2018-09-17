Imports System.Convert
Imports System.Diagnostics
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.Encoding

<DebuggerStepThrough()>
Public Class EncryptDecrypt

    Private Shared _key As String = "&%#@?,:*"
    Private Shared IV As Byte() = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

    ' Encrypt the text
    Public Shared Function EncryptText(ByVal strText As String) As String
        Return Encrypt(strText, _key)
    End Function

    'Decrypt the text
    Public Shared Function DecryptText(ByVal strText As String) As String
        Return Decrypt(strText, _key)
    End Function

    'The function used to encrypt the text
    Private Shared Function Encrypt(strText As String, strEncrKey As String) As String
        Dim byKey = UTF8.GetBytes(Left(strEncrKey, 8))
        Dim inputByteArray = UTF8.GetBytes(strText)

        Using ms As New MemoryStream(),
            des As New DESCryptoServiceProvider(),
            cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)

            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()

            Return ToBase64String(ms.ToArray())

        End Using
    End Function

    'The function used to decrypt the text
    Private Shared Function Decrypt(strText As String, sDecrKey As String) As String
        Dim byKey = UTF8.GetBytes(Left(sDecrKey, 8))
        Dim inputByteArray = FromBase64String(strText)

        Using ms As New MemoryStream(),
            des As New DESCryptoServiceProvider(),
            cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)

            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()

            Return UTF8.GetString(ms.ToArray())

        End Using
    End Function
End Class
