using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.DTO
{
    public class userLoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
