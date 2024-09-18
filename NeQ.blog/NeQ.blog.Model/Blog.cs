using NeQ.blog.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeQ.blog.Model
{
    public class Blog : IEntity
    {
        [Key] public Guid ID { get ; set; }
        public string? Title { get; set; }
        public String? Content { get; set; }
        public String[]? Tags { get; set; }
        public String[]? Imgs { get; set; }
        public DateTime? Date {get; set; }
        public int Watch { get; set; } = 0;
        public int Like { get; set; } = 0;
        public int Reply { get; set; } = 0;
        public int Collect { get; set; } = 0;
        public Guid AuthorID { get; set; }
        [ForeignKey("AuthorID")]
        public User? Author { get; set; }
    }
}
