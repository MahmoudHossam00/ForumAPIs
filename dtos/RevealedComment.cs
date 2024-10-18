namespace BlogProject.dtos
{
    public class RevealedComment:RevealedInteraction
    {
		//public List<RevealedReply> Replies { get; set; } = new List<RevealedReply>();

		public int PostId { get; set; }

		public string PostContent { get; set; } = null!;
	}
}
