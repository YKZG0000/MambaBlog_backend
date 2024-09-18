using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeQ.blog.Model;

namespace NeQ.blog.api.Controllers
{
    /// <summary>
    /// 评论控制器
    /// </summary>
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
            blog.Reply++;
            _context.Blogs.Update(blog);
            response.AuthorID = new System.Guid(user_id);
            response.TargetID = new System.Guid(blog_id);
            response.Content = content;
            response.Like = 0;
            response.Reply = 0;
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();
            return Ok(new {message="发表评论成功"});
        }


        /// <summary>
        /// 删除评论api
        /// </summary>
        /// <param name="response_id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteResponse")]
        [Authorize]
        public async Task<ActionResult<int>> DeleteR(string response_id)
        {
            var response = await _context.Responses.FindAsync(new System.Guid(response_id));
            if (response == null)
            {
                return NotFound(new { Message = "目标评论不存在" });
            }
            var blog = await _context.Blogs.FindAsync(response.TargetID);
            if (blog == null || blog == null)
            {
                return NotFound(new { Message = "目标博客不存在" });
            }
            blog.Reply--;
            _context.Blogs.Update(blog);
            //删除数据库中所有评论id为response_id的二级评论
            var secondResponses = await _context.NextResponses.Where(x => x.ResponseID == response.ID).ToListAsync();
            foreach (var secondResponse in secondResponses)
            {
                _context.NextResponses.Remove(secondResponse);
            }
            _context.Responses.Remove(response);
            await _context.SaveChangesAsync();
            return Ok(new { message = "删除评论成功" });
        }

        /// <summary>
        /// 点赞评论api
        /// </summary>
        /// <param name="response_id"></param>
        /// <returns></returns>
        [HttpPost("LikeResponse")]
        [Authorize]
        public async Task<ActionResult<int>> LikeR(string response_id)
        {
            var response = await _context.Responses.FindAsync(new System.Guid(response_id));
            if (response == null)
            {
                return NotFound(new { Message = "目标评论不存在" });
            }
            response.Like++;
            _context.Responses.Update(response);
            await _context.SaveChangesAsync();
            return Ok(new { message = "点赞评论成功" });
        }




    }
}
