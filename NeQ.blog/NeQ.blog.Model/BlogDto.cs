using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeQ.blog.Model
{
    public class BlogDto
    {
        public Guid ID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string[]? Tags { get; set; }
        public string[]? Imgs { get; set; }
        public DateTime? Date { get; set; }
        public int Watch { get; set; } = 0;
        public int Like { get; set; } = 0;
        public int Collect { get; set; } = 0;
        public int Reply { get; set; } = 0;
        public string? AuthorName { get; set; }
    }
}
