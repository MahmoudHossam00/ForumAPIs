
using BlogProject.Data;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlogProject.Services
{
	public class PostService(ApplicationDbContext _context,ICommentService _commentService
		,UserManager<ApplicationUser> _userManager) : IPostService
	{
		

		public async Task<List<Post>> GetAllAsync(string UserName = "")
		{
			return await _context.Posts//.Include(p=>p.Category)//.Include(p=>p.Comments).Include(P=>P.User).Include(p=>p.Likers)
				.Where(P=>P.AuthorName == UserName || string.IsNullOrEmpty(UserName)).Include(p=>p.Category).ToListAsync();
		}

		public async Task<Post> GetByIdAsync(int id)
		{
			return await _context.Posts.Include(p => p.Category).FirstOrDefaultAsync(P=>P.Id==id);
		}

		public async Task<List<Post>> GetFilteredAsync(PostsFilterationModel model)
		{
			string[] Words = model.Search.Trim().ToLower().Split(" ");
			return await _context.Posts
				.Where(p=>p.CategoryId == model.CategoryId|| model.CategoryId==0)
				.Where(p=> Words.Any(word=> (p.Content.ToLower().Contains(word) ||p.Title.ToLower().Contains(word)))
				|| String.IsNullOrEmpty(model.Search))
				//.Where(p=>p.Content.ToLower().Contains(model.Search.ToLower()||
				//p=>p.Title.ToLower().Contains(model.Search.ToLower))
				.Skip((model.PageNumber-1) * model.ElementsPerPage)
				.Take(model.ElementsPerPage)
				.Include(p => p.Category).ToListAsync();
		}
		public async Task<Post> AddAsync(Post post)
		{
			await _context.Posts.AddAsync(post);
			await _context.SaveChangesAsync();
			return post;
		}
		public async Task<bool> IsValidPost(int id)
		{
			return await _context.Posts.AnyAsync(post => post.Id == id);
		}
		
		public Post Update(Post post)
		{
			_context.Posts.Update(post);
			_context.SaveChanges();
			return post;
		}
		public Post Delete(Post post)
		{
			var Comments =_context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == post.Id).Comments.ToList();
			
				foreach (var comment in Comments)
				{
					_commentService.Delete(comment);
				}
		
			_context.Posts.Remove(post);
			_context.SaveChanges();
			return post;
		}

		public async Task<Post> GetFullPost(int Id)
		{
			return await _context.Posts.Include(x=>x.Category).Include(x => x.Comments).ThenInclude(c => c.Replies).FirstOrDefaultAsync(x => x.Id == Id);
		}
		/////////////////--------------------------------------
		///

		public async Task<Post> GetPostLikesIncluded (int Id)
		{
			return await _context.Posts.Include(x => x.Likers).FirstOrDefaultAsync(x => x.Id == Id);
		}
		public async Task<Post> LikePost(Post post, string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			if ( post.Likers.Any(u => u.UserName == user.UserName))
					{
				return null;
					}
			post.Likes++;
			post.Likers.Add(user);
			user.LikedPosts.Add(post);
			_context.Update(post);
			await _context.SaveChangesAsync();
			return post;
		}
		public async Task<Post> UnLikePost(Post post, string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (post.Likers.Any(u => u.UserName == user.UserName))
			{
				post.Likes--;
				post.Likers.Remove(user);
				user.LikedPosts.Remove(post);
				_context.Update(post);
				await _context.SaveChangesAsync();
				return post;
			}
			return null;
		}
		public async Task<List<ApplicationUser>> GetLikes(Post post)
		{
			var Likers =  post.Likers.ToList();
			return Likers;
		}
	}
}
