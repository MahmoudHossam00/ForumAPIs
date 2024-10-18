using BlogProject.Data;
using BlogProject.Models;

namespace BlogProject.Services
{
	public class CategoryService : ICategoryService
	{
		private  ApplicationDbContext _context;
		private IPostService _postService;
		public CategoryService(ApplicationDbContext Context, IPostService postService)
		{
			_context = Context;
			_postService = postService;
		}

		public async Task<IEnumerable<Category>> GetCategoriesAsync()
		{
			return await _context.Categories.OrderBy(g => g.Name).ToListAsync();
		}
		public async Task<Category> GetCategoryByIdAsync(byte id)
		{
			return await _context.Categories.FindAsync(id);
		}
		public async Task<Category> AddAsync(Category category)
		{
			await _context.AddAsync(category);
			await _context.SaveChangesAsync();
			return category;
		}
		public Category Update(Category category)
		{
			_context.Update(category);
			_context.SaveChanges();
			return category;
		}
		public Category Delete(Category category)
		{
			var Posts = _context.Categories.Include(p => p.Posts)
				.FirstOrDefault(c => c.Id == category.Id).Posts.ToList();

			foreach (var post in Posts)
			{
				_postService.Delete(post);
			}
			_context.Remove(category);
			_context.SaveChanges();
			return category;
		}

		public async Task<bool> IsValidCategory(byte id)
		{
			return await _context.Categories.AnyAsync(g => g.Id == id);
		}

	}
}
