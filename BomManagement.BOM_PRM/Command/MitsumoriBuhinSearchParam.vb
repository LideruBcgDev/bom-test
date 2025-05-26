Imports System.Data
Imports BomManagement.FW_WEB

Public Class MitsumoriBuhinSearchParam
    Inherits IParamBase
    Public Property MitsumoriCode As String
End Class

Public Class MitsumoriBuhinSearchResult
    Inherits OParamBase
    Public Property ResultTable As DataTable
End Class

