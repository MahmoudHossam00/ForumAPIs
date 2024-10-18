
using BlogProject.Data;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogProject.Services
{
	public class CommentsService(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager) : ICommentService
	{
		/*public async Task<List<Post>> GetAllAsync(string UserName = "")
		{
			return await _context.Posts//.Include(p=>p.Category)//.Include(p=>p.Comments).Include(P=>P.User).Include(p=>p.Likers)
				.Where(P=>P.AuthorName == UserName || string.IsNullOrEmpty(UserName)).ToListAsync();
		}

		public async Task<Post> GetByIdAsync(int id)
		{
			return await _context.Posts.FindAsync(id);
		//}

		public async Task<List<Post>> GetFilteredAsync(FilterationModel model)
		{
			string[] Words = model.Search.ToLower().Split(" ");
			return await _context.Posts
				.Where(p=>p.CategoryId == model.CategoryId|| model.CategoryId==0)
				.Where(p=> Words.Any(word=> (p.Content.ToLower().Contains(word) ||p.Title.ToLower().Contains(word)))
				|| String.IsNullOrEmpty(model.Search))
				//.Where(p=>p.Content.ToLower().Contains(model.Search.ToLower()||
				//p=>p.Title.ToLower().Contains(model.Search.ToLower))
				.Skip((model.PageNumber-1) * model.PostsPerPage)
				.Take(model.PostsPerPage)
				.ToListAsync();
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
			_context.Posts.Remove(post);
			_context.SaveChanges();
			return post;
		}*/
		public async Task<Comment> AddAsync(Comment comment)
		{
			await _context.Comments.AddAsync(comment);
			await _context.SaveChangesAsync();
			return comment;
		}

		

		public async Task<List<Comment>> GetAllAsync(string UserName = "")
		{
			return await _context.Comments.Where(C => C.AuthorName == UserName || string.IsNullOrEmpty(UserName)).ToListAsync();
			//return awiat _context.Posts.Where()
		}

		public async Task<Comment> GetByIdAsync(int id)
		{
			return await _context.Comments.FindAsync(id);
		}

		public async Task<List<Comment>> GetByPost(int PostId)
		{
			return await _context.Comments.Where(c => c.PostId == PostId).ToListAsync();
		}
		public async Task<List<Comment>> GetFilteredAsync(string Search)
		{
			string[] Words = Search.Trim().ToLower().Split(" ");
			return await _context.Comments
				//.Where(C => C.PostId == model.PostId || model.PostId == 0)
				.Where(p => Words.Any(word => (p.Content.ToLower().Contains(word))))
				//|| String.IsNullOrEmpty(model.Search))
				//.Where(p=>p.Content.ToLower().Contains(model.Search.ToLower()||
				//p=>p.Title.ToLower().Contains(model.Search.ToLower))
				//.Skip((model.PageNumber - 1) * model.ElementsPerPage)
				//.Take(model.ElementsPerPage)
				.ToListAsync();
		}

		public async Task<bool> IsValidComment(int id)
		{
			return await _context.Comments.AnyAsync(C => C.Id == id);
		}

		public Comment Update(Comment comment)
		{
			_context.Comments.Update(comment);
			_context.SaveChanges();
			return comment;
		}
		public Comment Delete(Comment comment)
		{
			_context.Comments.Remove(comment);
			_context.SaveChanges();
			return comment;
		}
		/////////////////--------------------------------------
		///

		public async Task<Comment> GetCommentLikesIncluded(int Id)
		{
			return await _context.Comments.Include(x => x.Likers).FirstOrDefaultAsync(x => x.Id == Id);
		}
		public async Task<Comment> LikeComment(Comment comment, string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (comment.Likers.Any(u => u.UserName == user.UserName))
			{
				return null;
			}
			comment.Likes++;
			comment.Likers.Add(user);
			user.LikedComments.Add(comment);
			_context.Update(comment);
			await _context.SaveChangesAsync();
			return comment;
		}
		public async Task<Comment> UnLikeComment(Comment comment, string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (comment.Likers.Any(u => u.UserName == user.UserName))
			{
				comment.Likes--;
				comment.Likers.Remove(user);
				user.LikedComments.Remove(comment);
				_context.Update(comment);
				await _context.SaveChangesAsync();
				return comment;
			}
			return null;
		}
		public async Task<List<ApplicationUser>> GetLikes(Comment comment)
		{
			var Likers = comment.Likers.ToList();
			return Likers;
		}

	}
}
