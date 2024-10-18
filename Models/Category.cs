using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.Models
{
	public class Category
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public byte Id { get; set; }
		public string Name { get; set; }

		public List<Post> Posts { get; set; } = new List<Post>();
	}
}
