using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _categoryService.GetCategories();
            
            if(categories == null)
            {
                return NotFound("Categories not found");
            }
            else
            {
                return Ok(categories);
            }
            
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }
            else
            {
                return Ok(category);
            }

        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Invalid Data");
            await _categoryService.Add(categoryDto);
            return new CreatedAtRouteResult("GetCategory", new { id = categoryDto.Id }, categoryDto);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id)
                return BadRequest();

            if (categoryDto == null)
                return BadRequest();

            await _categoryService.Update(categoryDto);

            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Put(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }
            else
            {
                await _categoryService.Remove(id);

                return Ok(category);
            }
        }
    }
}
