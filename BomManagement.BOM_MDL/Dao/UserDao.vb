Imports BomManagement.BOM_PRM
Imports BomManagement.BOM_MDL.Common
Imports System.Data

Namespace Dao
    Public Class UserDao
        ''' <summary>
        ''' ユーザー情報を取得する
        ''' </summary>
        Public Function GetUser(userId As String) As UserInfo
            Dim sql As String = "SELECT UserId, UserName, Email, Department, IsActive, CreatedDate, UpdatedDate " &
                                "FROM M_User WHERE UserId = @UserId"

            Dim params As New Dictionary(Of String, Object) From {
                {"@UserId", userId}
            }

            Dim dt = DBUtility.ExecuteReader(sql, params)

            If dt.Rows.Count = 0 Then
                Return Nothing
            End If

            Dim row = dt.Rows(0)
            Return New UserInfo() With {
                .userId = row("UserId").ToString(),
                .UserName = row("UserName").ToString(),
                .Email = row("Email").ToString(),
                .Department = row("Department").ToString(),
                .IsActive = CBool(row("IsActive")),
                .CreatedDate = CDate(row("CreatedDate")),
                .UpdatedDate = If(IsDBNull(row("UpdatedDate")), Nothing, CDate(row("UpdatedDate")))
            }
        End Function

        ''' <summary>
        ''' ユーザー一覧を検索する
        ''' </summary>
        Public Function SearchUsers(searchParam As UserSearchParam) As List(Of UserInfo)
            Dim sql As String = "SELECT UserId, UserName, Email, Department, IsActive, CreatedDate, UpdatedDate " &
                                "FROM M_User WHERE 1=1"

            Dim params As New Dictionary(Of String, Object)

            If Not String.IsNullOrEmpty(searchParam.UserId) Then
                sql &= " AND UserId LIKE @UserId"
                params.Add("@UserId", "%" & searchParam.UserId & "%")
            End If

            If Not String.IsNullOrEmpty(searchParam.UserName) Then
                sql &= " AND UserName LIKE @UserName"
                params.Add("@UserName", "%" & searchParam.UserName & "%")
            End If

            If Not String.IsNullOrEmpty(searchParam.Department) Then
                sql &= " AND Department = @Department"
                params.Add("@Department", searchParam.Department)
            End If

            sql &= " ORDER BY UserId"

            Dim dt = DBUtility.ExecuteReader(sql, params)
            Dim users As New List(Of UserInfo)

            For Each row As DataRow In dt.Rows
                users.Add(New UserInfo() With {
                    .UserId = row("UserId").ToString(),
                    .UserName = row("UserName").ToString(),
                    .Email = row("Email").ToString(),
                    .Department = row("Department").ToString(),
                    .IsActive = CBool(row("IsActive")),
                    .CreatedDate = CDate(row("CreatedDate")),
                    .UpdatedDate = If(IsDBNull(row("UpdatedDate")), Nothing, CDate(row("UpdatedDate")))
                })
            Next

            Return users
        End Function

        ''' <summary>
        ''' ユーザー情報を登録する
        ''' </summary>
        Public Function InsertUser(user As UserInfo) As Boolean
            Dim sql As String = "INSERT INTO M_User (UserId, UserName, Email, Department, IsActive, CreatedDate) " &
                                "VALUES (@UserId, @UserName, @Email, @Department, @IsActive, GETDATE())"

            Dim params As New Dictionary(Of String, Object) From {
                {"@UserId", user.UserId},
                {"@UserName", user.UserName},
                {"@Email", user.Email},
                {"@Department", user.Department},
                {"@IsActive", user.IsActive}
            }

            Return DBUtility.ExecuteNonQuery(sql, params) > 0
        End Function

        ''' <summary>
        ''' ユーザー情報を更新する
        ''' </summary>
        Public Function UpdateUser(user As UserInfo) As Boolean
            Dim sql As String = "UPDATE M_User SET " &
                                "UserName = @UserName, " &
                                "Email = @Email, " &
                                "Department = @Department, " &
                                "IsActive = @IsActive, " &
                                "UpdatedDate = GETDATE() " &
                                "WHERE UserId = @UserId"

            Dim params As New Dictionary(Of String, Object) From {
                {"@UserId", user.UserId},
                {"@UserName", user.UserName},
                {"@Email", user.Email},
                {"@Department", user.Department},
                {"@IsActive", user.IsActive}
            }

            Return DBUtility.ExecuteNonQuery(sql, params) > 0
        End Function

        ''' <summary>
        ''' ログイン認証
        ''' </summary>
        Public Function Authenticate(userId As String, password As String) As UserInfo
            ' 実際はパスワードのハッシュ化が必要
            Dim sql As String = "SELECT UserId, UserName, Email, Department, IsActive, CreatedDate, UpdatedDate " &
                                "FROM M_User WHERE UserId = @UserId AND Password = @Password AND IsActive = 1"

            Dim params As New Dictionary(Of String, Object) From {
                {"@UserId", userId},
                {"@Password", password} ' 実際はハッシュ化したパスワードと比較
            }

            Dim dt = DBUtility.ExecuteReader(sql, params)

            If dt.Rows.Count = 0 Then
                Return Nothing
            End If

            Dim row = dt.Rows(0)
            Return New UserInfo() With {
                .userId = row("UserId").ToString(),
                .UserName = row("UserName").ToString(),
                .Email = row("Email").ToString(),
                .Department = row("Department").ToString(),
                .IsActive = CBool(row("IsActive")),
                .CreatedDate = CDate(row("CreatedDate")),
                .UpdatedDate = If(IsDBNull(row("UpdatedDate")), Nothing, CDate(row("UpdatedDate")))
            }
        End Function
    End Class
End Namespace
