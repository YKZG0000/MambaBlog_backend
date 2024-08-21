using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeQ.blog.Model;

namespace NeQ.blog.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {

        private readonly BlogContext _context;
        /// <summary>
        /// 构造函数依赖注入
        /// </summary>
        public ResponseController(BlogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 发表评论api
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="blog_id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("AddResponse")]
        [Authorize]
        public async Task<ActionResult<int>> AddR(string user_id, string blog_id, string content)
        {
            var response = new Response();
            var blog = await _context.Blogs.FindAsync(new System.Guid(blog_id));
            var user = await _context.Users.FindAsync(new System.Guid(user_id));
            if (blog == null || user==null) {
                return NotFound(new { Message = "目标博客不存在或者当前用户异常" });
            }
            response.AuthorID = new System.Guid(user_id);
            response.BlogID = new System.Guid(blog_id);
            response.Content = content;
            response.Like = 0;
            response.Reply = 0;
            response.NextResponseIDs = new List<Guid>();
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();
            return Ok(new {message="发表评论成功"});
        }




    }
}
