# API機能仕様書

## 変更履歴
| 日付 | 変更内容 | 変更者 |
|------|----------|--------|
| 2024/05/26 | 初版作成 | System |

## 1. 概要
BOM管理システムのAPI機能に関する仕様を定義します。

## 2. コントローラー構成
### 2.1 CommandController
- 場所: `Controllers/Client/CommandController.cs`
- 機能:
  - コマンド実行API
  - クライアントアプリケーション用インターフェース

## 3. ルーティング設定
### 3.1 クライアントAPI
```csharp
app.MapControllerRoute(
    name: "client",
    pattern: "client/{controller=Command}/{action=Execute}/{*commandPath}");
```

### 3.2 Web API
```csharp
app.MapControllerRoute(
    name: "web",
    pattern: "{controller=MainMenu}/{action=Index}/{id?}");
```

## 4. API仕様
### 4.1 コマンド実行API
- エンドポイント: `POST /client/Command/Execute/{commandPath}`
- 認証: JWT認証必須
- パラメータ:
  - commandPath: コマンドパス
  - param: コマンドパラメータ
- レスポンス:
  - success: 成功フラグ
  - message: エラーメッセージ
  - data: 実行結果

## 5. コマンドファクトリ
### 5.1 初期化
```csharp
CommandInitializer.RegisterInitializer(new BOMCommandInitializer());
CommandInitializer.Initialize();
```

### 5.2 コマンド生成
```csharp
var command = CommandFactory.GetInstance().CreateCommand(commandPath);
```

## 6. エラーハンドリング
### 6.1 認証エラー
- 未認証: 401 Unauthorized
- トークン期限切れ: 401 Unauthorized
- アクセス権限なし: 403 Forbidden

### 6.2 コマンド実行エラー
- 無効なコマンドパス: 400 Bad Request
- パラメータエラー: 400 Bad Request
- 実行エラー: 500 Internal Server Error

## 7. セキュリティ
### 7.1 認証
- JWTトークンによる認証
- トークンの有効期限: 8時間
- スライディング有効期限: 有効

### 7.2 アクセス制御
- コントローラーレベルでの認証要求
- エンドポイントごとの権限チェック

## 8. レスポンス形式
### 8.1 成功レスポンス
```json
{
    "success": true,
    "data": {
        // 実行結果データ
    }
}
```

### 8.2 エラーレスポンス
```json
{
    "success": false,
    "message": "エラーメッセージ"
}
``` 