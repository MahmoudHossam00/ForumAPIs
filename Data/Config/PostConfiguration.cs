using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Data.Config
{
	public class PostConfiguration : IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			builder.HasKey(p => p.Id);
			builder.Property(p => p.Id).ValueGeneratedOnAdd();
			builder.Property(p=>p.Title).HasColumnType("VARCHAR").HasMaxLength(100).IsRequired();
			builder.Property(p => p.AuthorName)//.HasColumnType("VARCHAR")
				.IsRequired();
			builder.Property(p=>p.DateTime).HasColumnType("date").IsRequired();
			builder.Property(p => p.Content).HasColumnType("VARCHAR").HasMaxLength(2500).IsRequired();
			builder.Property(p => p.Picture).IsRequired(false);
			builder.Property(p => p.Likes).HasDefaultValue(0).IsRequired();
			builder.HasOne(p => p.Category).WithMany(c=>c.Posts).HasForeignKey(x=>x.CategoryId).OnDelete(DeleteBehavior.Cascade);
			//builder.HasOne(p=>p.User).WithMany(u=>u.PublishedPosts).HasForeignKey(x=>x.AuthorName).HasPrincipalKey(u=>u.UserName).OnDelete(DeleteBehavior.Restrict);
			//builder.HasMany(p=>p.Likers).WithMany(l=>l.LikedPosts).UsingEntity(j => j.ToTable("UserLikedPosts"));
			builder.HasData(LoadPosts());


	
		}
		
		private List<Post> LoadPosts()
		{
			return new List<Post>
		{
			new Post
			{
				Id = 1,
				Title="Hello Friends My First Post",
				AuthorName = "1",
				DateTime = DateTime.Now,
				Content = "Sample content 1",
				Picture = null, // or some byte array
                CategoryId = 1
			},
			new Post
			{
				Id = 2,
				AuthorName = "1",
				Title="Hello Friends My Second Post",
				DateTime = DateTime.Now.AddDays(1),
				Content = "Sample content 2",
				Picture = null, // or some byte array
                CategoryId = 2
			}
		};
		}
	}
}
