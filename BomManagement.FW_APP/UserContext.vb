Imports System.Security.Claims

Public Class UserContext
    Private Shared _currentUser As ClaimsPrincipal

    Public Shared Property CurrentUser As ClaimsPrincipal
        Get
            Return _currentUser
        End Get
        Private Set(value As ClaimsPrincipal)
            _currentUser = value
        End Set
    End Property

    Public Shared Sub SetCurrentUser(user As ClaimsPrincipal)
        _currentUser = user
    End Sub

    Public Shared Function GetUserId() As String
        Return If(_currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value, String.Empty)
    End Function

    Public Shared Function GetUserName() As String
        Return If(_currentUser?.FindFirst(ClaimTypes.Name)?.Value, String.Empty)
    End Function

    Public Shared Function GetUserEmail() As String
        Return If(_currentUser?.FindFirst(ClaimTypes.Email)?.Value, String.Empty)
    End Function

    Public Shared Function IsAuthenticated() As Boolean
        Return _currentUser IsNot Nothing AndAlso _currentUser.Identity?.IsAuthenticated = True
    End Function
End Class 