
Imports System.Data
Imports BomManagement.FW_WEB
Public Class JuchuBuhinSearchParam
    Inherits IParamBase
    Public Property JuchuCode As String
End Class

Public Class JuchuBuhinSearchResult
    Inherits OParamBase

    Public Property ResultTable As DataTable
End Class

