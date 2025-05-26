using Microsoft.AspNetCore.Mvc;
using BomManagement.BOM_PRM;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BomManagement.WEB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

//        [HttpGet("login")]
//        public ContentResult LoginPage()
//        {
//            var html = @"
//<!DOCTYPE html>
//<html>
//<head>
//    <title>ログイン - BOM管理システム</title>
//    <style>
//        body {
//            font-family: Arial, sans-serif;
//            display: flex;
//            justify-content: center;
//            align-items: center;
//            height: 100vh;
//            margin: 0;
//            background-color: #f5f5f5;
//        }
//        .login-container {
//            background-color: white;
//            padding: 2rem;
//            border-radius: 8px;
//            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
//            width: 300px;
//        }
//        .form-group {
//            margin-bottom: 1rem;
//        }
//        label {
//            display: block;
//            margin-bottom: 0.5rem;
//        }
//        input {
//            width: 100%;
//            padding: 0.5rem;
//            border: 1px solid #ddd;
//            border-radius: 4px;
//            box-sizing: border-box;
//        }
//        button {
//            width: 100%;
//            padding: 0.75rem;
//            background-color: #007bff;
//            color: white;
//            border: none;
//            border-radius: 4px;
//            cursor: pointer;
//        }
//        button:hover {
//            background-color: #0056b3;
//        }
//        .error-message {
//            color: red;
//            margin-top: 1rem;
//            display: none;
//        }
//    </style>
//</head>
//<body>
//    <div class='login-container'>
//        <h2 style='text-align: center; margin-bottom: 1.5rem;'>BOM管理システム</h2>
//        <form id='loginForm'>
//            <div class='form-group'>
//                <label for='userId'>ユーザーID</label>
//                <input type='text' id='userId' name='userId' required>
//            </div>
//            <div class='form-group'>
//                <label for='password'>パスワード</label>
//                <input type='password' id='password' name='password' required>
//            </div>
//            <button type='submit'>ログイン</button>
//            <div id='errorMessage' class='error-message'></div>
//        </form>
//    </div>
//    <script>
//        document.getElementById('loginForm').addEventListener('submit', async (e) => {
//            e.preventDefault();
//            const userId = document.getElementById('userId').value;
//            const password = document.getElementById('password').value;
//            const errorMessage = document.getElementById('errorMessage');

//            try {
//                const response = await fetch('/auth/login', {
//                    method: 'POST',
//                    headers: {
//                        'Content-Type': 'application/json'
//                    },
//                    body: JSON.stringify({ userId, password })
//                });

//                const result = await response.json();
//                if (result.success) {
//                    // トークンを保存
//                    localStorage.setItem('token', result.token);
//                    // Web用のメインメニューにリダイレクト
//                    window.location.href = '/web/main';
//                } else {
//                    errorMessage.textContent = result.message;
//                    errorMessage.style.display = 'block';
//                }
//            } catch (error) {
//                errorMessage.textContent = 'ログインに失敗しました。';
//                errorMessage.style.display = 'block';
//            }
//        });
//    </script>
//</body>
//</html>";

//            return new ContentResult
//            {
//                ContentType = "text/html",
//                Content = html
//            };
//        }

        [HttpPost("login")]
        public ActionResult<LoginResult> Login([FromBody] LoginParam param)
        {
            _logger.LogInformation("Login attempt for user: {UserId}", param.UserId);
            
            try
            {
                // TODO: 実際の認証処理を実装
                // 例: データベースでユーザーを検索し、パスワードを検証

                // 仮の認証処理（実際の実装では削除）
                if (param.UserId == "admin" && param.Password == "password")
                {
                    _logger.LogInformation("Login successful for user: {UserId}", param.UserId);
                    
                    var user = new UserInfo
                    {
                        UserId = "1",
                        UserName = "管理者",
                        Email = "admin@example.com",
                        Department = "システム管理部",
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };

                    var token = GenerateJwtToken(user);

                    var result = new LoginResult
                    {
                        Success = true,
                        User = user,
                        Token = token
                    };
                    return Ok(result);
                }

                _logger.LogWarning("Login failed for user: {UserId}", param.UserId);
                return Ok(new LoginResult
                {
                    Success = false,
                    Message = "ユーザーIDまたはパスワードが正しくありません。"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user: {UserId}", param.UserId);
                return Ok(new LoginResult
                {
                    Success = false,
                    Message = $"認証処理でエラーが発生しました: {ex.Message}"
                });
            }
        }

        [HttpPost("saml/login")]
        public ActionResult<SamlLoginResult> SamlLogin([FromBody] SamlLoginParam param)
        {
            try
            {
                // TODO: SAML認証の初期化処理を実装
                // 例: SAMLプロバイダーの設定を読み込み、認証リクエストを生成

                // 仮の実装（実際の実装では削除）
                return Ok(new SamlLoginResult
                {
                    Success = false,
                    RedirectUrl = "https://saml-provider.example.com/login",
                    ErrorMessage = "SAML認証を開始します。"
                });
            }
            catch (Exception ex)
            {
                return Ok(new SamlLoginResult
                {
                    Success = false,
                    ErrorMessage = $"SAML認証の初期化に失敗しました: {ex.Message}"
                });
            }
        }

        [HttpGet("saml/callback")]
        public ActionResult<SamlLoginResult> SamlCallback([FromQuery] string samlResponse)
        {
            try
            {
                // TODO: SAMLレスポンスの処理を実装
                // 例: SAMLレスポンスを検証し、ユーザー情報を取得

                // 仮の実装（実際の実装では削除）
                var user = new UserInfo
                {
                    UserId = "2",
                    UserName = "SAML User",
                    Email = "saml.user@example.com",
                    Department = "一般ユーザー",
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                var token = GenerateJwtToken(user);

                return Ok(new SamlLoginResult
                {
                    Success = true,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return Ok(new SamlLoginResult
                {
                    Success = false,
                    ErrorMessage = $"SAML認証の処理に失敗しました: {ex.Message}"
                });
            }
        }

        [HttpGet("current")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var userName = User.FindFirst(ClaimTypes.GivenName)?.Value;

            return Ok(new { userId, userName });
        }

        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            // TODO: トークンの無効化処理を実装
            return Ok(new { Message = "ログアウトしました。" });
        }

        private string GenerateJwtToken(UserInfo user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Department", user.Department)
            };

            var expiryInHours = _configuration.GetValue<int>("Jwt:ExpiryInHours", 8);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(expiryInHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 