using Microsoft.AspNetCore.Mvc;
using BomManagement.BOM_PRM;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BomManagement.WEB.Controllers.Auth
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userId, string password, string returnUrl)
        {
            try
            {
                // TODO: 実際の認証処理を実装
                // 例: データベースでユーザーを検索し、パスワードを検証

                // 仮の認証処理（実際の実装では削除）
                if (userId == "admin" && password == "password")
                {
                    _logger.LogInformation("Login successful for user: {UserId}", userId);
                    
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

                    // トークンをCookieに保存
                    Response.Cookies.Append("token", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    // 認証クッキーを設定
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("Department", user.Department)
                    };

                    var identity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("Cookies", principal);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                _logger.LogWarning("Login failed for user: {UserId}", userId);
                ModelState.AddModelError(string.Empty, "ユーザーIDまたはパスワードが正しくありません。");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user: {UserId}", userId);
                ModelState.AddModelError(string.Empty, $"認証処理でエラーが発生しました: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 認証クッキーを削除
            await HttpContext.SignOutAsync("Cookies");
            
            // JWTトークンのクッキーを削除
            Response.Cookies.Delete("token");

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
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