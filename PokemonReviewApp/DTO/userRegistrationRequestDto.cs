using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.DTO
{
    public class userRegistrationRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
