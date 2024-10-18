using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
	public class ApplicationUser : IdentityUser
	{
		[MaxLength(100)]
		public string FirstName { get; set; }
		[MaxLength(100)]
		public string LastName { get; set; }
		public bool IsBanned { get; set; }
		public DateTime? RestrictedUntill { get;set; }
		public List<Post> LikedPosts { get; set; } = new List<Post>();
		public List<Post> PublishedPosts { get; set; } = new List<Post>();
		public List<Comment> Comments { get; set; } = new List<Comment>();
		public List<Comment> LikedComments { get; set; } = new List<Comment>();
		public List<Reply> Replies { get; set; } = new List<Reply>();
		public List<Reply> LikedReplies { get; set; } = new List<Reply>();


	}
}
