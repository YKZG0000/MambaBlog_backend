using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace NeQ.blog.api.Controllers
{
    /// <summary>
    /// �������������ɺ���֤��֤��
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly EmailService _emailService;

        /// <summary>
        /// ����ע��
        /// </summary>
        public CaptchaController(IMemoryCache memoryCache, EmailService emailService)
        {
            _memoryCache = memoryCache;
            _emailService = emailService;
        }

        /// <summary>
        /// ����������֤��
        /// </summary>
        /// <param name="email">Ŀ������</param>
        /// <returns>ע�����ID</returns>
        [HttpPost("RequestEmailCaptcha")]
        public IActionResult RequestEmailCaptcha([FromBody] string email)
        {
            // ������֤��
            var captchaCode = GenerateRandomCode(6);
            var operationId = Guid.NewGuid().ToString();

            // ����֤��Ͳ���ID�洢���ڴ滺���У����ù���ʱ��Ϊ5����
            _memoryCache.Set(operationId, captchaCode, TimeSpan.FromMinutes(5));

            // ������֤�뵽Ŀ������
            var subject = "Man ! What can i say ! ";
            var body = $"Man! ��ġ�������̳����֤����: {captchaCode} ,��֤����Чʱ��Ϊ5����,�뾡��ʹ��,Mamba out!";
            _emailService.SendEmail(email, subject, body);
            

            return Ok(new { OperationId = operationId });
        }

        //���������֤��
        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
   

