namespace PokemonReviewApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        //one to many relationship
        public ICollection<Review> Reviews { get; set; }

        //many to many relationship
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokemonCategeroy> PokemonCategeroies { get; set; }
    }
}
