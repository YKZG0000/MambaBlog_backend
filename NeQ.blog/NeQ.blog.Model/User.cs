using NeQ.blog.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeQ.blog.Model
{
    public class User : IEntity
    {
        [Key] public Guid ID { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }

        [InverseProperty("Author")]
        public List<Blog>? Blogs { get; set; }          //自己的博客

        public List<Guid>? Collections{ get; set; }     //收藏
        public List<Guid>? Broser { get; set; }         //浏览
        public List<Guid>? Fans { get; set; }           //粉丝
        public List<Guid>? Follows { get; set; }        //关注

    }
}
