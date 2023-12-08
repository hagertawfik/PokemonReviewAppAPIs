namespace PokemonReviewApp.Models
{
    public class PokemonCategeroy
    {
        public int PokemonId { get; set; }
        public int CategeroyId { get; set; }

        public Pokemon Pokemon { get; set; }
        public Category Category { get; set; }

    }
}
