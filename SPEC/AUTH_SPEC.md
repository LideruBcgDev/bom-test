# 認証機能仕様書

## 変更履歴
| 日付 | 変更内容 | 変更者 |
|------|----------|--------|
| 2024/05/26 | 初版作成 | System |

## 1. 概要
BOM管理システムの認証機能に関する仕様を定義します。

## 2. 認証方式
### 2.1 認証スキーム
- Cookie認証（Webアプリケーション用）
- JWT認証（API用）

### 2.2 認証設定
```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
})
```

## 3. コントローラー構成
### 3.1 AccountController
- 場所: `Controllers/Auth/AccountController.cs`
- 機能:
  - ログイン画面表示
  - ログイン処理
  - ログアウト処理
  - アクセス拒否画面表示

### 3.2 AuthController
- 場所: `Controllers/Auth/AuthController.cs`
- 機能:
  - JWTトークン生成
  - ログインAPI
  - SAML認証（開発予定）
  - 現在のユーザー情報取得

## 4. セキュリティ設定
### 4.1 Cookie設定
```csharp
options.Cookie.Name = "BomManagement.Auth";
options.Cookie.HttpOnly = true;
options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
options.Cookie.SameSite = SameSiteMode.Strict;
options.ExpireTimeSpan = TimeSpan.FromHours(8);
options.SlidingExpiration = true;
```

### 4.2 JWT設定
- 発行者（Issuer）の検証
- 受信者（Audience）の検証
- 有効期限の検証
- 署名キーの検証

## 5. ユーザー情報
### 5.1 ユーザーモデル
```csharp
public class UserInfo
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
```

### 5.2 クレーム情報
- NameIdentifier: ユーザーID
- Name: ユーザー名
- Email: メールアドレス
- Department: 部署名

## 6. エラーハンドリング
### 6.1 認証エラー
- トークン期限切れ
- 無効なトークン
- アクセス権限なし

### 6.2 エラーレスポンス
- ステータスコード: 401（未認証）
- ステータスコード: 403（アクセス拒否）
- エラーメッセージの表示 