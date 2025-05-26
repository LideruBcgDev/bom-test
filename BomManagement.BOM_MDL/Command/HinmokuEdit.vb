Imports System.Data
Imports BomManagement.BOM_PRM
Imports BomManagement.FW_WEB

Public Class HinmokuEdit
    Inherits CommandBase(Of HinmokuEditParam, OParamBase)

    Protected Overrides Function ExecuteCore(param As HinmokuEditParam) As OParamBase
        Try
            ' TODO: 実際のデータベース登録処理を実装
            ' 現在はダミーデータとして、登録内容をコンソールに出力
            Console.WriteLine($"品目コード: {param.HinmokuCode}")
            Console.WriteLine($"品目名: {param.HinmokuName}")
            Console.WriteLine($"単位: {param.Unit}")
            Console.WriteLine($"単価: {param.Price}")

            Return New OParamBase With {
                .Success = True,
                .Message = "登録が完了しました。"
            }
        Catch ex As Exception
            Return New OParamBase With {
                .Success = False,
                .Message = ex.Message
            }
        End Try
    End Function
End Class