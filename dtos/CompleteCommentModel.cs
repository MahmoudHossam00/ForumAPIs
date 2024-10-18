namespace BlogProject.dtos
{
	public class CompleteCommentModel : CompleteInteractionModel
	{
		public int PostId { get; set; }

		public List<CompleteReplyModel> Replies { set; get; } = new List<CompleteReplyModel>();
	}
}
