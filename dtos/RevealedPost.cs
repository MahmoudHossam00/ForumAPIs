namespace BlogProject.dtos
{
    public class RevealedPost: RevealedInteraction
    {
		[Required]
		public string Title { get; set; }

		//public List<RevealedComment> Comments { get; set; } = new List<RevealedComment>();

		public byte CategoryId { get; set; }

		public string CategoryName { get; set; } = null!;
	}
}
