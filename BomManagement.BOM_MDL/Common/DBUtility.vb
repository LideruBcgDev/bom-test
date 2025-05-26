Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data

Namespace Common
    Public Class DBUtility
        Private Shared _connectionString As String

        Shared Sub New()
            ' 接続文字列は後で設定ファイルから読み込む
            _connectionString = "Data Source=localhost;Initial Catalog=BomManagementDB;Integrated Security=True;TrustServerCertificate=True"
        End Sub

        ''' <summary>
        ''' データベース接続を取得する
        ''' </summary>
        Public Shared Function GetConnection() As SqlConnection
            Return New SqlConnection(_connectionString)
        End Function

        ''' <summary>
        ''' SQLコマンドを実行する（SELECT用）
        ''' </summary>
        Public Shared Function ExecuteReader(sql As String, params As Dictionary(Of String, Object)) As DataTable
            Dim dt As New DataTable()

            Using conn = GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    If params IsNot Nothing Then
                        For Each param In params
                            cmd.Parameters.AddWithValue(param.Key, If(param.Value, DBNull.Value))
                        Next
                    End If

                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        dt.Load(reader)
                    End Using
                End Using
            End Using

            Return dt
        End Function

        ''' <summary>
        ''' SQLコマンドを実行する（INSERT/UPDATE/DELETE用）
        ''' </summary>
        Public Shared Function ExecuteNonQuery(sql As String, params As Dictionary(Of String, Object)) As Integer
            Dim result As Integer = 0

            Using conn = GetConnection()
                Using cmd As New SqlCommand(sql, conn)
                    If params IsNot Nothing Then
                        For Each param In params
                            cmd.Parameters.AddWithValue(param.Key, If(param.Value, DBNull.Value))
                        Next
                    End If

                    conn.Open()
                    result = cmd.ExecuteNonQuery()
                End Using
            End Using

            Return result
        End Function

        ''' <summary>
        ''' トランザクション処理
        ''' </summary>
        Public Shared Function ExecuteTransaction(actions As List(Of Action(Of SqlConnection, SqlTransaction))) As Boolean
            Using conn = GetConnection()
                conn.Open()
                Using trans = conn.BeginTransaction()
                    Try
                        For Each action In actions
                            action(conn, trans)
                        Next
                        trans.Commit()
                        Return True
                    Catch ex As Exception
                        trans.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        End Function
    End Class
End Namespace
