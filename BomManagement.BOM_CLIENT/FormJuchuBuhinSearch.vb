Imports System.Windows.Forms
Imports System.Data
Imports BomManagement.FW_APP
Imports BomManagement.BOM_PRM

Public Class FormJuchuBuhinSearch
    Inherits Form

    Private label As New Label() With {.Text = "受注番号", .Left = 20, .Top = 20, .AutoSize = True}
    Private txtJuchu As New TextBox() With {.Left = 80, .Top = 18, .Width = 200}
    Private btnSearch As New Button() With {.Text = "検索", .Left = 300, .Top = 16, .Width = 80}
    Private btnClear As New Button() With {.Text = "クリア", .Left = 390, .Top = 16, .Width = 80}
    Private dgvResult As New DataGridView() With {.Left = 20, .Top = 60, .Width = 500, .Height = 250, .ReadOnly = True, .AllowUserToAddRows = False}

    Public Sub New()
        Me.Text = "受注部品表検索"
        Me.Width = 560
        Me.Height = 380
        Me.Controls.AddRange({label, txtJuchu, btnSearch, btnClear, dgvResult})
        AddHandler btnSearch.Click, AddressOf BtnSearch_Click
        AddHandler btnClear.Click, AddressOf BtnClear_Click
    End Sub

    Private Sub FormJuchuBuhinSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        Dim param As New JuchuBuhinSearchParam With {.JuchuCode = txtJuchu.Text}
        Try
            Dim result = Await HttpClientWrapper.PostAsync(Of JuchuBuhinSearchResult)("JuchuBuhinApi/search", param)
            dgvResult.DataSource = result.ResultTable
        Catch ex As Exception
            MessageBox.Show("検索処理でエラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs)
        ' 検索条件のクリア
        txtJuchu.Text = ""
        dgvResult.DataSource = Nothing
    End Sub
End Class 