using BlogProject.dtos;

using BlogProject.Helpers;
using BlogProject.Services;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApplication2.Helpers;
using AutoMapper;
using BlogProject.Models;
using Azure.Identity;
using NuGet.Protocol;
using System.Net.WebSockets;
using BlogProject.Filteers;
using BlogProject.Filters;
namespace BlogProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostsController(IMapper _mapper, IPostService _postService
		, ICategoryService _categoryService, IOptions<PictureSpecifications> _pictureSpecs
		, UserManager<ApplicationUser> _userManager, IAuthService _authServices) : ControllerBase
	{



		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var posts = await _postService.GetAllAsync();
			var data = _mapper.Map<IEnumerable<RevealedPost>>(posts);
			return Ok(data);
		}

		[HttpGet("GetPostWithComments/{Id}")]
		public async Task<IActionResult> GetPostWithCommentsAsync(int Id)
		{

			var post = await _postService.GetFullPost(Id);
			if (post is null)
				return NotFound($"There is no Post with Id {Id}");
			var data = _mapper.Map<CompletePostModel>(post);
			return Ok(data);
		}
		[HttpGet("{Id}")]
		public async Task<IActionResult> GetByIdAsync(int Id)
		{
			var post = await _postService.GetByIdAsync(Id);
			if (post is null)
				return NotFound($"There is no Post with Id {Id}");

			var data = _mapper.Map<RevealedPost>(post);
			return Ok(data);

		}
		[HttpGet("Filteration")]
		public async Task<IActionResult> GetPostByFilteration([FromQuery] PostsFilterationModel model = null)
		{
			model ??= new PostsFilterationModel();
			var posts = await _postService.GetFilteredAsync(model);
			if (posts is null)
				return NotFound("There are no More Posts to show or no posts matchign ur searching criteria");

			var data = _mapper.Map<IEnumerable<RevealedPost>>(posts);
			var categories = await _categoryService.GetCategoriesAsync();
			var CategoriesData = categories.Select(c => new { Name = c.Name
				, Id = c.Id }).ToList();
			return Ok(new { data, model.ElementsPerPage, CategoriesData });
		}

		[HttpGet("GetByUser/{UserName}")]
		public async Task<IActionResult> GetByUserAsync(string UserName)
		{
			var posts = await _postService.GetAllAsync(UserName);
			if (posts is null)
				return NotFound($"There are no Posts by the User {UserName}");

			var data = _mapper.Map<IEnumerable<RevealedPost>>(posts);
			return Ok(data);
		}
		[HttpPost]
		[Authorize]

		public async Task<IActionResult> CreateAsync([FromForm] PostDto dto)
		{
			var AuthString = await _authServices.CheckIfBanned(User);
			if (!string.IsNullOrEmpty(AuthString))
				return BadRequest(AuthString);

			var UserName = _authServices.GetCurrentUserName(User);


			var post = new Post();
			if (dto.Picture is not null) {

				var specs = _pictureSpecs.Value;
				if (!specs._allowedExtensions.Contains(Path.GetExtension(dto.Picture.FileName)))
					return BadRequest("Only .jpg .png pictures are allowed");

				if (!(specs._maxAllowedPosterSize > dto.Picture.Length))
					return BadRequest("Maximum Size allowed for pictures is 1MB");



				using var DataStream = new MemoryStream();
				await dto.Picture.CopyToAsync(DataStream);
				post.Picture = DataStream.ToArray();
			}

			if (!await _categoryService.IsValidCategory(dto.CategoryId))
				return NotFound($"there is no category with the id {dto.CategoryId}");

			post.Title = dto.Title;
			post.CategoryId = dto.CategoryId;
			post.DateTime = DateTime.Now;
			post.Content = dto.Content;
			post.Likes = 0;


			//var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			//if (UserId == null)
			//{
			//	return NotFound("User not found.");
			//}

			////var user = await _userManager.FindByIdAsync(UserId);
			//if (user == null)
			//{
			//	return NotFound("User not found.");
			//}
			post.AuthorName = UserName;
			//HttpContext.User.Identity.Name;

			await _postService.AddAsync(post);
			var _post = await _postService.GetByIdAsync(post.Id);
			var data = _mapper.Map<RevealedPost>(_post);
			return Ok(data);


		}
		[HttpPut("{id}")]
		[Authorize]
		[ServiceFilter(typeof(AuthorFilterAttribute))]
		public async Task<IActionResult> UpdateAsync([FromForm]PostDto dto,int id)
		{
			var AuthString = await _authServices.CheckIfBanned(User);
			if (!string.IsNullOrEmpty(AuthString))
				return BadRequest(AuthString);

			//var UserName = _authServices.GetCurrentUserName(User);
			var post = await _postService.GetByIdAsync(id);	
			
			if (post == null) return NotFound($"there are not posts with the id {id}");


			//if (post.AuthorName != UserName)
			//	{
			//	return BadRequest("Only Author Can edit the post");
			//    }
		

			if (dto.Picture is not null)
			{

				var specs = _pictureSpecs.Value;
				if (!specs._allowedExtensions.Contains(Path.GetExtension(dto.Picture.FileName)))
					return BadRequest("Only .jpg .png pictures are allowed");

				if (!(specs._maxAllowedPosterSize > dto.Picture.Length))
					return BadRequest("Maximum Size allowed for pictures is 1MB");

		

				using var DataStream = new MemoryStream();
				await dto.Picture.CopyToAsync(DataStream);
				post.Picture = DataStream.ToArray();
			}
			else
			{
				post.Picture = null;
			}

				if (!await _categoryService.IsValidCategory(dto.CategoryId))
					return NotFound($"there is no category with the id {dto.CategoryId}");

			post.Title = dto.Title;
			post.CategoryId = dto.CategoryId;
			post.Content = dto.Content;
		
			 _postService.Update(post);
			var _post = await _postService.GetByIdAsync(post.Id);
			var data = _mapper.Map<RevealedPost>(_post);
			return Ok(data);
		}

		[HttpDelete("{id}")]
		[Authorize]
		[ServiceFilter(typeof(AuthorAndAdminsFilterAttribute))]
		public async Task<IActionResult> DeleteAsync( int id)
		{
			var post = await _postService.GetByIdAsync(id);
			if (post == null) 
				return NotFound($"there is no post with the id {id}");

			//if (post.AuthorName !=_authServices.GetCurrentUserName(User)
			//	&&!User.IsInRole(RolesStrings._admin.ToUpper())
			//	&&!User.IsInRole(RolesStrings._Moderator.ToUpper()))
			//{
			//	return BadRequest("Only Author and admins Can Delete the post");
			//}
			var _post = await _postService.GetByIdAsync(post.Id);
			_postService.Delete(post);
			var data = _mapper.Map<RevealedPost>(_post);
			return Ok(data);
		}
		//-------------------------------------------//
		[HttpPost("LIKE/{id}")]
		[Authorize]
		public async Task<IActionResult> Like(int id)
		{
			var post = await _postService.GetPostLikesIncluded(id);
			if(post == null)
			{
				return NotFound($"there is no Post with the id {id}");
			}
			var userName = _authServices.GetCurrentUserName(User);

			var result = await _postService.LikePost(post, userName);
			if(result is null)
			{
				return BadRequest("the post is already liked by the user");
			}
			return Ok($"{result.Likes} users liked this post ");
		}
		//public Task<List<ApplicationUser>> GetLikes(Post post);
		[HttpGet(("Likes/{id}"))]
		public async Task<IActionResult> GetLikes(int id)
		{
			var post = await _postService.GetPostLikesIncluded(id);
			if (post == null)
			{
				return NotFound($"there is no Post with the id {id}");
			}
			var result = await _postService.GetLikes(post);
			var UserNames = result.Select(x => x.UserName);
			return Ok(UserNames);
		}
		[HttpDelete(("Unlike/{id}"))]
		[Authorize]
		public async Task<IActionResult> Unlike(int id)
		{
			var post = await _postService.GetPostLikesIncluded(id);
			if (post == null)
			{
				return NotFound($"there is no Post with the id {id}");
			}
			var userName = _authServices.GetCurrentUserName(User);

			var result = await _postService.UnLikePost(post, userName);
			if (result is null)
			{
				return BadRequest("the post is not liked by the user");
			}
			return Ok($"{result.Likes} users liked this post ");
		}
	}
}
