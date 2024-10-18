using BlogProject.Filteers;
using BlogProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Helpers;

namespace BlogProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentsController(IPostService _postService,
		ICommentService _commentService, IOptions<PictureSpecifications> _pictureSpecs,
		UserManager<ApplicationUser> _userManager,IAuthService _authServices) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var Comments = await _commentService.GetAllAsync();
			return Ok(Comments);
		}
		[HttpGet("{Id}")]
		public async Task<IActionResult> GetByIdAsync(int Id)
		{
			var comment = await _commentService.GetByIdAsync(Id);
			if (comment is null)
				return NotFound($"There is no Comment with Id {Id}");
			return Ok(comment);

		}
		[HttpGet("Search/{search}")]
		public async Task<IActionResult> Search(string search )
		{
			var comments = await _commentService.GetFilteredAsync(search);
			if (comments is null)
				return NotFound("There are no More Comments to show or no Comments matchign ur searching criteria");
			return Ok(comments );
		}

		[HttpGet("GetByUser/{UserName}")]
		public async Task<IActionResult> GetByUser(string UserName)
		{
			var Comments = await _commentService.GetAllAsync(UserName);
			if (Comments is null)
				return NotFound($"There are no Comments by the User {UserName}");
			return Ok(Comments);
		}

		[HttpGet("GetByPost/{PostId}")]
		public async Task<IActionResult> GetByPost(int PostId)
		{
			if(!await _postService.IsValidPost(PostId))
				return NotFound($"There are no Posts with the Id {PostId}");
			var Comments = await _commentService.GetByPost(PostId);
			if (Comments is null)
				return NotFound($"There are no Comments in a post with the id {PostId}");
			return Ok(Comments);
		}
		[HttpPost]
		[Authorize]

		public async Task<IActionResult> CreateAsync([FromForm] CommentDto dto)
		{
			var AuthString = await _authServices.CheckIfBanned(User);
			if (!string.IsNullOrEmpty(AuthString))
				return BadRequest(AuthString);

			var UserName = _authServices.GetCurrentUserName(User);

			var Comment = new Comment();
			if (dto.Picture is not null)
			{

				var specs = _pictureSpecs.Value;
				if (!specs._allowedExtensions.Contains(Path.GetExtension(dto.Picture.FileName)))
					return BadRequest("Only .jpg .png pictures are allowed");

				if (!(specs._maxAllowedPosterSize > dto.Picture.Length))
					return BadRequest("Maximum Size allowed for pictures is 1MB");



				using var DataStream = new MemoryStream();
				await dto.Picture.CopyToAsync(DataStream);
				Comment.Picture = DataStream.ToArray();
			}

			if (!await _postService.IsValidPost(dto.PostId))
				return NotFound($"there is no Post with the id {dto.PostId}");

			//comment.Title = dto.Title;
			Comment.PostId = dto.PostId;
			Comment.DateTime = DateTime.Now;
			Comment.Content = dto.Content;
			Comment.Likes = 0;


			

			////var user = await _userManager.FindByIdAsync(UserId);
			//if (user == null)
			//{
			//	return NotFound("User not found.");
			//}
			Comment.AuthorName = UserName;
			//HttpContext.User.Identity.Name;

			await _commentService.AddAsync(Comment);
			return Ok(Comment);

		}
		[HttpPut("{id}")]
		[Authorize]
		[ServiceFilter(typeof(AuthorFilterAttribute))]
		public async Task<IActionResult> UpdateAsync([FromForm] CommentDto dto, int id)
		{
			var AuthString = await _authServices.CheckIfBanned(User);
			if (!string.IsNullOrEmpty(AuthString))
				return BadRequest(AuthString);

			//var UserName = _authServices.GetCurrentUserName(User);

			var Comment = await _commentService.GetByIdAsync(id);

			if (Comment == null) return NotFound($"there are not Comments with the id {id}");


			//if (Comment.AuthorName != UserName)
			//{
			//	return BadRequest("Only Author Can edit the Comment");
			//}


			if (dto.Picture is not null)
			{

				var specs = _pictureSpecs.Value;
				if (!specs._allowedExtensions.Contains(Path.GetExtension(dto.Picture.FileName)))
					return BadRequest("Only .jpg .png pictures are allowed");

				if (!(specs._maxAllowedPosterSize > dto.Picture.Length))
					return BadRequest("Maximum Size allowed for pictures is 1MB");



				using var DataStream = new MemoryStream();
				await dto.Picture.CopyToAsync(DataStream);
				Comment.Picture = DataStream.ToArray();
			}
			else
			{
				Comment.Picture = null;
			}

			if (!await _postService.IsValidPost(dto.PostId))
				return NotFound($"there is no Post with the id {dto.PostId}");

			////comment.Title = dto.Title;
			Comment.PostId = dto.PostId;
			Comment.Content = dto.Content;
			_commentService.Update(Comment);
			return Ok(Comment);
		}

		[HttpDelete("{id}")]
		[Authorize]
		[ServiceFilter(typeof(AuthorAndAdminsFilterAttribute))]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var Comment = await _commentService.GetByIdAsync(id);
			if (Comment == null)
				return NotFound($"there is no Comment with the id {id}");

			_commentService.Delete(Comment);
			return Ok(Comment);
		}

		//-------------------------------------------//
		[HttpPost("LIKE/{id}")]
		[Authorize]
		public async Task<IActionResult> Like(int id)
		{
			var comment = await _commentService.GetCommentLikesIncluded(id);
			if (comment == null)
			{
				return NotFound($"there is no comment with the id {id}");
			}
			var userName = _authServices.GetCurrentUserName(User);

			var result = await _commentService.LikeComment(comment, userName);
			if (result is null)
			{
				return BadRequest("the comment is already liked by the user");
			}
			return Ok($"{result.Likes} users liked this comment ");
		}
		//public Task<List<ApplicationUser>> GetLikes(Post post);
		[HttpGet(("Likes/{id}"))]
		public async Task<IActionResult> GetLikes(int id)
		{
			var comment = await _commentService.GetCommentLikesIncluded(id);
			if (comment == null)
			{
				return NotFound($"there is no comment with the id {id}");
			}
			var result = await _commentService.GetLikes(comment);
			var UserNames = result.Select(x => x.UserName);
			return Ok(UserNames);
		}
		[HttpDelete(("Unlike/{id}"))]
		[Authorize]
		public async Task<IActionResult> Unlike(int id)
		{
			var comment = await _commentService.GetCommentLikesIncluded(id);
			if (comment == null)
			{
				return NotFound($"there is no Comment with the id {id}");
			}
			var userName = _authServices.GetCurrentUserName(User);

			var result = await _commentService.UnLikeComment(comment, userName);
			if (result is null)
			{
				return BadRequest("the Comment is not liked by the user");
			}
			return Ok($"{result.Likes} users liked this comment ");
		}
	}
}
