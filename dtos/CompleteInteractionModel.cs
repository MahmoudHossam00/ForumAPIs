namespace BlogProject.dtos
{
	public class CompleteInteractionModel
	{
		public int Id { get; set; }

		public DateTime DateTime { get; set; }
		public string Content { get; set; }
		public byte[] Picture { get; set; }
		public int Likes { get; set; }
		public string? AuthorName { get; set; }
	}
}
