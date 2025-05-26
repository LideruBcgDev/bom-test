# アーキテクチャ仕様書

## 変更履歴
| 日付 | 変更内容 | 変更者 |
|------|----------|--------|
| 2024/05/26 | 初版作成 | System |

## 1. 概要
BOM管理システムのアーキテクチャに関する仕様を定義します。

## 2. プロジェクト構成
### 2.1 プロジェクト構造
```
BomManagement/
├── BomManagement.WEB/          # Webアプリケーション
├── BomManagement.BOM_MDL/      # BOMドメインモデル
├── BomManagement.BOM_PRM/      # BOMパラメータ
├── BomManagement.FW_APP/       # アプリケーションフレームワーク
└── BomManagement.FW_WEB/       # Webフレームワーク
```

### 2.2 コントローラー構成
```
Controllers/
├── Web/                    # Webアプリケーションのメイン機能
│   ├── HinmokuController.cs
│   └── MainMenuController.cs
├── Auth/                   # 認証・認可関連
│   ├── AccountController.cs
│   └── AuthController.cs
└── Client/                 # クライアントアプリケーション用
    └── CommandController.cs
```

## 3. 技術スタック
### 3.1 フレームワーク
- .NET 9.0
- ASP.NET Core MVC
- Entity Framework Core

### 3.2 認証
- Cookie認証
- JWT認証
- SAML認証（開発予定）

### 3.3 フロントエンド
- Bootstrap 5
- jQuery
- Font Awesome

## 4. アーキテクチャパターン
### 4.1 レイヤー構造
- プレゼンテーション層（Web）
- アプリケーション層（FW_APP）
- ドメイン層（BOM_MDL）
- インフラストラクチャ層

### 4.2 デザインパターン
- コマンドパターン
- ファクトリパターン
- リポジトリパターン

## 5. セキュリティ
### 5.1 認証・認可
- Cookie認証（Webアプリケーション）
- JWT認証（API）
- ロールベースのアクセス制御

### 5.2 データ保護
- HTTPS通信
- セキュアCookie
- 入力値のバリデーション

## 6. パフォーマンス
### 6.1 キャッシュ戦略
- クライアントサイドキャッシュ
- サーバーサイドキャッシュ

### 6.2 データベース最適化
- インデックス設計
- クエリ最適化

## 7. 拡張性
### 7.1 モジュール化
- ドメインごとの分離
- 機能ごとの分離

### 7.2 プラグインアーキテクチャ
- コマンドの動的ロード
- 機能の動的拡張

## 8. 保守性
### 8.1 コーディング規約
- 命名規則
- コメント規約
- エラー処理規約

### 8.2 テスト戦略
- 単体テスト
- 統合テスト
- E2Eテスト 