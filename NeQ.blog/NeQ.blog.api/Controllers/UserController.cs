using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NeQ.blog.Model;

namespace NeQ.blog.api.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [ApiExplorerSettings(GroupName = "v0")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly BlogContext _context;
        private readonly IMemoryCache _memoryCache;
        /// <summary>
        /// 构造函数依赖注入
        /// </summary>
        public UserController(BlogContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddUser")]
        public async Task<ActionResult<int>> AddU(string name,string account,string password, string operationId, string userInputCaptcha)
        {
            // 从内存缓存中获取存储的验证码
            if (_memoryCache.TryGetValue(operationId, out string storedCaptchaCode))
            {
                // 比对用户输入的验证码和存储的验证码
                if (storedCaptchaCode != userInputCaptcha)
                {
                    // 验证码不匹配
                    return BadRequest(new { Message = "验证码错误" });
                }     
            }
            else
            {
                // 验证码已过期或不存在
                return BadRequest(new { Message = "验证码已过期或无效的操作ID" });
            }
            User user = new User();
            user.account = account;
            user.password = password;
            user.Name = name;
            user.Avatar = "";
            user.Blogs = new List<Blog>();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new{ Message="注册成功"});
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUser")]
        public async Task<List<User>> GetU()
        {
            var res = await _context.Users.ToListAsync();
            return res;
        }

        /// <summary>
        /// 获取指定id用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAUser")]
        public async Task<User> GetAU(string id)
        {
            var res = await _context.Users.FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            return res;
        }

        /// <summary>
        /// 更新某个用户的信息
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<ActionResult<int>> UpdateU(string id,string name,string avatar)
        {
            var res = await _context.Users.FirstOrDefaultAsync(x => x.ID==Guid.Parse(id));
            res.Name= name;
            res.Avatar = avatar;
            _context.Users.Update(res);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 删除某个用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("DelteUser")]
        [Authorize]
        public async Task<ActionResult<int>> DeleteU(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var e = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            if (e == null) return -1;
            _context.Users.Remove(e);
            return await _context.SaveChangesAsync();
        }
    }
}
