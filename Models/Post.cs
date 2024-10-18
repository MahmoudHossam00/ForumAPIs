using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
	public class Post:Interaction
	{
		[Required]
		public string Title { get; set; }

		public List<Comment> Comments { get; set; } = new List<Comment>() ;

		public byte CategoryId { get; set; }

		public Category Category { get; set; } = null!;

	}
}
