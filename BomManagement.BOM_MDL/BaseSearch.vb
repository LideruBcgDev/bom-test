Imports System.Data
Imports BomManagement.BOM_PRM
Imports BomManagement.FW_WEB

Public MustInherit Class BaseSearch(Of TIn As IParamBase, TOut As OParamBase)
    Inherits CommandBase(Of TIn, TOut)


    Protected MustOverride Function CreateDataTable() As DataTable
    Protected MustOverride Function GetFilterColumn() As String
    Protected MustOverride Function GetFilterValue(param As TIn) As String

    Protected Overrides Function ExecuteCore(param As TIn) As TOut
        Dim dt = CreateDataTable()
        Dim filterValue = GetFilterValue(param)

        If Not String.IsNullOrEmpty(filterValue) Then
            ' 簡易フィルタ
            Dim rows = dt.Select($"{GetFilterColumn()} LIKE '%{filterValue}%'")
            Dim filtered = dt.Clone()
            For Each row In rows
                filtered.ImportRow(row)
            Next
            dt = filtered
        End If

        Return CreateResult(dt)
    End Function

    Protected MustOverride Function CreateResult(dt As DataTable) As TOut
End Class