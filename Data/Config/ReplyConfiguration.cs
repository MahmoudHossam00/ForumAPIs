using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Data.Config
{
	public class ReplyConfiguration : IEntityTypeConfiguration<Reply>
	{

		public void Configure(EntityTypeBuilder<Reply> builder)
		{		
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();
		builder.Property(p => p.AuthorName)//.HasColumnType("VARCHAR")
		.IsRequired();
			builder.Property(x => x.DateTime).HasColumnType("date").IsRequired();
		builder.Property(x => x.Content).HasColumnType("VARCHAR").HasMaxLength(2500).IsRequired();
		builder.Property(x => x.Picture).IsRequired(false);
		builder.Property(x => x.Likes).HasDefaultValue(0).IsRequired();
		builder.HasOne(x => x.Comment).WithMany(x => x.Replies).HasForeignKey(x => x.CommentId).OnDelete(DeleteBehavior.Cascade);
		//
		//builder.HasOne(R => R.User).WithMany(u => u.Replies).HasForeignKey(x => x.AuthorName).HasPrincipalKey(u => u.UserName).OnDelete(DeleteBehavior.Restrict);
		//builder.HasMany(R => R.Likers).WithMany(u => u.LikedReplies).UsingEntity(j => j.ToTable("UserLikedReplies"));

			builder.HasData(LoadReplies());
			
		}
		private List<Reply> LoadReplies()
		{
			return new List<Reply>
		{
			new Reply
			{
				Id = 1,
				AuthorName = "1",
				DateTime = DateTime.Now,
				Content = "This is a sample comment.",
				Picture = null, // or some byte array
                Likes = 0,
				CommentId = 1
			},
			new Reply
			{
				Id = 2,
				AuthorName = "1",
				DateTime = DateTime.Now,
				Content = "Another sample comment.",
				Picture = null, // or some byte array
                Likes = 0,
				CommentId = 2
			}
		};
		}
	}
}
