using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.DTO;
using PokemonReviewApp.interfaces;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.repository;

namespace PokemonReviewApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController:Controller
    {
        private readonly IPokemonReposotry _pokemonReposotry;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonReposotry pokemonReposotry, IReviewRepository reviewRepository , IMapper mapper)
        {
            _pokemonReposotry = pokemonReposotry;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons =_mapper.Map<List<PokemoneDto>>( _pokemonReposotry.GetPokemons());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(pokemons);
           
        }

        [HttpGet("PokemonId")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int PokeId)
        {
            if (!_pokemonReposotry.PokemonExsits(PokeId))
                return NotFound();

            var pokemon =_mapper.Map<PokemoneDto>( _pokemonReposotry.GetPokemon(PokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("{PokeId}/rating")]
        [ProducesResponseType(200,Type =typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int PokeId)
        {
            if (!_pokemonReposotry.PokemonExsits(PokeId))
                return NotFound();
            
            var rating = _pokemonReposotry.GetPokemonRating(PokeId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);

            
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemoneDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonReposotry.GetPokemons()
                 .Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault(); 

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);


            if (!_pokemonReposotry.CreatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId,
            [FromQuery] int ownerId, [FromQuery] int catId,
            [FromBody] PokemoneDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokeId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonReposotry.PokemonExsits(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            if (!_pokemonReposotry.UpdatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonReposotry.PokemonExsits(pokeId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewRepository.GetReviewsOfAPokemon(pokeId);
            var pokemonToDelete = _pokemonReposotry.GetPokemon(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_pokemonReposotry.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }

    }
}
