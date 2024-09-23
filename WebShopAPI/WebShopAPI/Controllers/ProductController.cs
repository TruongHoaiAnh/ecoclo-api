using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Models;
using WebShopAPI.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int? page, string? searchString, string? IdCate, float? from, float? to)
        {
            List<ProductDto> lsProduct = new List<ProductDto>();
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 8;
            try
            {
                lsProduct = await _productRepo.GetAll(searchString, IdCate, from, to);
                PagedList<ProductDto> models = new PagedList<ProductDto>(lsProduct.AsQueryable(), pageNumber, pageSize);
                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{idPro}")]
        public async Task<IActionResult> GetDetail(string idPro)
        {
            var productDetail = await _productRepo.ProductDetai(idPro);

            if (productDetail == null)
            {
                return NotFound(new ApiResponse { Success = false, Message = "ProducDetail not found"}); 
            }

            return Ok(productDetail);
        }

        [HttpPost]
        [Authorize(Roles = AppRole.Administrator)]
        public async Task<IActionResult> Create([FromForm] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productRepo.Create(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.Administrator)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _productRepo.DeleteById(id);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
