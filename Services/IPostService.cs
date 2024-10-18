using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Services
{
    public interface IPostService
    {
		
		public Task<Post> GetByIdAsync(int id);
		public Task<List<Post>> GetAllAsync(string UserName="");
		//public Task<Post> GetByIdAsync();
		public Task<List<Post>> GetFilteredAsync(PostsFilterationModel model);

		public  Task<Post> GetFullPost(int Id);

		public Task<Post> AddAsync(Post post);
		public Post Update(Post post);
		public Post Delete(Post post);
		public Task<bool> IsValidPost(int id);

		public  Task<Post> GetPostLikesIncluded(int Id);
		public Task<Post> LikePost(Post post, string username);
		public Task<Post> UnLikePost(Post post, string username);
		public Task<List<ApplicationUser>> GetLikes(Post post);
	}
}
