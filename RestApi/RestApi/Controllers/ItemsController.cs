using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApi.DTOs;
using RestApi.Entities;
using RestApi.Helpers;
using RestApi.Interfaces;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Get([FromQuery] ItemParams itemParams)
        {
            var items = await _unitOfWork.ItemRepository.GetItems(itemParams);
            Response.AddPaginationHeader(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);
            return Ok(items);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult> Post(ItemDto itemDto)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategory(itemDto.CategoryId);
            if (category == null) return NotFound();
            var item = new Item() 
            { 
                Name = itemDto.Name,
                Category = category
            };
            _unitOfWork.ItemRepository.AddItem(item);
            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Post),item);
            return BadRequest("Failed to create Item");
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ItemDto itemDto)
        {
            var item = await _unitOfWork.ItemRepository.GetItem(id);
            if (item == null)
                return NotFound("Item with that id doesn't exist");
            

            var category = await _unitOfWork.CategoryRepository.GetCategory(itemDto.CategoryId);
            if (category == null) return NotFound();
            item.Name = itemDto.Name;
            item.CategoryId = itemDto.CategoryId;

            _unitOfWork.ItemRepository.UpdateItem(item);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Problem updating the Item");
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetItem(id);
            if (item == null)
                return NotFound("Item with that id doesn't exist");
            _unitOfWork.ItemRepository.DeleteItem(item);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Problem deleting the item");
        }
    }
}
