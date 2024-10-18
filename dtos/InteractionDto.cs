namespace BlogProject.dtos
{
	public class InteractionDto
	{
		[MaxLength(2500)]
		public string Content { get; set; }
		public IFormFile? Picture { get; set; }

	}
}
