Imports System.Data
Imports BomManagement.BOM_PRM
Imports BomManagement.FW_WEB

Public Class SekkeiBuhinSearch
    Inherits CommandBase(Of SekkeiBuhinSearchParam, SekkeiBuhinSearchResult)
    Protected Overrides Function ExecuteCore(param As SekkeiBuhinSearchParam) As SekkeiBuhinSearchResult
        Dim dt As New DataTable()
        dt.Columns.Add("SekkeiCode")
        dt.Columns.Add("SekkeiName")
        dt.Columns.Add("UpdateDate", GetType(DateTime))
        dt.Rows.Add("S001", "設計部品表1", DateTime.Now)
        dt.Rows.Add("S002", "設計部品表2", DateTime.Now)

        '' 検索条件の適用
        'If Not String.IsNullOrEmpty(param.HinmokuCode) Then
        '    dt.DefaultView.RowFilter = $"HinmokuCode LIKE '%{param.HinmokuCode}%'"
        'End If

        ' 結果の作成
        Dim result As New SekkeiBuhinSearchResult()
        result.ResultTable = dt.DefaultView.ToTable()
        result.Success = True

        Return result
    End Function

End Class 