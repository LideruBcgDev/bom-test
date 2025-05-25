Imports BomManagement.PRM
Imports BomManagement.MDL.Common
Imports System.Data.SqlClient
Imports System.Data

Namespace Dao
    Public Class BomDao
        ''' <summary>
        ''' 部品表ヘッダー情報を取得する
        ''' </summary>
        Public Function GetBomHeader(parentItemCode As String, revision As Integer) As BomHeader
            Dim sql As String = "SELECT ParentItemCode, Revision, EffectiveDate, ExpirationDate, " &
                                "Status, Description, CreatedDate, UpdatedDate " &
                                "FROM T_BomHeader WHERE ParentItemCode = @ParentItemCode AND Revision = @Revision"

            Dim params As New Dictionary(Of String, Object) From {
                {"@ParentItemCode", parentItemCode},
                {"@Revision", revision}
            }

            Dim dt = DBUtility.ExecuteReader(sql, params)

            If dt.Rows.Count = 0 Then
                Return Nothing
            End If

            Dim row = dt.Rows(0)
            Return New BomHeader() With {
                .ParentItemCode = row("ParentItemCode").ToString(),
                .Revision = CInt(row("Revision")),
                .EffectiveDate = CDate(row("EffectiveDate")),
                .ExpirationDate = If(IsDBNull(row("ExpirationDate")), Nothing, CDate(row("ExpirationDate"))),
                .Status = row("Status").ToString(),
                .Description = If(IsDBNull(row("Description")), "", row("Description").ToString()),
                .CreatedDate = CDate(row("CreatedDate")),
                .UpdatedDate = If(IsDBNull(row("UpdatedDate")), Nothing, CDate(row("UpdatedDate")))
            }
        End Function

        ''' <summary>
        ''' 部品表明細情報を取得する
        ''' </summary>
        Public Function GetBomDetails(parentItemCode As String, revision As Integer) As List(Of BomDetail)
            Dim sql As String = "SELECT bd.ParentItemCode, bd.Revision, bd.LineNo, bd.ChildItemCode, " &
                                "bd.Quantity, bd.Unit, bd.LeadTime, bd.IsActive, " &
                                "mi.ItemName, mi.ItemType " &
                                "FROM T_BomDetail bd " &
                                "INNER JOIN M_Item mi ON bd.ChildItemCode = mi.ItemCode " &
                                "WHERE bd.ParentItemCode = @ParentItemCode AND bd.Revision = @Revision " &
                                "ORDER BY bd.LineNo"

            Dim params As New Dictionary(Of String, Object) From {
                {"@ParentItemCode", parentItemCode},
                {"@Revision", revision}
            }

            Dim dt = DBUtility.ExecuteReader(sql, params)
            Dim details As New List(Of BomDetail)

            For Each row As DataRow In dt.Rows
                details.Add(New BomDetail() With {
                    .ParentItemCode = row("ParentItemCode").ToString(),
                    .Revision = CInt(row("Revision")),
                    .LineNo = CInt(row("LineNo")),
                    .ChildItemCode = row("ChildItemCode").ToString(),
                    .ChildItemName = row("ItemName").ToString(),
                    .Quantity = CDec(row("Quantity")),
                    .Unit = row("Unit").ToString(),
                    .LeadTime = CInt(row("LeadTime")),
                    .IsActive = CBool(row("IsActive"))
                })
            Next

            Return details
        End Function

        ''' <summary>
        ''' 部品表を検索する
        ''' </summary>
        Public Function SearchBom(searchParam As BomSearchParam) As List(Of BomHeader)
            Dim sql As String = "SELECT DISTINCT bh.ParentItemCode, bh.Revision, bh.EffectiveDate, " &
                                "bh.ExpirationDate, bh.Status, bh.Description, " &
                                "bh.CreatedDate, bh.UpdatedDate, mi.ItemName " &
                                "FROM T_BomHeader bh " &
                                "INNER JOIN M_Item mi ON bh.ParentItemCode = mi.ItemCode " &
                                "WHERE 1=1"

            Dim params As New Dictionary(Of String, Object)

            If Not String.IsNullOrEmpty(searchParam.ParentItemCode) Then
                sql &= " AND bh.ParentItemCode LIKE @ParentItemCode"
                params.Add("@ParentItemCode", "%" & searchParam.ParentItemCode & "%")
            End If

            If Not String.IsNullOrEmpty(searchParam.Status) Then
                sql &= " AND bh.Status = @Status"
                params.Add("@Status", searchParam.Status)
            End If

            If searchParam.EffectiveDate.HasValue Then
                sql &= " AND bh.EffectiveDate <= @EffectiveDate"
                params.Add("@EffectiveDate", searchParam.EffectiveDate.Value)
            End If

            sql &= " ORDER BY bh.ParentItemCode, bh.Revision DESC"

            Dim dt = DBUtility.ExecuteReader(sql, params)
            Dim headers As New List(Of BomHeader)

            For Each row As DataRow In dt.Rows
                headers.Add(New BomHeader() With {
                    .ParentItemCode = row("ParentItemCode").ToString(),
                    .ParentItemName = row("ItemName").ToString(),
                    .Revision = CInt(row("Revision")),
                    .EffectiveDate = CDate(row("EffectiveDate")),
                    .ExpirationDate = If(IsDBNull(row("ExpirationDate")), Nothing, CDate(row("ExpirationDate"))),
                    .Status = row("Status").ToString(),
                    .Description = If(IsDBNull(row("Description")), "", row("Description").ToString()),
                    .CreatedDate = CDate(row("CreatedDate")),
                    .UpdatedDate = If(IsDBNull(row("UpdatedDate")), Nothing, CDate(row("UpdatedDate")))
                })
            Next

            Return headers
        End Function

        ''' <summary>
        ''' 部品表を登録する（ヘッダーと明細を同時に登録）
        ''' </summary>
        Public Function InsertBom(header As BomHeader, details As List(Of BomDetail)) As Boolean
            Dim actions As New List(Of Action(Of SqlConnection, SqlTransaction))

            ' ヘッダー登録
            actions.Add(
                Sub(conn, trans)
                    Dim sql As String = "INSERT INTO T_BomHeader (ParentItemCode, Revision, EffectiveDate, " &
                                        "ExpirationDate, Status, Description, CreatedDate) " &
                                        "VALUES (@ParentItemCode, @Revision, @EffectiveDate, " &
                                        "@ExpirationDate, @Status, @Description, GETDATE())"

                    Using cmd As New SqlCommand(sql, conn, trans)
                        cmd.Parameters.AddWithValue("@ParentItemCode", header.ParentItemCode)
                        cmd.Parameters.AddWithValue("@Revision", header.Revision)
                        cmd.Parameters.AddWithValue("@EffectiveDate", header.EffectiveDate)
                        cmd.Parameters.AddWithValue("@ExpirationDate", If(header.ExpirationDate, DBNull.Value))
                        cmd.Parameters.AddWithValue("@Status", header.Status)
                        cmd.Parameters.AddWithValue("@Description", If(header.Description, DBNull.Value))
                        cmd.ExecuteNonQuery()
                    End Using
                End Sub
            )

            ' 明細登録
            For Each detail In details
                Dim currentDetail = detail
                actions.Add(
                    Sub(conn, trans)
                        Dim sql As String = "INSERT INTO T_BomDetail (ParentItemCode, Revision, LineNo, " &
                                            "ChildItemCode, Quantity, Unit, LeadTime, IsActive) " &
                                            "VALUES (@ParentItemCode, @Revision, @LineNo, " &
                                            "@ChildItemCode, @Quantity, @Unit, @LeadTime, @IsActive)"

                        Using cmd As New SqlCommand(sql, conn, trans)
                            cmd.Parameters.AddWithValue("@ParentItemCode", currentDetail.ParentItemCode)
                            cmd.Parameters.AddWithValue("@Revision", currentDetail.Revision)
                            cmd.Parameters.AddWithValue("@LineNo", currentDetail.LineNo)
                            cmd.Parameters.AddWithValue("@ChildItemCode", currentDetail.ChildItemCode)
                            cmd.Parameters.AddWithValue("@Quantity", currentDetail.Quantity)
                            cmd.Parameters.AddWithValue("@Unit", currentDetail.Unit)
                            cmd.Parameters.AddWithValue("@LeadTime", currentDetail.LeadTime)
                            cmd.Parameters.AddWithValue("@IsActive", currentDetail.IsActive)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Sub
                )
            Next

            Return DBUtility.ExecuteTransaction(actions)
        End Function

        ''' <summary>
        ''' 部品表を更新する
        ''' </summary>
        Public Function UpdateBom(header As BomHeader, details As List(Of BomDetail)) As Boolean
            Dim actions As New List(Of Action(Of SqlConnection, SqlTransaction))

            ' ヘッダー更新
            actions.Add(
                Sub(conn, trans)
                    Dim sql As String = "UPDATE T_BomHeader SET " &
                                        "EffectiveDate = @EffectiveDate, " &
                                        "ExpirationDate = @ExpirationDate, " &
                                        "Status = @Status, " &
                                        "Description = @Description, " &
                                        "UpdatedDate = GETDATE() " &
                                        "WHERE ParentItemCode = @ParentItemCode AND Revision = @Revision"

                    Using cmd As New SqlCommand(sql, conn, trans)
                        cmd.Parameters.AddWithValue("@ParentItemCode", header.ParentItemCode)
                        cmd.Parameters.AddWithValue("@Revision", header.Revision)
                        cmd.Parameters.AddWithValue("@EffectiveDate", header.EffectiveDate)
                        cmd.Parameters.AddWithValue("@ExpirationDate", If(header.ExpirationDate, DBNull.Value))
                        cmd.Parameters.AddWithValue("@Status", header.Status)
                        cmd.Parameters.AddWithValue("@Description", If(header.Description, DBNull.Value))
                        cmd.ExecuteNonQuery()
                    End Using
                End Sub
            )

            ' 明細削除
            actions.Add(
                Sub(conn, trans)
                    Dim sql As String = "DELETE FROM T_BomDetail " &
                                        "WHERE ParentItemCode = @ParentItemCode AND Revision = @Revision"

                    Using cmd As New SqlCommand(sql, conn, trans)
                        cmd.Parameters.AddWithValue("@ParentItemCode", header.ParentItemCode)
                        cmd.Parameters.AddWithValue("@Revision", header.Revision)
                        cmd.ExecuteNonQuery()
                    End Using
                End Sub
            )

            ' 明細再登録
            For Each detail In details
                Dim currentDetail = detail
                actions.Add(
                    Sub(conn, trans)
                        Dim sql As String = "INSERT INTO T_BomDetail (ParentItemCode, Revision, LineNo, " &
                                            "ChildItemCode, Quantity, Unit, LeadTime, IsActive) " &
                                            "VALUES (@ParentItemCode, @Revision, @LineNo, " &
                                            "@ChildItemCode, @Quantity, @Unit, @LeadTime, @IsActive)"

                        Using cmd As New SqlCommand(sql, conn, trans)
                            cmd.Parameters.AddWithValue("@ParentItemCode", currentDetail.ParentItemCode)
                            cmd.Parameters.AddWithValue("@Revision", currentDetail.Revision)
                            cmd.Parameters.AddWithValue("@LineNo", currentDetail.LineNo)
                            cmd.Parameters.AddWithValue("@ChildItemCode", currentDetail.ChildItemCode)
                            cmd.Parameters.AddWithValue("@Quantity", currentDetail.Quantity)
                            cmd.Parameters.AddWithValue("@Unit", currentDetail.Unit)
                            cmd.Parameters.AddWithValue("@LeadTime", currentDetail.LeadTime)
                            cmd.Parameters.AddWithValue("@IsActive", currentDetail.IsActive)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Sub
                )
            Next

            Return DBUtility.ExecuteTransaction(actions)
        End Function

        ''' <summary>
        ''' 部品表を展開する（階層構造を取得）
        ''' </summary>
        Public Function GetBomTree(parentItemCode As String, revision As Integer) As List(Of BomTreeNode)
            Dim nodes As New List(Of BomTreeNode)
            GetBomTreeRecursive(parentItemCode, revision, 0, nodes, New List(Of String))
            Return nodes
        End Function

        Private Sub GetBomTreeRecursive(itemCode As String, revision As Integer, level As Integer,
                                         nodes As List(Of BomTreeNode), visitedItems As List(Of String))
            ' 循環参照チェック
            If visitedItems.Contains(itemCode) Then
                Return
            End If

            visitedItems.Add(itemCode)

            ' 現在の品目情報を取得
            Dim itemDao As New ItemDao()
            Dim item = itemDao.GetItem(itemCode)

            If item IsNot Nothing Then
                nodes.Add(New BomTreeNode() With {
                    .Level = level,
                    .ItemCode = item.ItemCode,
                    .ItemName = item.ItemName,
                    .ItemType = item.ItemType,
                    .Quantity = 1,
                    .Unit = item.Unit
                })
            End If

            ' 部品表明細を取得
            Dim details = GetBomDetails(itemCode, revision)

            For Each detail In details
                ' 子品目の情報を追加
                nodes.Add(New BomTreeNode() With {
                    .Level = level + 1,
                    .ItemCode = detail.ChildItemCode,
                    .ItemName = detail.ChildItemName,
                    .ItemType = "",
                    .Quantity = detail.Quantity,
                    .Unit = detail.Unit
                })

                ' 子品目の最新リビジョンを取得して再帰的に展開
                Dim childRevision = GetLatestRevision(detail.ChildItemCode)
                If childRevision > 0 Then
                    GetBomTreeRecursive(detail.ChildItemCode, childRevision, level + 1, nodes, visitedItems)
                End If
            Next

            visitedItems.Remove(itemCode)
        End Sub

        Private Function GetLatestRevision(itemCode As String) As Integer
            Dim sql As String = "SELECT MAX(Revision) FROM T_BomHeader WHERE ParentItemCode = @ItemCode"
            Dim params As New Dictionary(Of String, Object) From {
                {"@ItemCode", itemCode}
            }

            Dim dt = DBUtility.ExecuteReader(sql, params)
            If dt.Rows.Count > 0 AndAlso Not IsDBNull(dt.Rows(0)(0)) Then
                Return CInt(dt.Rows(0)(0))
            End If

            Return 0
        End Function
    End Class
End Namespace
