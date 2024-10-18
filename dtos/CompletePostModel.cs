using System.ComponentModel;

namespace BlogProject.dtos
{
	public class CompletePostModel : CompleteInteractionModel
	{
		public byte CategoryId { get; set; }

		public string CategoryName { get; set; } = null!;
		public List<CompleteCommentModel> Comments {get;set;} = new List<CompleteCommentModel>();
	}
}
