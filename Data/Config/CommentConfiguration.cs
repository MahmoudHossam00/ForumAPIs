using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Data.Config
{
	public class CommentConfiguration : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.Property(C => C.AuthorName)//.HasColumnType("VARCHAR")
			.IsRequired();
			builder.Property(x => x.DateTime).HasColumnType("date").IsRequired();
			builder.Property(x => x.Content).HasColumnType("VARCHAR").HasMaxLength(2500).IsRequired();
			builder.Property(x => x.Picture).IsRequired(false);
			builder.Property(x => x.Likes).HasDefaultValue(0).IsRequired();
			builder.HasOne(C => C.Post).WithMany(P => P.Comments)
				.HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
			
			//builder.HasOne(C => C.User).WithMany(u => u.Comments).HasForeignKey(x => x.AuthorName)
			//	.HasPrincipalKey(u => u.UserName).OnDelete(DeleteBehavior.Restrict);
			//builder.HasMany(c => c.Likers).WithMany(l => l.LikedComments).UsingEntity(j => j.ToTable("UserLikedComments"));

			builder.HasData(LoadComments());
		}
		private List<Comment> LoadComments()
		{
			return new List<Comment>
		{
			new Comment
			{
				Id = 1,
				AuthorName = "1",               
				DateTime = DateTime.Now,
				Content = "This is a sample comment.",
				Picture = null, // or some byte array
		              Likes = 0,
				PostId = 1
			},
			new Comment
			{
				Id = 2,
				AuthorName = "1",
				DateTime = DateTime.Now,
				Content = "Another sample comment.",
				Picture = null, // or some byte array
		              Likes = 0,
				PostId = 2
			}
		};
		}

	}
}
