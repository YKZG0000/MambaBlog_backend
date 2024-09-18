using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace NeQ.blog.api.Controllers
{
    /// <summary>
    /// 控制器用于生成和验证验证码
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly EmailService _emailService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public CaptchaController(IMemoryCache memoryCache, EmailService emailService)
        {
            _memoryCache = memoryCache;
            _emailService = emailService;
        }

        /// <summary>
        /// 请求邮箱验证码
        /// </summary>
        /// <param name="email">目标邮箱</param>
        /// <returns>注册操作ID</returns>
        [HttpPost("RequestEmailCaptcha")]
        public IActionResult RequestEmailCaptcha([FromBody] string email)
        {
            // 生成验证码
            var captchaCode = GenerateRandomCode(6);
            var operationId = Guid.NewGuid().ToString();

            // 将验证码和操作ID存储在内存缓存中，设置过期时间为5分钟
            _memoryCache.Set(operationId, captchaCode, TimeSpan.FromMinutes(5));

            // 发送验证码到目标邮箱
            var subject = "Man ! What can i say ! ";
            var body = $"Man! 你的「曼波论坛」验证码是: {captchaCode} ,验证码有效时间为5分钟,请尽快使用,Mamba out!";
            _emailService.SendEmail(email, subject, body);
            

            return Ok(new { OperationId = operationId });
        }

        //生成随机验证码
        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
   

