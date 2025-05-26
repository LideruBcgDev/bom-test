Imports BomManagement.FW_WEB
Imports BomManagement.BOM_PRM

Public Class BOMCommandInitializer
    Implements ICommandInitializer

    Public Sub Initialize() Implements ICommandInitializer.Initialize
        Dim factory = CommandFactory.GetInstance()

        ' BOMドメインのコマンド登録
        factory.RegisterCommand(Of HinmokuSearch)(ApiPathConstants.HINMOKU_SEARCH)
        factory.RegisterCommand(Of HinmokuEdit)(ApiPathConstants.HINMOKU_EDIT)
        ' 必要に応じて他のBOMコマンドも登録
    End Sub
End Class 