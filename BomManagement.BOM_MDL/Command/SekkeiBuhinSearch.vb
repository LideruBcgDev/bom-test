Imports System.Data
Imports BomManagement.BOM_PRM

Public Class SekkeiBuhinSearch
    Inherits BaseSearch(Of SekkeiBuhinSearchParam, SekkeiBuhinSearchResult)

    Protected Overrides Function CreateDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("SekkeiCode")
        dt.Columns.Add("SekkeiName")
        dt.Columns.Add("UpdateDate", GetType(DateTime))
        dt.Rows.Add("S001", "設計部品表1", DateTime.Now)
        dt.Rows.Add("S002", "設計部品表2", DateTime.Now)
        Return dt
    End Function

    Protected Overrides Function GetFilterColumn() As String
        Return "SekkeiCode"
    End Function

    Protected Overrides Function GetFilterValue(param As SekkeiBuhinSearchParam) As String
        Return param.SekkeiCode
    End Function

    Protected Overrides Function CreateResult(dt As DataTable) As SekkeiBuhinSearchResult
        Return New SekkeiBuhinSearchResult With {.ResultTable = dt}
    End Function
End Class 