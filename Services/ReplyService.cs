using BlogProject.Data;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace BlogProject.Services
{
	public class ReplyService(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager) : IReplyService
	{
		
		

		public async Task<List<Reply>> GetAllAsync(string UserName = "")
		{
			return await _context.Replies//.Include(p=>p.Category)//.Include(p=>p.Comments).Include(P=>P.User).Include(p=>p.Likers)
				.Where(P => P.AuthorName == UserName || string.IsNullOrEmpty(UserName)).Include(p => p.Comment).ToListAsync();
		}

		public async Task<Reply> GetByIdAsync(int id)
		{
			return await _context.Replies.Include(p => p.Comment).FirstOrDefaultAsync(P => P.Id == id);
		}

		public async Task<List<Reply>> GetByComment(int CommentId)
		{
			return await _context.Replies.Where(c => c.CommentId == CommentId).Include(p=>p.Comment).ToListAsync();
		}

		public async Task<List<Reply>> GetFilteredAsync(string Search)
		{
			string[] Words = Search.Trim().ToLower().Split(" ");
			return await _context.Replies
				//.Where(R => R.CommentId == model.CommentId || model.CommentId == 0)
				.Where(R => Words.Any(word => (R.Content.ToLower().Contains(word) )))
				//|| String.IsNullOrEmpty(model.Search))
				//.Where(p=>p.Content.ToLower().Contains(model.Search.ToLower()||
				//p=>p.Title.ToLower().Contains(model.Search.ToLower))
				//.Skip((model.PageNumber - 1) * model.ElementsPerPage)
				//.Take(model.ElementsPerPage)
				.Include(p => p.Comment).ToListAsync();
		}	
		public async Task<Reply> AddAsync(Reply Reply)
		{
			await _context.Replies.AddAsync(Reply);
			await _context.SaveChangesAsync();
			return Reply;
		}
		public Reply Update(Reply Reply)
		{
			_context.Replies.Update(Reply);
			_context.SaveChanges();
			return Reply;
		}
		

		public Reply Delete(Reply Reply)
		{

			_context.Replies.Remove(Reply);
			_context.SaveChanges();
			return Reply;
		}
		public async Task<bool> IsValidReply(int id)
		{
			return await _context.Replies.AnyAsync(post => post.Id == id);
		}
		/////////////////--------------------------------------
		///

		public async Task<Reply> GetReplyLikesIncluded(int Id)
		{
			return await _context.Replies.Include(x => x.Likers).FirstOrDefaultAsync(x => x.Id == Id);
		}
		public async Task<Reply> LikeReply(Reply reply, string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (reply.Likers.Any(u => u.UserName == user.UserName))
			{
				return null;
			}
			reply.Likes++;
			reply.Likers.Add(user);
			user.LikedReplies.Add(reply);
			_context.Update(reply);
			await _context.SaveChangesAsync();
			return reply;
		}
		public async Task<Reply> UnLikeReply(Reply reply, string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (reply.Likers.Any(u => u.UserName == user.UserName))
			{
				reply.Likes--;
				reply.Likers.Remove(user);
				user.LikedReplies.Remove(reply);
				_context.Update(reply);
				await _context.SaveChangesAsync();
				return reply;
			}
			return null;
		}
		public async Task<List<ApplicationUser>> GetLikes(Reply reply)
		{
			var Likers = reply.Likers.ToList();
			return Likers;
		}

	}
}
