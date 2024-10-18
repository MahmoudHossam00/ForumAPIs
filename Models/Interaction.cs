using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.Models
{
	public class Interaction
	{
		public int Id { get; set; }
		
		public DateTime DateTime { get; set; }
		public string Content { get; set; }
		public byte[] Picture { get; set; }
		public int Likes { get; set; }

		
		public string? AuthorName { get; set; }

		//public string UserId { get; set; }

		[ForeignKey("AuthorName")]
		public ApplicationUser User { get; set; } 
		//public string AuthorId { get; set; }
		public List<ApplicationUser> Likers { get; set; }=new List<ApplicationUser>();
	}
}
