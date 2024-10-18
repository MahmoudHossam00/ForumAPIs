namespace BlogProject.dtos
{
	public class CommentsFilterationModel : FilterationModel
	{
		//public int PageNumber { get; set; } = 1;
		//public int CommentsPerPage { get; set; } = 5;
		public int PostId { get; set; } = 0;
		//public string Search { get; set; } = "";
		public CommentsFilterationModel()
		{

		}
	}
}
