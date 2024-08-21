using NeQ.blog.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeQ.blog.Model
{
    public class User : IEntity
    {
        [Key] public Guid ID { get; set; }
        public string? account { get; set; }
        public string? password { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }

        [InverseProperty("Author")]
        public List<Blog>? Blogs { get; set; }

        public List<Guid>? Collections{ get; set; }
        public List<Guid>? broser { get; set; }

        public List<Guid>? fans { get; set; }
        public List<Guid>? follows { get; set; }

    }
}
