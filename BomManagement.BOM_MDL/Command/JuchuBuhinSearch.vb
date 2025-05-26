Imports System.Data
Imports BomManagement.BOM_PRM
Imports BomManagement.FW_WEB

Public Class JuchuBuhinSearch
    Inherits CommandBase(Of JuchuBuhinSearchParam, JuchuBuhinSearchResult)

    Protected Overrides Function ExecuteCore(param As JuchuBuhinSearchParam) As JuchuBuhinSearchResult
        Dim dt As New DataTable()
        dt.Columns.Add("JuchuCode")
        dt.Columns.Add("JuchuName")
        dt.Columns.Add("OrderDate", GetType(DateTime))
        dt.Columns.Add("DeliveryDate", GetType(DateTime))
        dt.Rows.Add("J001", "受注部品表1", DateTime.Now, DateTime.Now.AddDays(7))
        dt.Rows.Add("J002", "受注部品表2", DateTime.Now, DateTime.Now.AddDays(14))

        '' 検索条件の適用
        'If Not String.IsNullOrEmpty(param.HinmokuCode) Then
        '    dt.DefaultView.RowFilter = $"HinmokuCode LIKE '%{param.HinmokuCode}%'"
        'End If

        ' 結果の作成
        Dim result As New JuchuBuhinSearchResult()
        result.ResultTable = dt.DefaultView.ToTable()
        result.Success = True

        Return result
    End Function

End Class 