Imports System.Data
Imports BomManagement.BOM_PRM

Public Class MitsumoriBuhinSearch
    Inherits BaseSearch(Of MitsumoriBuhinSearchParam, MitsumoriBuhinSearchResult)

    Protected Overrides Function CreateDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("MitsumoriCode")
        dt.Columns.Add("MitsumoriName")
        dt.Columns.Add("Price", GetType(Decimal))
        dt.Columns.Add("UpdateDate", GetType(DateTime))
        dt.Rows.Add("M001", "見積部品表1", 10000, DateTime.Now)
        dt.Rows.Add("M002", "見積部品表2", 20000, DateTime.Now)
        Return dt
    End Function

    Protected Overrides Function GetFilterColumn() As String
        Return "MitsumoriCode"
    End Function

    Protected Overrides Function GetFilterValue(param As MitsumoriBuhinSearchParam) As String
        Return param.MitsumoriCode
    End Function

    Protected Overrides Function CreateResult(dt As DataTable) As MitsumoriBuhinSearchResult
        Return New MitsumoriBuhinSearchResult With {.ResultTable = dt}
    End Function
End Class 