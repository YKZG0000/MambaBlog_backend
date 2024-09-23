using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeQ.blog.Model;
using System.Linq;

namespace NeQ.blog.api.Controllers
{
    /// <summary>
    /// 博客控制器
    /// </summary>
    [ApiExplorerSettings(GroupName = "v0")]
    [ApiController]
    [Route("[controller]")]
    public class BlogController : Controller
    {
        private readonly BlogContext _context;
        /// <summary>
        /// 构造函数依赖注入
        /// </summary>
        public BlogController(BlogContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 添加一个博客
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddBlog")]
        [Authorize]
        public async Task<ActionResult<int>> AddB(string author_id,string title, string content, [FromQuery] string[] tags, [FromBody] string[] imgs)
        {
            Blog b = new Blog();
            b.AuthorID = new Guid(author_id);
            b.Title = title;
            b.Content = content;
            b.Tags = tags;
            b.Imgs = imgs;
            b.Date = DateTime.Now;
            b.Watch = 0;
            b.Like = 0;
            b.Reply = 0;
            b.Collect = 0;
            _context.Blogs.Add(b);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 获取所有博客
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllBlog")]
        public async Task<List<Blog>> GetB()
        {
            var res = await _context.Blogs.ToListAsync();
            return res;
        }

        /// <summary>
        /// 获取所有博客部分信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllBlogInfo")]
        public async Task<List<BlogDto>> GetBlogInfo()
        {
            var res = await _context.Blogs.Select(blog => new BlogDto
            {
                ID = blog.ID,
                Title = blog.Title,
                Content = blog.Content,
                Tags = blog.Tags,
                Imgs = blog.Imgs,
                Date = blog.Date,
                Watch = blog.Watch,
                Like = blog.Like,
                Collect = blog.Collect,
                Reply = blog.Reply,
                AuthorName = blog.Author.Name,
            }).ToListAsync();
            return res;
        }

        /// <summary>
        /// 点赞帖子
        /// </summary>
        /// <param name="blog_id"></param>
        /// <returns></returns>
        [HttpPost("LikeBlog")]
        public async Task<ActionResult<int>> LikeB(string blog_id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.ID == Guid.Parse(blog_id));
            if (blog == null) return NotFound();
            blog.Like++;
            _context.Blogs.Update(blog);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 取消点赞
        /// </summary>
        /// <param name="blog_id"></param>
        /// <returns></returns>
        [HttpPost("DislikeBlog")]
        public async Task<ActionResult<int>> DislikeB(string blog_id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.ID == Guid.Parse(blog_id));
            if (blog == null) return NotFound();
            blog.Like--;
            _context.Blogs.Update(blog);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 收藏帖子
        /// </summary>
        /// <param name="blog_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost("CollectBlog")]
        public async Task<ActionResult<int>> CollectB(string blog_id,string user_id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.ID == Guid.Parse(blog_id));
            if (blog == null) return NotFound();
            blog.Collect++;
            _context.Blogs.Update(blog);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ID == Guid.Parse(user_id));
            if (user == null) return NotFound(new { message="用户id错误"});
            if (user.Collections == null) user.Collections = new List<Guid>();
            user.Collections.Add(blog.ID);
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="blog_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost("DisCollectBlog")]
        public async Task<ActionResult<int>> DisCollectB(string blog_id, string user_id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.ID == Guid.Parse(blog_id));
            if (blog == null) return NotFound();
            blog.Collect--;
            _context.Blogs.Update(blog);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ID == Guid.Parse(user_id));
            if (user == null) return NotFound(new { message = "用户id错误" });
            if (user.Collections == null) user.Collections = new List<Guid>();
            user.Collections.Remove(blog.ID);
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }



        /// <summary>
        /// 获取排行前20的帖子部分信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTop20BlogInfo")]
        public async Task<List<BlogDto>> GetTopBlogInfo()
        {
            var res = await _context.Blogs.OrderByDescending(blog=>2*blog.Like+blog.Reply+4*blog.Collect)
                .Select(blog => new BlogDto
            {
                ID = blog.ID,
                Title = blog.Title,
                Content = blog.Content,
                Tags = blog.Tags,
                Imgs = blog.Imgs,
                Date = blog.Date,
                Watch = blog.Watch,
                Like = blog.Like,
                Collect = blog.Collect,
                Reply = blog.Reply,
                AuthorName = blog.Author.Name,
            }).ToListAsync();
            return res;
        }


        /// <summary>
        /// 获取指定博客的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetBlogByID")]
        public async Task<Blog> GetB(string id, string user_id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            if (blog == null) return null;
            blog.Watch++;
            _context.Blogs.Update(blog);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ID == Guid.Parse(user_id));
            user.Broser.Add(blog.ID);
            await _context.SaveChangesAsync();
            var res = await _context.Blogs.FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            return res;
        }

        /// <summary>
        /// 更新某个博客的信息,old
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateBlog")]
        [Authorize]
        public async Task<ActionResult<int>> UpdateB(string id,string title,string content, string image, string?[] tags)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var e = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            if (e == null) return NotFound();
            e.Title = title;
            e.Content = content;
            if (tags != null) e.Tags = tags;
            e.Imgs = new string[] { image };
            _context.Blogs.Update(e);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 删除博客,根据给定id删除某个博客
        /// </summary>
        /// <returns></returns>
        [HttpDelete("DelteBlog")]
        [Authorize]
        public async Task<ActionResult<int>> DeleteB(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            //根据id删除对应blog
            var e = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            if (e == null) return -1;
            _context.Blogs.Remove(e);
            return await _context.SaveChangesAsync();
        }
    }
}
