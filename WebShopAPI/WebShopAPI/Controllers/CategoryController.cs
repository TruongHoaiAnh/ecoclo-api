using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebShopAPI.Helpers;
using WebShopAPI.Models;
using WebShopAPI.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _categoryRepo;
        public CategoryController(ICategoryRepo categoryRepo) { 
            _categoryRepo = categoryRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepo.GetAll();
            return Ok(categories);
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    ErrorCode = "ID_IS_NOTNUL",
                    Message = "Id is not null"
                });
            }
            var category = await _categoryRepo.GetById(id);
            if (category == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    ErrorCode = "NOT_FOUND",
                    Message = "Category not found."
                });
            }
            return Ok(category);
        }*/

        [HttpPost]
        [Authorize(Roles = AppRole.Administrator)]
        public async Task<IActionResult> Create([FromBody] CategoryModel model)
        {
            var result = await _categoryRepo.Create(model);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.Administrator)]
        public async Task<IActionResult> Update(string id, [FromBody] CategoryModel model)
        {
            var result = await _categoryRepo.Update(model, id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.Administrator)]
        public async Task<IActionResult> Delete(string id)
        {
            var category = await _categoryRepo.DeleteById(id);
            if (category.Success)
            {
                return Ok(category);
            }
            return BadRequest(category);
        }

    }
}
