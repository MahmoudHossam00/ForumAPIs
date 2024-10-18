using System.Security.Claims;

namespace BlogProject.Services
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(RegisterModel model);
		Task<AuthModel> GetTokenAsync(TokenRequestModel model);
		Task<string> AddRoleAsync(EditRoleModel model);
		Task<string> RemoveRoleAsync(EditRoleModel model);
		Task<string> BanUserAsync(BanUserModel model);
		Task<string> UnBanUserAsync(string UserName);
		Task<string> CheckIfBanned(ClaimsPrincipal user);
		public string GetCurrentUserName(ClaimsPrincipal user);
	}
}
