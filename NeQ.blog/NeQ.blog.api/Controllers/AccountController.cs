using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeQ.blog.Model;
using System.Security.Claims;

namespace NeQ.blog.api.Controllers
{
    /// <summary>
    /// 账号控制器
    /// </summary>
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly BlogContext _context;
        /// <summary>
        /// 构造函数依赖注入
        /// </summary>
        public AccountController(BlogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 登录
        /// </summary>s
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string account, string password)
        {
            var res = await _context.Users.FirstOrDefaultAsync(x => x.Account == account);
            if (res == null) {
                return NotFound(new { Message = "用户不存在" });
            }
            // 验证用户名和密码
            if (account == res.Account && password == res.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok(new {Message="登陆成功"});
            }

            return Unauthorized();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
