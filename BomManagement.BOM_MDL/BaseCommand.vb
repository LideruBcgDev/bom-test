Imports BomManagement.PRM

Public MustInherit Class BaseCommand(Of TIn As IParamBase, TOut As OParamBase)
    Inherits CommandBase(Of TIn, TOut)

    Protected Overrides Function Execute(param As TIn) As TOut
        Try
            ' 前処理
            PreExecute(param)

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