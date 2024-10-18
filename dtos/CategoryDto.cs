namespace BlogProject.dtos
{
	public class CategoryDto
	{
	
		[MaxLength(100)]
		public string Name { get; set; }
	}
}
