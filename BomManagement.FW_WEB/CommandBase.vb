'Imports BomManagement.BOM_PRM

Public MustInherit Class CommandBase(Of TIn As IParamBase, TOut As OParamBase)
    Implements ICommand(Of TIn, TOut)

    Public ReadOnly Property IParam As TIn Implements ICommand(Of TIn, TOut).IParam
        Get
            Return Activator.CreateInstance(Of TIn)()
        End Get
    End Property

    Public ReadOnly Property OParam As TOut Implements ICommand(Of TIn, TOut).OParam
        Get
            Return Activator.CreateInstance(Of TOut)()
        End Get
    End Property

    Public ReadOnly Property ResponseType As String Implements ICommand(Of TIn, TOut).ResponseType
        Get
            Return ""
        End Get
    End Property

    Public Function Execute(param As TIn) As TOut Implements ICommand(Of TIn, TOut).Execute
        Try
            ' 前処理
            PreExecute(param)

            ' バリデーション
            If Not Validate(param) Then
                Dim ret1 = Activator.CreateInstance(Of TOut)()
                ret1.Success = False
                ret1.Message = "パラメータの検証に失敗しました。"
                Return ret1
            End If

            ' メイン処理
            Dim result = ExecuteCore(param)

            ' 後処理
            PostExecute(param, result)

            Return result
        Catch ex As Exception
            ' エラー処理
            Return HandleError(ex)
        End Try
    End Function

    Public Function Validate(param As TIn) As Boolean Implements ICommand(Of TIn, TOut).Validate
        Return True ' デフォルトの実装
    End Function

    Protected MustOverride Function ExecuteCore(param As TIn) As TOut

    Protected Overridable Sub PreExecute(param As TIn)
        ' デフォルトの前処理（必要に応じてオーバーライド）
    End Sub

    Protected Overridable Sub PostExecute(param As TIn, result As TOut)
        ' デフォルトの後処理（必要に応じてオーバーライド）
    End Sub

    Protected Overridable Function HandleError(ex As Exception) As TOut
        ' デフォルトのエラー処理
        Dim result = Activator.CreateInstance(Of TOut)()
        result.Success = False
        result.Message = ex.Message
        Return result
    End Function
End Class 