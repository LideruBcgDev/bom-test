# 部品管理機能仕様書

## 変更履歴
| 日付 | 変更内容 | 変更者 |
|------|----------|--------|
| 2024/05/26 | 初版作成 | System |

## 1. 概要
BOM管理システムの部品管理機能に関する仕様を定義します。

## 2. コントローラー構成
### 2.1 HinmokuController（Web）
- 場所: `Controllers/Web/HinmokuController.cs`
- 機能:
  - 部品一覧表示
  - 部品検索
  - 部品編集画面表示
  - 部品更新処理

### 2.2 HinmokuController（API）
- 場所: `Controllers/Api/HinmokuController.cs`
- 機能:
  - 部品情報取得API
  - 部品更新API

## 3. データモデル
### 3.1 HinmokuInfo
```csharp
public class HinmokuInfo
{
    public string HinmokuCode { get; set; }
    public string HinmokuName { get; set; }
    public string Unit { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
```

## 4. 画面仕様
### 4.1 部品一覧画面（Index.cshtml）
- 検索フォーム
  - 部品コード入力フィールド
  - 検索ボタン
- 検索結果テーブル
  - 部品コード
  - 部品名
  - 単位
  - 単価
  - 作成日時
  - 更新日時
  - 操作ボタン（編集・削除）

### 4.2 部品編集画面（Edit.cshtml）
- 編集フォーム
  - 部品コード（表示のみ）
  - 部品名（編集可能）
  - 単位（編集可能）
  - 単価（編集可能）
  - 作成日時（表示のみ）
  - 更新日時（表示のみ）
- 操作ボタン
  - 保存ボタン
  - 戻るボタン

## 5. API仕様
### 5.1 部品検索API
- エンドポイント: `POST /Hinmoku/Search`
- パラメータ:
  - hinmokuCode: 部品コード
- レスポンス:
  - success: 成功フラグ
  - data: 部品情報の配列
  - message: エラーメッセージ

### 5.2 部品更新API
- エンドポイント: `POST /Hinmoku/Edit`
- パラメータ:
  - HinmokuInfoオブジェクト
- レスポンス:
  - success: 成功フラグ
  - message: エラーメッセージ

## 6. バリデーション
### 6.1 部品情報のバリデーション
- 部品名: 必須
- 単位: 必須
- 単価: 必須、数値

## 7. エラーハンドリング
### 7.1 検索エラー
- 検索条件エラー
- データベース接続エラー

### 7.2 更新エラー
- バリデーションエラー
- データベース更新エラー
- 同時更新エラー 