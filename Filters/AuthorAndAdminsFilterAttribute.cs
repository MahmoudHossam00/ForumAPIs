using BlogProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using WebApplication2.Helpers;

namespace BlogProject.Filteers
{
	public class AuthorAndAdminsFilterAttribute : ActionFilterAttribute
	{
		private readonly ApplicationDbContext _context;
		//public TypesEnum _type { get; set; } 
		private readonly UserManager<ApplicationUser> _userManager;
		public AuthorAndAdminsFilterAttribute(ApplicationDbContext Context, UserManager<ApplicationUser> UserManager)
		{

			_context = Context;
			_userManager = UserManager;
		}
		
		public  override void OnActionExecuting(ActionExecutingContext context)
		{
			var request = context.HttpContext.Request;
			var entityType = context.RouteData.Values["controller"]?.ToString();

			// Safely parse the id from RouteData
			if (!context.RouteData.Values.TryGetValue("id", out var idValue) || !int.TryParse(idValue.ToString(), out var id))
			{
				context.Result = new BadRequestObjectResult(new { message = "Invalid ID" });
				return ;
			}

			string authorName;
			switch (entityType)
			{
				case "Posts":
					authorName = (_context.Posts.Find(id))?.AuthorName;
					break;

				case "Comments":
					authorName = ( _context.Comments.Find(id))?.AuthorName;
					break;

				case "Replies":
					authorName = ( _context.Replies.Find(id))?.AuthorName;
					break;

				default:
					context.Result = new BadRequestObjectResult(new { message = "Unknown Entity" });
					return;
			}

			var userName = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user =  _userManager.FindByNameAsync(userName).GetAwaiter().GetResult();
			if (user is null)
			{
				context.Result = new BadRequestObjectResult(new { message = "User Is Not Authenticated" });
				return;
			}

			if (authorName != userName &&
				! _userManager.IsInRoleAsync(user, RolesStrings._admin.ToUpper()).GetAwaiter().GetResult() &&
				! _userManager.IsInRoleAsync(user, RolesStrings._Moderator.ToUpper()).GetAwaiter().GetResult())
			{
				context.Result = new BadRequestObjectResult(new { message = "Only Author and Admins Are Allowed" });
				return;
			}

			if (user.IsBanned)
			{
				context.Result = new BadRequestObjectResult(new { message = $"User is banned until {user.RestrictedUntill}" });
				return;
			}

			// Call base method at the end
			base.OnActionExecuting(context);
		}
		
	}
}
