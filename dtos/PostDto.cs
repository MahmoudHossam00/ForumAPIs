namespace BlogProject.dtos
{
	public class PostDto:InteractionDto
	{
	
		[MaxLength(100)]
		public string Title { get; set; }

		public byte CategoryId { get; set; }
	}
}
