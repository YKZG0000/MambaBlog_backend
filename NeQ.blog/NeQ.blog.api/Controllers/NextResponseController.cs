using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeQ.blog.Model;

namespace NeQ.blog.api.Controllers
{
    /// <summary>
    /// 二级评论控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NextResponseController : ControllerBase
    {
        private readonly BlogContext _context;
        /// <summary>
        /// 构造函数依赖注入
        /// </summary>
        public NextResponseController(BlogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 添加二级评论
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="response_id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("AddNextResponse")]
        public async Task<ActionResult<int>> AddNR(string user_id, string response_id, string content)
        {
            var nextResponse = new NextResponse();
            var response = await _context.Responses.FindAsync(new System.Guid(response_id));
            var user = await _context.Users.FindAsync(new System.Guid(user_id));
            if (response == null || user == null)
            {
                return NotFound(new { Message = "目标评论不存在或者当前用户异常" });
            }
            response.Reply++;
            _context.Responses.Update(response);
            nextResponse.AuthorID = new System.Guid(user_id);
            nextResponse.ResponseID = new System.Guid(response_id);
            nextResponse.Content = content;
            _context.NextResponses.Add(nextResponse);
            await _context.SaveChangesAsync();
            return Ok(new { message = "发表评论成功" });
        }

        /// <summary>
        /// 删除二级评论
        /// </summary>
        /// <param name="nextResponse_id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteNextResponse")]
        public async Task<ActionResult<int>> DeleteNR(string nextResponse_id)
        {
            var nextResponse = await _context.NextResponses.FindAsync(new System.Guid(nextResponse_id));
            if (nextResponse == null)
            {
                return NotFound(new { Message = "目标二级评论不存在" });
            }
            var response = await _context.Responses.FindAsync(nextResponse.ResponseID);
            if (response == null)
            {
                return NotFound(new { Message = "目标评论不存在" });
            }
            response.Reply--;
            _context.Responses.Update(response);
            _context.NextResponses.Remove(nextResponse);
            await _context.SaveChangesAsync();
            return Ok(new { message = "删除评论成功" });
        }

    }
}
