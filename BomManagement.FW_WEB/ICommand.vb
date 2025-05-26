'Imports BomManagement.BOM_PRM

Public Interface ICommand(Of TIn As IParamBase, TOut As OParamBase)
    ReadOnly Property IParam As TIn
    ReadOnly Property OParam As TOut
    ReadOnly Property ResponseType As String
    Function Execute(param As TIn) As TOut
    Function Validate(param As TIn) As Boolean
End Interface