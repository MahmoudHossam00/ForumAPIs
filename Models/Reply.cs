namespace BlogProject.Models
{
	public class Reply:Interaction
	{
		public Comment Comment { get; set; }
		public int CommentId { get; set; }
	}
}
