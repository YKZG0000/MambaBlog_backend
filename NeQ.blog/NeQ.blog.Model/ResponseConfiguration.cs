using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NeQ.blog.Model
{
    public class ResponseConfiguration : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> builder)
        {
            builder.HasOne(r => r.Author)
                .WithMany()
                .HasForeignKey(r => r.AuthorID)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

            builder.HasOne(r => r.Blog)
                .WithMany()
                .HasForeignKey(r => r.BlogID)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction
        }
    }
}
