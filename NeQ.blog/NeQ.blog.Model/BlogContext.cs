using Microsoft.EntityFrameworkCore;

namespace NeQ.blog.Model
{
    public class BlogContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<NextResponse> NextResponses { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ResponseConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
