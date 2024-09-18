using System.ComponentModel.DataAnnotations.Schema;
using NeQ.blog.Common;
using System.ComponentModel.DataAnnotations;

namespace NeQ.blog.Model
{
    public class NextResponse : IEntity
    {
        [Key] public Guid ID { get; set; }
        public string? Content { get; set; }
        public Guid AuthorID { get; set; }
        [ForeignKey("AuthorID")]
        public User? Author { get; set; }
        public Guid ResponseID { get; set; }
    }
    
}