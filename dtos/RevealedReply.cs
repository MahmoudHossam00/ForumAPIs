namespace BlogProject.dtos
{
	public class RevealedReply:RevealedInteraction
	{
		public int CommentId { get; set; }
		public string CommentContent = null!;
	}
}
