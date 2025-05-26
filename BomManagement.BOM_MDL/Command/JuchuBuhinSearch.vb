Imports System.Data
Imports BomManagement.BOM_PRM

Public Class JuchuBuhinSearch
    Inherits BaseSearch(Of JuchuBuhinSearchParam, JuchuBuhinSearchResult)

    Protected Overrides Function CreateDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("JuchuCode")
        dt.Columns.Add("JuchuName")
        dt.Columns.Add("OrderDate", GetType(DateTime))
        dt.Columns.Add("DeliveryDate", GetType(DateTime))
        dt.Rows.Add("J001", "受注部品表1", DateTime.Now, DateTime.Now.AddDays(7))
        dt.Rows.Add("J002", "受注部品表2", DateTime.Now, DateTime.Now.AddDays(14))
        Return dt
    End Function

    Protected Overrides Function GetFilterColumn() As String
        Return "JuchuCode"
    End Function

    Protected Overrides Function GetFilterValue(param As JuchuBuhinSearchParam) As String
        Return param.JuchuCode
    End Function

    Protected Overrides Function CreateResult(dt As DataTable) As JuchuBuhinSearchResult
        Return New JuchuBuhinSearchResult With {.ResultTable = dt}
    End Function
End Class 