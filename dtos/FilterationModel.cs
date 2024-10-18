namespace BlogProject.dtos
{
	public class FilterationModel
	{
		public int PageNumber { get; set; } = 1;
		public int ElementsPerPage { get; set; } = 5;
		//public int CategoryId { get; set; } = 0;
		public string Search { get; set; } = "";
        public FilterationModel()
        {
             
        }
  //      public FilterationModel(int PageNumber=1, int PostsPerPage = 5, int CategoryId = 0, string Search = "")
  //      {
		//	this.PageNumber = PageNumber;
		//	this.PostsPerPage = PostsPerPage;
		//	this.CategoryId = CategoryId;
		//	this.Search = Search;
		//}
    }
}
