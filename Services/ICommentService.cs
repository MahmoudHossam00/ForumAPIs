using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Services
{
	public interface ICommentService
	{
		public Task<Comment> GetByIdAsync(int id);
		public Task<List<Comment>> GetAllAsync(string UserName = "");
		public Task<List<Comment>> GetByPost(int PostId );
		//public Task<Post> GetByIdAsync();
		public Task<List<Comment>> GetFilteredAsync(string Search);
		public Task<Comment> AddAsync(Comment comment);
		public Comment Update(Comment comment);
		public Comment Delete(Comment comment);
		public Task<bool> IsValidComment(int id);
		public  Task<Comment> GetCommentLikesIncluded(int Id);
		public  Task<Comment> LikeComment(Comment comment, string username);
		public  Task<Comment> UnLikeComment(Comment comment, string username);
		public  Task<List<ApplicationUser>> GetLikes(Comment comment);
	}
}
