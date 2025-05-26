Imports System.Windows.Forms
Imports BomManagement.BOM_PRM
Imports BomManagement.FW_APP

Public Class FormHinmokuSearch
    Inherits Form

    Private label As New Label() With {.Text = "品目名", .Left = 20, .Top = 20, .AutoSize = True}
    Private txtHinmoku As New TextBox() With {.Left = 80, .Top = 18, .Width = 200}
    Private btnSearch As New Button() With {.Text = "検索", .Left = 300, .Top = 16, .Width = 80}
    Private btnClear As New Button() With {.Text = "クリア", .Left = 390, .Top = 16, .Width = 80}
    Private btnAdd As New Button() With {.Text = "追加", .Left = 290, .Top = 20, .Width = 80}
    Private dgvResult As New DataGridView() With {.Left = 20, .Top = 60, .Width = 500, .Height = 250, .ReadOnly = True, .AllowUserToAddRows = False}
    'Private _hinmokuSearch As HinmokuSearch

    Public Sub New()
        InitializeComponent()
        '_hinmokuSearch = New HinmokuSearch()
    End Sub

    'Private Sub FormHinmokuSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    ' 初期表示時は全件表示
    '    Dim param As New HinmokuSearchParam()
    '    Dim result = _hinmokuSearch.Search(param)
    '    dgvResult.DataSource = result.ResultTable
    'End Sub
    Sub InitializeComponent()
        Me.Text = "品目検索"
        Me.Width = 560
        Me.Height = 380
        Me.Controls.AddRange({label, txtHinmoku, btnSearch, btnClear, btnAdd, dgvResult})
        AddHandler btnSearch.Click, AddressOf BtnSearch_Click
        AddHandler btnClear.Click, AddressOf btnClear_Click
        AddHandler btnAdd.Click, AddressOf btnAdd_Click
    End Sub

    Private Async Sub BtnSearch_Click(sender As Object, e As EventArgs)
        Dim param As New HinmokuSearchParam With {.HinmokuCode = txtHinmoku.Text}
        Try
            Dim result = Await HttpClientWrapper.PostAsync(Of HinmokuSearchResult)("hinmokuapi/search", param)
            dgvResult.DataSource = result.ResultTable
        Catch ex As Exception
            MessageBox.Show("検索に失敗しました: " & ex.Message)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ' 検索条件のクリア
        txtHinmoku.Text = ""
        dgvResult.DataSource = Nothing
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        ' 品目登録画面を表示
        Using form As New FormHinmokuEdit()
            form.ShowDialog()
        End Using
    End Sub
End Class 