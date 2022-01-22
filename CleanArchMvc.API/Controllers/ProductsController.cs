using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;            

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var product = await _productService.GetProducts();

            if (product == null)
            {
                return NotFound("Products not found");
            }
            else
            {
                return Ok(product);
            }

        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound("Product not found");
            }
            else
            {
                return Ok(product);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest("Invalid Data");

            await _productService.Add(productDto);

            return new CreatedAtRouteResult("GetProduct", new { id = productDto.Id }, productDto);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDTO productDto)
        {
            if (id != productDto.Id)
                return BadRequest();

            if (productDto == null)
                return BadRequest();

            await _productService.Update(productDto);

            return Ok(productDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Put(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound("Product not found");
            }
            else
            {
                await _productService.Remove(id);

                return Ok(product);
            }
        }
    }
}
