using BlogProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogProject.Data.Config
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.Property(x => x.Name).HasColumnType("VARCHAR").HasMaxLength(100).IsRequired();
			builder.ToTable("Categories");

			

			//builder.HasData(LoadCategories());
		}

		private List<Category> LoadCategories()
		{
			return new List<Category>
			{
				new Category { Id = 1, Name = "Technology" },
				new Category { Id = 2, Name = "Science" },
				new Category { Id = 3, Name = "Art" }
			};
		}
	}
}


