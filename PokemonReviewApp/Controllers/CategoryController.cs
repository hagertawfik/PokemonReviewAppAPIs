using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.repository;

namespace PokemonReviewApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]

   
    public class CategoryController : Controller
    {
        private readonly ICategoryReposotry _categoryReposotry;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryReposotry categoryReposotry, IMapper mapper)
        {
            _categoryReposotry = categoryReposotry;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryReposotry.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);

        }

        [HttpGet("CategoryId")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int CategoryId)
        {
            if (!_categoryReposotry.CategoryExsits(CategoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryReposotry.GetCategory(CategoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }


        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon> ))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonByCategory(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemoneDto>>(_categoryReposotry.GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);

        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody]CategoryDto categorycreate)
        {
            if(categorycreate == null)
                return BadRequest(ModelState);

            var category = _categoryReposotry.GetCategories().Where(c => c.Name.Trim().ToUpper() ==
            categorycreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "this category is already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var categoryMap = _mapper.Map<Category>(categorycreate);
            if (!_categoryReposotry.CreateCategory(categoryMap)) {
                ModelState.AddModelError("", "somthing went wrong whie saving");
                return StatusCode(500, ModelState);
            
            }
            return Ok("successifuly created");
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId ,[FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);
            if (categoryId!= updatedCategory.Id)
                return BadRequest(ModelState);
            if (!_categoryReposotry.CategoryExsits(categoryId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var categoryMap = _mapper.Map < Category >( updatedCategory);
            if (_categoryReposotry.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "somthing went wrong while updating category");
                return StatusCode(500,ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryReposotry.CategoryExsits(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryReposotry.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryReposotry.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
