Imports System.Windows.Forms
Imports System.Data
Imports BomManagement.FW_APP
Imports BomManagement.BOM_PRM
Imports BomManagement.FW_WIN

Public Class FormSekkeiBuhinSearch
    Inherits Form

    Private label As New Label() With {.Text = "設計番号", .Left = 20, .Top = 20, .AutoSize = True}
    Private txtSekkei As New TextBox() With {.Left = 80, .Top = 18, .Width = 200}
    Private btnSearch As New Button() With {.Text = "検索", .Left = 300, .Top = 16, .Width = 80}
    Private btnClear As New Button() With {.Text = "クリア", .Left = 390, .Top = 16, .Width = 80}
    Private dgvResult As New DataGridView() With {.Left = 20, .Top = 60, .Width = 500, .Height = 250, .ReadOnly = True, .AllowUserToAddRows = False}

    Public Sub New()
        Me.Text = "設計部品表検索"
        Me.Width = 560
        Me.Height = 380
        Me.Controls.AddRange({label, txtSekkei, btnSearch, btnClear, dgvResult})
        AddHandler btnSearch.Click, AddressOf BtnSearch_Click
        AddHandler btnClear.Click, AddressOf btnClear_Click
    End Sub

    Private Sub FormSekkeiBuhinSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 初期表示時の処理
        InitializeDataGridView()
    End Sub

    Private Sub InitializeDataGridView()
        ' DataGridViewの初期設定
        With dgvResult
            '.AutoGenerateColumns = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
        End With
    End Sub

    Private Async Sub BtnSearch_Click(sender As Object, e As EventArgs)
        Dim param As New SekkeiBuhinSearchParam With {.SekkeiCode = txtSekkei.Text}
        Try
            Dim result = Await HttpClientWrapper.PostAsync(Of HinmokuSearchResult)("sekkeibuhinapi/search", param)
            dgvResult.DataSource = result.ResultTable
        Catch ex As Exception
            MessageBox.Show("検索に失敗しました: " & ex.Message)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ' 検索条件のクリア
        txtSekkei.Text = ""
        dgvResult.DataSource = Nothing
    End Sub
End Class 