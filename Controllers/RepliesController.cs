using AutoMapper;
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
	public class RepliesController(IMapper _mapper,IReplyService _replyService,
		ICommentService _commentService, IOptions<PictureSpecifications> _pictureSpecs,
		UserManager<ApplicationUser> _userManager,IAuthService _authServices) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var Replies = await _replyService.GetAllAsync();
			var data = _mapper.Map<IEnumerable<RevealedReply>>(Replies);
			return Ok(data);
		}
		[HttpGet("{Id}")]
		public async Task<IActionResult> GetByIdAsync(int Id)
		{
			var Reply = await _replyService.GetByIdAsync(Id);
			if (Reply is null)
				return NotFound($"There is no Reply with Id {Id}");
			var data = _mapper.Map<RevealedReply>(Reply);
			return Ok(data);
			

		}
		[HttpGet("Search/{Search}")]
		public async Task<IActionResult> Search(string Search = "")
		{
			
			var Replies = await _replyService.GetFilteredAsync(Search);
			if (Replies is null)
				return NotFound("There are no More Replys to show or no Replys matchign ur searching criteria");
			var data = _mapper.Map<IEnumerable<RevealedReply>>(Replies);
			return Ok(data);
			
		}

		[HttpGet("GetByUser/{UserName}")]
		public async Task<IActionResult> GetByUser(string UserName)
		{
			var Replies = await _replyService.GetAllAsync(UserName);
			if (Replies is null)
				return NotFound($"There are no Replys by the User {UserName}");
			var data = _mapper.Map<IEnumerable<RevealedReply>>(Replies);
			return Ok(data);
		}

		[HttpGet("ByComment/{CommentId}")]
		public async Task<IActionResult> GetReplyByComment(int CommentId)
		{
			if (!await _commentService.IsValidComment(CommentId))
				return NotFound($"There are no Posts with the Id {CommentId}");
			var Replies = await _replyService.GetByComment(CommentId);
			if (Replies is null)
				return NotFound($"There are no Replys in a post with the id {CommentId}");
			var data = _mapper.Map<IEnumerable<RevealedReply>>(Replies);
			return Ok(data);
		}
		[HttpPost]
		[Authorize]

		public async Task<IActionResult> CreateAsync([FromForm] ReplyDto dto)
		{
			var AuthString = await _authServices.CheckIfBanned(User);
			if (!string.IsNullOrEmpty(AuthString))
				return BadRequest(AuthString);

			var UserName = _authServices.GetCurrentUserName(User);

			var Reply = new Reply();
			if (dto.Picture is not null)
			{

				var specs = _pictureSpecs.Value;
				if (!specs._allowedExtensions.Contains(Path.GetExtension(dto.Picture.FileName)))
					return BadRequest("Only .jpg .png pictures are allowed");

				if (!(specs._maxAllowedPosterSize > dto.Picture.Length))
					return BadRequest("Maximum Size allowed for pictures is 1MB");



				using var DataStream = new MemoryStream();
				await dto.Picture.CopyToAsync(DataStream);
				Reply.Picture = DataStream.ToArray();
			}

			if (!await _commentService.IsValidComment(dto.CommentId))
				return NotFound($"there is no Post with the id {dto.CommentId}");

			//Reply.Title = dto.Title;
			Reply.CommentId = dto.CommentId;
			Reply.DateTime = DateTime.Now;
			Reply.Content = dto.Content;
			Reply.Likes = 0;



			////var user = await _userManager.FindByIdAsync(UserId);
			//if (user == null)
			//{
			//	return NotFound("User not found.");
			//}
			Reply.AuthorName = UserName;
			//HttpContext.User.Identity.Name;

			await _replyService.AddAsync(Reply);
			var data = _mapper.Map<RevealedReply>(Reply);
			return Ok(data);

		}
		[HttpPut("{id}")]
		[Authorize]
		[ServiceFilter(typeof(AuthorFilterAttribute))]
		public async Task<IActionResult> UpdateAsync([FromForm] ReplyDto dto, int id)
		{
			var AuthString = await _authServices.CheckIfBanned(User);
			if (!string.IsNullOrEmpty(AuthString))
				return BadRequest(AuthString);

			//var UserName = _authServices.GetCurrentUserName(User);

			var Reply = await _replyService.GetByIdAsync(id);

			if (Reply == null) return NotFound($"there are not Replys with the id {id}");


			//if (Reply.AuthorName != UserName)
			//{
			//	return BadRequest("Only Author Can edit the Reply");
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
				Reply.Picture = DataStream.ToArray();
			}
			else
			{
				Reply.Picture = null;
			}

			if (!await _commentService.IsValidComment(dto.CommentId))
				return NotFound($"there is no Post with the id {dto.CommentId}");

			////Reply.Title = dto.Title;
			Reply.CommentId = dto.CommentId;
			Reply.Content = dto.Content;
			_replyService.Update(Reply);
			var data = _mapper.Map<RevealedReply>(Reply);
			return Ok(data);
		}

		[HttpDelete("{id}")]
		[Authorize]
		[ServiceFilter(typeof(AuthorAndAdminsFilterAttribute))]
		public async Task<IActionResult> DeleteAsync(int id)
		{

			var Reply = await _replyService.GetByIdAsync(id);
			if (Reply == null)
				return NotFound($"there is no Reply with the id {id}");

			//if (Reply.AuthorName != _authServices.GetCurrentUserName(User)
			//	&& !User.IsInRole(RolesStrings._admin.ToUpper())
			//	&& !User.IsInRole(RolesStrings._Moderator.ToUpper()))
			//{
			//	return BadRequest("Only Author and admins Can Delete the Reply");
			//}
			_replyService.Delete(Reply);
			var data = _mapper.Map<RevealedReply>(Reply);
			return Ok(data);
		}
		/***************************************************/
		[HttpPost("LIKE/{id}")]
		[Authorize]
		public async Task<IActionResult> Like(int id)
		{
			var reply = await _replyService.GetReplyLikesIncluded(id);
			if (reply == null)
			{
				return NotFound($"there is no reply with the id {id}");
			}
			var userName = _authServices.GetCurrentUserName(User);

			var result = await _replyService.LikeReply(reply, userName);
			if (result is null)
			{
				return BadRequest("the Reply is already liked by the user");
			}
			return Ok($"{result.Likes} users liked this Reply ");
		}
		//public Task<List<ApplicationUser>> GetLikes(Post post);
		[HttpGet(("GetLikes/{id}"))]
		public async Task<IActionResult> GetLikes(int id)
		{
			var reply = await _replyService.GetReplyLikesIncluded(id);
			if (reply == null)
			{
				return NotFound($"there is no reply with the id {id}");
			}
			var result = await _replyService.GetLikes(reply);
			var UserNames = result.Select(x => x.UserName);
			return Ok(UserNames);
		}
		[HttpDelete(("Unlike/{id}"))]
		[Authorize]
		public async Task<IActionResult> Unlike(int id)
		{
			var reply = await _replyService.GetReplyLikesIncluded(id);
			if (reply == null)
			{
				return NotFound($"there is no Reply with the id {id}");
			}
			var userName = _authServices.GetCurrentUserName(User);

			var result = await _replyService.UnLikeReply(reply, userName);
			if (result is null)
			{
				return BadRequest("the Reply is not liked by the user");
			}
			return Ok($"{result.Likes} users liked this Reply ");
		}
	}
}
