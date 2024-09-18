using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeQ.blog.Model;
using System.Security.Claims;

namespace NeQ.blog.api.Controllers
{
    /// <summary>
    /// �˺ſ�����
    /// </summary>
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly BlogContext _context;
        /// <summary>
        /// ���캯������ע��
        /// </summary>
        public AccountController(BlogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// ��¼
        /// </summary>s
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string account, string password)
        {
            var res = await _context.Users.FirstOrDefaultAsync(x => x.Account == account);
            if (res == null) {
                return NotFound(new { Message = "�û�������" });
            }
            // ��֤�û���������
            if (account == res.Account && password == res.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok(new {Message="��½�ɹ�"});
            }

            return Unauthorized();
        }

        /// <summary>
        /// �ǳ�
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
