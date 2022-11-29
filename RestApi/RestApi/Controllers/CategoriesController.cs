using Microsoft.AspNetCore.Mvc;
using RestApi.DTOs;
using RestApi.Entities;
using RestApi.Helpers;
using RestApi.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get([FromQuery] PaginationParams paginationParams)
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategories(paginationParams);
            Response.AddPaginationHeader(categories.CurrentPage, categories.PageSize, categories.TotalCount, categories.TotalPages);

            return Ok(categories);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult> Post(CategoryDto categoryDto)
        {
            var category = new Category() { Name = categoryDto.Name };
            _unitOfWork.CategoryRepository.AddCategory(category);
            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Post), category);
            return BadRequest("Failed to create category");
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CategoryDto categoryDto)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategory(id);
            if (category == null)
                return NotFound("Category with that id doesn't exist");
            category.Name = categoryDto.Name;
            _unitOfWork.CategoryRepository.UpdateCategory(category);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Problem updating the category");
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategory(id);
            if (category == null)
                return NotFound("Category with that id doesn't exist");
            _unitOfWork.CategoryRepository.DeleteCategory(category);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Problem deleting the category");
        }
    }
}
