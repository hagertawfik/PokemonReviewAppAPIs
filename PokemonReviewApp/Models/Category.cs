namespace PokemonReviewApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //many to many relation
        public ICollection<PokemonCategeroy> PokemonCategeroies { get; set; }
    }
}
