using BlogProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BlogProject.Filters
{
	
	public class AuthorFilterAttribute : ActionFilterAttribute
	{

		private readonly ApplicationDbContext _context;
		//public TypesEnum _type { get; set; } 
		private readonly UserManager<ApplicationUser> _userManager;
		public AuthorFilterAttribute(ApplicationDbContext Context, UserManager<ApplicationUser> userManager)
		{

			_context = Context;
			_userManager = userManager;
		}


		public override void OnActionExecuting(ActionExecutingContext context)	
		{
			//base.OnActionExecuting(context);
			var request = context.HttpContext.Request;
			var entityType = context.RouteData.Values["controller"]?.ToString();
			if (!context.RouteData.Values.TryGetValue("id", out var idValue) || !int.TryParse(idValue.ToString(), out var id))
			{
				context.Result = new BadRequestObjectResult(new { message = "Invalid ID" });
				return;
			}
			string authorName;
			switch (entityType)
			{
				case "Posts":
					authorName = ( _context.Posts.Find(id))?.AuthorName;
					break;

				case "Comments":
					authorName = ( _context.Comments.Find(id))?.AuthorName;
					break;
				case "Replies":
					authorName = ( _context.Replies.Find(id))?.AuthorName;
					break;

				default:
					context.Result = new BadRequestObjectResult(new { message = "UnknownEntity" });
					return;
			}

			var UserName = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user =  _userManager.FindByNameAsync(UserName).GetAwaiter().GetResult();
			if(user is null)
			{
				context.Result = new BadRequestObjectResult(new { message = "Only Author Allowed" });
				return;
			}

			if (! (authorName == UserName))
			{
				context.Result = new BadRequestObjectResult(new { message = "Only Author Allowed"});
				return;
			}
			
			if(user.IsBanned)
			{
				context.Result = new BadRequestObjectResult(new { message = $"User is banned untill {user.RestrictedUntill}" });
				return;
			}
		

			base.OnActionExecuting(context);
		}

		
	}
}
