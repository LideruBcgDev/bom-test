using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BomManagement.WEB.Controllers
{
    [ApiController]
    [Route("web")]
    [Authorize]
    public class WebMainController : ControllerBase
    {
        [HttpGet("main")]
        public ContentResult MainMenu()
        {
            var html = @"
<!DOCTYPE html>
<html>
<head>
    <title>メインメニュー - BOM管理システム</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            display: flex;
        }
        .menu-panel {
            width: 200px;
            background-color: #f8f9fa;
            height: 100vh;
            padding: 20px;
            box-shadow: 2px 0 5px rgba(0,0,0,0.1);
        }
        .main-panel {
            flex: 1;
            padding: 20px;
        }
        .menu-button {
            display: block;
            width: 100%;
            padding: 10px;
            margin: 5px 0;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            text-align: left;
        }
        .menu-button:hover {
            background-color: #0056b3;
        }
        .user-info {
            margin-top: 20px;
            padding: 10px;
            background-color: #e9ecef;
            border-radius: 4px;
        }
        .logout-button {
            display: block;
            width: 100%;
            padding: 10px;
            margin-top: 20px;
            background-color: #dc3545;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }
        .logout-button:hover {
            background-color: #c82333;
        }
    </style>
</head>
<body>
    <div class='menu-panel'>
        <h2>BOM管理システム</h2>
        <button class='menu-button' onclick='loadContent(""hinmoku"")'>品目</button>
        <button class='menu-button' onclick='loadContent(""sekkei"")'>設計部品表</button>
        <button class='menu-button' onclick='loadContent(""mitsumori"")'>見積部品表</button>
        <button class='menu-button' onclick='loadContent(""juchu"")'>受注部品表</button>
        <div class='user-info'>
            <p>ログインユーザー: <span id='userName'>管理者</span></p>
        </div>
        <button class='logout-button' onclick='logout()'>ログアウト</button>
    </div>
    <div class='main-panel' id='content'>
        <h1>ようこそ、BOM管理システムへ</h1>
        <p>左のメニューから機能を選択してください。</p>
    </div>

    <script>
        // コンテンツの読み込み
        async function loadContent(type) {
            const token = localStorage.getItem('token');
            if (!token) {
                window.location.href = '/auth/login';
                return;
            }

            try {
                const response = await fetch(`/client/${type}/search`, {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    document.getElementById('content').innerHTML = `<pre>${JSON.stringify(data, null, 2)}</pre>`;
                } else {
                    document.getElementById('content').innerHTML = '<p>データの取得に失敗しました。</p>';
                }
            } catch (error) {
                document.getElementById('content').innerHTML = '<p>エラーが発生しました。</p>';
            }
        }

        // ログアウト処理
        function logout() {
            localStorage.removeItem('token');
            window.location.href = '/auth/login';
        }

        // ユーザー情報の取得
        async function loadUserInfo() {
            const token = localStorage.getItem('token');
            if (!token) {
                window.location.href = '/auth/login';
                return;
            }

            try {
                const response = await fetch('/auth/current', {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

                if (response.ok) {
                    const user = await response.json();
                    document.getElementById('userName').textContent = user.userName;
                }
            } catch (error) {
                console.error('ユーザー情報の取得に失敗しました:', error);
            }
        }

        // ページ読み込み時にユーザー情報を取得
        loadUserInfo();
    </script>
</body>
</html>";

            return new ContentResult
            {
                ContentType = "text/html",
                Content = html
            };
        }
    }
} 