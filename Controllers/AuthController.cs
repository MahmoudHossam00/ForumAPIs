using BlogProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlogProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController (IAuthService _authServices): ControllerBase
	{

		[HttpPost("Register")]
		public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authServices.RegisterAsync(model);
			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			return Ok(result);
		}
		[HttpPost("token")]
		public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)

		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _authServices.GetTokenAsync(model);
			if (!result.IsAuthenticated)
			{
				return BadRequest(result.Message);
			}
			return Ok(result);
		}
		[Authorize(Roles = "ADMIN")]
		[HttpPost("addrole")]
		public async Task<IActionResult> AddRoleAsync([FromBody] EditRoleModel model)

		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _authServices.AddRoleAsync(model);
			if (!string.IsNullOrEmpty(result))
			{
				return BadRequest(result);
			}
			return Ok(model);
		}
		[Authorize(Roles = "ADMIN")]
		[HttpPost("Removerole")]
		public async Task<IActionResult> RemoveRoleAsync([FromBody] EditRoleModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _authServices.RemoveRoleAsync(model);
			if (!string.IsNullOrEmpty(result))
			{
				return BadRequest(result);
			}
			return Ok(model);
		}
		[Authorize(Policy = "ModsAndAdmins")]
		[HttpPost("BanUser")]
		public async Task<IActionResult> BanUserAsync([FromBody] BanUserModel model)

		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _authServices.BanUserAsync(model);
			if (!string.IsNullOrEmpty(result))
			{
				return BadRequest(result);
			}
			return Ok(model);
		}
		[Authorize(Policy = "ModsAndAdmins")]
		[HttpPut("UnBanUser")]
		public async Task<IActionResult> UnBanUserAsync(string UserName)

		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _authServices.UnBanUserAsync(UserName);
			if (!string.IsNullOrEmpty(result))
			{
				return BadRequest(result);
			}
			return Ok("User Is UnBanned");
		}
	}
}
