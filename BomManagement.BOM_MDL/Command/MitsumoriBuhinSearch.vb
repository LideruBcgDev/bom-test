Imports System.Data
Imports BomManagement.BOM_PRM
Imports BomManagement.FW_WEB

Public Class MitsumoriBuhinSearch
    Inherits CommandBase(Of MitsumoriBuhinSearchParam, MitsumoriBuhinSearchResult)

    Protected Overrides Function ExecuteCore(param As MitsumoriBuhinSearchParam) As MitsumoriBuhinSearchResult
        Dim dt As New DataTable()
        dt.Columns.Add("MitsumoriCode")
        dt.Columns.Add("MitsumoriName")
        dt.Columns.Add("Price", GetType(Decimal))
        dt.Columns.Add("UpdateDate", GetType(DateTime))
        dt.Rows.Add("M001", "見積部品表1", 10000, DateTime.Now)
        dt.Rows.Add("M002", "見積部品表2", 20000, DateTime.Now)

        '' 検索条件の適用
        'If Not String.IsNullOrEmpty(param.HinmokuCode) Then
        '    dt.DefaultView.RowFilter = $"HinmokuCode LIKE '%{param.HinmokuCode}%'"
        'End If

        ' 結果の作成
        Dim result As New MitsumoriBuhinSearchResult()
        result.ResultTable = dt.DefaultView.ToTable()
        result.Success = True

        Return result
    End Function

End Class 