namespace BlogProject.Services
{
	public interface ICategoryService
	{
	    public Task<IEnumerable<Category>> GetCategoriesAsync();
		public Task<Category> GetCategoryByIdAsync(byte id);
		public Task<Category> AddAsync(Category category);
		public Category Update(Category category);
		public Category Delete(Category category);

		public Task<bool> IsValidCategory(byte id);
	}

}
