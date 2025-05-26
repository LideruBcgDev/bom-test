Imports System.Data
Imports BomManagement.BOM_PRM

Public Class HinmokuSearch
    Inherits BaseSearch(Of HinmokuSearchParam, HinmokuSearchResult)

    Protected Overrides Function Execute(param As HinmokuSearchParam) As HinmokuSearchResult
        ' データテーブルの作成
        Dim dt As New DataTable()
        dt.Columns.Add("HinmokuCode", GetType(String))
        dt.Columns.Add("HinmokuName", GetType(String))
        dt.Columns.Add("Unit", GetType(String))
        dt.Columns.Add("Price", GetType(Decimal))

        ' サンプルデータの追加
        dt.Rows.Add("H001", "部品A", "個", 1000)
        dt.Rows.Add("H002", "部品B", "個", 2000)
        dt.Rows.Add("H003", "部品C", "個", 3000)

        ' 検索条件の適用
        If Not String.IsNullOrEmpty(param.HinmokuCode) Then
            dt.DefaultView.RowFilter = $"HinmokuCode LIKE '%{param.HinmokuCode}%'"
        End If

        ' 結果の作成
        Dim result As New HinmokuSearchResult()
        result.ResultTable = dt.DefaultView.ToTable()
        result.Success = True

        Return result
    End Function
End Class
