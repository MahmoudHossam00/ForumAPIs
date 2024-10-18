using AutoMapper;

namespace BlogProject.Helpers
{
    public class MappingProfile:Profile
	{
        public MappingProfile()
        {
			CreateMap<Post, RevealedPost>().ForMember(RevealedPost => RevealedPost.CategoryName
			, opt => opt.MapFrom(Post => Post.Category.Name));

				  CreateMap<Comment, RevealedComment>()
			.ForMember(RevealedComment => RevealedComment.PostContent, opt => opt
			.MapFrom(Comment => Comment.Post.Content));

			CreateMap<Reply, RevealedReply>()
			.ForMember(RevealedReply => RevealedReply.CommentContent, opt => opt
			.MapFrom(Reply => Reply.Comment.Content));


			CreateMap<Post, CompletePostModel>()
			.ForMember(CompletedPost => CompletedPost.CategoryName, opt => opt.MapFrom(Post => Post.Category.Name));
			CreateMap<Comment, CompleteCommentModel>();
			//.ForMember(dest => dest.CompletedComments, opt => opt.MapFrom(Comment => Comment.Post.Content));
			CreateMap<Reply, CompleteReplyModel>();
			//.ForMember(dest => dest.CompletedComments, opt => opt.MapFrom(Reply => Reply.Comment.Content));

		}
    }
}
