
using BlogProject.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Helpers;

namespace BlogProject.Services
{
	public class AuthService
		(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IOptions<JWT> _jwt) 
		: IAuthService
	{
		

		public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
		{
			var authModel = new AuthModel();
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				authModel.Message = "the email/password is incorrect";
				return authModel;
			}
			var jwtSecurityToken = await CreateJwtToken(user);
			authModel.IsAuthenticated = true;
			authModel.Email = user.Email;
			authModel.UserName = user.UserName;
			authModel.ExpiresOn = jwtSecurityToken.ValidTo;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			var rolesList = await _userManager.GetRolesAsync(user);
			authModel.Roles = rolesList.ToList();


			return authModel;
		}
		
		public async Task<AuthModel> RegisterAsync(RegisterModel model)
		{
			if (await _userManager.FindByEmailAsync(model.Email) is not null)
				return new AuthModel { Message = "Email is already registered" };
			if (await _userManager.FindByNameAsync(model.Username) is not null)
				return new AuthModel { Message = "UserName is already registered" };

			var user = new ApplicationUser
			{
				UserName = model.Username,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				string errors = string.Empty;
				foreach (var error in result.Errors)
				{
					errors += $"{error.Description}";
				}
				return new AuthModel { Message = errors };
			}

			await _userManager.AddToRoleAsync(user, RolesStrings._user);
			var jwtToken = await CreateJwtToken(user);
			return new AuthModel
			{
				Email = user.Email,
				ExpiresOn = jwtToken.ValidTo,
				IsAuthenticated = true,
				Roles = new List<string> { RolesStrings._user.ToUpper() },
				Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
				UserName = user.UserName
			};
		
		}
		public async Task<string> AddRoleAsync(EditRoleModel model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return "Invalid User Id";
			}
			if (!await _roleManager.RoleExistsAsync(model.Role.ToUpper()))
			{
				return "Role Is Invalid";
			}

			if (await _userManager.IsInRoleAsync(user, model.Role.ToUpper()))
			{
				return "User Is already assigned to this rule";
			}

			var result = await _userManager.AddToRoleAsync(user, model.Role);

			return result.Succeeded ? string.Empty : "SomethingWentWrong";
		}
		public async Task<string> RemoveRoleAsync(EditRoleModel model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return "Invalid User Id";
			}
			if (!await _roleManager.RoleExistsAsync(model.Role.ToUpper()))
			{
				return "Role Is Invalid";
				
			}
			if (await _userManager.IsInRoleAsync(user, model.Role.ToUpper()))
			{
				var result = await _userManager.RemoveFromRoleAsync(user, model.Role);
				return result.Succeeded ? string.Empty : "SomethingWentWrong";
			}
				return "User Is Not assigned to this rule";
		}
		public async Task<string> BanUserAsync(BanUserModel model)
		{
			var user = await _userManager.FindByNameAsync(model.UserName);
			if (user == null)
				return "Invalid User Name";
			if (await _userManager.IsInRoleAsync(user, RolesStrings._admin.ToUpper()) || await _userManager.IsInRoleAsync(user, RolesStrings._Moderator.ToUpper()))
				return "You Cant Ban An Admin/Moderator";
		
			
				user.IsBanned = true;
			
			user.RestrictedUntill = DateTime.Now.AddDays((double)model.banDurationInDays).AddHours((double)model.BanDurationInHours).AddMinutes((double)model.BanDurationInMinutes);
			
			var result = await _userManager.UpdateAsync(user);
			return result.Succeeded ? string.Empty : "SomethingWentWrong";
		}
		public async Task<string> UnBanUserAsync(string UserName)
		{
			var user = await _userManager.FindByNameAsync(UserName);
			if (user == null)
				return "Invalid User Name";
			//if (await _userManager.IsInRoleAsync(user, RolesStrings._admin.ToUpper()) || await _userManager.IsInRoleAsync(user, RolesStrings._Moderator.ToUpper()))
			//	return "You Cant Ban An Admin/Moderator";
			if(!user.IsBanned)
			{
				return "User Is Not Banned";
			}

			user.IsBanned = false;

			user.RestrictedUntill = DateTime.Now;

			var result = await _userManager.UpdateAsync(user);
			return result.Succeeded ? string.Empty : "SomethingWentWrong";
		}
		public async Task<string> CheckIfBanned(ClaimsPrincipal user)
		{
			var User = await _userManager.FindByNameAsync(GetCurrentUserName(user));
			if (user is null)
			{
				return " Username NotFound";
			}
			if (User.IsBanned)
			{
				if (User.RestrictedUntill <= DateTime.Now)
				{

					User.IsBanned = false;
					var result = await _userManager.UpdateAsync(User);
					return result.Succeeded ?  string.Empty : "SomethingWentWrong";
				}
				else
				{
					return $"User Is Banned From posting/Editing till {User.RestrictedUntill}";
				}
			}
			return string.Empty;

		}
		public string GetCurrentUserName(ClaimsPrincipal user)
		{
			return  user.FindFirstValue(ClaimTypes.NameIdentifier);
		}
		private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var RoleClaims = new List<Claim>();
			foreach (var role in roles)
			{
				RoleClaims.Add(new Claim("roles", role));
			}
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email,user.Email),
				new Claim("uid",user.Id),
			}.Union(userClaims).Union(RoleClaims);
			var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
			var SigningCredentialss = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken

				(

				issuer: _jwt.Value.Issuer,
				audience: _jwt.Value.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddDays(_jwt.Value.DurationInDays),
				signingCredentials: SigningCredentialss
				);

			return jwtSecurityToken;

		}
		


	}
}
