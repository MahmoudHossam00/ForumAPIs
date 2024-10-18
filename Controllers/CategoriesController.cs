using BlogProject.dtos;
using BlogProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
	public class CategoriesController(ICategoryService _categories) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var categories = await _categories.GetCategoriesAsync();
			return Ok(categories);
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetById(byte Id)
		{
			var category = await _categories.GetCategoryByIdAsync(Id);
			if(category is null)
				return NotFound($"there is no category with the Id of {Id}");
			return Ok(category);
		}

		[HttpPost]
		[Authorize(Policy = "ModsAndAdmins")]
		public async Task<IActionResult> CreateAsync(CategoryDto Dto)
		{
			Category category = new Category { Name = Dto.Name };

			await _categories.AddAsync(category);
			return Ok(category);
		}

		[HttpPut("{Id}")]
		[Authorize(Policy = "ModsAndAdmins")]
		public async Task<IActionResult> UpdateAsync(byte Id, CategoryDto Dto)
		{
			if(!await _categories.IsValidCategory(Id))
			{
				return NotFound($"there is no category with the Id of {Id}");
			}
			Category category = await _categories.GetCategoryByIdAsync(Id);
			if (category is null)
				return BadRequest("An error happened");
			category.Name = Dto.Name;
			_categories.Update(category);
			return Ok(category);
		}
		[HttpDelete("{Id}")]
		[Authorize(Policy = "ModsAndAdmins")]
		public async Task<IActionResult> DeleteAsync(byte Id)
		{
			if (!await _categories.IsValidCategory(Id))
			{
				return NotFound($"there is no category with the Id of {Id}");
			}
			Category category = await _categories.GetCategoryByIdAsync(Id);
			if (category is null)
				return BadRequest("An error happened");
			_categories.Delete(category);
			return Ok(category);
		}

	}
}
