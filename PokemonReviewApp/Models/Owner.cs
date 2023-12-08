namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }

        //one to one relationship
        public Country Country { get; set; }
        //many to many relationship
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
