Imports System
Imports System.Collections.Generic
'Imports BomManagement.BOM_PRM

Public Class CommandFactory
    Private Shared _instance As CommandFactory
    Private _commandMap As New Dictionary(Of String, Type)

    Private Sub New()
        ' コマンドの登録
        'RegisterCommand("hinmokuapi/search", GetType(HinmokuSearch))
        'RegisterCommand("hinmokuapi/edit", GetType(HinmokuEdit))
    End Sub

    Public Shared Function GetInstance() As CommandFactory
        If _instance Is Nothing Then
            _instance = New CommandFactory()
        End If
        Return _instance
    End Function

    Private Sub RegisterCommand(commandPath As String, commandType As Type)
        ' CommandBaseを継承しているかチェック
        If Not IsCommandType(commandType) Then
            Throw New ArgumentException($"Type {commandType.Name} must inherit from CommandBase")
        End If
        _commandMap(commandPath) = commandType
    End Sub

    Private Function IsCommandType(type As Type) As Boolean
        ' CommandBaseを継承しているかチェック
        While type IsNot Nothing
            If type.IsGenericType AndAlso type.GetGenericTypeDefinition() = GetType(CommandBase(Of,)) Then
                Return True
            End If
            type = type.BaseType
        End While
        Return False
    End Function

    Public Function CreateCommand(commandPath As String) As Object
        If Not _commandMap.ContainsKey(commandPath) Then
            Throw New ArgumentException($"Command not found: {commandPath}")
        End If

        Return Activator.CreateInstance(_commandMap(commandPath))
    End Function
End Class 