namespace BlogProject.dtos
{
	public class PostsFilterationModel : FilterationModel
	{
		public int CategoryId { get; set; } = 0;
        public PostsFilterationModel()
        {
             
        }
    }
}
