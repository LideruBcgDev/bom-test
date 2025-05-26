Imports System.Data
Imports BomManagement.FW_WEB

Public Class SekkeiBuhinSearchParam
    Inherits IParamBase
    Public Property SekkeiCode As String
End Class

Public Class SekkeiBuhinSearchResult
    Inherits OParamBase
    Public Property ResultTable As DataTable
End Class

