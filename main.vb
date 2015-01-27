'Licensed under MIT.
Imports System.Net
Module Module1

    Sub Main()
        Dim keywords, sites As String
        Dim response As String
        Dim IsHijacked As Boolean = False
        Using file As New System.IO.StreamReader("keywords.txt")
            keywords = file.ReadToEnd().Replace(vbCrLf, ";").Replace(",", ";")
            file.Close() : End Using
        Using file As New System.IO.StreamReader("websites.txt")
            sites = file.ReadToEnd().Replace(vbCrLf, ";").Replace(",", ";")
            file.Close() : End Using
        Dim currentsite As String
        Do Until sites = ""
            currentsite = Mid(sites, 1, InStr(sites, ";") - 1)
            response = GetStringFromUrl(currentsite)
            If Mid(response, 1, 6) <> "ERROR:" Then
                If IfMatchw(response, keywords, True) Then
                    Console.WriteLine(currentsite + " is hijacked.")
                    IsHijacked = True
                End If
            Else
                Console.WriteLine("Failed to get contents from " + currentsite)
            End If
            sites = Mid(sites, InStr(sites, ";") + 1)
        Loop
        If IsHijacked = False Then
            Console.WriteLine("No hijack was found during this test.")
        End If
        Console.WriteLine("Press ENTER to exit.")
        Console.ReadLine()
    End Sub
    Function GetStringFromUrl(url As String) As String
        Using wc As New WebClient
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.91 Safari/537.36")
            Console.WriteLine("Testing:" + url)
            Try
                Return System.Text.Encoding.UTF8.GetString(wc.DownloadData(url))
            Catch ex As Exception
                Return ("ERROR:" + ex.Message)
            End Try
        End Using
    End Function
    Function IfMatchw(q As String, list As String, Optional CaseNotSensitive As Boolean = False) As Boolean
        If CaseNotSensitive = True Then
            q = UCase(q)
            list = UCase(list)
        End If
        Dim kw As String
        Do Until list = ""
            kw = Mid(list, 1, InStr(list, ";") - 1)
            If InStr(q, kw) > 0 Then Return True
            list = Mid(list, InStr(list, ";") + 1)
        Loop
        Return False
    End Function
End Module
