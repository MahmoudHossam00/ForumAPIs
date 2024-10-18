namespace BlogProject.Services
{
	public interface IReplyService
	{
		public Task<Reply> GetByIdAsync(int id);
		public Task<List<Reply>> GetAllAsync(string UserName = "");
		public Task<List<Reply>> GetByComment(int CommentId);
		//public Task<Post> GetByIdAsync();
		public Task<List<Reply>> GetFilteredAsync(string Search);
		public Task<Reply> AddAsync(Reply Reply);
		public Reply Update(Reply Reply);
		public Reply Delete(Reply Reply);
		public Task<bool> IsValidReply(int id);
		public Task<Reply> GetReplyLikesIncluded(int Id);
		public Task<Reply> LikeReply(Reply reply, string username);
		
		public Task<Reply> UnLikeReply(Reply reply, string username);
		
		public Task<List<ApplicationUser>> GetLikes(Reply reply);
	}
}
