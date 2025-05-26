Imports System.Data
Imports BomManagement.BOM_PRM
Imports BomManagement.FW_APP

Public Class FormHinmokuEdit
    Inherits Form

    ' コントロールの定義
    Private WithEvents txtHinmokuCode As New TextBox() With {.Left = 120, .Top = 20, .Width = 200}
    Private WithEvents txtHinmokuName As New TextBox() With {.Left = 120, .Top = 60, .Width = 200}
    Private WithEvents txtUnit As New TextBox() With {.Left = 120, .Top = 100, .Width = 200}
    Private WithEvents txtPrice As New TextBox() With {.Left = 120, .Top = 140, .Width = 200}
    Private WithEvents btnSave As New Button() With {.Text = "登録", .Left = 120, .Top = 200, .Width = 80}
    Private WithEvents btnCancel As New Button() With {.Text = "キャンセル", .Left = 220, .Top = 200, .Width = 80}

    ' ラベルの定義
    Private lblHinmokuCode As New Label() With {.Text = "品目コード：", .Left = 20, .Top = 23}
    Private lblHinmokuName As New Label() With {.Text = "品目名：", .Left = 20, .Top = 63}
    Private lblUnit As New Label() With {.Text = "単位：", .Left = 20, .Top = 103}
    Private lblPrice As New Label() With {.Text = "単価：", .Left = 20, .Top = 143}

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.Text = "品目登録"
        Me.Width = 400
        Me.Height = 300
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent

        ' コントロールの追加
        Me.Controls.AddRange({
            lblHinmokuCode, txtHinmokuCode,
            lblHinmokuName, txtHinmokuName,
            lblUnit, txtUnit,
            lblPrice, txtPrice,
            btnSave, btnCancel
        })
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            ' 入力値の検証
            If String.IsNullOrEmpty(txtHinmokuCode.Text) Then
                MessageBox.Show("品目コードを入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If String.IsNullOrEmpty(txtHinmokuName.Text) Then
                MessageBox.Show("品目名を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' 登録パラメータの作成
            Dim param As New HinmokuEditParam()
            param.HinmokuCode = txtHinmokuCode.Text
            param.HinmokuName = txtHinmokuName.Text
            param.Unit = txtUnit.Text
            param.Price = If(String.IsNullOrEmpty(txtPrice.Text), 0, Decimal.Parse(txtPrice.Text))

            ' Web APIを使用して登録処理を実行
            Dim result = Await HttpClientWrapper.PostAsync(Of Object)("hinmokuapi/edit", param)

            MessageBox.Show("登録が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show($"登録中にエラーが発生しました。{vbCrLf}{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class