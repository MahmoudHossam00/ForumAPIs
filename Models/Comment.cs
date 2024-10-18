using System.Runtime.InteropServices;

namespace BlogProject.Models
{
	public class Comment: Interaction
	{
		public List<Reply> Replies { get; set; } = new List<Reply>();

		public int PostId { get; set; }

		public Post Post { get; set; } = null!;

	}
}
