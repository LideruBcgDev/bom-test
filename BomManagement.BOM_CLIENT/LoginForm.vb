Imports System.Windows.Forms

Public Class LoginForm
    Inherits Form

    Private userIdLabel As New Label() With {.Text = "ユーザーID", .Left = 20, .Top = 20, .AutoSize = True}
    Private userIdTextBox As New TextBox() With {.Left = 100, .Top = 20, .Width = 150}
    Private passwordLabel As New Label() With {.Text = "パスワード", .Left = 20, .Top = 60, .AutoSize = True}
    Private passwordTextBox As New TextBox() With {.Left = 100, .Top = 60, .Width = 150, .PasswordChar = "*"c}
    Private loginButton As New Button() With {.Text = "ログイン", .Left = 100, .Top = 100, .Width = 80}
    Private errorLabel As New Label() With {.Left = 20, .Top = 140, .ForeColor = Drawing.Color.Red, .AutoSize = True, .Visible = False}

    Public Sub New()
        Me.Text = "ログイン"
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.ClientSize = New Drawing.Size(280, 180)
        Me.Controls.AddRange({userIdLabel, userIdTextBox, passwordLabel, passwordTextBox, loginButton, errorLabel})
        AddHandler loginButton.Click, AddressOf LoginButton_Click
        AcceptButton = loginButton
    End Sub

    Private Sub LoginButton_Click(sender As Object, e As EventArgs)
        If userIdTextBox.Text = "admin" AndAlso passwordTextBox.Text = "admin" Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            errorLabel.Text = "ユーザーIDまたはパスワードが正しくありません。"
            errorLabel.Visible = True
        End If
    End Sub
End Class 