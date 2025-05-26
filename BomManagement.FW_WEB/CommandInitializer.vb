'Imports BomManagement.BOM_MDL
' 将来的に他のドメインの参照を追加
' Imports BomManagement.PLM_MDL
' Imports BomManagement.DRAW_MDL
' Imports BomManagement.CAD_MDL

Public Class CommandInitializer
    Private Shared _initializers As New List(Of ICommandInitializer)

    Public Shared Sub RegisterInitializer(initializer As ICommandInitializer)
        _initializers.Add(initializer)
    End Sub

    Public Shared Sub Initialize()
        For Each initializer In _initializers
            initializer.Initialize()
        Next
    End Sub
End Class 