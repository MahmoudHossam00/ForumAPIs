using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Data.Config
{
	public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		PasswordHasher<ApplicationUser> hasher =
			new Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser>();

		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(e=>e.IsBanned).HasDefaultValue(false);
			builder.Property(e => e.RestrictedUntill).HasColumnType("DATETIME").IsRequired(false);
			builder.HasData(new ApplicationUser
			{
				Id = "1",
				UserName = "user1",
				NormalizedUserName = "USER1",
				Email = "user1@example.com",
				NormalizedEmail = "USER1@EXAMPLE.COM",
				EmailConfirmed = true,
				PasswordHash = hasher.HashPassword(null, "Password123!"),
				SecurityStamp = string.Empty,
				FirstName = "John",
				LastName = "Doe",
				IsBanned = false
			});
			builder.HasMany(user=>user.LikedComments).WithMany(c=>c.Likers).UsingEntity(j => j.ToTable("UserLikedComments"));
			builder.HasMany(user => user.LikedReplies).WithMany(c => c.Likers).UsingEntity(j => j.ToTable("UserLikedReplies"));
			builder.HasMany(user => user.LikedPosts).WithMany(c => c.Likers).UsingEntity(j => j.ToTable("UserLikedPosts"));
			builder.HasMany(user => user.PublishedPosts).WithOne(c => c.User).HasForeignKey(c=>c.AuthorName).HasPrincipalKey(u=>u.Id).OnDelete(DeleteBehavior.Cascade);
			builder.HasMany(user => user.Replies).WithOne(c => c.User).HasForeignKey(c => c.AuthorName).HasPrincipalKey(u => u.Id).OnDelete(DeleteBehavior.Cascade);
			builder.HasMany(user => user.Comments).WithOne(c => c.User).HasForeignKey(c => c.AuthorName).HasPrincipalKey(u => u.Id).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
