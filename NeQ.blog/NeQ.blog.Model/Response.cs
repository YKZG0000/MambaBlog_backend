using NeQ.blog.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeQ.blog.Model
{
    public class Response:IEntity
    {
        [Key] public Guid ID { get ; set ; }
        public string? Content { get; set ; }
        public int Like { get; set; } = 0;
        public int Reply { get; set; } = 0;
        public Guid AuthorID { get; set; }
        [ForeignKey("AuthorID")]
        public User? Author { get; set; }
        public Guid BlogID { get; set; }
        [ForeignKey("BlogID")]
        public Blog? Blog { get; set; }
        public List<Guid>? NextResponseIDs { get; set; }

    }
}
